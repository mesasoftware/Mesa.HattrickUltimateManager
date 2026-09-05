namespace Mesa.HUM.Presentation.Tests.Contracts
{
    using System;
    using Mesa.HUM.Presentation.Contracts;

    public class ScopeTests
    {
        [Theory]
        [InlineData ( "" , "value" )]
        [InlineData ( "   " , "value" )]
        [InlineData ( "name" , "" )]
        [InlineData ( "name" , "   " )]
        public void Constructor_WhenParameterIsEmpty_ShouldThrowArgumentException (
        string name ,
        string value )
        {
            // Assert.
            Assert.Throws<ArgumentException> ( ( ) => new Scope (
                name ,
                true ,
                value ) );
        }

        [Theory]
        [InlineData ( null , "value" )]
        [InlineData ( "name" , null )]
        public void Constructor_WhenParameterIsNull_ShouldThrowArgumentNullException (
            string? name ,
            string? value )
        {
            // Assert.
            Assert.Throws<ArgumentNullException> ( ( ) => new Scope (
                name! ,
                true ,
                value! ) );
        }

        [Fact]
        public void Constructor_WhenParametersAreValid_ShouldPopulateProperties ( )
        {
            // Act.
            var sut = new Scope (
                "name" ,
                true ,
                "value" );

            // Assert.
            Assert.Equal ( "name" , sut.Name );
            Assert.True ( sut.RequiresSupporter );
            Assert.Equal ( "value" , sut.Value );
        }
    }
}