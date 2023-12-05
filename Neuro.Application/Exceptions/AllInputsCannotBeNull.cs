using Neuro.Application.Base.Exceptions;
using Neuro.Application.Exceptions.Enums;

namespace Neuro.Application.Exceptions;

public class AllInputsCannotBeNull : BaseException
{
    public AllInputsCannotBeNull(string title, string message) : base(title, message)
    {
    }

    public AllInputsCannotBeNull(string title, string message, ExceptionTypesEnum? exceptionType) : base(title, message, exceptionType)
    {
    }

    public AllInputsCannotBeNull(string title, string message, ExceptionTypesEnum? exceptionType, Exception innerException) : base(title, message, exceptionType, innerException)
    {
    }
}