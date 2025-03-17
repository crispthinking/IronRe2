using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace IronRe2.Tests;

public class RegexTests
{
    [Fact]
    public void InvalidRegexThrowsException()
    {
        var ex = Assert.Throws<RegexCompilationException>(() => new Regex("foo (bar"));
        Assert.Contains("missing )", ex.Message);
        Assert.Equal("foo (bar", ex.OffendingPortion);
    }

    [Fact]
    public void RegexCreateWithPatternExposesPattern()
    {
        //Given
        Regex regex = new(".+");

        //When
        var pattern = regex.Pattern;

        //Then
        Assert.Equal(".+", pattern);
    }

    [Fact]
    public void RegexCreateWithOptions()
    {
        //Given
        Regex regex = new(@"\n", new Options { NeverNewline = true });

        //When
        var match = regex.IsMatch("foo\nbar");

        //Then
        Assert.False(match);
    }

    [Fact]
    public void RegexCreateEsposesProgramSize()
    {
        //Given
        Regex helloRe = new("hello world");
        Regex emptyRe = new("");

        //When
        var helloSize = helloRe.ProgramSize;
        var emptySize = emptyRe.ProgramSize;

        //Then
        Assert.Equal(15, helloSize);
        Assert.Equal(4, emptySize);
    }

    [Fact]
    public void RegexCreateExposesCaptureGroupInfo()
    {
        //Given
        Regex regex = new("(.+) (?P<foo>.*)");

        //When
        var numCaptures = regex.CaptureGroupCount;
        var fooCaptureId = regex.FindNamedCapture("foo");
        var invalidCaptureId = regex.FindNamedCapture("bar");

        //Then
        Assert.Equal(2, numCaptures);
        Assert.Equal(2, fooCaptureId);
        Assert.Equal(-1, invalidCaptureId);
    }

    [Theory]
    [MemberData(nameof(IsMatchData))]
    public void RegexEasyIsMatch(string pattern, string haystack, bool match)
    {
        Assert.Equal(match, Regex.IsMatch(pattern, haystack));
    }

    [Theory]
    [MemberData(nameof(IsMatchData))]
    public void RegexIsMatch(string pattern, string haystack, bool match)
    {
        using Regex re = new(pattern);
        Assert.Equal(match, re.IsMatch(haystack));
    }

    [Theory]
    [MemberData(nameof(IsMatchData))]
    public void RegexBytesIsMatch(string p, string h, bool match)
    {
        var pattern = Encoding.UTF8.GetBytes(p);
        var haystack = Encoding.UTF8.GetBytes(h);
        using Regex re = new(pattern);
        Assert.Equal(match, re.IsMatch(haystack));
    }

    [Theory]
    [MemberData(nameof(FindData))]
    public void RegexEasyFind(string pattern, string haystack, int start, int end)
    {
        var match = Regex.Find(pattern, haystack);
        if (start != -1)
        {
            Assert.True(match.Matched);
            Assert.Equal(start, match.Start);
            Assert.Equal(end, match.End);
        }
        else
        {
            Assert.False(match.Matched);
        }
    }

    [Theory]
    [MemberData(nameof(FindData))]
    public void RegexFind(string pattern, string haystack, int start, int end)
    {
        using Regex re = new(pattern);
        var match = re.Find(haystack);
        if (start != -1)
        {
            Assert.True(match.Matched);
            Assert.Equal(start, match.Start);
            Assert.Equal(end, match.End);
        }
        else
        {
            Assert.False(match.Matched);
        }
    }

    [Theory]
    [MemberData(nameof(FindData))]
    public void RegexByteFind(string p, string h, int start, int end)
    {
        var pattern = Encoding.UTF8.GetBytes(p);
        var haystack = Encoding.UTF8.GetBytes(h);
        using Regex re = new(pattern);
        var match = re.Find(haystack);
        if (start != -1)
        {
            Assert.True(match.Matched);
            Assert.Equal(start, match.Start);
            Assert.Equal(end, match.End);
        }
        else
        {
            Assert.False(match.Matched);
        }
    }

    [Fact]
    public void RegexFindAll()
    {
        Regex re = new(@"\b\w+\b");

        List<Match> matches = [.. re.FindAll("hello world")];

        Assert.Collection(matches,
            m =>
            {
                Assert.Equal(0, m.Start);
                Assert.Equal(5, m.End);
                Assert.Equal("hello", m.ExtractedText);
            },
            m =>
            {
                Assert.Equal(6, m.Start);
                Assert.Equal(11, m.End);
                Assert.Equal("world", m.ExtractedText);
            });
    }

    [Fact]
    public void RegexFindAllWithZeroSizedMatch()
    {
        Regex re = new(@"\b");

        List<Match> matches = [.. re.FindAll(" fizz 9 buzz ")];

        Assert.Collection(matches,
            m =>
            {
                Assert.Equal(1, m.Start);
                Assert.Equal(1, m.End);
                Assert.Equal("", m.ExtractedText);
            },
            m =>
            {
                Assert.Equal(5, m.Start);
                Assert.Equal(5, m.End);
                Assert.Equal("", m.ExtractedText);
            },
            m =>
            {
                Assert.Equal(6, m.Start);
                Assert.Equal(6, m.End);
                Assert.Equal("", m.ExtractedText);
            },
            m =>
            {
                Assert.Equal(7, m.Start);
                Assert.Equal(7, m.End);
                Assert.Equal("", m.ExtractedText);
            },
            m =>
            {
                Assert.Equal(8, m.Start);
                Assert.Equal(8, m.End);
                Assert.Equal("", m.ExtractedText);
            },
            m =>
            {
                Assert.Equal(12, m.Start);
                Assert.Equal(12, m.End);
                Assert.Equal("", m.ExtractedText);
            });
    }

    [Fact]
    public void FindWithCaptures()
    {
        const string pattern = "(?P<h>hello) (?P<w>world)";
        Regex re = new(pattern);

        var captures = re.Captures("hello world");

        Assert.Equal(3, captures.Count);
        Assert.True(captures.Matched);
        Assert.Equal(0, captures.Start);
        Assert.Equal(11, captures.End);
        Assert.True(captures[0].Matched);
        Assert.Equal(0, captures[0].Start);
        Assert.Equal(11, captures[0].End);
        Assert.True(captures[1].Matched);
        Assert.Equal(0, captures[1].Start);
        Assert.Equal(5, captures[1].End);
        Assert.True(captures[2].Matched);
        Assert.Equal(6, captures[2].Start);
        Assert.Equal(11, captures[2].End);
    }

    [Fact]
    public void FindWithCapturesBytes()
    {
        const string pattern = "(?P<h>hello) (?P<w>world)";
        Regex re = new(Encoding.UTF8.GetBytes(pattern));

        var captures = re.Captures(
            "hello world"u8.ToArray());

        Assert.Equal(3, captures.Count);
        Assert.True(captures.Matched);
        Assert.Equal(0, captures.Start);
        Assert.Equal(11, captures.End);
        Assert.True(captures[0].Matched);
        Assert.Equal(0, captures[0].Start);
        Assert.Equal(11, captures[0].End);
        Assert.True(captures[1].Matched);
        Assert.Equal(0, captures[1].Start);
        Assert.Equal(5, captures[1].End);
        Assert.True(captures[2].Matched);
        Assert.Equal(6, captures[2].Start);
        Assert.Equal(11, captures[2].End);
    }

    [Fact]
    public void FindWithOptionalCaptures()
    {
        const string pattern = " (.)(.)? ";
        Regex re = new(pattern);

        var captures = re.Captures(" a ");

        Assert.Equal(3, captures.Count);
        Assert.True(captures.Matched);
        Assert.Equal(0, captures.Start);
        Assert.Equal(3, captures.End);
        Assert.True(captures[0].Matched);
        Assert.Equal(0, captures[0].Start);
        Assert.Equal(3, captures[0].End);
        Assert.True(captures[1].Matched);
        Assert.Equal(1, captures[1].Start);
        Assert.Equal(2, captures[1].End);
        Assert.False(captures[2].Matched);
        Assert.Equal(-1, captures[2].Start);
        Assert.Equal(-1, captures[2].End);
    }

    [Fact]
    public void CaptureNamesReturnsExpectedNames()
    {
        Regex re = new(@"(unnamed_cap): (?P<year>\d{4})-(?P<month>\d{2})(-)(?P<day>\d{2})");

        List<NamedCaptureGroup> namedCaps = [.. re.NamedCaptures().OrderBy(cap => cap.Index)];

        Assert.Collection(namedCaps,
            c =>
            {
                Assert.Equal("year", c.Name);
                Assert.Equal(2, c.Index);
            },
            c =>
            {
                Assert.Equal("month", c.Name);
                Assert.Equal(3, c.Index);
            },
            c =>
            {
                Assert.Equal("day", c.Name);
                Assert.Equal(5, c.Index);
            });
    }

    [Fact]
    public void CapturesWithExtractedText()
    {
        using Regex re = new(@"(?P<year>\d{4})-(?P<month>\d{2})-(?P<day>\d{2})");
        const string haystack = "The first woman in space launched on 1963-06-16 in Vostok 6";

        var captures = re.Captures(haystack);

        Assert.True(captures.Matched);

        // The first capture group is the whole match.
        Assert.True(captures[0].Matched);

        // Each capture group has position information
        Assert.True(captures[1].Matched);
        Assert.Equal(37, captures[1].Start);
        Assert.Equal(41, captures[1].End);

        /// As well as the UTF-8 indices the extracted text is available
        Assert.True(captures[2].Matched);
        Assert.Equal("06", captures[2].ExtractedText);

        // capture group indices can be looked up by name with th `Regex`
        Assert.True(captures[re.FindNamedCapture("day")].Matched);
    }

    [Fact]
    public void CapturesAllReturnsAllMatches()
    {
        Regex re = new(@"(\w+) (\w+)");

        List<Captures> captures = [.. re.CaptureAll("a bee too CD")];

        // We are looking for non-overlapping matches so we don't expect
        // a match on `bee too`.
        Assert.Collection(captures,
            c =>
            {
                Assert.True(c.Matched);
                Assert.Equal(0, c.Start);
                Assert.Equal(5, c.End);
                Assert.Equal("a", c[1].ExtractedText);
                Assert.Equal("bee", c[2].ExtractedText);
            },
            c =>
            {
                Assert.True(c.Matched);
                Assert.Equal(6, c.Start);
                Assert.Equal(12, c.End);
                Assert.Equal("too", c[1].ExtractedText);
                Assert.Equal("CD", c[2].ExtractedText);
            });
    }

    [Fact]
    public void CapturesOutOfBounds()
    {
        //Given
        Regex re = new("\\[([a-z]+)\\]");

        //When
        var match = re.Captures("some [test] content");

        //Then
        Assert.True(match.Matched);
        Assert.True(match[0].Matched);
        Assert.Equal("test", match[1].ExtractedText);
        Assert.Throws<IndexOutOfRangeException>(() => match[2]);
        Assert.Throws<IndexOutOfRangeException>(() => match[-2]);
    }

    public static IEnumerable<object[]> IsMatchData()
    {
        yield return [".+", "hello world", true];
        yield return ["hello", "hello world", true];
        yield return ["world", "hello world", true];
        yield return [@"\s+", "hello world", true];
        yield return [".", "", false];
        yield return ["invalid", "i'm Ok", false];
    }

    public static IEnumerable<object[]> FindData()
    {
        yield return [".+", "hello world", 0, 11];
        yield return [@"\b[^\s]+\b", "hello world", 0, 5];
        yield return [@"\b[^\s]+\b$", "hello world", 6, 11];
        yield return [@".\b", "foo bar", 2, 3];
        yield return ["b", "foo bar", 4, 5];
        yield return ["b", "nothing to see here", -1, -1];
    }
}
