using System.Net;
using Neuro.Infrastructure.Logging.Exceptions;



namespace Neuro.Application.Exceptions;

public class ExcelException : BaseException
{
    public ExcelException(int code,ExceptionTypesEnum exceptionTypesEnum ,string? exceptionMessage = null, HttpStatusCode? status = null) 
        : base(exceptionTypesEnum, code, exceptionMessage, status)
    {
    }
}

public static class ExcelExceptions
{
    public static ExcelException ExcelFileMustBeXlsx =>
        new ExcelException(0001,ExceptionTypesEnum.InvalidInput, "Excel file must be .xlsx");
}


