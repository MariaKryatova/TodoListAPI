using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.DTOs
{
    /// <summary>
    /// DTO для создания новой категории
    /// </summary>
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Название категории обязательно")]
        [MaxLength(100, ErrorMessage = "Название категории не может превышать 100 символов")]
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO для обновления категории
    /// </summary>
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Название категории обязательно")]
        [MaxLength(100, ErrorMessage = "Название категории не может превышать 100 символов")]
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO для отображения категории
    /// </summary>
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TodoItemsCount { get; set; }
    }

    /// <summary>
    /// DTO для отображения категории с задачами
    /// </summary>
    public class CategoryWithItemsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<TodoItemDto> TodoItems { get; set; } = new();
    }
}