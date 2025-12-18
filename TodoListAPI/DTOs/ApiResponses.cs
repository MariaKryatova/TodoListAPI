namespace TodoListAPI.DTOs
{
    /// <summary>
    /// Стандартный ответ API
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "Успешно")
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = data };
        }

        public static ApiResponse<T> Fail(string message, List<string>? errors = null)
        {
            return new ApiResponse<T> { Success = false, Message = message, Errors = errors };
        }
    }

    /// <summary>
    /// Ответ API для коллекций
    /// </summary>
    public class ApiCollectionResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiCollectionResponse<T> Ok(IEnumerable<T> data, int totalCount, string message = "Успешно")
        {
            return new ApiCollectionResponse<T> 
            { 
                Success = true, 
                Message = message, 
                Data = data, 
                TotalCount = totalCount 
            };
        }
    }
}