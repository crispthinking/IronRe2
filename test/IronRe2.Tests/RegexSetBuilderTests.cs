using System.Linq;
using Xunit;

namespace IronRe2.Tests;

public sealed class RegexSetBuilderTests
{
    [Fact]
    public void CreateEmptySet()
    {
        //Given
        RegexSetBuilder builder = new();

        //When
        RegexSet set = builder.Build();

        //Then
        Assert.Equal(0, set.Count);
    }

    [Fact]
    public void CreateSetWithMultiplePatterns()
    {
        //Given
        RegexSetBuilder builder = new();

        //When
        int first = builder.Add("hello");
        int second = builder.Add("world"u8.ToArray());
        RegexSet set = builder.Build();

        //Then
        Assert.Equal(0, first);
        Assert.Equal(1, second);
        Assert.Equal(2, set.Count);
    }

    [Fact]
    public void SetWithOptionsMatchesCorrectly()
    {
        //Given
        RegexSetBuilder builder = new(new Options { Literal = true });
        int squares = builder.Add("[]");
        builder.Add("()");
        int curlies = builder.Add("{}");
        int dot = builder.Add(".");
        RegexSet set = builder.Build();

        //When
        SetMatch parenMatches = set.Match("I have some {} and [] in");
        SetMatch dotMatches = set.Match("I am the container of a . dot");
        SetMatch bland = set.Match("boring");

        //Then
        Assert.True(parenMatches.Matched);
        Assert.Equal(2, parenMatches.MatchCount);
        Assert.Collection(parenMatches.MatchingPatterns.Order(),
            p => Assert.Equal(squares, p),
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
