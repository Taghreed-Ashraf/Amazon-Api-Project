using Amazon.Core.Entities;
using Amazon.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Reposoitory<TEntity>() where TEntity : BaseEntity;

        Task<int> Complete();
    }
}
