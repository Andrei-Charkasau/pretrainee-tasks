using Task_4_1_Library_ControlSystem.Middleware;

namespace Task_4_1_Library_ControlSystem.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>(); //Запомни - для работы с GEH !!!!!!
        }
    }
}