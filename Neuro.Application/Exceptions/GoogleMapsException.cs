using System.Net;
using Neuro.Infrastructure.Logging.Exceptions;



namespace Neuro.Application.Exceptions;

public class GoogleMapsException : BaseException
{
    public GoogleMapsException(int code, string? exceptionMessage = null, HttpStatusCode? status = null) 
        : base(ExceptionTypesEnum.InvalidInput, code, exceptionMessage, status)
    {
    }

    public static class GoogleMapsExceptions
    {
        public static GoogleMapsException BothArgumentsCannotBeNull => new GoogleMapsException(0001, "Both Longitude latitude and address information cannot be filled, please just use one of them");
        public static GoogleMapsException AllArgumentsCannotBeNull => new GoogleMapsException(0002, "All inputs cannot be null, please fill inputs and try again.");
    }
}