namespace CrozzleEngine.Model
{
  /// <summary>
  /// Regex patterns
  /// </summary>
  public static class Patterns
  {
    /// <summary>
    /// Groups limit pattern
    /// </summary>
    public const string GROUPS_LIMIT_PATTERN = @"GROUPSPERCROZZLELIMIT=\d+$";

    /// <summary>
    /// Point per word pattern
    /// </summary>
    public const string POINTS_PER_WORD_PATTERN = @"POINTSPERWORD=\d+$";

    /// <summary>
    /// Intersecting point pattern
    /// </summary>
    public const string INTERSECTING_POINT_PATTERN = @"INTERSECTING:[A-Z]=\d+$";

    /// <summary>
    /// Nonintersecting point pattern
    /// </summary>
    public const string NONINTERSECTING_POINT_PATTERN = @"NONINTERSECTING:[A-Z]=\d+$";

    /// <summary>
    /// Numeric pattern
    /// </summary>
    public const string NUMERIC_PATTERN = @"\d+$";

    /// <summary>
    /// Letter pattern
    /// </summary>
    public const string LETTER_PATTERN = ":[A-Z]=";
  }
}
