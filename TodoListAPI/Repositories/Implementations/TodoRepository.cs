using Microsoft.EntityFrameworkCore;
using TodoListAPI.Data;
using TodoListAPI.Models;
using TodoListAPI.Repositories.Interfaces;

namespace TodoListAPI.Repositories.Implementations
{
    public class TodoRepository : Repository<TodoItem>, ITodoRepository
    {
        public TodoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TodoItem>> GetCompletedAsync()
        {
            return await _context.TodoItems
                .Where(t => t.IsCompleted)
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetByCategoryAsync(int categoryId)
        {
            return await _context.TodoItems
                .Where(t => t.CategoryId == categoryId)
                .Include(t => t.Category)
                .ToListAsync();
        }
    }
}
