using FluentAssertions;
using PortfolioTracker.Core;
using System;
using System.Collections.Generic;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class PortfolioGroupTests
    {
        [Fact]
        public void Constructor_Initializes_All_Values()
        {
            //arrange.
            var id = Guid.NewGuid();
            var name = "name123";
            var portfolioIdList = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid() 
            };
            var notes = "notes123";

            //act.
            var sut = new PortfolioGroup(id, name, portfolioIdList, notes);

            //assert.
            sut.Id.Should().Be(id);
            sut.Name.Should().Be(name);
            sut.PortfolioIdList.Should().Equal(portfolioIdList);
            sut.Notes.Should().Be(notes);
        }

        [Fact]
        public void Constructor_Initializes_Empty_PortfolioIdList_If_None_Passed()
        {
            //arrange / act.
            var sut = new PortfolioGroup(Guid.NewGuid(), "name");

            //assert.
            sut.PortfolioIdList.Should().NotBeNull();
            sut.PortfolioIdList.Should().BeEmpty();
        }

        public static object[][] IsSameAs_TestData
        {
            get
            {
                var sut = new PortfolioGroup(Guid.NewGuid(), "name123", new List<Guid> { Guid.NewGuid() }, "notes123");
                var clone = new PortfolioGroup(sut.Id, sut.Name, sut.PortfolioIdList, sut.Notes);
                var same = new PortfolioGroup(sut.Id, "other name");
                var different = new PortfolioGroup(Guid.NewGuid(), sut.Name, sut.PortfolioIdList, sut.Notes);
                var veryDifferent = new object();

                return new[]
                {
                    new object[] { sut, sut, true },
                    new object[] { sut, clone, true },
                    new object[] { sut, same, true },
                    new object[] { sut, different, false },
                    new object[] { sut, veryDifferent, false },
                    new object[] { sut, null, false }
                };
            }
        }

        [Theory]
        [MemberData(nameof(IsSameAs_TestData))]
        public void IsSameAs_Compares_As_Entities(PortfolioGroup sut, object other, bool same)
        {
            //act.
            var isSame = sut.IsSameAs(other);

            //assert.
            isSame.Should().Be(same);
        }
    }
}
