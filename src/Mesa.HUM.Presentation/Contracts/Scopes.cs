namespace Mesa.HUM.Presentation.Contracts
{
    using System;

    public sealed class Scopes
    {
        public Scopes ( string name , bool requiresSupporter , string value )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace ( name );
            ArgumentException.ThrowIfNullOrWhiteSpace ( value );

            Name = name;
            RequiresSupporter = requiresSupporter;
            Value = value;
        }

        public string Name { get; }

        public bool RequiresSupporter { get; set; }

        public string Value { get; set; }
    }
}