using System.Net;

namespace Neuro.Infrastructure.Logging.Exceptions
{
    public abstract class BaseException : Exception
    {
        public ExceptionTypesEnum ExceptionType { get; init; }
        public int Code { get; init; }
        public string? DisplayMessage { get; init; }
        public HttpStatusCode StatusCode { get; init; }

        public BaseException(ExceptionTypesEnum exceptionType, int code, string? exceptionMessage = null, HttpStatusCode? status = null, Exception? innerException = null) : base(exceptionMessage, innerException)
        {
            ExceptionType = exceptionType;
            Code = code;
            DisplayMessage = Localizer.Instance.Translate($"EXC_{this.GetType().Name}_{code.ToString().PadLeft(4, '0')}") ?? Localizer.Instance.Translate($"EXC_{this.GetType().Name}");

            if (status != null)
                StatusCode = status.Value;
            else
            {
                StatusCode = exceptionType switch
                {
                    ExceptionTypesEnum.Authentication => HttpStatusCode.Unauthorized,
                    ExceptionTypesEnum.Authorization => HttpStatusCode.Forbidden,
                    ExceptionTypesEnum.Validation => HttpStatusCode.BadRequest,
                    ExceptionTypesEnum.Verification => HttpStatusCode.BadRequest,
                    ExceptionTypesEnum.InvalidInput => HttpStatusCode.UnprocessableEntity,
                    ExceptionTypesEnum.NoContent => HttpStatusCode.NoContent,
                    ExceptionTypesEnum.Information => HttpStatusCode.OK,
                    ExceptionTypesEnum.InternalError => HttpStatusCode.InternalServerError,
                    _ => HttpStatusCode.InternalServerError
                };
            }
        }

        public string ErrorCode
        {
            get
            {
                return ((int)ExceptionType).ToString().PadRight(2, '0') + Code.ToString().PadLeft(4, '0');
            }
        }
        
    }
    
    
    public enum ExceptionTypesEnum
    {
        Authentication = 10, //401
        Authorization = 11, //403
        Validation = 12, //400
        Verification = 13, //400
        InvalidInput = 14, //422
        NoContent = 20, //204
        Information = 21, //200

        InternalError = 30, //500
    }
}
