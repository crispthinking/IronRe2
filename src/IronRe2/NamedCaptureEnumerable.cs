using System.Collections;
using System.Collections.Generic;

namespace IronRe2
{
    /// <summary>
    /// Enumerable of named capture groups
    /// </summary>
    internal class NamedCaptureEnumerable : IEnumerable<NamedCaptureGroup>
    {
        private Regex _regex;

        public NamedCaptureEnumerable(Regex regex)
        {
            _regex = regex;
        }

        public IEnumerator<NamedCaptureGroup> GetEnumerator()
        {
            return new NamedCaptureEnumerator(_regex);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
