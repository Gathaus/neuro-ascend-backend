using NLog;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Neuro.Infrastructure.Logging
{
    public class BasicNeuroLogger
    {
        private static Logger _logger = LogManager.GetLogger("default");

        public static async Task<Guid> LogError(Exception exp, [CallerMemberName] string caller = "")
        {
            var guid = Guid.NewGuid();
            var logEvent = new LogEventInfo(LogLevel.Error, _logger.Name, exp.Message)
            {
                Properties =
                {
                    ["guid"] = guid,
                    ["caller"] = caller,
                    ["exp-message"] = exp.Message,
                    ["exp-source"] = exp.Source,
                    ["exp-stacktrace"] = exp.StackTrace,
                    // Diğer özellikler buraya eklenebilir
                }
            };

            _logger.Log(logEvent);

            // Burada loglama işlemi asenkron bir operasyon olmadığı için Task.FromResult kullanılarak
            // bir Task döndürülebilir. Eğer loglama işleminiz asenkron bir işlem gerektiriyorsa,
            // bu kısmı uygun şekilde değiştirebilirsiniz.
            return await Task.FromResult(guid);
        }

        public static async Task<Guid> LogInfo(string message, [CallerMemberName] string caller = "")
        {
            var guid = Guid.NewGuid();
            var logEvent = new LogEventInfo(LogLevel.Info, _logger.Name, message)
            {
                Properties =
                {
                    ["guid"] = guid,
                    ["caller"] = caller,
                    ["message"] = message,
                }
            };

            _logger.Log(logEvent);

            // Aynı şekilde, Task.FromResult ile bir Task döndürülür
            return await Task.FromResult(guid);
        }

        // Diğer loglama metotları buraya eklenebilir (örneğin, LogWarning, LogDebug vb.)
    }
}