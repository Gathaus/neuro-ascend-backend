namespace Neuro.Application.Base.Result
{
    public class Result :BaseResult
    {

        protected Result(bool isSuccess, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success()
        {
            return new Result(true);
        }

        public static Result Fail(string errorMessage)
        {
            return new Result(false, errorMessage);
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; private set; }

        public Result(T data, bool isSuccess, string errorMessage = null) : base(isSuccess, errorMessage)
        {
            Data = data;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(data, true);
        }

        public new static Result<T> Fail(string errorMessage)
        {
            return new Result<T>(default, false, errorMessage);
        }
    }
}