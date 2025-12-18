using TodoListAPI.DTOs;

namespace TodoListAPI.Services
{
    /// <summary>
    /// Сервис для работы с задачами
    /// </summary>
    public interface ITodoItemService
    {
        /// <summary>
        /// Получить все задачи с фильтрацией
        /// </summary>
        Task<ApiCollectionResponse<TodoItemDto>> GetAllTodoItemsAsync(TodoItemFilterDto? filter = null);

        /// <summary>
        /// Получить задачу по ID
        /// </summary>
        Task<ApiResponse<TodoItemDto>> GetTodoItemByIdAsync(int id);

        /// <summary>
        /// Создать новую задачу
        /// </summary>
        Task<ApiResponse<TodoItemDto>> CreateTodoItemAsync(CreateTodoItemDto createDto);

        /// <summary>
        /// Обновить задачу
        /// </summary>
        Task<ApiResponse<TodoItemDto>> UpdateTodoItemAsync(int id, UpdateTodoItemDto updateDto);

        /// <summary>
        /// Удалить задачу
        /// </summary>
        Task<ApiResponse<bool>> DeleteTodoItemAsync(int id);

        /// <summary>
        /// Отметить задачу как выполненную
        /// </summary>
        Task<ApiResponse<TodoItemDto>> MarkAsCompletedAsync(int id);

        /// <summary>
        /// Получить статистику по задачам
        /// </summary>
        Task<ApiResponse<object>> GetTodoStatsAsync();
    }
}