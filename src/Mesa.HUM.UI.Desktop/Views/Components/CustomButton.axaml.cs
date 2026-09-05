namespace Mesa.HUM.UI.Desktop.Views.Components
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Media;
    using Mesa.HUM.UI.Desktop.Views.Components.Enums;

    public class CustomButton : Button
    {
        public static readonly StyledProperty<ContentDistribution> DistributionProperty = AvaloniaProperty.Register<CustomButton , ContentDistribution> ( nameof ( Distribution ) , ContentDistribution.ImageBeforeText );

        public static readonly StyledProperty<Geometry?> ImageProperty = AvaloniaProperty.Register<CustomButton , Geometry?> ( nameof ( Image ) );

        public static readonly StyledProperty<string?> TextProperty = AvaloniaProperty.Register<CustomButton , string?> ( nameof ( Text ) );

        public CustomButton ( )
        {
            UpdatePseudoClasses ( Distribution );
        }

        public ContentDistribution Distribution
        {
            get { return GetValue ( DistributionProperty ); }
            set { SetValue ( DistributionProperty , value ); }
        }

        public Geometry? Image
        {
            get { return GetValue ( ImageProperty ); }
            set { SetValue ( ImageProperty , value ); }
        }

        public string? Text
        {
            get { return GetValue ( TextProperty ); }
            set { SetValue ( TextProperty , value ); }
        }

        protected override void OnPropertyChanged ( AvaloniaPropertyChangedEventArgs change )
        {
            base.OnPropertyChanged ( change );

            if ( change.Property == DistributionProperty )
            {
                UpdatePseudoClasses ( change.GetNewValue<ContentDistribution> ( ) );
            }
        }

        private void UpdatePseudoClasses ( ContentDistribution distribution )
        {
            PseudoClasses.Set ( ":image-after-text" , distribution == ContentDistribution.ImageAfterText );
            PseudoClasses.Set ( ":image-above-text" , distribution == ContentDistribution.ImageAboveText );
            PseudoClasses.Set ( ":image-below-text" , distribution == ContentDistribution.ImageBelowText );
        }
    }
}