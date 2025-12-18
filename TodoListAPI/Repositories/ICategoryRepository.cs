using TodoListAPI.Models;

namespace TodoListAPI.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы с категориями задач
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Получить категорию по названию
        /// </summary>
        Task<Category?> GetByNameAsync(string name);

        /// <summary>
        /// Получить все категории с привязанными задачами
        /// </summary>
        Task<IEnumerable<Category>> GetCategoriesWithTodoItemsAsync();
    }
}