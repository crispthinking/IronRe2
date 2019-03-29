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

        [Fact]
        public void RegexEasymatch()
        {
            Assert.True(Regex.IsMatch(".+", "hello world"));
            Assert.True(Regex.IsMatch("hello", "hello world"));
            Assert.True(Regex.IsMatch("world", "hello world"));
            Assert.True(Regex.IsMatch(@"\s+", "hello world"));
            Assert.False(Regex.IsMatch(".", ""));
            Assert.False(Regex.IsMatch("invalid", "i'm Ok"));
        }
    }
}
