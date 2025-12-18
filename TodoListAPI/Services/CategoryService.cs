using AutoMapper;
using TodoListAPI.DTOs;
using TodoListAPI.Models;
using TodoListAPI.Repositories;

namespace TodoListAPI.Services
{
    /// <summary>
    /// Реализация сервиса для работы с категориями
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        /// <summary>
        /// Конструктор сервиса категорий
        /// </summary>
        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Получить все категории
        /// </summary>
        public async Task<ApiCollectionResponse<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Получение всех категорий");

                var categories = await _categoryRepository.GetAllAsync();
                var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

                return ApiCollectionResponse<CategoryDto>.Ok(categoryDtos, categoryDtos.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении категорий");
                throw;
            }
        }

        /// <summary>
        /// Получить категорию по ID
        /// </summary>
        public async Task<ApiResponse<CategoryWithItemsDto>> GetCategoryByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Получение категории с ID: {CategoryId}", id);

                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return ApiResponse<CategoryWithItemsDto>.Fail($"Категория с ID {id} не найдена");
                }

                var categoryDto = _mapper.Map<CategoryWithItemsDto>(category);
                return ApiResponse<CategoryWithItemsDto>.Ok(categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении категории с ID: {CategoryId}", id);
                throw;
            }
        }

        /// <summary>
        /// Создать новую категорию
        /// </summary>
        public async Task<ApiResponse<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createDto)
        {
            try
            {
                _logger.LogInformation("Создание новой категории: {CategoryName}", createDto.Name);

                var existingCategory = await _categoryRepository.GetByNameAsync(createDto.Name);
                if (existingCategory != null)
                {
                    return ApiResponse<CategoryDto>.Fail($"Категория с именем '{createDto.Name}' уже существует");
                }

                var category = _mapper.Map<Category>(createDto);
                var createdCategory = await _categoryRepository.AddAsync(category);
                var categoryDto = _mapper.Map<CategoryDto>(createdCategory);

                _logger.LogInformation("Категория создана с ID: {CategoryId}", createdCategory.Id);
                return ApiResponse<CategoryDto>.Ok(categoryDto, "Категория успешно создана");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании категории: {CategoryName}", createDto.Name);
                throw;
            }
        }

        /// <summary>
        /// Обновить категорию
        /// </summary>
        public async Task<ApiResponse<CategoryDto>> UpdateCategoryAsync(int id, UpdateCategoryDto updateDto)
        {
            try
            {
                _logger.LogInformation("Обновление категории с ID: {CategoryId}", id);

                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return ApiResponse<CategoryDto>.Fail($"Категория с ID {id} не найдена");
                }

                if (category.Name != updateDto.Name)
                {
                    var existingCategory = await _categoryRepository.GetByNameAsync(updateDto.Name);
                    if (existingCategory != null && existingCategory.Id != id)
                    {
                        return ApiResponse<CategoryDto>.Fail($"Категория с именем '{updateDto.Name}' уже существует");
                    }
                }

                _mapper.Map(updateDto, category);
                var updatedCategory = await _categoryRepository.UpdateAsync(category);
                var categoryDto = _mapper.Map<CategoryDto>(updatedCategory);

                _logger.LogInformation("Категория с ID: {CategoryId} обновлена", id);
                return ApiResponse<CategoryDto>.Ok(categoryDto, "Категория успешно обновлена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении категории с ID: {CategoryId}", id);
                throw;
            }
        }

        /// <summary>
        /// Удалить категорию
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                _logger.LogInformation("Удаление категории с ID: {CategoryId}", id);

                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return ApiResponse<bool>.Fail($"Категория с ID {id} не найдена");
                }

                if (category.TodoItems != null && category.TodoItems.Any())
                {
                    return ApiResponse<bool>.Fail("Невозможно удалить категорию, так как к ней привязаны задачи");
                }

                var result = await _categoryRepository.DeleteAsync(id);
                if (!result)
                {
                    return ApiResponse<bool>.Fail($"Не удалось удалить категорию с ID {id}");
                }

                _logger.LogInformation("Категория с ID: {CategoryId} удалена", id);
                return ApiResponse<bool>.Ok(true, "Категория успешно удалена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении категории с ID: {CategoryId}", id);
                throw;
            }
        }

        /// <summary>
        /// Получить статистику по категориям
        /// </summary>
        public async Task<ApiResponse<object>> GetCategoryStatsAsync()
        {
            try
            {
                _logger.LogInformation("Получение статистики по категориям");

                var categories = await _categoryRepository.GetCategoriesWithTodoItemsAsync();

                var stats = new
                {
                    TotalCategories = categories.Count(),
                    TotalTodoItems = categories.Sum(c => c.TodoItems?.Count ?? 0),
                    Categories = categories.Select(c => new
                    {
                        CategoryId = c.Id,
                        CategoryName = c.Name,
                        TodoItemsCount = c.TodoItems?.Count ?? 0,
                        CompletedItems = c.TodoItems?.Count(t => t.IsCompleted) ?? 0,
                        PendingItems = c.TodoItems?.Count(t => !t.IsCompleted) ?? 0
                    }).OrderByDescending(x => x.TodoItemsCount)
                };

                return ApiResponse<object>.Ok(stats, "Статистика по категориям");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении статистики по категориям");
                throw;
            }
        }
    }
}