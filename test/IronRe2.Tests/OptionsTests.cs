using Xunit;

namespace IronRe2.Tests;

public class OptionsTests
{
    [Fact]
    public void OptionsCreateHasDefaultValues()
    {
        //Given
        Options options = new();

        //Then
        Assert.False(options.PosixSyntax);
        Assert.False(options.LongestMatch);
        Assert.True(options.LogErrors);
        Assert.False(options.Literal);
        Assert.False(options.NeverNewline);
        Assert.False(options.DotNewline);
        Assert.False(options.NeverCapture);
        Assert.True(options.CaseSensitive);
        // POSIX-only flags
        Assert.False(options.PerlClasses);
        Assert.False(options.WordBoundary);
        Assert.False(options.OneLine);

        Assert.Equal(RegexEncoding.Utf8, options.Encoding);
        Assert.Equal(8 << 20, options.MaxMemory);
    }

    [Fact]
    public void OptionsSetValuesStoresValues()
    {
        //Given
        Options options = new()
        {
            //When
            PosixSyntax = true,
            LongestMatch = true,
            LogErrors = false,
            Literal = true,
            NeverNewline = true,
            DotNewline = true,
            NeverCapture = true,
            CaseSensitive = false,
            PerlClasses = true,
            WordBoundary = true,
            OneLine = true,
            Encoding = RegexEncoding.Latin1,
            MaxMemory = 700
        };

        //Then
        Assert.True(options.PosixSyntax);
        Assert.True(options.LongestMatch);
        Assert.False(options.LogErrors);
        Assert.True(options.Literal);
        Assert.True(options.NeverNewline);
        Assert.True(options.DotNewline);
        Assert.True(options.NeverCapture);
        Assert.False(options.CaseSensitive);
        // POSIX-only flags
        Assert.True(options.PerlClasses);
        Assert.True(options.WordBoundary);
        Assert.True(options.OneLine);

        Assert.Equal(RegexEncoding.Latin1, options.Encoding);
        Assert.Equal(700, options.MaxMemory);
    }
}
