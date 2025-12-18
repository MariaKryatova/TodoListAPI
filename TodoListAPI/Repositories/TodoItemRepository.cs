using Microsoft.EntityFrameworkCore;
using TodoListAPI.Data;
using TodoListAPI.Models;

namespace TodoListAPI.Repositories
{
    public class TodoItemRepository : BaseRepository<TodoItem>, ITodoItemRepository
    {
        public TodoItemRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TodoItem>> GetCompletedAsync()
        {
            return await _dbSet
                .Where(t => t.IsCompleted)
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetPendingAsync()
        {
            return await _dbSet
                .Where(t => !t.IsCompleted)
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetItemsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(t => t.CategoryId == categoryId)
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetItemsDueBeforeAsync(DateTime date)
        {
            return await _dbSet
                .Where(t => t.DueDate != null && t.DueDate <= date)
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task<bool> MarkAsCompletedAsync(int id)
        {
            var item = await _dbSet.FindAsync(id);
            if (item == null)
                return false;

            item.IsCompleted = true;
            _dbSet.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public override async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.Category)
                .ToListAsync();
        }

        public override async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public override async Task<IEnumerable<TodoItem>> FindAsync(System.Linq.Expressions.Expression<Func<TodoItem, bool>> predicate)
        {
            return await _dbSet
                .Where(predicate)
                .Include(t => t.Category)
                .ToListAsync();
        }
    }
}