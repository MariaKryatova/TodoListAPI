using Microsoft.AspNetCore.Mvc;
using TodoListAPI.DTOs;
using TodoListAPI.Services;

namespace TodoListAPI.Controllers
{
    /// <summary>
    /// Контроллер для управления задачами (TodoItems)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly ILogger<TodoItemsController> _logger;

        /// <summary>
        /// Конструктор контроллера задач
        /// </summary>
        public TodoItemsController(
            ITodoItemService todoItemService,
            ILogger<TodoItemsController> logger)
        {
            _todoItemService = todoItemService;
            _logger = logger;
        }

        /// <summary>
        /// Получить все задачи с возможностью фильтрации
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiCollectionResponse<TodoItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] TodoItemFilterDto? filter)
        {
            var result = await _todoItemService.GetAllTodoItemsAsync(filter);
            return Ok(result);
        }

        /// <summary>
        /// Получить задачу по ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TodoItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TodoItemDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _todoItemService.GetTodoItemByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Создать новую задачу
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<TodoItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TodoItemDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTodoItemDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<TodoItemDto>.Fail("Ошибка валидации", errors));
            }

            var result = await _todoItemService.CreateTodoItemAsync(createDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Обновить задачу
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TodoItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TodoItemDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<TodoItemDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoItemDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<TodoItemDto>.Fail("Ошибка валидации", errors));
            }

            var result = await _todoItemService.UpdateTodoItemAsync(id, updateDto);
            if (!result.Success)
            {
                if (result.Message.Contains("не найдена"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _todoItemService.DeleteTodoItemAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Отметить задачу как выполненную
        /// </summary>
        [HttpPatch("{id}/complete")]
        [ProducesResponseType(typeof(ApiResponse<TodoItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TodoItemDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            var result = await _todoItemService.MarkAsCompletedAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Получить статистику по задачам
        /// </summary>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStats()
        {
            var result = await _todoItemService.GetTodoStatsAsync();
            return Ok(result);
        }
    }
}