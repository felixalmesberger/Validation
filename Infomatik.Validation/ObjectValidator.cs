﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Infomatik.Validation;

/// <summary>
/// Validate objects
/// </summary>
public class ObjectValidator : IObjectValidator
{

  internal static readonly string ValidatorKey = "Validator";
  internal static readonly string VisitedKey = "Visited";

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
  /// <param name="breakOnFirstError">Cancel validation after first error</param>
  /// <returns>Result of the validation</returns>
  public ObjectValidationResult Validate(object instance, bool breakOnFirstError = false, ValidationContext? validationContext = null)
  {
    if (validationContext is null)
    {
      var items = new Dictionary<object, object?>()
      {
        { VisitedKey, new HashSet<object>() },
        { ValidatorKey, this }
      };

      validationContext = new ValidationContext(instance, this.ServiceProvider, items);
    }

    var results = this.EnumerateValidationResults(instance, validationContext, breakOnFirstError).ToList();

    var warnings = results.OfType<WarningValidationResult>()
                                              .Cast<ValidationResult>()
                                              .ToList();

    var missing = results.OfType<RequiredValidationResult>()
                                              .Cast<ValidationResult>()
                                              .ToList();

    var errors = results.Except(warnings)
                                            .Except(missing)
                                            .ToList();

    return new ObjectValidationResult(errors, warnings, missing);
  }

  /// <summary>
  /// Evaluates all ValidationAttributes and the IValidatableObject.Validate method
  /// </summary>
  private IEnumerable<ValidationResult> EnumerateValidationResults(object instance, ValidationContext context,
    bool breakOnFirstError)
  {
    foreach (var validationResult in ValidationAttributeValidator.Validate(instance, context, breakOnFirstError))
    {
      yield return validationResult;
      if (breakOnFirstError)
        yield break;
    }

    if (instance is not IValidatableObject validatable)
      yield break;

    foreach (var validationResult in validatable.Validate(context))
    {
      if (validationResult is not null)
        yield return validationResult;
      if (breakOnFirstError)
        yield break;
    }
  }
}