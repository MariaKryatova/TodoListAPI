using Microsoft.AspNetCore.Mvc;
using TodoListAPI.Services;

namespace TodoListAPI.Controllers
{
    /// <summary>
    /// Контроллер для получения статистики
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ITodoItemService _todoItemService;

        /// <summary>
        /// Конструктор контроллера статистики
        /// </summary>
        public StatsController(
            ICategoryService categoryService,
            ITodoItemService todoItemService)
        {
            _categoryService = categoryService;
            _todoItemService = todoItemService;
        }

        /// <summary>
        /// Получить общую статистику
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetOverallStats()
        {
            var categoryStats = await _categoryService.GetCategoryStatsAsync();
            var todoStats = await _todoItemService.GetTodoStatsAsync();

            var result = new
            {
                CategoryStats = categoryStats.Data,
                TodoStats = todoStats.Data
            };

            return Ok(new
            {
                Success = true,
                Message = "Общая статистика",
                Data = result
            });
        }
    }
}