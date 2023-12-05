namespace Neuro.Application.Base.Result
{
    public class FilterResult<T> : BaseResult
    {
        public T DataGroup { get; private set; }

        protected FilterResult(T dataGroup, bool isSuccess, string errorMessage)
        {
            DataGroup = dataGroup;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static FilterResult<T> Success(T dataGroup)
        {
            return new FilterResult<T>(dataGroup, true, null);
        }

        public static FilterResult<T> Fail(string errorMessage)
        {
            return new FilterResult<T>(default, false, errorMessage);
        }
    }

}