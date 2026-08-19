namespace Mesa.HUM.Domain.Tests.Abstractions
{
    using AutoFixture;
    using Mesa.HUM.Domain.Abstractions;
    using Mesa.HUM.Domain.Abstractions.Interfaces;
    using Mesa.HUM.Tests.Shared.Helpers;
    using Xunit.Internal;

    public class AggregateRootTests
    {
        public class ClearDomainEventsTests
        {
            [Fact]
            public void ClearDomainEvents_GivenDomainEventsExist_ShouldClearDomainEvents ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );
                var sut = new TestAggregateRoot ( );

                var domainEvents = fixture.CreateMany<TestDomainEvent> ( 5 );

                domainEvents.ForEach ( sut.RaiseDomainEvent );

                // Act.
                sut.ClearDomainEvents ( );

                // Assert.
                Assert.Empty ( sut.DomainEvents );
            }

            [Fact]
            public void ClearDomainEvents_GivenNoDomainEvents_ShouldNotThrowException ( )
            {
                // Arrange.
                var sut = new TestAggregateRoot ( );

                // Act.
                var exception = Record.Exception ( sut.ClearDomainEvents );

                // Assert.
                Assert.Null ( exception );
            }
        }

        public class ConstructorTests
        {
            [Fact]
            public void Constructor_DomainEvents_ShouldBeEmpty ( )
            {
                // Arrange and Act.
                var sut = new TestAggregateRoot ( );

                // Assert.
                Assert.Empty ( sut.DomainEvents );
            }
        }

        public class RaiseDomainEventTests
        {
            [Fact]
            public void RaiseDomainEvent_WhenAddingMultipleDomainEvents_ShouldContainAllDomainEvents ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new TestAggregateRoot ( );

                var expected = fixture.CreateMany<TestDomainEvent> ( 5 );

                // Act
                expected.ForEach ( sut.RaiseDomainEvent );

                // Assert.
                Assert.Equivalent ( expected , sut.DomainEvents , true );
            }

            [Fact]
            public void RaiseDomainEvent_WhenAddingOneDomainEvent_ShouldOnlyHaveAddedEvent ( )
            {
                // Arrange.
                var sut = new TestAggregateRoot ( );
                var expected = new TestDomainEvent ( );

                // Act.
                sut.RaiseDomainEvent ( expected );

                // Assert.
                Assert.Single ( sut.DomainEvents , expected );
            }
        }

        private class TestAggregateRoot : AggregateRoot
        {
            public TestAggregateRoot ( )
            {
            }
        }

        private class TestDomainEvent : IDomainEvent
        {
        }
    }
}