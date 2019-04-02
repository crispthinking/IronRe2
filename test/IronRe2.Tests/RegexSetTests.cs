using Xunit;

namespace IronRe2.Tests
{
    public class RegexSetTests
    {
        [Fact]
        public void RegexSetCreate()
        {
        //Given
            var patterns = new [] {
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
        public void RegexSetThrowsWithInvalidPattern()
        {
            var ex = Assert.Throws<RegexCompilationException>(() =>
            {
                new RegexSet(new [] {
                    "I'm OK",
                    ")unmatched]parens",
                });
            });
            Assert.Contains("missing )", ex.Message);
            Assert.Equal(")unmatched]parens", ex.OffendingPortion);
        }

        [Fact]
        public void RegexSetMatch()
        {
        //Given
            var set = new RegexSet(new [] {
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
    }
}
