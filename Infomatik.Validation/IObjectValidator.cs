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
  /// <param name="cancelOnFirstError">Cancel validation after first error</param>
  /// <returns>Result of the validation</returns>
  ObjectValidationResult Validate(object instance, bool cancelOnFirstError);
}