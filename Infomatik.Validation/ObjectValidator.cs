using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation;

/// <summary>
/// Validate objects
/// </summary>
public class ObjectValidator : IObjectValidator
{

  #region properties

  /// <summary>
  /// Service Provider used to create ValidationContext
  /// </summary>
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public IServiceProvider? ServiceProvider { get; set; }

  #endregion

  #region singleton

  public static ObjectValidator Default { get; } = new();

  #endregion

  /// <summary>
  /// Validates an object
  /// </summary>
  /// <param name="instance">The object instance to test</param>
  /// <param name="cancelOnFirstError">Cancel validation after first error</param>
  /// <returns>Result of the validation</returns>
  public ObjectValidationResult Validate(object instance, bool cancelOnFirstError)
  {
    var validationContext = new ValidationContext(instance, this.ServiceProvider, null);

    var results = this.EnumerateValidationResults(instance, validationContext, cancelOnFirstError).ToList();

    var warnings = results.OfType<WarningValidationResult>()
                                              .Cast<ValidationResult>()
                                              .ToList();

    var required = results.OfType<RequiredValidationResult>()
                                              .Cast<ValidationResult>()
                                              .ToList();

    var errors = results.Except(warnings)
                                            .Except(required)
                                            .ToList();

    return new ObjectValidationResult(errors, warnings, required);
  }

  /// <summary>
  /// Evaluates all ValidationAttributes and the IValidatableObject.Validate method
  /// </summary>
  private IEnumerable<ValidationResult> EnumerateValidationResults(object instance, ValidationContext context,
    bool cancelOnFirstError)
  {
    foreach (var validationResult in ValidationAttributeValidator.Validate(instance, context, cancelOnFirstError))
    {
      yield return validationResult;
      if (cancelOnFirstError)
        yield break;
    }

    if (instance is not IValidatableObject validatable)
      yield break;

    foreach (var validationResult in validatable.Validate(context))
    {
      yield return validationResult;
      if (cancelOnFirstError)
        yield break;
    }
  }
}