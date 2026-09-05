namespace Mesa.HUM.Presentation.Tests.Stores
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Mesa.HUM.Presentation.Abstractions.Interfaces;
    using Mesa.HUM.Presentation.Stores;
    using Mesa.HUM.Presentation.Stores.Contracts;
    using Moq;

    public class NotificationStoreTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Constructor_DismissCommand_ShouldNotBeNull ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                // Act.
                var sut = new NotificationStore ( dispatcherMock.Object );

                // Assert.
                Assert.NotNull ( sut.DismissCommand );
            }

            [Fact]
            public void Constructor_ShouldInitializeEmptyNotifications ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                // Act.
                var sut = new NotificationStore ( dispatcherMock.Object );

                // Assert.
                Assert.Empty ( sut.Notifications );
            }
        }

        public class DismissCommandTests
        {
            [Fact]
            public void DismissCommand_GivenExistingNotification_ShouldRemoveIt ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationStore ( dispatcherMock.Object );

                sut.Notify ( "Message" , NotificationSeverity.Information , TimeSpan.Zero );

                var notification = sut.Notifications.Single ( );

                // Act.
                sut.DismissCommand.Execute ( notification );

                // Assert.
                Assert.Empty ( sut.Notifications );
            }

            [Fact]
            public void DismissCommand_GivenNull_ShouldNotThrow ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationStore ( dispatcherMock.Object );

                // Act.
                var actual = Record.Exception ( ( ) => sut.DismissCommand.Execute ( null ) );

                // Assert.
                Assert.Null ( actual );
            }

            [Fact]
            public void DismissCommand_GivenUnknownNotification_ShouldNotChangeNotifications ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationStore ( dispatcherMock.Object );

                sut.Notify ( "Existing" , NotificationSeverity.Information , TimeSpan.Zero );

                var unknown = new NotificationModel ( "Unknown" , NotificationSeverity.Error );

                // Act.
                sut.DismissCommand.Execute ( unknown );

                // Assert.
                Assert.Single ( sut.Notifications );
            }
        }

        public class NotifyTests
        {
            [Fact]
            public void Notify_GivenMessageAndSeverity_ShouldAddNotification ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationStore ( dispatcherMock.Object );

                // Act.
                sut.Notify ( "Message" , NotificationSeverity.Error , TimeSpan.Zero );

                // Assert.
                var notification = Assert.Single ( sut.Notifications );
                Assert.Equal ( "Message" , notification.Message );
                Assert.Equal ( NotificationSeverity.Error , notification.Severity );
            }

            [Fact]
            public void Notify_GivenNoSeverity_ShouldDefaultToInformation ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationStore ( dispatcherMock.Object );

                // Act.
                sut.Notify ( "Message" , duration: TimeSpan.Zero );

                // Assert.
                var notification = Assert.Single ( sut.Notifications );
                Assert.Equal ( NotificationSeverity.Information , notification.Severity );
            }

            [Theory]
            [InlineData ( null )]
            [InlineData ( "" )]
            [InlineData ( " " )]
            public void Notify_GivenNullOrWhiteSpaceMessage_ShouldThrowArgumentException ( string? message )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationStore ( dispatcherMock.Object );

                // Act.
                var actual = Record.Exception ( ( ) => sut.Notify ( message! , NotificationSeverity.Information , TimeSpan.Zero ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsAssignableFrom<ArgumentException> ( actual );
                Assert.Equal ( "message" , exception.ParamName );
            }

            [Fact]
            public async Task Notify_GivenPositiveDuration_ShouldRemoveNotificationAfterDelay ( )
            {
                // Arrange.
                var completion = new TaskCompletionSource ( );

                var dispatcherMock = new Mock<IUIDispatcher> ( );

                dispatcherMock
                    .Setup ( x => x.Post ( It.IsAny<Action> ( ) ) )
                    .Callback<Action> ( action =>
                    {
                        action ( );
                        completion.TrySetResult ( );
                    } );

                var sut = new NotificationStore ( dispatcherMock.Object );

                // Act.
                sut.Notify ( "Message" , NotificationSeverity.Information , TimeSpan.FromMilliseconds ( 1 ) );

                await completion.Task.WaitAsync ( TimeSpan.FromSeconds ( 5 ) , TestContext.Current.CancellationToken );

                // Assert.
                Assert.Empty ( sut.Notifications );
            }

            [Fact]
            public void Notify_GivenZeroDuration_ShouldNotScheduleDismissal ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationStore ( dispatcherMock.Object );

                // Act.
                sut.Notify ( "Message" , NotificationSeverity.Information , TimeSpan.Zero );

                // Assert.
                Assert.Single ( sut.Notifications );
                dispatcherMock.Verify ( x => x.Post ( It.IsAny<Action> ( ) ) , Times.Never );
            }
        }
    }
}