using Xunit;

namespace IronRe2.Tests;

public class MetaTests
{
    [Fact]
    public void MetaGetVersionString()
    {
        //Given

        //When
        string version = Meta.VersionString;

        //Then
        Assert.Equal("0.0.0", version);
    }

    [Fact]
    public void MetaGetVersion()
    {
        //Given

        //When
        (int current, int revision, int age) version = Meta.Version;

        //Then
        Assert.Equal(0, version.current);
        Assert.Equal(0, version.revision);
        Assert.Equal(0, version.age);
    }
}
