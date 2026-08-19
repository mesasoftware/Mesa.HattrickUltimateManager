namespace Mesa.HUM.Domain.Abstractions
{
    using System.Collections.Generic;
    using Mesa.HUM.Domain.Abstractions.Interfaces;

    public abstract class AggregateRoot : IAggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents;

        protected AggregateRoot ( )
        {
            _domainEvents = [ ];
        }

        public IReadOnlyCollection<IDomainEvent> DomainEvents
        {
            get
            {
                return _domainEvents.AsReadOnly ( );
            }
        }

        public void ClearDomainEvents ( )
        {
            _domainEvents.Clear ( );
        }

        public void RaiseDomainEvent ( IDomainEvent domainEvent )
        {
            _domainEvents.Add ( domainEvent );
        }
    }
}