using Xunit;

namespace IronRe2.Tests;

public class MetaTests
{
    [Fact]
    public void MetaGetVersionString()
    {
        //Given

        //When
        var version = Meta.VersionString;

        //Then
        Assert.Equal("0.0.0", version);
    }

    [Fact]
    public void MetaGetVersion()
    {
        //Given

        //When
        var version = Meta.Version;

        //Then
        Assert.Equal(0, version.current);
        Assert.Equal(0, version.revision);
        Assert.Equal(0, version.age);
    }
}
