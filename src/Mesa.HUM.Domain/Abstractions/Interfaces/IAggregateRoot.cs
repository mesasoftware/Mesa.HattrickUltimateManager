namespace Mesa.HUM.Domain.Abstractions.Interfaces
{
    using System.Collections.Generic;

    public interface IAggregateRoot
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        void ClearDomainEvents ( );

        void RaiseDomainEvent ( IDomainEvent domainEvent );
    }
}