using Xunit;
using Xunit.Abstractions;

namespace IronRe2.Tests;

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
        string[] patterns =
        [
            "hello world",
            ".+",
            @"(\d{2,4})"
        ];

        //When
        RegexSet set = new(patterns);

        //Then
        Assert.Equal(3, set.Count);
    }

    [Fact]
    public void CreateSetWithOptions()
    {
        //Given
        RegexSet set = new([
            "()",
            "[]",
            "."
        ], new Options
        {
            Literal = true
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
        RegexSet set = new([
            "()"u8.ToArray(),
            "[]"u8.ToArray(),
            "."u8.ToArray()
        ], new Options
        {
            Literal = true
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
            new RegexSet([
                "I'm OK",
                ")unmatched]parens"
            ]);
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
        RegexSet set = new([
            @"\w+", // words
            @"\d+", // digits
            @"\d{4}-\d{2}-\d{2}" // dates 
        ]);

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
        RegexSet set = new([
            @"\w+", // words
            @"\d+", // digits
            @"\d{4}-\d{2}-\d{2}" // dates 
        ]);

        //When
        var matches = set.Match(
            "I have 1 date: 1969-07-11"u8);

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
