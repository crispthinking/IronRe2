using System;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace IronRe2.Tests
{
    public class RegexSetTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public RegexSetTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void RegexSetCreate()
        {
            //Given
            var patterns = new[] {
                @"hello world",
                @".+",
                @"(\d{2,4})"
            };

            //When
            var set = new RegexSet(patterns);

            //Then
            Assert.Equal(3, set.Count);
        }

        [Fact]
        public void CreateSetWithOptions()
        {
            //Given
            var set = new RegexSet(new[] {
                "()",
                "[]",
                "."
            }, new Options
            {
                Literal = true,
            });

            //When
            var parenMatches = set.Match("I have some () in");
            var dotMatches = set.Match("I am the container of a . dot");
            var bland = set.Match("boring");

            //Then
            Assert.True(parenMatches.Matched);
            Assert.True(dotMatches.Matched);
            Assert.False(bland.Matched);
        }

        [Fact]
        public void CreateSetWithBytesOptions()
        {
            //Given
            var set = new RegexSet(new[] {
                Encoding.UTF8.GetBytes("()"),
                Encoding.UTF8.GetBytes("[]"),
                Encoding.UTF8.GetBytes("."),
            }, new Options
            {
                Literal = true,
            });

            //When
            var parenMatches = set.Match("I have some () in");
            var dotMatches = set.Match("I am the container of a . dot");
            var bland = set.Match("boring");

            //Then
            Assert.True(parenMatches.Matched);
            Assert.True(dotMatches.Matched);
            Assert.False(bland.Matched);
        }

        [Fact]
        public void RegexSetThrowsWithInvalidPattern()
        {
            var ex = Assert.Throws<RegexCompilationException>(() =>
            {
                new RegexSet(new[]
                {
                    "I'm OK",
                    ")unmatched]parens",
                });
            });

            // Debugging output
            _testOutputHelper.WriteLine($"Actual Exception Message: \"{ex.Message}\"");
            // Check the real message
            Assert.True(
                ex.Message.Contains("missing )") || 
                ex.Message.Contains("unexpected )"),
                $"Unexpected error message: {ex.Message}"
            );
        }

        [Fact]
        public void RegexSetMatch()
        {
            //Given
            var set = new RegexSet(new[] {
                @"\w+", // words
                @"\d+", // digits
                @"\d{4}-\d{2}-\d{2}", // dates 
            });

            //When
            var matches = set.Match("I have 1 date: 1969-07-11");

            //Then
            Assert.True(matches.Matched);
            Assert.Equal(3, matches.MatchCount);
            Assert.Collection(
                matches.MatchingPatterns,
                p => Assert.Equal(0, p),
                p => Assert.Equal(1, p),
                p => Assert.Equal(2, p));
        }


        [Fact]
        public void RegexSetMatchWithBytes()
        {
            //Given
            var set = new RegexSet(new[] {
                @"\w+", // words
                @"\d+", // digits
                @"\d{4}-\d{2}-\d{2}", // dates 
            });

            //When
            var matches = set.Match(
                Encoding.UTF8.GetBytes("I have 1 date: 1969-07-11"));

            //Then
            Assert.True(matches.Matched);
            Assert.Equal(3, matches.MatchCount);
            Assert.Collection(
                matches.MatchingPatterns,
                p => Assert.Equal(0, p),
                p => Assert.Equal(1, p),
                p => Assert.Equal(2, p));
        }
    }
}
