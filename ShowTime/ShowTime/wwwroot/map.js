window.ShowTimeMaps = (function () {
    const maps = {};

    function createOrUpdateMap(containerId, options) {
        if (!window.L) {
            console.error('Leaflet is not loaded');
            return;
        }

        const container = document.getElementById(containerId);
        if (!container) {
            console.warn('Map container not found for id:', containerId);
            return;
        }

        const {
            latitude,
            longitude,
            address,
            zoom = 13
        } = options || {};

        let map = maps[containerId];
        // If a map exists but is bound to a different (recreated) container, dispose it
        if (map && map._container !== container) {
            try { map.remove(); } catch (e) { /* ignore */ }
            map = null;
            delete maps[containerId];
        }

        if (!map) {
            map = L.map(containerId, { zoomControl: true });
            maps[containerId] = map;
        }

        // Set tile layer once
        if (!map._tileLayerAdded) {
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                maxZoom: 19,
                attribution: '&copy; OpenStreetMap contributors'
            }).addTo(map);
            map._tileLayerAdded = true;
        }

        // Remove previous marker if present
        if (map._marker) {
            map.removeLayer(map._marker);
            map._marker = null;
        }

        if (typeof latitude === 'number' && typeof longitude === 'number') {
            const latLng = [latitude, longitude];
            map.setView(latLng, zoom);
            map._marker = L.marker(latLng).addTo(map);
            if (address) {
                map._marker.bindPopup(address).openPopup();
            }
        }

        // Ensure proper rendering when container becomes visible
        setTimeout(() => { map.invalidateSize(); }, 50);
        if (window.requestAnimationFrame) {
            window.requestAnimationFrame(() => map.invalidateSize());
        }
    }

    async function geocodeAddress(address) {
        const url = `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(address)}`;
        const res = await fetch(url, {
            headers: {
                'Accept': 'application/json'
            }
        });
        if (!res.ok) return null;
        const data = await res.json();
        if (!Array.isArray(data) || data.length === 0) return null;
        const first = data[0];
        return {
            lat: parseFloat(first.lat),
            lon: parseFloat(first.lon)
        };
    }

    return {
        showAddressOnMap: async function (containerId, address, zoom) {
            const result = await geocodeAddress(address);
            if (!result) {
                console.warn('Geocoding failed for address:', address);
                return false;
            }
            createOrUpdateMap(containerId, {
                latitude: result.lat,
                longitude: result.lon,
                address,
                zoom: zoom || 13
            });
            return true;
        },
        showLatLngOnMap: function (containerId, latitude, longitude, address, zoom) {
            createOrUpdateMap(containerId, {
                latitude,
                longitude,
                address,
                zoom: zoom || 13
            });
            return true;
        },
        disposeMap: function (containerId) {
            const map = maps[containerId];
            if (map) {
                try { map.remove(); } catch (e) { /* ignore */ }
                delete maps[containerId];
            }
            return true;
        }
    };
})();


