using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Infomatik.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class DeepValidationAttribute : ValidationAttribute
{
  private static readonly string DeepErrorMessage = "Error";
  private static readonly string DeepWarningMessage = "Warning";

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    if (!validationContext.TryGetObjectValidator(out var validator))
      return ValidationResult.Success;

    if (!validationContext.TryGetObjectVisited(out var visited))
      return ValidationResult.Success;

    if (value is null)
      return ValidationResult.Success;

    if (visited!.Contains(value))
      return ValidationResult.Success;

    visited.Add(this);

    var results = new List<ObjectValidationResult>();

    if (value is IEnumerable<object> collection)
      results.AddRange(this.ValidateCollection(collection, validator, visited, validationContext));
    else
      results.Add(this.ValidateObject(value, validator, validationContext));

    return this.WrapResult(results, validationContext);
  }

  private IEnumerable<ObjectValidationResult> ValidateCollection(IEnumerable<object> enumerable, IObjectValidator validator, HashSet<object> visited, ValidationContext validationContext)
  {
    foreach (var child in enumerable)
    {
      if(visited.Contains(child))
        continue;

      visited.Add(child);

      yield return validator.Validate(child, false, validationContext);
    }
  }
  

  private ObjectValidationResult ValidateObject(object value, IObjectValidator validator, ValidationContext validationContext)
  {
    return validator.Validate(value, false, validationContext);
  }

  private ValidationResult? WrapResult(IList<ObjectValidationResult> results, ValidationContext validationContext)
  {
    var isError = results.Any(x => x.IsError);
    var isWarning = results.Any(x => x.IsWarning);

    var memberNames = !string.IsNullOrEmpty(validationContext.MemberName)
      ? new string[] { validationContext.MemberName }
      : Array.Empty<string>();

    if (isError)
    {
      return new DeepValidationErrorResult(DeepErrorMessage, memberNames)
      {
        Children = results
      };
    }
    else if (isWarning)
    {
      return new DeepValidationWarningResult(DeepWarningMessage, memberNames)
      {
        Children = results
      };
    }
    else
    {
      return ValidationResult.Success;
    }
  }
}