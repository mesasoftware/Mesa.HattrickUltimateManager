namespace Mesa.HUM.Application.Tests.Behaviors
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using Mesa.HUM.Application.Behaviors;
    using Mesa.HUM.Domain.Common;
    using Mesa.HUM.Domain.Common.Enums;

    public class ValidationBehaviorTests
    {
        [Fact]
        public async Task Handle_WhenMultipleRulesFail_ShouldAggregateMessagesInDescription ( )
        {
            // Arrange.
            var behavior = new ValidationBehavior<TestRequest , Result<string>> ( [ new TestRequestValidator ( ) ] );

            // Act.
            var actual = await behavior.Handle (
                new TestRequest ( string.Empty , string.Empty ) ,
                _ => Task.FromResult ( Result<string>.Success ( "handled" ) ) ,
                CancellationToken.None );

            // Assert.
            Assert.True ( actual.IsFailure );
            Assert.NotNull ( actual.Error );
            Assert.Contains ( "NAME_REQUIRED" , actual.Error.Description );
            Assert.Contains ( "VALUE_REQUIRED" , actual.Error.Description );
        }

        [Fact]
        public async Task Handle_WhenNoValidatorsRegistered_ShouldInvokeNextAndReturnItsResult ( )
        {
            // Arrange.
            var behavior = new ValidationBehavior<TestRequest , Result<string>> ( [ ] );

            var expected = Result<string>.Success ( "handled" );

            // Act.
            var actual = await behavior.Handle (
                new TestRequest ( "name" , "value" ) ,
                _ => Task.FromResult ( expected ) ,
                CancellationToken.None );

            // Assert.
            Assert.Same ( expected , actual );
        }

        [Fact]
        public async Task Handle_WhenValidationFails_ShouldReturnValidationFailureWithoutInvokingNext ( )
        {
            // Arrange.
            bool nextInvoked = false;

            var behavior = new ValidationBehavior<TestRequest , Result<string>> ( [ new TestRequestValidator ( ) ] );

            // Act.
            var actual = await behavior.Handle (
                new TestRequest ( string.Empty , "value" ) ,
                _ =>
                {
                    nextInvoked = true;
                    return Task.FromResult ( Result<string>.Success ( "handled" ) );
                } ,
                CancellationToken.None );

            // Assert.
            Assert.False ( nextInvoked );
            Assert.True ( actual.IsFailure );
            Assert.NotNull ( actual.Error );
            Assert.Equal ( ErrorType.Validation , actual.Error.Type );
            Assert.Equal ( "VALIDATION_ERROR" , actual.Error.Code );
            Assert.Contains ( "NAME_REQUIRED" , actual.Error.Description );
        }

        [Fact]
        public async Task Handle_WhenValidationFailsForNonGenericResult_ShouldReturnValidationFailure ( )
        {
            // Arrange.
            var behavior = new ValidationBehavior<TestRequest , Result> ( [ new TestRequestValidator ( ) ] );

            // Act.
            var actual = await behavior.Handle (
                new TestRequest ( string.Empty , "value" ) ,
                _ => Task.FromResult ( Result.Success ( ) ) ,
                CancellationToken.None );

            // Assert.
            Assert.True ( actual.IsFailure );
            Assert.NotNull ( actual.Error );
            Assert.Equal ( ErrorType.Validation , actual.Error.Type );
            Assert.Equal ( "VALIDATION_ERROR" , actual.Error.Code );
        }

        [Fact]
        public async Task Handle_WhenValidationPasses_ShouldInvokeNextAndReturnItsResult ( )
        {
            // Arrange.
            var behavior = new ValidationBehavior<TestRequest , Result<string>> ( [ new TestRequestValidator ( ) ] );

            var expected = Result<string>.Success ( "handled" );

            // Act.
            var actual = await behavior.Handle (
                new TestRequest ( "name" , "value" ) ,
                _ => Task.FromResult ( expected ) ,
                CancellationToken.None );

            // Assert.
            Assert.Same ( expected , actual );
        }

        private sealed record TestRequest ( string Name , string Value );

        private sealed class TestRequestValidator : AbstractValidator<TestRequest>
        {
            public TestRequestValidator ( )
            {
                RuleFor ( x => x.Name ).NotEmpty ( ).WithMessage ( "NAME_REQUIRED" );

                RuleFor ( x => x.Value ).NotEmpty ( ).WithMessage ( "VALUE_REQUIRED" );
            }
        }
    }
}