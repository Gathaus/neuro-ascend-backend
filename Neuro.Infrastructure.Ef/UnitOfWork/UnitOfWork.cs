using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Neuro.Domain.Repositories;
using Neuro.Domain.UnitOfWork;
using Neuro.Infrastructure.Ef.Contexts;

namespace Neuro.Infrastructure.Ef.UnitOfWork;

public sealed class UnitOfWork:IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private Dictionary<string, object>? _repositories;
    private readonly IServiceProvider _serviceProvider;
    private IDbContextTransaction? _currentTransaction;

    private bool _disposed = false;   
    public IRepository<T, int> Repository<T>() where T : class
    {
        _repositories ??= new Dictionary<string, object>();

        var type = typeof(T).Name;
        if (!_repositories.ContainsKey(type))
        {
            var repositoryInstance = _serviceProvider.GetRequiredService(typeof(IRepository<T, int>));
            _repositories.Add(type, repositoryInstance);
        }

        return (IRepository<T, int>)_repositories[type];
    }
    public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }


    public async Task<int> SaveChangesAsync()
    {
        if (_currentTransaction != null)
        {
            try
            {
                int result = await _context.SaveChangesAsync();
                await _currentTransaction.CommitAsync();
                return result;
            }
            catch (Exception)
            {
                await _currentTransaction.RollbackAsync();
                throw;
            }
        }
        else
        {
            return await _context.SaveChangesAsync();
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction == null)
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }
        return _currentTransaction;
    }

    public async Task CommitAsync()
    {
        if (_currentTransaction != null)
        {
            await _context.SaveChangesAsync();
            await _currentTransaction.CommitAsync();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
        else
        {
            throw new InvalidOperationException("No transaction is in progress.");
        }
    }

    public async Task RollbackAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
        else
        {
            throw new InvalidOperationException("No transaction is in progress.");
        }
    }

}
