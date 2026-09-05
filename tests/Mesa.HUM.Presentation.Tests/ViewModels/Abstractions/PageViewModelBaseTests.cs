namespace Mesa.HUM.Presentation.Tests.ViewModels.Abstractions
{
    using Mesa.HUM.Presentation.ViewModels.Abstractions;

    public class PageViewModelBaseTests
    {
        public class DisposeTests
        {
            [Fact]
            public void Dispose_ShouldInvokeDisposeWithDisposingTrue ( )
            {
                // Arrange.
                var sut = new TestPageViewModel ( );

                // Act.
                sut.Dispose ( );

                // Assert.
                Assert.Equal ( 1 , sut.DisposeCallCount );
                Assert.True ( sut.LastDisposing );
            }

            [Fact]
            public void Dispose_ShouldSetIsDisposed ( )
            {
                // Arrange.
                var sut = new TestPageViewModel ( );

                // Act.
                sut.Dispose ( );

                // Assert.
                Assert.True ( sut.IsDisposedValue );
            }

            [Fact]
            public void Dispose_WhenCalledMultipleTimes_ShouldInvokeDisposeOnlyOnce ( )
            {
                // Arrange.
                var sut = new TestPageViewModel ( );

                // Act.
                sut.Dispose ( );
                sut.Dispose ( );
                sut.Dispose ( );

                // Assert.
                Assert.Equal ( 1 , sut.DisposeCallCount );
            }
        }

        public class IsDisposedTests
        {
            [Fact]
            public void IsDisposed_BeforeDispose_ShouldBeFalse ( )
            {
                // Act.
                var sut = new TestPageViewModel ( );

                // Assert.
                Assert.False ( sut.IsDisposedValue );
            }
        }

        // Minimal concrete page view model that records how the disposable pattern
        // drives it, so the abstract base can be exercised directly.
        private sealed class TestPageViewModel : PageViewModelBase
        {
            public int DisposeCallCount { get; private set; }

            public bool IsDisposedValue
            { get { return IsDisposed; } }

            public bool? LastDisposing { get; private set; }

            protected override void Dispose ( bool disposing )
            {
                DisposeCallCount++;
                LastDisposing = disposing;

                base.Dispose ( disposing );
            }
        }
    }
}