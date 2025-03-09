using System.Diagnostics;

namespace MyLoggerMiddleWare;

public class LoggerMiddleWare{
    private readonly RequestDelegate next;
    private readonly ILogger logger;
    private readonly string logFilePath;
    public LoggerMiddleWare(RequestDelegate next, ILogger<LoggerMiddleWare> logger){
        this.next = next;
        this.logger = logger; 
        logFilePath="logs.txt";
    }
    public async Task Invoke(HttpContext c) {
        var sw = new Stopwatch();
        sw.Start();
        await next.Invoke(c);
        logger.LogDebug($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");
            var requestLog = $"[{DateTime.Now}] Request: {c.Request.Method} {c.Request.Path}\n";
            try
            {
                await File.AppendAllTextAsync(logFilePath, requestLog);

                await next(c);
            }
            catch (Exception ex)
            {
                var errorLog = $"[{DateTime.Now}] Error: {ex.Message}\n{ex.StackTrace}\n";

                await File.AppendAllTextAsync(logFilePath, errorLog);

                throw;
            }
            finally
            {
                var responseLog = $"[{DateTime.Now}] Response: {c.Response.StatusCode}\n\n";
                await File.AppendAllTextAsync(logFilePath, responseLog);
            }
        
    }
}
public static partial class LoggerMiddleWareExtensions{
    public static IApplicationBuilder UseMyLoggerMiddleWare(this IApplicationBuilder builder){
        return builder.UseMiddleware<LoggerMiddleWare>();
    }
}