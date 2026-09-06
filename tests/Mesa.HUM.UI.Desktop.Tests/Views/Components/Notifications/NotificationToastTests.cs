namespace Mesa.HUM.UI.Desktop.Tests.Views.Components.Notifications
{
    using System;
    using System.Linq;
    using Avalonia.Controls;
    using Avalonia.Controls.Templates;
    using Avalonia.Headless.XUnit;
    using Avalonia.Threading;
    using Avalonia.VisualTree;
    using Mesa.HUM.Presentation.Abstractions.Interfaces;
    using Mesa.HUM.Presentation.Stores;
    using Mesa.HUM.Presentation.Stores.Contracts;
    using Mesa.HUM.Presentation.Stores.Enums;
    using Mesa.HUM.Presentation.ViewModels.Components.Notifications;
    using Mesa.HUM.UI.Desktop.Views.Components.Notifications;
    using Moq;

    public class NotificationToastTests
    {
        private static NotificationToast CreateShownToast ( NotificationModel notification )
        {
            var sut = new NotificationToast
            {
                DataContext = notification
            };

            var window = new Window
            {
                Width = 500 ,
                Height = 100 ,
                Content = sut
            };

            window.Show ( );

            Dispatcher.UIThread.RunJobs ( );

            return sut;
        }

        public class ConstructorTests
        {
            [AvaloniaFact]
            public void Constructor_ShouldLoadWithoutError ( )
            {
                // Act.
                var sut = new NotificationToast ( );

                // Assert.
                Assert.NotNull ( sut );
            }
        }

        public class DismissTests
        {
            [AvaloniaFact]
            public void DismissButton_WhenInvoked_ShouldDismissThroughParentViewModel ( )
            {
                // Arrange.
                var store = new NotificationsStore ( Mock.Of<IUIDispatcher> ( ) );

                store.Notify ( "Message" , NotificationSeverity.Error , TimeSpan.Zero );

                var viewModel = new NotificationsPanelViewModel ( store );

                var itemsControl = new ItemsControl
                {
                    DataContext = viewModel ,
                    ItemsSource = store.Notifications ,
                    ItemTemplate = new FuncDataTemplate<NotificationModel> ( ( _ , _ ) => new NotificationToast ( ) )
                };

                var window = new Window
                {
                    Width = 500 ,
                    Height = 100 ,
                    Content = itemsControl
                };

                window.Show ( );

                Dispatcher.UIThread.RunJobs ( );

                var toast = itemsControl.GetVisualDescendants ( ).OfType<NotificationToast> ( ).Single ( );
                var dismissButton = toast.GetVisualDescendants ( ).OfType<Button> ( ).Single ( );

                // Act.
                dismissButton.Command!.Execute ( dismissButton.CommandParameter );

                Dispatcher.UIThread.RunJobs ( );

                // Assert.
                Assert.Empty ( store.Notifications );
            }
        }

        public class RenderingTests
        {
            [AvaloniaFact]
            public void DataContext_GivenNotification_ShouldDisplayMessage ( )
            {
                // Arrange.
                var notification = new NotificationModel ( "Toast message" , NotificationSeverity.Warning );

                // Act.
                var sut = CreateShownToast ( notification );

                // Assert.
                var messages = sut.GetVisualDescendants ( )
                    .OfType<TextBlock> ( )
                    .Select ( x => x.Text );

                Assert.Contains ( "Toast message" , messages );
            }

            [AvaloniaFact]
            public void DataContext_GivenNotification_ShouldRenderDismissButton ( )
            {
                // Arrange.
                var notification = new NotificationModel ( "Toast message" , NotificationSeverity.Warning );

                // Act.
                var sut = CreateShownToast ( notification );

                // Assert.
                var dismissButton = sut.GetVisualDescendants ( ).OfType<Button> ( ).Single ( );
                Assert.Equal ( "✕" , dismissButton.Content );
            }
        }
    }
}