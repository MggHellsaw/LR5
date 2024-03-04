using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LR5_.Services
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorLoggingMiddleware> _logger;

        public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogErrorToFile(ex, context);
                throw;
            }

        }

        private void LogErrorToFile(Exception ex, HttpContext context)
        {

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string logFilePath = Path.Combine(appDirectory, "Logs", "error_log.txt");

            string errorMessage = $"[{DateTime.Now}] An error occurred: {ex.Message}\n";
            errorMessage += $"Requested URL: {context.Request.Path}\n";
            errorMessage += $"Stack Trace: {ex.StackTrace}\n\n";

            File.AppendAllText(logFilePath, errorMessage);

            _logger.LogError(ex, errorMessage);
        }
    }
}
