using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation;

/// <summary>
/// Validation result indicating the property is required and missing
/// </summary>
public class RequiredValidationResult : ValidationResult
{
  internal RequiredValidationResult(ValidationResult validationResult)
    : base(validationResult)
  {
  }

  public RequiredValidationResult(string? errorMessage)
    : base(errorMessage)
  {
  }

  public RequiredValidationResult(string? errorMessage, IEnumerable<string>? memberNames)
    : base(errorMessage, memberNames)
  {
  }
}