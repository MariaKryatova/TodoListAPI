using System.Linq.Expressions;

namespace TodoListAPI.Repositories
{
    /// <summary>
    /// Базовый интерфейс репозитория для работы с сущностями
    /// </summary>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Получить все записи сущности
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Получить запись по идентификатору
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Найти записи по условию
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Добавить новую запись
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Обновить существующую запись
        /// </summary>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Удалить запись по идентификатору
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}