# Iron RE2

RE2 is a [fast and powerful regular expression library][re2] created and
maintained by Google. This repository provides a managed wrapper so it can
be used from .NET.

IronRe2 targets the .NET Standard 2.0 framework. This means it is can be used
from both .NET Core and .NET Framework.

## 🐉 Here be Dragons 🐉

This repository is under development. There are still many things TODO:

 [x] - Provide [pre-built pacakges][batteries] containing the native RE2 code
 [ ] - Provide a managed wrapper of the RE2 classes
    [x] - Regex - wrapping base pattern matches
    [x] - RegexSet - wrapping regex set operations
    [ ] - Options - wrapping options object
 [x] - Setup a CI pipeline to produce packages
 [ ] - Update README to include example usage
 [ ] - Create some benchmarks with BenchmarkDotNet

## License

This repository is distributed under [the MIT license][mit-license].

 [re2]: https://github.com/google/re2
 [batteries]: https://github.com/crispthinking/IronRure-Batteries
 [mit-license]: https://opensource.org/licenses/MIT
