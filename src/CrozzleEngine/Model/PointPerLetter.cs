namespace CrozzleEngine.Model
{
    /// <summary>
    /// Point per letter model object
    /// </summary>
    public class PointPerLetter
    {
        /// <summary>
        /// Letter
        /// </summary>
        public char Letter { get; set; }

        /// <summary>
        /// Point size
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Point type
        /// </summary>
        public LetterPointType PointType { get; set; }
    }
}
