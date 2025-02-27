using OrderProcessingSystem.Shared.Constants;
using Serilog;
using System.Net;
using System.Reflection;

namespace OrderProcessingSystem.Server.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                // Handle the exception
                await HandleExceptionAsync(context, ex);
            }
        }

        private async static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("An error occurred. Please try again later.");

            string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
            string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
            string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
            Log.Error(ex, exLocationAndMessage);
            return;
        }

    }
}
