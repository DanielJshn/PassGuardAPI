namespace apief
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;

            await _next(context);

            var endTime = DateTime.UtcNow;
            var processingTime = endTime - startTime;

            _logger.LogInformation($"Request [{context.Request.Method}] {context.Request.Path} took {processingTime.TotalMilliseconds} ms");
        }
    }
}
