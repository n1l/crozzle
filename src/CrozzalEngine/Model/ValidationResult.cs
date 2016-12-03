using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace CrozzleEngine.Model
{
  /// <summary>
  /// Validation result model object
  /// </summary>
  public class ValidationResult
  {
    /// <summary>
    /// Ctor without parameters, validation result is valid 
    /// </summary>
    public ValidationResult()
    {
      IsValid = true;
      Messages = new List<string>();
      CriticalMessages = new List<string>();
    }

    /// <summary>
    /// Ctor, change validation result state
    /// </summary>
    /// <param name="isValid">state of result validation</param>
    public ValidationResult(bool isValid)
    {
      IsValid = isValid;
      Messages = new List<string>();
      CriticalMessages = new List<string>();
    }

    /// <summary>
    /// Contruct validation from messages and state
    /// </summary>
    /// <param name="isValid">State of validation</param>
    /// <param name="messages">Messages</param>
    public ValidationResult(bool isValid, List<string> messages)
    {
      IsValid = isValid;
      Messages = messages;
      CriticalMessages = new List<string>();
      ErrorsCount = messages.Count;
    }

    /// <summary>
    /// Contruct validation from messages and state
    /// </summary>
    /// <param name="isValid">State of validation</param>
    /// <param name="messages">Messages</param>
    /// <param name="criticalMessages">Critical messages</param>
    public ValidationResult(bool isValid, List<string> messages, List<string> criticalMessages)
    {
      Critical = criticalMessages.Any();
      IsValid = isValid;
      Messages = messages;
      CriticalMessages = criticalMessages;
      ErrorsCount = Messages.Count + CriticalMessages.Count;
    }

    /// <summary>
    /// Has critical errors, not show crozzle
    /// </summary>
    public bool Critical { get; set; }

    /// <summary>
    /// State of validation, can be true - sucess, false - failed
    /// </summary>
    [XmlAttribute(AttributeName = "valid")]
    public bool IsValid { get; set; }


    /// <summary>
    /// Is config corrupted
    /// </summary>
    [XmlIgnore]
    public bool ConfigError { get; set; }

    /// <summary>
    /// Exception warning messages
    /// </summary>
    [XmlIgnore]
    public List<string> Messages { get; }

    /// <summary>
    /// Erros messages
    /// </summary>
    [XmlElement(ElementName = "errors")]
    public int ErrorsCount { get; set; }

    /// <summary>
    /// Critical error messages
    /// </summary>
    public List<string> CriticalMessages { get; }

    /// <summary>
    /// Add error, and change state
    /// </summary>
    /// <param name="message">error message</param>
    public void AddError(string message)
    {
      IsValid = false;
      Messages.Add(message);
      ErrorsCount = Messages.Count;
    }

    /// <summary>
    /// Add error, and change state
    /// </summary>
    /// <param name="message">error message</param>
    public void AddCriticalError(string message)
    {
      IsValid = false;
      Critical = true;
      CriticalMessages.Add(message);
      ErrorsCount = Messages.Count + CriticalMessages.Count;
    }

    /// <summary>
    /// Add error, and change state
    /// </summary>
    /// <param name="message">error message</param>
    public void AddConfigError(string message)
    {
      IsValid = false;
      ConfigError = true;
      Messages.Add(message);
      ErrorsCount = Messages.Count;
    }

    /// <summary>
    /// Return all messages from validation
    /// </summary>
    /// <returns>result message</returns>
    public override string ToString()
    {
      return string.Join("\n ,", Critical ? CriticalMessages : Messages);
    }

    /// <summary>
    /// create validation result from others
    /// </summary>
    /// <param name="x">first</param>
    /// <param name="y">second</param>
    /// <returns>result <see cref="ValidationResult"/></returns>
    public static ValidationResult operator +(ValidationResult x, ValidationResult y)
    {
      if (x == null && y == null)
        throw new InvalidOperationException("All parameters can't be null");

      if (x == null)
      {
        return y;
      }
      if (y == null)
      {
        return x;
      }

      var result = new ValidationResult(x.IsValid && y.IsValid,
                                        x.Messages.Concat(y.Messages).ToList(),
                                        x.CriticalMessages.Concat(y.CriticalMessages).ToList());

      return result;
    }

  }
}
