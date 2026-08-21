namespace Mesa.HUM.Domain.Common
{
    using Mesa.HUM.Domain.Common.Enums;

    public record Error
    {
        private Error ( string code , string description , ErrorType type )
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public string Code { get; }

        public string Description { get; }

        public ErrorType Type { get; }

        public static Error Failure ( string code , string description )
        {
            return new Error ( code , description , ErrorType.Failure );
        }

        public static Error NotFound ( string code , string description )
        {
            return new Error ( code , description , ErrorType.NotFound );
        }

        public static Error Problem ( string code , string description )
        {
            return new Error ( code , description , ErrorType.Problem );
        }

        public static Error Conflict ( string code , string description )
        {
            return new Error ( code , description , ErrorType.Conflict );
        }

        public static Error Validation ( string code , string description )
        {
            return new Error ( code , description , ErrorType.Validation );
        }
    }
}