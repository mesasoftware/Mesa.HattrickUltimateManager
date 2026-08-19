namespace Mesa.HUM.Domain.Profiles.ValueObjects
{
    using System;

    public readonly record struct ProfileId
    {
        public ProfileId ( Guid value )
        {
            if ( Guid.Empty.Equals ( value ) )
            {
                throw new ArgumentException ( "VALUE_CANNOT_BE_GUID_EMPTY" );
            }

            Value = value;
        }

        public Guid Value { get; }

        public static ProfileId New ( )
        {
            return new ProfileId ( Guid.CreateVersion7 ( ) );
        }

        public override string ToString ( )
        {
            return Value.ToString ( );
        }
    }
}