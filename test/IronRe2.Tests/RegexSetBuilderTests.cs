using System.Linq;
using System.Text;
using Xunit;

namespace IronRe2.Tests
{
    public sealed class RegexSetBuilderTests
    {
        [Fact]
        public void CreateEmptySet()
        {
        //Given
            var builder = new RegexSetBuilder();
        
        //When
            var set = builder.Build();
        
        //Then
            Assert.Equal(0, set.Count);
        }

        [Fact]
        public void CreateSetWithMultiplePatterns()
        {
        //Given
            var builder = new RegexSetBuilder();
        
        //When
            var first = builder.Add("hello");
            var second = builder.Add(Encoding.UTF8.GetBytes("world"));
            var set = builder.Build();
        
        //Then
            Assert.Equal(0, first);
            Assert.Equal(1, second);
            Assert.Equal(2, set.Count);
        }

        [Fact]
        public void SetWithOptionsMatchesCorrectly()
        {
        //Given
            var builder = new RegexSetBuilder(new Options { Literal = true });
            var sqares = builder.Add("[]");
            builder.Add("()");
            var curlies = builder.Add("{}");
            var dot = builder.Add(".");
            var set = builder.Build();
        
        //When
            var parenMatches = set.Match("I have some {} and [] in");
            var dotMatches = set.Match("I am the container of a . dot");
            var bland = set.Match("boring");

        //Then
            Assert.True(parenMatches.Matched);
            Assert.Equal(2, parenMatches.MatchCount);
            Assert.Collection(parenMatches.MatchingPatterns.OrderBy(p => p),
                p => Assert.Equal(sqares, p),
                p => Assert.Equal(curlies, p));
            Assert.True(dotMatches.Matched);
            Assert.Equal(1, dotMatches.MatchCount);
            Assert.Collection(dotMatches.MatchingPatterns,
                p => Assert.Equal(dot, p));
            Assert.False(bland.Matched);
            Assert.Equal(0, bland.MatchCount);
            Assert.Empty(bland.MatchingPatterns);
        }
    }
}