namespace Middle0.Middlewares
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context); // ➤ передаём запрос дальше по цепочке
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Произошла ошибка"); // ➤ логируем ошибку

				// ➤ возвращаем клиенту BadRequest (400)
				context.Response.StatusCode = StatusCodes.Status400BadRequest;
				context.Response.ContentType = "application/json";

				var result = new { error = ex.Message };
				await context.Response.WriteAsJsonAsync(result);
			}
		}
	}
}
