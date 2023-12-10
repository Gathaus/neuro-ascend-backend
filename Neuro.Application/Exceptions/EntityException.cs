using System.Net;
using Neuro.Infrastructure.Logging.Exceptions;



namespace Neuro.Application.Exceptions;

public class EntityException : BaseException
{
    public EntityException(int code, string? exceptionMessage = null, HttpStatusCode? status = null) 
        : base(ExceptionTypesEnum.InvalidInput, code, exceptionMessage, status)
    {
    }

    public static class EntityExceptions
    {
        public static EntityException EntityNotFound =>
            new EntityException(0001, "Girilen data bulunamadı lütfen datanızı kontrol ediniz.");

    }
}