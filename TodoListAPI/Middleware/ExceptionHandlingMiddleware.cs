using System.Net;
using System.Text.Json;
using TodoListAPI.DTOs;

namespace TodoListAPI.Middleware
{
    /// <summary>
    /// Middleware для глобальной обработки исключений
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        /// <summary>
        /// Конструктор middleware обработки исключений
        /// </summary>
        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Метод обработки HTTP-запроса
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Необработанное исключение: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Обработка исключения и формирование ответа
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "Произошла внутренняя ошибка сервера",
                Errors = new List<string>()
            };

            if (_env.IsDevelopment())
            {
                response.Errors.Add($"Исключение: {exception.GetType().Name}");
                response.Errors.Add($"Сообщение: {exception.Message}");

                if (exception.StackTrace != null)
                {
                    response.Errors.Add($"Стек вызовов: {exception.StackTrace}");
                }

                if (exception.InnerException != null)
                {
                    response.Errors.Add($"Внутреннее исключение: {exception.InnerException.Message}");
                }
            }
            else
            {
                response.Message = "Произошла ошибка. Пожалуйста, обратитесь к администратору.";
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _env.IsDevelopment()
            };

            var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
            await context.Response.WriteAsync(jsonResponse);
        }
    }

    /// <summary>
    /// Методы расширения для регистрации middleware
    /// </summary>
    public static class ExceptionHandlingMiddlewareExtensions
    {
        /// <summary>
        /// Регистрация middleware обработки исключений
        /// </summary>
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}