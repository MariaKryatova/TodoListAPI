using Microsoft.EntityFrameworkCore;
using TodoListAPI.Data;
using TodoListAPI.Models;

namespace TodoListAPI.Repositories
{

    /// <summary>
    /// Репозиторий для работы с категориями задач
    /// </summary>
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        /// <summary>
        /// Конструктор репозитория категорий
        /// </summary>
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Получить категорию по названию
        /// </summary>
        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Name == name);
        }

        /// <summary>
        /// Получить все категории с задачами
        /// </summary>
        public async Task<IEnumerable<Category>> GetCategoriesWithTodoItemsAsync()
        {
            return await _dbSet
                .Include(c => c.TodoItems)
                .ToListAsync();
        }

        /// <summary>
        /// Получить все категории
        /// </summary>
        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.TodoItems)
                .ToListAsync();
        }

        /// <summary>
        /// Получить категорию по идентификатору
        /// </summary>
        public override async Task<Category?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.TodoItems)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}