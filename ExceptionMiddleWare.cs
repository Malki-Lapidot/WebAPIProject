namespace MyMiddleWareExceptions;

public class ExceptionMiddleWare{
    private RequestDelegate next;

    public ExceptionMiddleWare(RequestDelegate next)
    {
        this.next = next;        
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try{
            await next.Invoke(httpContext);
            // throw new Exception("error");
        }
        catch(Exception ex){
            Console.WriteLine($"error: {ex.Message}");
        }
    }
}

public static partial class MiddleWareExtensions{
    public static IApplicationBuilder useMyMiddleWareExceptions(this IApplicationBuilder builder){
        return builder.UseMiddleware<ExceptionMiddleWare>();
    }
}
