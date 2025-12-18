using Microsoft.EntityFrameworkCore;
using TodoListAPI.Data;
using TodoListAPI.Middleware;
using TodoListAPI.Repositories;
using TodoListAPI.Services;
using TodoListAPI.Mappings;
var builder = WebApplication.CreateBuilder(args);

// Конфигурация базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация репозиториев
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();

// Регистрация сервисов
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITodoItemService, TodoItemService>();

// Регистрация AutoMapper - ПРАВИЛЬНЫЙ СПОСОБ
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});
// Контроллеры
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Логирование
builder.Services.AddLogging();

var app = builder.Build();

// Настройка конвейера запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Глобальный обработчик исключений
app.UseExceptionHandlingMiddleware();

app.UseAuthorization();
app.MapControllers();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    dbContext.Database.Migrate();

    SeedData.Initialize(scope.ServiceProvider);
}

app.Run();