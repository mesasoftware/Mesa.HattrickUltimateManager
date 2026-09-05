namespace Mesa.HUM.UI.Desktop.Tests.Views.Components
{
    using System.Linq;
    using Avalonia;
    using Avalonia.Media;
    using Mesa.HUM.UI.Desktop.Views.Components;
    using Mesa.HUM.UI.Desktop.Views.Components.Enums;

    public class CustomButtonTests
    {
        private static readonly string [ ] DistributionPseudoClasses =
        [
            ":image-after-text" ,
            ":image-above-text" ,
            ":image-below-text"
        ];

        public class ConstructorTests
        {
            [Fact]
            public void Constructor_ShouldDefaultProperties ( )
            {
                // Act.
                var sut = new CustomButton ( );

                // Assert.
                Assert.Equal ( ContentDistribution.ImageBeforeText , sut.Distribution );
                Assert.Null ( sut.Image );
                Assert.Null ( sut.Text );
            }

            [Fact]
            public void Constructor_WhenDistributionIsDefault_ShouldNotSetAnyDistributionPseudoClass ( )
            {
                // Act.
                var sut = new CustomButton ( );

                // Assert.
                Assert.DoesNotContain ( ":image-after-text" , sut.Classes );
                Assert.DoesNotContain ( ":image-above-text" , sut.Classes );
                Assert.DoesNotContain ( ":image-below-text" , sut.Classes );
            }
        }

        public class DistributionTests
        {
            [Theory]
            [InlineData ( ContentDistribution.ImageAfterText , ":image-after-text" )]
            [InlineData ( ContentDistribution.ImageAboveText , ":image-above-text" )]
            [InlineData ( ContentDistribution.ImageBelowText , ":image-below-text" )]
            public void Distribution_WhenSet_ShouldSetOnlyMatchingPseudoClass (
                ContentDistribution distribution ,
                string expectedPseudoClass )
            {
                // Arrange.
                var sut = new CustomButton
                {
                    // Act.
                    Distribution = distribution
                };

                // Assert.
                Assert.Contains ( expectedPseudoClass , sut.Classes );

                foreach ( string pseudoClass in DistributionPseudoClasses.Where ( x => x != expectedPseudoClass ) )
                {
                    Assert.DoesNotContain ( pseudoClass , sut.Classes );
                }
            }

            [Fact]
            public void Distribution_WhenSet_ShouldUpdateValue ( )
            {
                // Arrange.
                var sut = new CustomButton
                {
                    // Act.
                    Distribution = ContentDistribution.ImageAboveText
                };

                // Assert.
                Assert.Equal ( ContentDistribution.ImageAboveText , sut.Distribution );
            }

            [Fact]
            public void Distribution_WhenSetBackToImageBeforeText_ShouldClearDistributionPseudoClasses ( )
            {
                // Arrange.
                var sut = new CustomButton ( )
                {
                    Distribution = ContentDistribution.ImageAfterText
                };

                // Act.
                sut.Distribution = ContentDistribution.ImageBeforeText;

                // Assert.
                Assert.DoesNotContain ( ":image-after-text" , sut.Classes );
                Assert.DoesNotContain ( ":image-above-text" , sut.Classes );
                Assert.DoesNotContain ( ":image-below-text" , sut.Classes );
            }
        }

        public class ImageTests
        {
            [Fact]
            public void Image_WhenSet_ShouldUpdateValue ( )
            {
                // Arrange.
                var sut = new CustomButton ( );

                // Constructing a geometry only sets its properties; the platform impl is
                // realized lazily and is never touched by assigning/reading the styled
                // property, so this needs no rendering platform.
                Geometry geometry = new RectangleGeometry ( new Rect ( 0 , 0 , 10 , 10 ) );

                // Act.
                sut.Image = geometry;

                // Assert.
                Assert.Same ( geometry , sut.Image );
            }
        }

        public class TextTests
        {
            [Fact]
            public void Text_WhenSet_ShouldNotAffectDistributionPseudoClasses ( )
            {
                // Arrange.
                var sut = new CustomButton
                {
                    // Act.
                    Text = "Save"
                };

                // Assert.
                Assert.DoesNotContain ( ":image-after-text" , sut.Classes );
                Assert.DoesNotContain ( ":image-above-text" , sut.Classes );
                Assert.DoesNotContain ( ":image-below-text" , sut.Classes );
            }

            [Fact]
            public void Text_WhenSet_ShouldUpdateValue ( )
            {
                // Arrange.
                var sut = new CustomButton
                {
                    // Act.
                    Text = "Save"
                };

                // Assert.
                Assert.Equal ( "Save" , sut.Text );
            }
        }
    }
}