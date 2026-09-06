namespace Mesa.HUM.UI.Desktop.Tests.Views.Components.Notifications
{
    using System;
    using System.Linq;
    using Avalonia.Controls;
    using Avalonia.Headless.XUnit;
    using Avalonia.Threading;
    using Avalonia.VisualTree;
    using Mesa.HUM.Presentation.Abstractions.Interfaces;
    using Mesa.HUM.Presentation.Stores;
    using Mesa.HUM.Presentation.Stores.Enums;
    using Mesa.HUM.Presentation.ViewModels.Components.Notifications;
    using Mesa.HUM.UI.Desktop.Views.Components.Notifications;
    using Moq;

    public class NotificationPanelTests
    {
        private static NotificationPanel CreateShownPanel ( NotificationsStore store )
        {
            var sut = new NotificationPanel
            {
                DataContext = new NotificationsPanelViewModel ( store )
            };

            var window = new Window
            {
                Width = 500 ,
                Height = 400 ,
                Content = sut
            };

            window.Show ( );

            Dispatcher.UIThread.RunJobs ( );

            return sut;
        }

        private static NotificationsStore CreateStore ( )
        {
            return new NotificationsStore ( Mock.Of<IUIDispatcher> ( ) );
        }

        public class ConstructorTests
        {
            [AvaloniaFact]
            public void Constructor_ShouldLoadWithoutError ( )
            {
                // Act.
                var sut = new NotificationPanel ( );

                // Assert.
                Assert.NotNull ( sut );
            }
        }

        public class DismissTests
        {
            [AvaloniaFact]
            public void DismissButton_WhenInvoked_ShouldRemoveTheNotification ( )
            {
                // Arrange.
                var store = CreateStore ( );

                store.Notify ( "First" , NotificationSeverity.Error , TimeSpan.Zero );
                store.Notify ( "Second" , NotificationSeverity.Success , TimeSpan.Zero );

                var sut = CreateShownPanel ( store );

                var dismissButton = sut.GetVisualDescendants ( ).OfType<Button> ( ).First ( );

                // Act.
                dismissButton.Command!.Execute ( dismissButton.CommandParameter );

                Dispatcher.UIThread.RunJobs ( );

                // Assert.
                Assert.Single ( store.Notifications );
            }
        }

        public class RenderingTests
        {
            [AvaloniaFact]
            public void DataContext_GivenNotifications_ShouldDisplayMessages ( )
            {
                // Arrange.
                var store = CreateStore ( );

                store.Notify ( "First message" , NotificationSeverity.Error , TimeSpan.Zero );
                store.Notify ( "Second message" , NotificationSeverity.Success , TimeSpan.Zero );

                // Act.
                var sut = CreateShownPanel ( store );

                // Assert.
                string? [ ] messages = sut.GetVisualDescendants ( )
                    .OfType<TextBlock> ( )
                    .Select ( x => x.Text )
                    .ToArray ( );

                Assert.Contains ( "First message" , messages );
                Assert.Contains ( "Second message" , messages );
            }

            [AvaloniaFact]
            public void DataContext_GivenNotifications_ShouldRenderOneItemPerNotification ( )
            {
                // Arrange.
                var store = CreateStore ( );

                store.Notify ( "First" , NotificationSeverity.Error , TimeSpan.Zero );
                store.Notify ( "Second" , NotificationSeverity.Success , TimeSpan.Zero );

                // Act.
                var sut = CreateShownPanel ( store );

                // Assert.
                var itemsControl = sut.GetVisualDescendants ( ).OfType<ItemsControl> ( ).Single ( );
                Assert.Equal ( 2 , itemsControl.ItemCount );
            }
        }
    }
}