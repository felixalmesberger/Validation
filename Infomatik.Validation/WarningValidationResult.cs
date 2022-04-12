using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation;

/// <summary>
/// Validation result indicating a warning, not an error
/// </summary>
public class WarningValidationResult : ValidationResult
{
  public WarningValidationResult(string errorMessage)
    : base(errorMessage)
  {
  }

  public WarningValidationResult(string errorMessage, IEnumerable<string> memberNames)
    : base(errorMessage, memberNames)
  {
  }

  public WarningValidationResult(ValidationResult validationResult)
    : base(validationResult)
  {
  }
}