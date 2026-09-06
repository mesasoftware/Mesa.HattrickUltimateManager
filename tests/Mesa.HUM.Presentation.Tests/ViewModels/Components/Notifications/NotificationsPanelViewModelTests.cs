namespace Mesa.HUM.Presentation.Tests.ViewModels.Components.Notifications
{
    using System;
    using Mesa.HUM.Presentation.Stores.Contracts;
    using Mesa.HUM.Presentation.Stores.Enums;
    using Mesa.HUM.Presentation.Stores.Interfaces;
    using Mesa.HUM.Presentation.ViewModels.Components.Notifications;
    using Moq;

    public class NotificationsPanelViewModelTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Constructor_ShouldExposeStore ( )
            {
                // Arrange.
                var storeMock = new Mock<INotificationsStore> ( );

                // Act.
                var sut = new NotificationsPanelViewModel ( storeMock.Object );

                // Assert.
                Assert.Same ( storeMock.Object , sut.Store );
            }

            [Fact]
            public void Constructor_ShouldInitializeDismissCommand ( )
            {
                // Arrange.
                var storeMock = new Mock<INotificationsStore> ( );

                // Act.
                var sut = new NotificationsPanelViewModel ( storeMock.Object );

                // Assert.
                Assert.NotNull ( sut.DismissCommand );
            }
        }

        public class DismissCommandTests
        {
            [Fact]
            public void DismissCommand_GivenNotification_ShouldDismissThroughStore ( )
            {
                // Arrange.
                var storeMock = new Mock<INotificationsStore> ( );

                var sut = new NotificationsPanelViewModel ( storeMock.Object );

                var notification = new NotificationModel ( "Message" , NotificationSeverity.Information );

                // Act.
                sut.DismissCommand.Execute ( notification );

                // Assert.
                storeMock.Verify ( x => x.Dismiss ( notification ) , Times.Once );
            }

            [Fact]
            public void DismissCommand_GivenNull_ShouldThrowArgumentNullException ( )
            {
                // Arrange.
                var storeMock = new Mock<INotificationsStore> ( );

                var sut = new NotificationsPanelViewModel ( storeMock.Object );

                // Act.
                var actual = Record.Exception ( ( ) => sut.DismissCommand.Execute ( null ) );

                // Assert.
                Assert.IsType<ArgumentNullException> ( actual );
                storeMock.Verify ( x => x.Dismiss ( It.IsAny<NotificationModel> ( ) ) , Times.Never );
            }
        }
    }
}