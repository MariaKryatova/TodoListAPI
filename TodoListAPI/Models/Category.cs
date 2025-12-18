using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // Инициализация по умолчанию

        public virtual ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }
}