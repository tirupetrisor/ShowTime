using Microsoft.EntityFrameworkCore;
using ShowTime.DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Repositories.Implementations;

public class Repository <T> : IRepository<T> where T: class
{
    private readonly ShowTimeDbContext Context;

    public Repository(ShowTimeDbContext context)
    {
        Context = context;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        try
        {
            return await Context.Set<T>().FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error retrieving entity with id {id}", ex);
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await Context.Set<T>().AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error retrieving all entities", ex);
        }
    }

    public async Task AddAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error adding entity", ex);
        }
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error updating entity", ex);
        }
    }


    public async Task DeleteAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error deleting entity", ex);
        }
    }

}
