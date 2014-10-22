public enum TagMatch
{
    /// <summary>
    /// Find objects containing all of these tags
    /// </summary>
    And,
    /// <summary>
    /// Find objects containing any one of these tags
    /// </summary>
    Or,
    /// <summary>
    /// Return all tagged objects except for this with any one of these tags
    /// </summary>
    Not,
    /// <summary>
    /// Find objects that contain all of these tags and no others
    /// </summary>
    Exact
}
