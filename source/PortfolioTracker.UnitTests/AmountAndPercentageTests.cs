using PortfolioTracker.Core;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class AmountAndPercentageTests
    {
        [Fact]
        public void Constructor_Initializes_All_Properties()
        {
            //arrange.
            var expectedAmount = 123m;
            var expectedPercentage = 321m;

            //act.
            var sut = new AmountAndPercentage(expectedAmount, expectedPercentage);

            //assert.
            Assert.Equal(expectedAmount, sut.Amount);
            Assert.Equal(expectedPercentage, sut.Percentage);
        }
    }
}
