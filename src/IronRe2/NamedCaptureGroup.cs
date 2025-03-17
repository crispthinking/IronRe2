namespace IronRe2;

/// <summary>
///     Named Capture Group Information
/// </summary>
public class NamedCaptureGroup
{
    internal NamedCaptureGroup(string name, int index)
    {
        Name = name;
        Index = index;
    }

    /// <summary>
    ///     The name of the capture group
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The index in the captures array for this
    /// </summary>
    public int Index { get; }
}
