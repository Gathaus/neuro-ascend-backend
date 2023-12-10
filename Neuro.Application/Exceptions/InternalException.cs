using System.Net;
using Neuro.Infrastructure.Logging.Exceptions;



namespace Neuro.Application.Exceptions
{
    public class InternalException : BaseException
    {
        public InternalException(int code, string? exceptionMessage = null, HttpStatusCode? status = null, Exception? innerException = null) : base(ExceptionTypesEnum.InternalError, code, exceptionMessage, status, innerException)
        {
        }
    }

    public class InternalExceptions
    {
		public static AuthorizationException UserNotAuthorizedException => new AuthorizationException(0001, "Kullanıcı isSoUser ve isSuperUser değil ya da salespointin hierarchy'inde değil");

		public static InternalException Unknown(Exception? innerException = null) => new InternalException(0000, "Kategorize edilmemiş hata!", innerException: innerException);
        public static InternalException OutOfMemory(Exception? innerException = null) => new InternalException(0001, "OutOfMemory Exception.", innerException: innerException);
    }
}
