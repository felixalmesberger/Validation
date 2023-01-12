using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation;

/// <summary>
/// Validate objects
/// </summary>
public interface IObjectValidator
{
  /// <summary>
  /// Validates an object
  /// </summary>
  /// <param name="instance">The object instance to test</param>
  /// <param name="breakOnFirstError">Cancel validation after first error</param>
  /// <returns>Result of the validation</returns>
  ObjectValidationResult Validate(object instance, bool breakOnFirstError, ValidationContext? context = null);
}