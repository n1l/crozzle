using System;

namespace CrozzleEngine.Exceptions
{
    public class ModelLoadException : Exception
    {
        /// <summary>
        /// Model load exception ctor
        /// </summary>
        /// <param name="message">Message</param>
        public ModelLoadException(string message) : base(message)
        {
        }

        /// <summary>
        /// Model load exception ctor
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        public ModelLoadException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
