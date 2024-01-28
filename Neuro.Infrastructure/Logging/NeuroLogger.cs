using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Neuro.Domain.Logging;
using Neuro.Infrastructure.Contexts;
using Neuro.Infrastructure.Logging.Exceptions;
using NLog;

namespace Neuro.Infrastructure.Logging
{
	public class NeuroLogger : INeuroLogger
    {
        //TODO:Exception specification

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemberContext _memberContext;
        private Logger? Logger;

        public NeuroLogger(IHttpContextAccessor httpContextAccessor, IMemberContext memberContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _memberContext = memberContext;
        }

        public async Task<Guid> LogError(Exception exp, [CallerMemberName] string? caller = null)
        {
            Guid? innerExceptionId = null;

            if (exp.InnerException != null)
                innerExceptionId = await LogError(exp.InnerException, caller);

            var logger = await GetLogger();
            var guid = Guid.NewGuid();

            var log = new LogEventInfo();

            if (exp is BaseException baseExp)
            {
                log.Level = baseExp.ExceptionType switch
                {
                    ExceptionTypesEnum.NoContent => LogLevel.Debug,
                    ExceptionTypesEnum.InternalError => LogLevel.Error,
                    ExceptionTypesEnum.Verification => LogLevel.Warn,
                    ExceptionTypesEnum.Authorization => LogLevel.Warn,
                    ExceptionTypesEnum.Validation => LogLevel.Trace,
                    ExceptionTypesEnum.Authentication => LogLevel.Warn,
                    _ => LogLevel.Error
                };

                log.Properties.Add("error-code", baseExp.ErrorCode);
                log.Properties.Add("message", baseExp.DisplayMessage);
            }
            else
            {
                log.Level = LogLevel.Error;
            }

            log.Properties.Add("guid", guid);
            log.Properties.Add("caller", caller);
            log.Properties.Add("exp-message", exp.Message);
            log.Properties.Add("exp-source", exp.Source);
            log.Properties.Add("exp-stacktrace", exp.StackTrace);
            log.Properties.Add("inner-exception-id", innerExceptionId);

            logger.Log(log);

            return guid;
        }

        public async Task<Guid> LogInfo(string message, [CallerMemberName] string? caller = null)
        {
            var logger = await GetLogger();
            var guid = Guid.NewGuid();

            var log = new LogEventInfo();

            log.Level = LogLevel.Info;
            log.Properties.Add("guid", guid);
            log.Properties.Add("caller", caller);
            log.Properties.Add("message", message);

            logger.Log(log);

            return guid;
        }

        public async Task AddProperty(string key, object value)
        {
            Logger = (await GetLogger()).WithProperty(key, value);
        }

        private async Task<Logger> GetLogger()
        {
            if (Logger != null)
                return Logger;

            var neuroLogger = LogManager.GetLogger("default");

            if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null)
                return neuroLogger;
            
            var routeData = _httpContextAccessor.HttpContext.Features.Get<IRouteValuesFeature>();
            
            if (routeData == null)
                return neuroLogger;
            
            var controller = routeData.RouteValues["controller"];
            var action = routeData.RouteValues["action"];
            var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var method = _httpContextAccessor.HttpContext.Request.Method;

            var request = _httpContextAccessor.HttpContext.Request;
            var response = _httpContextAccessor.HttpContext.Response;

            var url = new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port ?? -1, request.Path, request.QueryString.ToString());

            string? reqJson = default;

            if (request.Body.CanRead)
                using (var stream = new StreamReader(request.BodyReader.AsStream()))
                {
                    reqJson = await stream.ReadToEndAsync();
                }

            string? resJson = default;
            if (response.Body.CanRead)
                using (var stream = new StreamReader(response.Body))
                {
                    resJson = await stream.ReadToEndAsync();
                }

            var defaultProperties = new List<KeyValuePair<string, object>>
                {
                    new("controller", controller),
                    new("action", action),
                    new("ip", ip),
                    new("httpmethod", method),
                    new("url", url.ToString()),
                    new("request", reqJson ?? string.Empty),
                    new("response", resJson ?? string.Empty),
                    new("userId", _memberContext.UserId ?? 1),
                    new("email", _memberContext.Email ?? string.Empty),
                    new("username", _memberContext.Username ?? string.Empty),
                    new("deviceId", _memberContext.DeviceId ?? string.Empty),
                    new("version", _memberContext.Version ?? string.Empty),
                };

            neuroLogger = neuroLogger.WithProperties(defaultProperties);

            Logger = neuroLogger;

            return Logger;
        }
    }
}
