using Microsoft.EntityFrameworkCore.Storage;
using Neuro.Domain.Repositories;

namespace Neuro.Domain.UnitOfWork;

public interface IUnitOfWork
{
    public IRepository<T, int> Repository<T>() where T : class;

    public Task<IDbContextTransaction> BeginTransactionAsync();

    public Task CommitAsync();

    public Task RollbackAsync();
        
    public Task<int> SaveChangesAsync();
}