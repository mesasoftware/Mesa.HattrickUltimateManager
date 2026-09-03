namespace Mesa.HUM.Presentation.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Mesa.HUM.Presentation.Abstractions.Interfaces;

    public abstract class ObservableComponent : IObservableComponent
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public event PropertyChangingEventHandler? PropertyChanging;

        /// <summary>
        /// Assigns <paramref name="value" /> to <paramref name="field" /> when it differs, raising the
        /// PropertyChanging and PropertyChanged events for the property and any dependent child properties.
        /// </summary>
        /// <typeparam name="T">The type of the backing field.</typeparam>
        /// <param name="field">The backing field to assign, passed by reference.</param>
        /// <param name="value">The new value to assign.</param>
        /// <param name="propertyName">
        /// The name of the property being set. Supplied automatically by the compiler via
        /// <see cref="CallerMemberNameAttribute" /> when omitted. When you also pass
        /// <paramref name="childPropertiesNames" /> and want the caller name inferred, pass the children by
        /// name (childPropertiesNames:) so the first child is not bound to this parameter.
        /// </param>
        /// <param name="childPropertiesNames">The names of dependent properties to raise change events for.</param>
        /// <returns><c>true</c> if the value changed and events were raised; otherwise, <c>false</c>.</returns>
        protected bool SetField<T> ( ref T field , T value , [CallerMemberName] string? propertyName = null , params string [ ]? childPropertiesNames )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace ( propertyName );

            string [ ] children = childPropertiesNames ?? [ ];

            foreach ( string childProperty in children )
            {
                ArgumentException.ThrowIfNullOrWhiteSpace ( childProperty );
            }

            if ( EqualityComparer<T>.Default.Equals ( field , value ) )
            {
                return false;
            }

            PropertyChanging?.Invoke (
                this ,
                new PropertyChangingEventArgs (
                    propertyName ) );

            foreach ( string childPropertyName in children )
            {
                PropertyChanging?.Invoke (
                    this ,
                    new PropertyChangingEventArgs (
                        childPropertyName ) );
            }

            field = value;

            PropertyChanged?.Invoke (
                this ,
                new PropertyChangedEventArgs (
                    propertyName ) );

            foreach ( string childPropertyName in children )
            {
                PropertyChanged?.Invoke (
                    this ,
                    new PropertyChangedEventArgs (
                        childPropertyName ) );
            }

            return true;
        }
    }
}