namespace Mesa.HUM.Tests.Shared.Helpers
{
    using AutoFixture;
    using AutoFixture.AutoMoq;

    public static class FixtureHelper
    {
        public static IFixture GetFixture ( )
        {
            var fixture = new Fixture ( )
                .Customize (
                    new AutoMoqCustomization ( ) );

            fixture.Behaviors.Clear ( );
            fixture.Behaviors.Add (
                new OmitOnRecursionBehavior ( ) );

            return fixture;
        }
    }
}