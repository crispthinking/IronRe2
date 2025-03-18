using System.Collections;
using System.Collections.Generic;

namespace IronRe2;

/// <summary>
///     Enumerable of named capture groups
/// </summary>
internal class NamedCaptureEnumerable(Regex regex) : IEnumerable<NamedCaptureGroup>
{
    public IEnumerator<NamedCaptureGroup> GetEnumerator()
    {
        return new NamedCaptureEnumerator(regex);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
