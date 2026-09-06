namespace Mesa.HUM.Presentation.Tests.Stores.Contracts
{
    using Mesa.HUM.Presentation.Stores.Contracts;
    using Mesa.HUM.Presentation.Stores.Enums;

    public class NotificationModelTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Constructor_ShouldSetMessage ( )
            {
                // Act.
                var sut = new NotificationModel ( "Message" , NotificationSeverity.Information );

                // Assert.
                Assert.Equal ( "Message" , sut.Message );
            }

            [Theory]
            [InlineData ( NotificationSeverity.Information )]
            [InlineData ( NotificationSeverity.Success )]
            [InlineData ( NotificationSeverity.Warning )]
            [InlineData ( NotificationSeverity.Error )]
            public void Constructor_ShouldSetSeverity ( NotificationSeverity severity )
            {
                // Act.
                var sut = new NotificationModel ( "Message" , severity );

                // Assert.
                Assert.Equal ( severity , sut.Severity );
            }
        }
    }
}