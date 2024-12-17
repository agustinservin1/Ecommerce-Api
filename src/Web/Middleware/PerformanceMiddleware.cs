using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Web.Middleware
{
    public class PerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMiddleware> _logger;

        public PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation(
                    "Request {Method} {Path} completed in {ElapsedMilliseconds}ms",
                    context.Request.Method,
                    context.Request.Path,//ruta api
                    sw.ElapsedMilliseconds);//tiempo total transcurrido;
            }
        }
    }
}