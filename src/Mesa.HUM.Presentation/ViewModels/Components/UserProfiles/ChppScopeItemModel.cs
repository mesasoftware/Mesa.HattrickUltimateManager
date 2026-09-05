namespace Mesa.HUM.Presentation.ViewModels.Components.UserProfiles
{
    using System;
    using Mesa.HUM.Domain.Profiles.Enums;
    using Mesa.HUM.Presentation.Abstractions;

    public class ChppScopeItemModel : ObservableComponent
    {
        public ChppScopeItemModel (
            string name ,
            bool requiresSupporter ,
            string value )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace ( name );
            ArgumentException.ThrowIfNullOrWhiteSpace ( value );

            Name = name;
            RequiresSupporter = requiresSupporter;
            Value = value;
            IsSelected = false;

            if ( !Enum.TryParse ( name , out ChppScope scope ) )
            {
                throw new InvalidCastException ( "INVALID_CAST_EXCEPTION" );
            }

            Scope = scope;
        }

        public bool IsSelected
        {
            get;

            set
            {
                SetField ( ref field , value );
            }
        }

        public string Name
        {
            get;
        }

        public bool RequiresSupporter
        {
            get;
        }

        public ChppScope Scope
        {
            get;
        }

        public string Value
        {
            get;
        }
    }
}