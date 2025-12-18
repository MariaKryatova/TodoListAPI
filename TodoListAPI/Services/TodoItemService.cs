using AutoMapper;
using TodoListAPI.DTOs;
using TodoListAPI.Models;
using TodoListAPI.Repositories;

namespace TodoListAPI.Services
{
    /// <summary>
    /// Реализация сервиса для работы с задачами
    /// </summary>
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TodoItemService> _logger;

        /// <summary>
        /// Конструктор сервиса задач
        /// </summary>
        public TodoItemService(
           ITodoItemRepository todoItemRepository,
           ICategoryRepository categoryRepository,
           IMapper mapper,
           ILogger<TodoItemService> logger)
        {
            _todoItemRepository = todoItemRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Получить все задачи с фильтрацией
        /// </summary>
        public async Task<ApiCollectionResponse<TodoItemDto>> GetAllTodoItemsAsync(TodoItemFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Получение задач с фильтрацией {@Filter}", filter);

                var todoItems = await _todoItemRepository.GetAllAsync();

                if (filter != null)
                {
                    todoItems = ApplyFilters(todoItems, filter);
                }

                var todoItemDtos = _mapper.Map<List<TodoItemDto>>(todoItems);
                return ApiCollectionResponse<TodoItemDto>.Ok(todoItemDtos, todoItemDtos.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении задач с фильтрацией {@Filter}", filter);
                throw;
            }
        }

        /// <summary>
        /// Получить задачу по ID
        /// </summary>
        public async Task<ApiResponse<TodoItemDto>> GetTodoItemByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Получение задачи с ID: {TodoItemId}", id);

                var todoItem = await _todoItemRepository.GetByIdAsync(id);
                if (todoItem == null)
                {
                    return ApiResponse<TodoItemDto>.Fail($"Задача с ID {id} не найдена");
                }

                var todoItemDto = _mapper.Map<TodoItemDto>(todoItem);
                return ApiResponse<TodoItemDto>.Ok(todoItemDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении задачи с ID: {TodoItemId}", id);
                throw;
            }
        }

        /// <summary>
        /// Создать новую задачу
        /// </summary>
        public async Task<ApiResponse<TodoItemDto>> CreateTodoItemAsync(CreateTodoItemDto createDto)
        {
            try
            {
                _logger.LogInformation("Создание новой задачи: {TodoItemTitle}", createDto.Title);

                if (createDto.CategoryId.HasValue)
                {
                    var category = await _categoryRepository.GetByIdAsync(createDto.CategoryId.Value);
                    if (category == null)
                    {
                        return ApiResponse<TodoItemDto>.Fail($"Категория с ID {createDto.CategoryId} не найдена");
                    }
                }

                var todoItem = _mapper.Map<TodoItem>(createDto);
                var createdTodoItem = await _todoItemRepository.AddAsync(todoItem);
                var todoItemDto = _mapper.Map<TodoItemDto>(createdTodoItem);

                _logger.LogInformation("Задача создана с ID: {TodoItemId}", createdTodoItem.Id);
                return ApiResponse<TodoItemDto>.Ok(todoItemDto, "Задача успешно создана");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании задачи: {TodoItemTitle}", createDto.Title);
                throw;
            }
        }

        /// <summary>
        /// Обновить задачу
        /// </summary>
        public async Task<ApiResponse<TodoItemDto>> UpdateTodoItemAsync(int id, UpdateTodoItemDto updateDto)
        {
            try
            {
                _logger.LogInformation("Обновление задачи с ID: {TodoItemId}", id);

                var todoItem = await _todoItemRepository.GetByIdAsync(id);
                if (todoItem == null)
                {
                    return ApiResponse<TodoItemDto>.Fail($"Задача с ID {id} не найдена");
                }

                if (updateDto.CategoryId.HasValue && updateDto.CategoryId != todoItem.CategoryId)
                {
                    var category = await _categoryRepository.GetByIdAsync(updateDto.CategoryId.Value);
                    if (category == null)
                    {
                        return ApiResponse<TodoItemDto>.Fail($"Категория с ID {updateDto.CategoryId} не найдена");
                    }
                }

                _mapper.Map(updateDto, todoItem);
                var updatedTodoItem = await _todoItemRepository.UpdateAsync(todoItem);
                var todoItemDto = _mapper.Map<TodoItemDto>(updatedTodoItem);

                _logger.LogInformation("Задача с ID: {TodoItemId} обновлена", id);
                return ApiResponse<TodoItemDto>.Ok(todoItemDto, "Задача успешно обновлена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении задачи с ID: {TodoItemId}", id);
                throw;
            }
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteTodoItemAsync(int id)
        {
            try
            {
                _logger.LogInformation("Удаление задачи с ID: {TodoItemId}", id);

                var result = await _todoItemRepository.DeleteAsync(id);
                if (!result)
                {
                    return ApiResponse<bool>.Fail($"Задача с ID {id} не найдена");
                }

                _logger.LogInformation("Задача с ID: {TodoItemId} удалена", id);
                return ApiResponse<bool>.Ok(true, "Задача успешно удалена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении задачи с ID: {TodoItemId}", id);
                throw;
            }
        }

        /// <summary>
        /// Отметить задачу как выполненную
        /// </summary>
        public async Task<ApiResponse<TodoItemDto>> MarkAsCompletedAsync(int id)
        {
            try
            {
                _logger.LogInformation("Отметка задачи как выполненной с ID: {TodoItemId}", id);

                var result = await _todoItemRepository.MarkAsCompletedAsync(id);
                if (!result)
                {
                    return ApiResponse<TodoItemDto>.Fail($"Задача с ID {id} не найдена");
                }

                var todoItem = await _todoItemRepository.GetByIdAsync(id);
                var todoItemDto = _mapper.Map<TodoItemDto>(todoItem);

                _logger.LogInformation("Задача с ID: {TodoItemId} отмечена как выполненная", id);
                return ApiResponse<TodoItemDto>.Ok(todoItemDto, "Задача отмечена как выполненная");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отметке задачи как выполненной с ID: {TodoItemId}", id);
                throw;
            }
        }

        /// <summary>
        /// Получить статистику по задачам
        /// </summary>
        public async Task<ApiResponse<object>> GetTodoStatsAsync()
        {
            try
            {
                _logger.LogInformation("Получение статистики по задачам");

                var allItems = await _todoItemRepository.GetAllAsync();
                var completedItems = await _todoItemRepository.GetCompletedAsync();
                var pendingItems = await _todoItemRepository.GetPendingAsync();

                var stats = new
                {
                    TotalTasks = allItems.Count(),
                    CompletedTasks = completedItems.Count(),
                    PendingTasks = pendingItems.Count(),
                    CompletionRate = allItems.Count() > 0 ?
                        (double)completedItems.Count() / allItems.Count() * 100 : 0,
                    TasksByCategory = allItems
                        .GroupBy(t => t.Category?.Name ?? "Без категории")
                        .Select(g => new
                        {
                            Category = g.Key,
                            Count = g.Count(),
                            Completed = g.Count(t => t.IsCompleted)
                        }).OrderByDescending(x => x.Count),
                    OverdueTasks = allItems
                        .Count(t => t.DueDate.HasValue && t.DueDate < DateTime.UtcNow && !t.IsCompleted)
                };

                return ApiResponse<object>.Ok(stats, "Статистика по задачам");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении статистики по задачам");
                throw;
            }
        }

        /// <summary>
        /// Применить фильтры к списку задач
        /// </summary>
        private IEnumerable<TodoItem> ApplyFilters(IEnumerable<TodoItem> todoItems, TodoItemFilterDto filter)
        {
            if (filter.IsCompleted.HasValue)
            {
                todoItems = todoItems.Where(t => t.IsCompleted == filter.IsCompleted.Value);
            }

            if (filter.CategoryId.HasValue)
            {
                todoItems = todoItems.Where(t => t.CategoryId == filter.CategoryId.Value);
            }

            if (filter.DueDateFrom.HasValue)
            {
                todoItems = todoItems.Where(t => t.DueDate >= filter.DueDateFrom.Value);
            }

            if (filter.DueDateTo.HasValue)
            {
                todoItems = todoItems.Where(t => t.DueDate <= filter.DueDateTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                todoItems = todoItems.Where(t =>
                    t.Title.ToLower().Contains(searchTerm) ||
                    t.Description.ToLower().Contains(searchTerm));
            }

            return todoItems;
        }
    }
}