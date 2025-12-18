using TodoListAPI.DTOs;

namespace TodoListAPI.Services
{
    /// <summary>
    /// Сервис для работы с категориями задач
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Получить все категории
        /// </summary>
        Task<ApiCollectionResponse<CategoryDto>> GetAllCategoriesAsync();

        /// <summary>
        /// Получить категорию по ID
        /// </summary>
        Task<ApiResponse<CategoryWithItemsDto>> GetCategoryByIdAsync(int id);

        /// <summary>
        /// Создать новую категорию
        /// </summary>
        Task<ApiResponse<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createDto);

        /// <summary>
        /// Обновить категорию
        /// </summary>
        Task<ApiResponse<CategoryDto>> UpdateCategoryAsync(int id, UpdateCategoryDto updateDto);

        /// <summary>
        /// Удалить категорию
        /// </summary>
        Task<ApiResponse<bool>> DeleteCategoryAsync(int id);

        /// <summary>
        /// Получить статистику по категориям
        /// </summary>
        Task<ApiResponse<object>> GetCategoryStatsAsync();
    }
}