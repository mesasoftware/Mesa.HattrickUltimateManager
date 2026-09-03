namespace Mesa.HUM.Presentation.ViewModels.Components.UserProfiles
{
    using System;
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
        }

        public bool IsSelected
        {
            get;

            set { SetField ( ref field , value ); }
        }

        public string Name { get; }

        public bool RequiresSupporter { get; }

        public string Value { get; }
    }
}