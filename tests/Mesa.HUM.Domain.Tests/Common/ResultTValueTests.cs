namespace Mesa.HUM.Domain.Tests.Common
{
    using System;
    using Mesa.HUM.Domain.Common;
    using Mesa.HUM.Domain.Tests.Common.Constants;

    public class ResultTValueTests
    {
        public class IsFailureTests
        {
            [Fact]
            public void IsFailure_GivenConflictResult_ShouldBeTrueAndErrorSameAsParameter ( )
            {
                // Arrange.
                var error = Error.Conflict ( Errors.ConflictCode , Errors.ConflictDescription );

                // Act.
                var actual = Result<FakeResultValue>.Failure ( error );

                // Assert.
                Assert.True ( actual.IsFailure );
                Assert.Equal ( error , actual.Error );
            }

            [Fact]
            public void IsFailure_GivenFailureResult_ShouldBeTrueAndErrorSameAsParameter ( )
            {
                // Arrange.
                var error = Error.Failure ( Errors.FailureCode , Errors.FailureDescription );

                // Act.
                var actual = Result<FakeResultValue>.Failure ( error );

                // Assert.
                Assert.True ( actual.IsFailure );
                Assert.Equal ( error , actual.Error );
            }

            [Fact]
            public void IsFailure_GivenNotFoundResult_ShouldBeTrueAndErrorSameAsParameter ( )
            {
                // Arrange.
                var error = Error.NotFound ( Errors.NotFoundCode , Errors.NotFoundDescription );

                // Act.
                var actual = Result<FakeResultValue>.Failure ( error );

                // Assert.
                Assert.True ( actual.IsFailure );
                Assert.Equal ( error , actual.Error );
            }

            [Fact]
            public void IsFailure_GivenProblemResult_ShouldBeTrueAndErrorSameAsParameter ( )
            {
                // Arrange.
                var error = Error.Problem ( Errors.ProblemCode , Errors.ProblemDescription );

                // Act.
                var actual = Result<FakeResultValue>.Failure ( error );

                // Assert.
                Assert.True ( actual.IsFailure );
                Assert.Equal ( error , actual.Error );
            }

            [Fact]
            public void IsFailure_GivenSuccessResult_ShouldBeFalseAndErrorNull ( )
            {
                // Arrange.
                var expected = new FakeResultValue ( );

                // Act.
                var actual = Result<FakeResultValue>.Success ( expected );

                // Assert.
                Assert.False ( actual.IsFailure );
                Assert.Null ( actual.Error );
            }

            [Fact]
            public void IsFailure_GivenValidationResult_ShouldBeTrueAndErrorSameAsParameter ( )
            {
                // Arrange.
                var expected = new FakeResultValue ( );
                var error = Error.Validation ( Errors.ValidationCode , Errors.ValidationDescription );

                // Act.
                var actual = Result<FakeResultValue>.Failure ( error );

                // Assert.
                Assert.True ( actual.IsFailure );
                Assert.Equal ( error , actual.Error );
            }
        }

        public class IsSuccessTests
        {
            [Fact]
            public void IsSuccess_GivenConflictResult_ShouldBeFalseAndErrorSameAsParameter ( )
            {
                // Arrange.
                var error = Error.Conflict ( Errors.ConflictCode , Errors.ConflictDescription );

                // Act.
                var actual = Result<FakeResultValue>.Failure ( error );

                // Assert.
                Assert.False ( actual.IsSuccess );
                Assert.Equal ( error , actual.Error );
            }

            [Fact]
            public void IsSuccess_GivenFailureResult_ShouldBeFalseAndErrorSameAsParameter ( )
            {
                // Arrange.
                var error = Error.Failure ( Errors.FailureCode , Errors.FailureDescription );

                // Act.
                var actual = Result<FakeResultValue>.Failure ( error );

                // Assert.
                Assert.False ( actual.IsSuccess );
                Assert.Equal ( error , actual.Error );
            }

            [Fact]
            public void IsSuccess_GivenNotFoundResult_ShouldBeFalseAndErrorSameAsParameter ( )
            {
                // Arrange.
                var error = Error.NotFound ( Errors.NotFoundCode , Errors.NotFoundDescription );

                // Act.
                var actual = Result<FakeResultValue>.Failure ( error );

                // Assert.
                Assert.False ( actual.IsSuccess );
                Assert.Equal ( error , actual.Error );
            }

            [Fact]
            public void IsSuccess_GivenProblemResult_ShouldBeFalseAndErrorSameAsParameter ( )
            {
                // Arrange.
                var error = Error.Problem ( Errors.ProblemCode , Errors.ProblemDescription );

                // Act.
                var actual = Result<FakeResultValue>.Failure ( error );

                // Assert.
                Assert.False ( actual.IsSuccess );
                Assert.Equal ( error , actual.Error );
            }

            [Fact]
            public void IsSuccess_GivenSuccessResult_ShouldBeTrueAndErrorNull ( )
            {
                // Arrange.
                var expected = new FakeResultValue ( );

                // Act.
                var actual = Result<FakeResultValue>.Success ( expected );

                // Assert.
                Assert.True ( actual.IsSuccess );
                Assert.Null ( actual.Error );
            }

            [Fact]
            public void IsSuccess_GivenValidationResult_ShouldBeFalseAndErrorSameAsParameter ( )
            {
                // Arrange.
                var expected = new FakeResultValue ( );
                var error = Error.Validation ( Errors.ValidationCode , Errors.ValidationDescription );

                // Act.
                var actual = Result<FakeResultValue>.Failure ( error );

                // Assert.
                Assert.False ( actual.IsSuccess );
                Assert.Equal ( error , actual.Error );
            }
        }

        public class ValueTests
        {
            private const string valueCannotBeAccessedOnFailure = "VALUE_CANNOT_BE_ACCESSED_ON_FAILURE";

            [Fact]
            public void Value_GivenConflictResult_ShouldThrowInvalidOperationExceptionWithCorrectMessage ( )
            {
                // Arrange.
                var error = Error.Conflict ( Errors.ConflictCode , Errors.ConflictDescription );
                var sut = Result<FakeResultValue>.Failure ( error );

                // Act.
                var actual = Record.Exception ( ( ) => sut.Value );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<InvalidOperationException> ( actual );
                Assert.Equal ( valueCannotBeAccessedOnFailure , exception.Message );
            }

            [Fact]
            public void Value_GivenFailureResult_ShouldThrowInvalidOperationExceptionWithCorrectMessage ( )
            {
                // Arrange.
                var error = Error.Failure ( Errors.FailureCode , Errors.FailureDescription );
                var sut = Result<FakeResultValue>.Failure ( error );

                // Act.
                var actual = Record.Exception ( ( ) => sut.Value );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<InvalidOperationException> ( actual );
                Assert.Equal ( valueCannotBeAccessedOnFailure , exception.Message );
            }

            [Fact]
            public void Value_GivenNotFoundResult_ShouldThrowInvalidOperationExceptionWithCorrectMessage ( )
            {
                // Arrange.
                var error = Error.NotFound ( Errors.NotFoundCode , Errors.NotFoundDescription );
                var sut = Result<FakeResultValue>.Failure ( error );

                // Act.
                var actual = Record.Exception ( ( ) => sut.Value );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<InvalidOperationException> ( actual );
                Assert.Equal ( valueCannotBeAccessedOnFailure , exception.Message );
            }

            [Fact]
            public void Value_GivenProblemResult_ShouldThrowInvalidOperationExceptionWithCorrectMessage ( )
            {
                // Arrange.
                var error = Error.Problem ( Errors.ProblemCode , Errors.ProblemDescription );
                var sut = Result<FakeResultValue>.Failure ( error );

                // Act.
                var actual = Record.Exception ( ( ) => sut.Value );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<InvalidOperationException> ( actual );
                Assert.Equal ( valueCannotBeAccessedOnFailure , exception.Message );
            }

            [Fact]
            public void Value_GivenSuccessResult_ShouldNotThrowExceptionAndValueBeSameAsParameter ( )
            {
                // Arrange.
                var expected = new FakeResultValue ( );
                var sut = Result<FakeResultValue>.Success ( expected );

                // Act.
                var actual = Record.Exception ( ( ) => sut.Value );

                // Assert.
                Assert.Null ( actual );
                Assert.Same ( expected , sut.Value );
            }

            [Fact]
            public void Value_GivenValidationResult_ShouldThrowInvalidOperationExceptionWithCorrectMessage ( )
            {
                // Arrange.
                var error = Error.Validation ( Errors.ValidationCode , Errors.ValidationDescription );
                var sut = Result<FakeResultValue>.Failure ( error );

                // Act.
                var actual = Record.Exception ( ( ) => sut.Value );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<InvalidOperationException> ( actual );
                Assert.Equal ( valueCannotBeAccessedOnFailure , exception.Message );
            }
        }

        private class FakeResultValue
        {
        }
    }
}