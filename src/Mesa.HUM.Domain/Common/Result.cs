namespace Mesa.HUM.Domain.Common
{
    public class Result
    {
        /// <summary>
        /// Creates a IsSuccess result.
        /// </summary>
        protected Result ( )
        {
            IsSuccess = true;
        }

        /// <summary>
        /// Creates a Failure result.
        /// </summary>
        /// <param name="error"></param>
        protected Result ( Error error )
        {
            IsSuccess = false;
            Error = error;
        }

        public Error? Error { get; }

        public bool IsFailure
        {
            get
            {
                return !IsSuccess;
            }
        }

        public bool IsSuccess { get; }

        public static Result Failure ( Error error )
        {
            return new Result ( error );
        }

        public static Result Success ( )
        {
            return new Result ( );
        }
    }
}