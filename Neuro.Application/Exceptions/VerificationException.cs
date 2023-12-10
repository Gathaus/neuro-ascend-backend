using System.Net;
using Neuro.Infrastructure.Logging.Exceptions;



namespace Neuro.Application.Exceptions
{
    public class VerificationException : BaseException
    {
        public VerificationException(int code, string? exceptionMessage = null, HttpStatusCode? status = null) : base(ExceptionTypesEnum.Verification, code, exceptionMessage, status)
        {
        }
    }

    public class VerificationExceptions
    {
        public static VerificationException BadRequest => new VerificationException(0001, "Hatalı istek");
	}
}
