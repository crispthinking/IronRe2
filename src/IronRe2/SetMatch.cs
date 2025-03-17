using System;
using System.Collections.Generic;

namespace IronRe2;

/// <summary>
///     Match information for a <see cref="RegexSet" />.
/// </summary>
public class SetMatch
{
    internal SetMatch(UIntPtr matchCount, int[] matchingPatterns)
    {
        MatchCount = (int)matchCount;
        MatchingPatterns = matchingPatterns;
    }

    /// <summary>
    ///     Convenience property to check if _any_ of the patterns matched
    /// </summary>
    public bool Matched => MatchCount > 0;

    /// <summary>
    ///     Get the number of patterns in the set which matched
    /// </summary>
    public int MatchCount { get; }

    /// <summary>
    ///     Get the indices or the patterns in the set which matched
    /// </summary>
    public IReadOnlyCollection<int> MatchingPatterns { get; }
}
