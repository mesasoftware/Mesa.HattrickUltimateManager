namespace Mesa.HUM.Domain.Tests.Common
{
    using Mesa.HUM.Domain.Common;
    using Mesa.HUM.Domain.Common.Enums;
    using Mesa.HUM.Domain.Tests.Common.Constants;

    public class ErrorTests
    {
        public class PropertyTests
        {
            public class ConstructorTests
            {
                public class TypedConstructors
                {
                    [Fact]
                    public void Conflict_ShouldBeOfCorrectType ( )
                    {
                        // Arrange & Act.
                        var sut = Error.Conflict ( Errors.ConflictCode , Errors.ConflictDescription );

                        // Assert.
                        Assert.Equal ( ErrorType.Conflict , sut.Type );
                    }

                    [Fact]
                    public void Failure_ShouldBeOfCorrectType ( )
                    {
                        // Arrange & Act.
                        var sut = Error.Failure ( Errors.FailureCode , Errors.FailureDescription );

                        // Assert.
                        Assert.Equal ( ErrorType.Failure , sut.Type );
                    }

                    [Fact]
                    public void NotFound_ShouldBeOfCorrectType ( )
                    {
                        // Arrange & Act.
                        var sut = Error.NotFound ( Errors.NotFoundCode , Errors.NotFoundDescription );

                        // Assert.
                        Assert.Equal ( ErrorType.NotFound , sut.Type );
                    }

                    [Fact]
                    public void Problem_ShouldBeOfCorrectType ( )
                    {
                        // Arrange & Act.
                        var sut = Error.Problem ( Errors.ProblemCode , Errors.ProblemDescription );

                        // Assert.
                        Assert.Equal ( ErrorType.Problem , sut.Type );
                    }

                    [Fact]
                    public void Validation_ShouldBeOfCorrectType ( )
                    {
                        // Arrange & Act.
                        var sut = Error.Validation ( Errors.ValidationCode , Errors.ValidationDescription );

                        // Assert.
                        Assert.Equal ( ErrorType.Validation , sut.Type );
                    }
                }
            }
        }
    }
}