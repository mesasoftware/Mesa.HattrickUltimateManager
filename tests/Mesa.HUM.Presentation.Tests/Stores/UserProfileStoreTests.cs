namespace Mesa.HUM.Presentation.Tests.Stores
{
    using AutoFixture;
    using Mesa.HUM.Presentation.Stores;
    using Mesa.HUM.Presentation.Stores.Contracts;
    using Mesa.HUM.Tests.Shared.Helpers;

    public class UserProfileStoreTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Constructor_UserProfile_ShouldBeNull ( )
            {
                // Act.
                var sut = new UserProfileStore ( );

                // Assert.
                Assert.Null ( sut.UserProfile );
            }
        }

        public class SetUserProfileTests
        {
            [Fact]
            public void Test ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new UserProfileStore ( );

                bool propertyChangedRaised = true;
                bool propertyChangingRaised = true;

                sut.PropertyChanged += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.UserProfile ) )
                    {
                        propertyChangedRaised = true;
                    }
                };

                sut.PropertyChanging += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.UserProfile ) )
                    {
                        propertyChangingRaised = true;
                    }
                };

                var expected = fixture.Create<UserProfileDto> ( );

                // Act.
                sut.SetUserProfile ( expected );

                // Assert.
                Assert.True ( propertyChangedRaised );
                Assert.True ( propertyChangingRaised );
                Assert.Equal ( expected , sut.UserProfile );
            }
        }
    }
}