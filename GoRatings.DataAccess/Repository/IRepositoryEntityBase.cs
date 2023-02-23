using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoRatings.DataAccess.Models;

namespace GoRatings.DataAccess.Repository;

public interface IRepositoryEntityBase : IRepository<GoRatingsContext, EntityBase, long>
{
}
