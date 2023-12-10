using System.Net;
using Neuro.Infrastructure.Logging.Exceptions;



namespace Neuro.Application.Exceptions;

public class InputException : BaseException
{
    public InputException(int code, string? exceptionMessage = null, HttpStatusCode? status = null) 
        : base(ExceptionTypesEnum.InvalidInput, code, exceptionMessage, status)
    {
    }
}

public static class InputExceptions
{
    public static InputException InputCannotBeNegative =>
        new InputException(0001, "Girilen değer negatif olamaz.");

    public static InputException InputCannotBeEmpty =>
        new InputException(0002, "Girilen değer boş olamaz.");

    public static InputException InputCannotBeZero =>
        new InputException(0003, "Girilen değer sıfır olamaz.");

    public static InputException InputCannotBeLessThanZero =>
        new InputException(0004, "Girilen değer sıfırdan küçük olamaz.");
    public static InputException AllInputsCannotBeEmpty =>
        new InputException(0005, "Tüm değerler boş olamaz.");

}
    