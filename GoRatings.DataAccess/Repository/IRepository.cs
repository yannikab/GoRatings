using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace GoRatings.DataAccess.Repository;

public interface IRepository<D, E, K> where D : DbContext where E : class
{
    public E Get(K id);
    public IEnumerable<E> GetAll();
    public IEnumerable<E> Find(Expression<Func<E, bool>> predicate);

    public void Add(E entity);
    public void AddRange(IEnumerable<E> entities);

    public void Remove(E entity);
    public void RemoveRange(IEnumerable<E> entities);


    public Task<E> GetAsync(K id);
    public Task<IEnumerable<E>> GetAllAsync();
    public Task<IEnumerable<E>> FindAsync(Expression<Func<E, bool>> predicate);
    
    public Task AddAsync(E entity);
    public Task AddRangeAsync(IEnumerable<E> entities);
}
