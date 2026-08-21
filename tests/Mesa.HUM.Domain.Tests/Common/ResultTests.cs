namespace Mesa.HUM.Domain.Tests.Common
{
    using Mesa.HUM.Domain.Common;
    using Mesa.HUM.Domain.Tests.Common.Constants;

    public class ResultTests
    {
        public class IsFailureTests
        {
            [Fact]
            public void IsFailure_GivenConflictResult_ShouldBeTrue ( )
            {
                // Act.
                var sut = Result.Failure ( Error.Conflict ( Errors.ConflictCode , Errors.ConflictDescription ) );

                // Assert.
                Assert.True ( sut.IsFailure );
            }

            [Fact]
            public void IsFailure_GivenFailureResult_ShouldBeTrue ( )
            {
                // Act.
                var sut = Result.Failure ( Error.Failure ( Errors.ValidationCode , Errors.ValidationDescription ) );

                // Assert.
                Assert.True ( sut.IsFailure );
            }

            [Fact]
            public void IsFailure_GivenNotFoundResult_ShouldBeTrue ( )
            {
                // Act.
                var sut = Result.Failure ( Error.NotFound ( Errors.NotFoundCode , Errors.NotFoundDescription ) );

                // Assert.
                Assert.True ( sut.IsFailure );
            }

            [Fact]
            public void IsFailure_GivenProblemResult_ShouldBeTrue ( )
            {
                // Act.
                var sut = Result.Failure ( Error.Problem ( Errors.ProblemCode , Errors.ProblemDescription ) );

                // Assert.
                Assert.True ( sut.IsFailure );
            }

            [Fact]
            public void IsFailure_GivenSuccessResult_ShouldBeFalse ( )
            {
                // Act.
                var sut = Result.Success ( );

                // Assert.
                Assert.False ( sut.IsFailure );
            }

            [Fact]
            public void IsFailure_GivenValidationResult_ShouldBeTrue ( )
            {
                // Act.
                var sut = Result.Failure ( Error.Validation ( Errors.ValidationCode , Errors.ValidationDescription ) );

                // Assert.
                Assert.True ( sut.IsFailure );
            }
        }

        public class IsSuccessTests
        {
            [Fact]
            public void IsSuccess_GivenConflictResult_ShouldBeFalse ( )
            {
                // Act.
                var sut = Result.Failure ( Error.Conflict ( Errors.ConflictCode , Errors.ConflictDescription ) );

                // Assert.
                Assert.False ( sut.IsSuccess );
            }

            [Fact]
            public void IsSuccess_GivenFailureResult_ShouldBeFalse ( )
            {
                // Act.
                var sut = Result.Failure ( Error.Failure ( Errors.ValidationCode , Errors.ValidationDescription ) );

                // Assert.
                Assert.False ( sut.IsSuccess );
            }

            [Fact]
            public void IsSuccess_GivenNotFoundResult_ShouldBeFalse ( )
            {
                // Act.
                var sut = Result.Failure ( Error.NotFound ( Errors.NotFoundCode , Errors.NotFoundDescription ) );

                // Assert.
                Assert.False ( sut.IsSuccess );
            }

            [Fact]
            public void IsSuccess_GivenProblemResult_ShouldBeFalse ( )
            {
                // Act.
                var sut = Result.Failure ( Error.Problem ( Errors.ProblemCode , Errors.ProblemDescription ) );

                // Assert.
                Assert.False ( sut.IsSuccess );
            }

            [Fact]
            public void IsSuccess_GivenSuccessResult_ShouldBeTrue ( )
            {
                // Act.
                var sut = Result.Success ( );

                // Assert.
                Assert.True ( sut.IsSuccess );
            }

            [Fact]
            public void IsSuccess_GivenValidationResult_ShouldBeFalse ( )
            {
                // Act.
                var sut = Result.Failure ( Error.Validation ( Errors.ValidationCode , Errors.ValidationDescription ) );

                // Assert.
                Assert.False ( sut.IsSuccess );
            }
        }
    }
}