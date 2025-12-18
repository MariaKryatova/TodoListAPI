using AutoMapper;
using TodoListAPI.DTOs;
using TodoListAPI.Models;

namespace TodoListAPI.Mappings
{
    /// <summary>
    /// Профиль маппинга для AutoMapper
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.TodoItemsCount, 
                    opt => opt.MapFrom(src => src.TodoItems.Count));
            
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            
            CreateMap<Category, CategoryWithItemsDto>();

            CreateMap<TodoItem, TodoItemDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
            
            CreateMap<CreateTodoItemDto, TodoItem>()
                .ForMember(dest => dest.CreatedAt, 
                    opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsCompleted,
                    opt => opt.MapFrom(_ => false));
            
            CreateMap<UpdateTodoItemDto, TodoItem>()
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.Ignore()); 
        }
    }
}