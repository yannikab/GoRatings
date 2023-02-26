using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using GoRatings.DataAccess.Models;
using GoRatings.DataAccess.Repository;

namespace GoRatings.DataAccess.UnitOfWork;

public class GoRatingsUnitOfWork : IGoRatingsUnitOfWork
{
    private readonly GoRatingsContext dbc;

    public GoRatingsUnitOfWork()
    {
        this.dbc = new GoRatingsContext();

        Entities = new RepositoryEntity(dbc);
        Properties = new RepositoryProperty(dbc);
        RealEstateAgents = new RepositoryRealEstateAgent(dbc);
        Ratings = new RepositoryRating(dbc);
    }

    public IRepositoryEntity Entities { get; }
    public IRepositoryProperty Properties { get; }
    public IRepositoryRealEstateAgent RealEstateAgents { get; }
    public IRepositoryRating Ratings { get; }

    public int Complete()
    {
        return dbc.SaveChanges();
    }

    public void Dispose()
    {
        dbc.Dispose();
        GC.SuppressFinalize(this);
    }

    public void LoadReference<E, R>(E entity, Expression<Func<E, R?>> propertyExpression) where E : class where R : class
    {
        dbc.Entry<E>(entity).Reference<R>(propertyExpression).Load();
    }

    public void LoadCollection<E, R>(E entity, Expression<Func<E, IEnumerable<R>>> propertyExpression) where E : class where R : class
    {
        dbc.Entry<E>(entity).Collection<R>(propertyExpression).Load();
    }

    public Task LoadReferenceAsync<E, R>(E entity, Expression<Func<E, R?>> propertyExpression) where E : class where R : class
    {
        return dbc.Entry<E>(entity).Reference<R>(propertyExpression).LoadAsync();
    }

    public Task LoadCollectionAsync<E, R>(E entity, Expression<Func<E, IEnumerable<R>>> propertyExpression) where E : class where R : class
    {
        return dbc.Entry<E>(entity).Collection<R>(propertyExpression).LoadAsync();
    }
}
