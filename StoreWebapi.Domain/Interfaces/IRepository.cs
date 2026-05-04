using System.Linq.Expressions;

namespace StoreWebapi.Domain.Interfaces;

public interface IRepository
{
    public IQueryable<T> GetQueryable<T>() where T : class;
    public Task<T> FindById<T>(Guid id, CancellationToken cancellationToken = default) where T : class;
    public Task<T> FindById<T>(string id, CancellationToken cancellationToken = default) where T : class;

    public Task<List<T>> FindAll<T>(CancellationToken cancellationToken = default) where T : class;

    public Task<List<T>> FindAll<T>(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default) where T : class;

    public Task<bool> AnyAsync<T>(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default) where T : class;
    public Task<T?> FindFirstOrDefault<T>(
        Expression<Func<T, bool>> predicate, 
        CancellationToken cancellationToken = default) where T : class;
    #region commands

    void Add<T>(T item) where T : class;
    void Update<T>(T item) where T : class;
    void Remove<T>(T item) where T : class;
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    #endregion
}