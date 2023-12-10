using System.Net;
using Neuro.Infrastructure.Logging.Exceptions;



namespace Neuro.Application.Exceptions
{
    public class ValidationException : BaseException
    {
        public ValidationException(int code, string? exceptionMessage = null, HttpStatusCode? status = null) : base(ExceptionTypesEnum.Validation, code, exceptionMessage, status)
        {
        }

		public class ValidationExceptions
		{
			public static ValidationException InvalidEmail => new ValidationException(0001, "Geçersiz email");
		}
	}
}
