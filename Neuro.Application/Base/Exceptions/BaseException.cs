using Neuro.Application.Exceptions.Enums;

namespace Neuro.Application.Base.Exceptions
{
    public abstract class BaseException : Exception
    {
        public string Title { get; }
        public ExceptionTypesEnum? ExceptionType { get; set; }
        public BaseException(string title, string message) : base(message)
        {
            Title = title;
        }
        protected BaseException(string title,string message,ExceptionTypesEnum? exceptionType)
            : base(message)
        {
            Title = title;
            ExceptionType = exceptionType;
        }
        protected BaseException(string title,string message,ExceptionTypesEnum? exceptionType, Exception innerException)
            : base(message, innerException)
        {
            Title = title;
            ExceptionType = exceptionType;
        }
    }
}
