using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ShowTime.BusinessLogic.Abstractions;
using ShowTime.BusinessLogic.Services;
using ShowTime.Components;
using ShowTime.DataAccess;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstractions;
using ShowTime.DataAccess.Repositories.Implementations;
using MudBlazor.Services;
using ShowTime.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth-token";
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMudServices();

var connectionString = builder.Configuration.GetConnectionString("ShowTimeContext");
builder.Services.AddDbContext<ShowTimeDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddTransient<IRepository<Artist>, Repository<Artist>>();
builder.Services.AddTransient<IArtistService, ArtistService>();

builder.Services.AddTransient<IRepository<Festival>, Repository<Festival>>();
builder.Services.AddTransient<ILineupRepository, LineupRepository>();
builder.Services.AddTransient<IFestivalService, FestivalService>();

builder.Services.AddTransient<IRepository<User>, Repository<User>>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddTransient<IRepository<Ticket>, Repository<Ticket>>();
builder.Services.AddTransient<ITicketService, TicketService>();

builder.Services.AddTransient<IRepository<Booking>, Repository<Booking>>();
builder.Services.AddTransient<IBookingService, BookingService>();

builder.Services.AddTransient<IRepository<Review>, Repository<Review>>();
builder.Services.AddTransient<IReviewService, ReviewService>();

// ML.NET sentiment service
builder.Services.AddSingleton<ISentimentService, SentimentService>();

// Cart service (scoped per user/session)
builder.Services.AddScoped<CartService>();

// AI search removed per user request

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();
