using System.Net;
using Neuro.Infrastructure.Logging.Exceptions;

namespace Neuro.Application.Exceptions
{
    public class AuthenticationException : BaseException
    {
        public AuthenticationException(int code, string? exceptionMessage = null, HttpStatusCode? status = null) : base(ExceptionTypesEnum.Authentication, code, exceptionMessage, status)
        {
        }
    }

    public class AuthenticationExceptions
    {
        public static AuthenticationException UsernameIsWrongException => new AuthenticationException(0001, "Username hatalı.");
        public static AuthenticationException PasswordIsWrongException => new AuthenticationException(0002, "Password yanlış.");
        public static AuthenticationException UserInvalidException => new AuthenticationException(0003, "Kullanıcı login olma kurallarını karşılamıyor.");
    }
}
