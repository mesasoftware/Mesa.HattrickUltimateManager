namespace Mesa.HUM.Presentation.Tests.Contracts
{
    using System;
    using Mesa.HUM.Presentation.Contracts;

    public class ScopesTests
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
            Assert.Throws<ArgumentException> ( ( ) => new Scopes (
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
            Assert.Throws<ArgumentNullException> ( ( ) => new Scopes (
                name! ,
                true ,
                value! ) );
        }

        [Fact]
        public void Constructor_WhenParametersAreValid_ShouldPopulateProperties ( )
        {
            // Act.
            var sut = new Scopes (
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