namespace Domain.Common
{
    public class Result<T>
    {
        public T Data { get; }
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        protected Result(T data, bool isSuccess, string errorMessage)
        {
            Data = data;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
        public static Result<T> Success(T data) => new(data, true, null!);

        public static Result<T> Failure(string errorMessage)
        {
            return new Result<T>(default!, false, errorMessage);
        }
    }
}
