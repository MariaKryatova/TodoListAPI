using TodoListAPI.Models;

namespace TodoListAPI.Repositories
{
    public interface ITodoItemRepository : IRepository<TodoItem>
    {
        Task<IEnumerable<TodoItem>> GetCompletedAsync();
        Task<IEnumerable<TodoItem>> GetPendingAsync();
        Task<IEnumerable<TodoItem>> GetItemsByCategoryAsync(int categoryId);
        Task<IEnumerable<TodoItem>> GetItemsDueBeforeAsync(DateTime date);
        Task<bool> MarkAsCompletedAsync(int id);
    }
}