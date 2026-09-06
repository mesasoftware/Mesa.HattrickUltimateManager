namespace Mesa.HUM.Presentation.Tests.Stores
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Mesa.HUM.Presentation.Abstractions.Interfaces;
    using Mesa.HUM.Presentation.Stores;
    using Mesa.HUM.Presentation.Stores.Contracts;
    using Mesa.HUM.Presentation.Stores.Enums;
    using Moq;

    public class NotificationsStoreTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Constructor_ShouldInitializeEmptyNotifications ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                // Act.
                var sut = new NotificationsStore ( dispatcherMock.Object );

                // Assert.
                Assert.Empty ( sut.Notifications );
            }
        }

        public class DismissTests
        {
            [Fact]
            public void Dismiss_GivenExistingNotification_ShouldRemoveIt ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationsStore ( dispatcherMock.Object );

                sut.Notify ( "Message" , NotificationSeverity.Information , TimeSpan.Zero );

                var notification = sut.Notifications.Single ( );

                // Act.
                sut.Dismiss ( notification );

                // Assert.
                Assert.Empty ( sut.Notifications );
            }

            [Fact]
            public void Dismiss_GivenNull_ShouldNotThrow ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationsStore ( dispatcherMock.Object );

                // Act.
                var actual = Record.Exception ( ( ) => sut.Dismiss ( null ) );

                // Assert.
                Assert.Null ( actual );
            }

            [Fact]
            public void Dismiss_GivenUnknownNotification_ShouldNotChangeNotifications ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationsStore ( dispatcherMock.Object );

                sut.Notify ( "Existing" , NotificationSeverity.Information , TimeSpan.Zero );

                var unknown = new NotificationModel ( "Unknown" , NotificationSeverity.Error );

                // Act.
                sut.Dismiss ( unknown );

                // Assert.
                Assert.Single ( sut.Notifications );
            }
        }

        public class NotifyTests
        {
            [Theory]
            [InlineData ( "" )]
            [InlineData ( " " )]
            public void Notify_GivenEmptyOrWhiteSpaceMessage_ShouldThrowArgumentException ( string? message )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationsStore ( dispatcherMock.Object );

                // Act.
                var actual = Record.Exception ( ( ) => sut.Notify ( message! , NotificationSeverity.Information , TimeSpan.Zero ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<ArgumentException> ( actual );
                Assert.Equal ( "message" , exception.ParamName );
            }

            [Fact]
            public void Notify_GivenMessageAndSeverity_ShouldAddNotification ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationsStore ( dispatcherMock.Object );

                // Act.
                sut.Notify ( "Message" , NotificationSeverity.Error , TimeSpan.Zero );

                // Assert.
                var notification = Assert.Single ( sut.Notifications );
                Assert.Equal ( "Message" , notification.Message );
                Assert.Equal ( NotificationSeverity.Error , notification.Severity );
            }

            [Fact]
            public void Notify_GivenNoDuration_ShouldScheduleDismissalUsingDefaultDuration ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationsStore ( dispatcherMock.Object );

                // Act.
                sut.Notify ( "Message" , NotificationSeverity.Information );

                // Assert.
                // The default duration is positive, so the notification is added and left in place
                // (dismissal is scheduled for later rather than applied synchronously).
                Assert.Single ( sut.Notifications );
                dispatcherMock.Verify ( x => x.Post ( It.IsAny<Action> ( ) ) , Times.Never );
            }

            [Fact]
            public void Notify_GivenNullMessage_ShouldThrowArgumentException ( )
            {
                // Arrange.
                var dispatcherMock = new Mock<IUIDispatcher> ( );

                var sut = new NotificationsStore ( dispatcherMock.Object );

                // Act.
                var actual = Record.Exception ( ( ) => sut.Notify ( null! , NotificationSeverity.Information , TimeSpan.Zero ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<ArgumentNullException> ( actual );
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

                var sut = new NotificationsStore ( dispatcherMock.Object );

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

                var sut = new NotificationsStore ( dispatcherMock.Object );

                // Act.
                sut.Notify ( "Message" , NotificationSeverity.Information , TimeSpan.Zero );

                // Assert.
                Assert.Single ( sut.Notifications );
                dispatcherMock.Verify ( x => x.Post ( It.IsAny<Action> ( ) ) , Times.Never );
            }
        }
    }
}