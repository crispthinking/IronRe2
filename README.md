# Iron RE2

RE2 is a [fast and powerful regular expression library][re2] created and
maintained by Google. This repository provides a managed wrapper so it can
be used from .NET.

    > Install-Package IronRe2

IronRe2 targets the .NET Standard 2.0 framework. This means it is can be used
from both .NET Core and .NET Framework. If you're targeting Linux, Windows, or
macOS then it's [batteries included][batteries]. If not then you'll need to
supply your own version of the `cre2` library.

## Usage

The simplest operation is to check if a pattern matches a given search text. If
you're just doing this once then you can use the "easy match" API:

```cs
using IronRe2;

Assert.True(Regex.IsMatch(@"\d+", "with 1 digit"));
Assert.False(Regex.IsMatch(@"\d+", "digit-less"));
```

To find the extent of the match then `Regex.Find` can be used:

```cs
using IronRe2;

var match = Regex.Find(@"\d+", "with 1 digit");
Assert.True(match.Match);
Assert.Equal(5, match.Start);
Assert.Equal(6, match.End);
```

If you're going to be re-using the same pattern it makes sense to create a
`Regex` object and hold on to it:

```cs
using IronRe2;

using (var re = new Regex("\w+"))
{
   Assert.False(re.IsMatch("(12323, 232, 4325235)"));

   var match = re.Find("hello world");
   Assert.True(match.Matched);
   Assert.Equal(0, match.Start);
   Assert.Equal(5, match.End);
}
```

To get information about capture group locations compiled `Regex`es offer a
`Captures` API:

```cs
using IronRe2;

using (var re = new Regex(@"(?P<year>\d{4})-(?P<month>\d{2})-(?P<day>\d{2})"))
{
    var haystack = "The first woman in space launched on 1963-06-16 in Vostok 6";

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
```

## 🐉 Here be Dragons 🐉

This repository is under development. There are still many things TODO:

 * [x] - Provide [pre-built pacakges][batteries] containing the native RE2 code
 * [x] - Provide a managed wrapper of the RE2 classes
    * [x] - Regex - wrapping base pattern matches
    * [x] - RegexSet - wrapping regex set operations
    * [x] - Options - wrapping options object
 * [x] - Setup a CI pipeline to produce packages
 * [x] - Update README to include example usage
 * [ ] - Create some benchmarks with BenchmarkDotNet

## License

This repository is distributed under [the MIT license][mit-license].

 [re2]: https://github.com/google/re2
 [batteries]: https://github.com/crispthinking/IronRe2-Batteries
 [mit-license]: https://opensource.org/licenses/MIT
