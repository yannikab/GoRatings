using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using GoRatings.DataAccess.Repository;

namespace GoRatings.DataAccess.UnitOfWork;

public interface IGoRatingsUnitOfWork : IDisposable
{
    IRepositoryEntity Entities { get; }
    IRepositoryProperty Properties { get; }
    IRepositoryRealEstateAgent RealEstateAgents { get; }
    IRepositoryRating Ratings { get; }

    void LoadReference<E, R>(E entity, Expression<Func<E, R?>> propertyExpression) where E : class where R : class;
    void LoadCollection<E, R>(E entity, Expression<Func<E, IEnumerable<R>>> propertyExpression) where E : class where R : class;

    Task LoadReferenceAsync<E, R>(E entity, Expression<Func<E, R?>> propertyExpression) where E : class where R : class;
    Task LoadCollectionAsync<E, R>(E entity, Expression<Func<E, IEnumerable<R>>> propertyExpression) where E : class where R : class;

    int Complete();
}
