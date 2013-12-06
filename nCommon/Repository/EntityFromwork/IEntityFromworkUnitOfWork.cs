using System.Data.Entity;
using nCommon.Repository.Infrastructure;

namespace nCommon.Repository.EntityFromwork
{
    public interface IEntityFromworkUnitOfWork : IUnitOfWork
    {
        DbSet<T> CreateSet<T>() where T : class;
        void SetModified<T>(T entity) where T : class;
    }
}
