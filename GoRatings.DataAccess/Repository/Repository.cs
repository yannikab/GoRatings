using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace GoRatings.DataAccess.Repository;

public class Repository<D, E, K> : IRepository<D, E, K> where D : DbContext where E : class
{
    protected readonly D dbc;
    protected readonly E none;

    public Repository(D dbc)
    {
        this.dbc = dbc;
        this.none = typeof(E).GetNone<E>();
    }

    public virtual E Get(K id)
    {
        return dbc.Set<E>().Find(id) ?? none;
    }

    public virtual IEnumerable<E> GetAll()
    {
        return dbc.Set<E>();
    }

    public virtual IEnumerable<E> Find(Expression<Func<E, bool>> predicate)
    {
        return dbc.Set<E>().Where(predicate);
    }

    public virtual void Add(E entity)
    {
        dbc.Set<E>().Add(entity);
    }

    public virtual void AddRange(IEnumerable<E> entities)
    {
        dbc.Set<E>().AddRange(entities);
    }

    public virtual void Remove(E entity)
    {
        dbc.Set<E>().Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<E> entities)
    {
        dbc.Set<E>().RemoveRange(entities);
    }

    public virtual async Task<E> GetAsync(K id)
    {
        return await dbc.Set<E>().FindAsync(id) ?? none;
    }

    public virtual async Task<IEnumerable<E>> GetAllAsync()
    {
        return await dbc.Set<E>().ToListAsync();
    }

    public virtual async Task<IEnumerable<E>> FindAsync(Expression<Func<E, bool>> predicate)
    {
        return await dbc.Set<E>().Where(predicate).ToListAsync();
    }

    public virtual Task AddAsync(E entity)
    {
        return dbc.Set<E>().AddAsync(entity).AsTask();
    }

    public virtual Task AddRangeAsync(IEnumerable<E> entities)
    {
        return dbc.Set<E>().AddRangeAsync(entities);
    }
}
