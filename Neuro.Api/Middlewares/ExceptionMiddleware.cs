using System.Diagnostics;
using Newtonsoft.Json;
using Neuro.Application.Exceptions;
using Neuro.Domain.Logging;
using Neuro.Infrastructure.Logging.Exceptions;

namespace Neuro.Api.Middlewares
{
	public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, INeuroLogger remLogger)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {

                var stackTrace = new StackTrace(ex);
                var frame = stackTrace.GetFrames().FirstOrDefault(sf => sf.GetMethod()?.DeclaringType?.FullName?.StartsWith("NeuroInstore.Mobile") ?? false);

                var caller = $"{frame?.GetMethod()?.DeclaringType?.FullName}.{frame?.GetMethod()?.Name}";

                BaseException exception;

                if (ex is BaseException)
                {
                    exception = (BaseException) ex;
                }
                else
                {
                    exception = DetectException(ex);
                }

                _ = await remLogger.LogError(exception, caller);
                httpContext.Response.StatusCode = (int) exception.StatusCode;
                await WriteResponseAsync(httpContext,exception);
            }
        }

        private BaseException DetectException(Exception ex)
        {
            return ex switch
            {
                OutOfMemoryException => InternalExceptions.OutOfMemory(ex),
                _ => InternalExceptions.Unknown(ex)
            };
        }
        private Task WriteResponseAsync(HttpContext context, BaseException exp)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)exp.StatusCode;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(exp));
        }
    }
}
