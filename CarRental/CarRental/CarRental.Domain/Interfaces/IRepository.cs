namespace CarRental.Domain.Interfaces;

/// <summary>
/// Обобщённый интерфейс репозитория с поддержкой eager loading
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);

    Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? include = null);
    IQueryable<T> GetQueryable(Func<IQueryable<T>, IQueryable<T>>? include = null);
}
