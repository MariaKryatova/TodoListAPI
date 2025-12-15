using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListAPI.Models;

namespace TodoListAPI.Repositories.Interfaces
{
    public interface ITodoRepository : IRepository<TodoItem>
    {
        Task<IEnumerable<TodoItem>> GetCompletedAsync();
        Task<IEnumerable<TodoItem>> GetByCategoryAsync(int categoryId);
    }
}
