using FluentAssertions;
using ScoreTracker.Server.MeetResultsProviders.MyUsaGym;
using Xunit;

namespace ScoreTracker.Server.Test
{
    public class SessionResultSetTest
    {
        [Fact]
        public void Level_ShouldBeIntegerWithoutDivision_WhenValueIsSingleInteger()
        {
            var resultSet = new SessionResultSet("1", "5", "All", 1);

            resultSet.GetLevel().Should().Be("5");
        }

        [Fact]
        public void Level_ShouldBeIntegerWithoutDivision_WhenValueIsSingleIntegerWithDivision()
        {
            var resultSet = new SessionResultSet("1", "51", "All", 1);

            resultSet.GetLevel().Should().Be("5");
        }

        [Fact]
        public void Level_ShouldBeIntegerWithoutDivision_WhenValueIsMultipleCommaSeparatedLevelsOfSameLevel()
        {
            var resultSet = new SessionResultSet("1", "51, 5,52", "All", 1);

            resultSet.GetLevel().Should().Be("5");
        }

        [Fact]
        public void Level_ShouldBeUnknown_WhenValueIsMultipleCommaSeparatedLevelsOfDifferentLevels()
        {
            var resultSet = new SessionResultSet("1", "5,6,10", "All", 1);

            resultSet.GetLevel().Should().Be("Unknown");
        }
    }
}