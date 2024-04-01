using DP_backend.Middleware;

namespace DP_backend.Configurators
{
    public static class ExceptionMiddlewareConfigurator
    {
        public static void UseExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
