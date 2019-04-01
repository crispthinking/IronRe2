using System;
using Xunit;
using IronRe2;

namespace IronRe2.Tests
{
    public class RegexTests
    {
        [Fact]
        public void InvalidRegexThrowsException()
        {
            var ex = Assert.Throws<RegexCompilationException>(() =>
            {
                new Regex("(");
            });
            Assert.Contains("missing )", ex.Message);
        }

        [Fact]
        public void RegexCreateWithPatternExposesPattern()
        {
        //Given
            var regex = new Regex(".+");

        //When
            var pattern = regex.Pattern;

        //Then
            Assert.Equal(".+", pattern);
        }

        [Fact]
        public void RegexCreateEsposesProgramSize()
        {
        //Given
            var helloRe = new Regex("hello world");
            var emptyRe = new Regex("");

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
            var regex = new Regex("(.+) (?P<foo>.*)");

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
        [InlineData(".+", "hello world", true)]
        [InlineData("hello", "hello world", true)]
        [InlineData("world", "hello world", true)]
        [InlineData(@"\s+", "hello world", true)]
        [InlineData(".", "", false)]
        [InlineData("invalid", "i'm Ok", false)]
        public void RegexEasyIsMatch(string pattern, string haystack, bool match)
        {
            Assert.Equal(match, Regex.IsMatch(pattern, haystack));
        }

        
        [Theory]
        [InlineData(@".+", "hello world", 0, 11)]
        [InlineData(@"\b[^\s]+\b", "hello world", 0, 5)]
        [InlineData(@"\b[^\s]+\b$", "hello world", 6, 11)]
        [InlineData(@".\b", "foo bar", 2, 3)]
        [InlineData(@"b", "foo bar", 4, 5)]
        [InlineData(@"b", "nothing to see here", -1, -1)]
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
    }
}
