using System.Runtime.CompilerServices;

namespace Neuro.Domain.Logging
{
    public interface INeuroLogger
    {
        Task<Guid> LogError(Exception exp, [CallerMemberName] string? caller = null);
        Task<Guid> LogInfo(string message, [CallerMemberName] string? caller = null);

        Task AddProperty(string key, object value);
    }
}
