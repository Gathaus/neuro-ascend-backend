using System.Net;
using Neuro.Infrastructure.Logging.Exceptions;



namespace Neuro.Application.Exceptions
{
    public class InformationException : BaseException
    {
        public InformationException(ExceptionTypesEnum category, int code, string? exceptionMessage = null, HttpStatusCode? status = null) : base(category, code, exceptionMessage, status)
        {
        }
    }
}
