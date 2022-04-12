using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation;

/// <summary>
/// Convenient extensions for Validatable Objects
/// to create  Validation Results
/// </summary>
public static class ValidatableObjectExtensions
{
  /// <summary>
  /// Creates a warning <see cref="ValidationResult"/>
  /// </summary>
  public static ValidationResult CreateWarning(this IValidatableObject _, string message, params string[] memberNames)
    => new WarningValidationResult(message, memberNames);

  /// <summary>
  /// Creates an error <see cref="ValidationResult"/>
  /// </summary>
  public static ValidationResult CreateError(this IValidatableObject _, string message, params string[] memberNames)
    => new ValidationResult(message, memberNames);

  /// <summary>
  /// Creates a required <see cref="ValidationResult"/>
  /// </summary>
  public static ValidationResult CreateMissing(this IValidatableObject _, string memberName)
    => new RequiredValidationResult($"Required field '{memberName}'", new[] { memberName });
}