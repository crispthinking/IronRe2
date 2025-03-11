namespace IronRe2;

/// <summary>
/// Managed alternative to the `cre2_range_t` structure
/// <para>
///  We can't use the `cre2_strings_to_ranges` method directly as the size
///  of the `cre2_range_t` structure varies depending on if the platform is
///  LP64 or LLP64. To solve this we do the pointer arithmetic in managed
///  code and store the result in this structure instead.
/// </para>
/// </summary>
internal struct ByteRange
{
    internal long start;
    internal long past;
}
