using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models;

namespace TodoListAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.Categories.Any())
                {
                    return;
                }

                var categories = new Category[]
                {
                    new Category { Name = "Работа" },
                    new Category { Name = "Дом" },
                    new Category { Name = "Учеба" },
                    new Category { Name = "Здоровье" },
                    new Category { Name = "Развлечения" }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();

                var todoItems = new TodoItem[]
                {
                    new TodoItem
                    {
                        Title = "Написать отчет",
                        Description = "Подготовить квартальный отчет",
                        CategoryId = 1,
                        DueDate = DateTime.Now.AddDays(2)
                    },
                    new TodoItem
                    {
                        Title = "Купить продукты",
                        Description = "Молоко, хлеб, яйца",
                        CategoryId = 2, 
                        IsCompleted = true 
                    },
                    new TodoItem
                    {
                        Title = "Сделать домашнее задание",
                        Description = "Математика, страницы 45-50",
                        CategoryId = 3, 
                        DueDate = DateTime.Now.AddDays(1)
                    },
                    new TodoItem
                    {
                        Title = "Сходить на пробежку",
                        Description = "30 минут бега",
                        CategoryId = 4,
                        DueDate = DateTime.Now.AddDays(3)
                    },
                    new TodoItem
                    {
                        Title = "Посмотреть фильм",
                        Description = "Новый фильм Marvel",
                        CategoryId = 5 
                    }
                };

                context.TodoItems.AddRange(todoItems);
                context.SaveChanges();
            }
        }
    }
}
