using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CrozzleEngine.Model
{
  /// <summary>
  /// Logger with rolling
  /// </summary>
  public static class CrozzleLogger
  {
    private const string SEARCH_PATTERN = "AppLogSession\\d+.log";
    private const string DIGIT_PATTERN = "\\d+";

    private const string CREATE_PATTERN = "AppLogSession{0}.log";
    private const string LOG_MSG_PATTERN = "[CROZZLE APP] ERROR: {0}";

    private const string LOG_INIT_FILE_MSG_PATTERN = "[CROZZLE APP] START PROCESSING FILE: {0}";
    private const string LOG_END_FILE_MSG_PATTERN = "[CROZZLE APP] END PROCESSING FILE: {0} WITH RESULT: {1}";
    private const string LOG_INIT_CROZZLE_MSG_PATTERN = "[CROZZLE APP] START PROCESSING CROZZLE: {0}";
    private const string LOG_END_CROZZLE_MSG_PATTERN = "[CROZZLE APP] END PROCESSING CROZZLE: {0}, WITH RESULT: {1}";


    private static readonly string filePath;

    static CrozzleLogger()
    {

      var index = Getindex();
      var pattern = GetPattern();
      filePath = string.Format(pattern, index);
    }

    /// <summary>
    /// Get pattern
    /// </summary>
    /// <returns>pattern</returns>
    private static string GetPattern()
    {
      return AppDomain.CurrentDomain.BaseDirectory + CREATE_PATTERN;
    }

    /// <summary>
    /// Get index
    /// </summary>
    /// <returns>index</returns>
    private static int Getindex()
    {
      var max = 0;
      var regEx = new Regex(SEARCH_PATTERN);
      var dRegEx = new Regex(DIGIT_PATTERN);
      foreach (var item in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
      {
        var fname = Path.GetFileName(item);
        if (regEx.IsMatch(fname))
        {
          var match = dRegEx.Match(fname);
          var tmp = match.Success ? int.Parse(match.Groups[0].Value) : 0;
          max = max > ++tmp ? max : tmp;
        }
      }

      return max;
    }

    /// <summary>
    /// Log to file
    /// </summary>
    /// <param name="message">Message for logging</param>
    public static void Log(string message)
    {
      using (var logStream = File.Open(filePath, FileMode.Append))
      using (var logWriter = new StreamWriter(logStream))
      {
        var msg = string.Format(LOG_MSG_PATTERN, message);
        logWriter.WriteLine(msg);
      }
    }

    /// <summary>
    /// Init Log crozzle or file messages
    /// </summary>
    /// <param name="fileName">Name of file for analize, use as well as crozzle name</param>
    /// <param name="crozzle">For crozzle messages used different patterns</param>
    public static void InitLog(string fileName, bool crozzle)
    {
      using (var logStream = File.Open(filePath, FileMode.Append))
      using (var logWriter = new StreamWriter(logStream))
      {
        var msg = crozzle ? string.Format(LOG_INIT_CROZZLE_MSG_PATTERN, fileName) 
                          : string.Format(LOG_INIT_FILE_MSG_PATTERN, fileName);
        logWriter.WriteLine(msg);
      }
    }

    /// <summary>
    /// End Log crozzle or file messages
    /// </summary>
    /// <param name="fileName">Name of file for analize, use as well as crozzle name</param>
    /// <param name="crozzle">For crozzle messages used different patterns</param>
    /// <param name="success">Is a result was successfull</param>
    public static void EndLog(string fileName, bool crozzle, bool success)
    {
      using (var logStream = File.Open(filePath, FileMode.Append))
      using (var logWriter = new StreamWriter(logStream))
      {
        var msg = crozzle ? string.Format(LOG_END_CROZZLE_MSG_PATTERN, fileName, success ? "SUCCESS" : "FAIL") 
                          : string.Format(LOG_END_FILE_MSG_PATTERN, fileName, success ? "SUCCESS" : "FAIL");
        logWriter.WriteLine(msg);
        logWriter.WriteLine(); //add space between different files
      }
    }
  }
}
