using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.DTOs
{
    /// <summary>
    /// DTO для создания новой задачи
    /// </summary>
    public class CreateTodoItemDto
    {
        [Required(ErrorMessage = "Заголовок задачи обязателен")]
        [MaxLength(200, ErrorMessage = "Заголовок не может превышать 200 символов")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "Описание не может превышать 1000 символов")]
        public string Description { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }
        public int? CategoryId { get; set; }
    }

    /// <summary>
    /// DTO для обновления задачи
    /// </summary>
    public class UpdateTodoItemDto
    {
        [Required(ErrorMessage = "Заголовок задачи обязателен")]
        [MaxLength(200, ErrorMessage = "Заголовок не может превышать 200 символов")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "Описание не может превышать 1000 символов")]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CategoryId { get; set; }
    }

    /// <summary>
    /// DTO для отображения задачи
    /// </summary>
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }

    /// <summary>
    /// DTO для фильтрации задач
    /// </summary>
    public class TodoItemFilterDto
    {
        public bool? IsCompleted { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public string? SearchTerm { get; set; }
    }
}