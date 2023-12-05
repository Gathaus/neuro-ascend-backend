using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using Neuro.Application.Base.Exceptions;
using Neuro.Application.Constants;
using Serilog.Context;

namespace Neuro.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var problemDetails = CreateProblemDetails(ex);
                await LogExceptionAsync(context, problemDetails,ex);
                await WriteResponseAsync(context, problemDetails);
            }
        }

        private ProblemDetails CreateProblemDetails(Exception exception)
        {
            if (exception is BaseException appException)
            {
                return new ProblemDetails
                {
                    Status = (int) HttpStatusCode.BadRequest,
                    Title = appException.Title,
                    Detail = appException.Message,
                    Instance = Guid.NewGuid().ToString(),
                    Type = null, //appException.ExceptionType.ToString() will be used as link to guide
                                 //"www.neuro/debug/system-error-codes-5123"
                                 //Example: https://learn.microsoft.com/en-us/windows/win32/debug/system-error-codes--6000-8199-
                };
            }
            
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var problemDetails = new ProblemDetails
            {
                Status = (int) HttpStatusCode.InternalServerError,
                Title = ExceptionMessages.GenericTitle,
                Detail = ExceptionMessages.GenericDetail,
                Instance = Guid.NewGuid().ToString(),
                Type = null, //appException.ExceptionType.ToString() will be used as link to guide
                            //"www.neuro/debug/system-error-codes-5123"
                            //Example: https://learn.microsoft.com/en-us/windows/win32/debug/system-error-codes--6000-8199-
            };
            
            if(environmentName.Equals("Development", StringComparison.OrdinalIgnoreCase))
                problemDetails.Extensions.Add("StackTrace", exception.StackTrace);
            
            
            return problemDetails;
        }

        private Task LogExceptionAsync(HttpContext context, ProblemDetails problemDetails, Exception exception)
        {
            var logLevel = problemDetails.Status == (int) HttpStatusCode.InternalServerError
                ? LogLevel.Error
                : LogLevel.Information;
            var action = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ActionName;
            using (LogContext.PushProperty("Action", action)) 
            using (LogContext.PushProperty("SourceContext", exception.Source)) 
            using (LogContext.PushProperty("Url", context.Request.Path)) 
            using (LogContext.PushProperty("HttpMethod", context.Request.Method)) 
                if (exception is BaseException)
                {
                    _logger.Log(logLevel, exception,
                        "An exception occurred: {Status} - {Title} - {Detail} - {StackTrace} - {Instance} " +
                        "- {Type} ", problemDetails.Status, problemDetails.Title,
                        problemDetails.Detail, "Business exception", problemDetails.Instance, problemDetails.Type);
                }
                else
                {
                    _logger.Log(logLevel, exception,
                        "An exception occurred: {Status} - {Title} - {Detail} - {StackTrace} - {Instance} " +
                        "- {Type} ", problemDetails.Status, problemDetails.Title,
                        problemDetails.Detail, exception.StackTrace, problemDetails.Instance, problemDetails.Type);
                }

            return Task.CompletedTask;
        }

        private Task WriteResponseAsync(HttpContext context, ProblemDetails problemDetails)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problemDetails.Status.Value;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
        }
    }
}