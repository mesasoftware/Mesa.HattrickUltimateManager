namespace Mesa.HUM.Domain.Common
{
    using System;

    public class Result<TValue> : Result where TValue : notnull
    {
        protected Result ( TValue value )
            : base ( )
        {
            Value = value;
        }

        protected Result ( Error error ) : base ( error )
        {
        }

        public TValue Value
        {
            get
            {
                return IsSuccess
                    ? field!
                    : throw new InvalidOperationException ( "VALUE_CANNOT_BE_ACCESSED_ON_FAILURE" );
            }
        }

        public static new Result<TValue> Failure ( Error error )
        {
            return new Result<TValue> ( error );
        }

        public static Result<TValue> Success ( TValue value )
        {
            return new Result<TValue> ( value );
        }
    }
}