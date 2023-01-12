using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Infomatik.Validation;

public interface IDeepValidationResult : IEnumerable<ObjectValidationResult>
{
  /// <summary>
  ///     Gets the collection of member names affected by this result.  The collection may be empty but will never be null.
  /// </summary>
  IEnumerable<string> MemberNames { get; }

  /// <summary>
  ///     Gets the error message for this result.  It may be null.
  /// </summary>
  string? ErrorMessage { get; set; }
}

/// <summary>
/// Contains the results of an object validation
/// </summary>
public class ObjectValidationResult
{
  public ObjectValidationResult(IList<ValidationResult>? errors,
                                IList<ValidationResult>? warnings,
                                IList<ValidationResult>? missing)
  {
    this.Warnings = warnings ?? new List<ValidationResult>();
    this.Errors = errors ?? new List<ValidationResult>();
    this.Missing = missing ?? new List<ValidationResult>();

    foreach (var deepValidationResult in this.DeepValidationResults.ToList())
      this.MergeWith(deepValidationResult);
  }

  /// <summary>
  /// All warnings. Those do not have an effect to <see cref="IsValid"/>
  /// </summary>
  public IList<ValidationResult> Warnings { get; }

  /// <summary>
  /// All errors
  /// </summary>
  public IList<ValidationResult> Errors { get; }

  /// <summary>
  /// All errors, indicating a missing value caused by the <see cref="RequiredAttribute"/>
  /// </summary>
  public IList<ValidationResult> Missing { get; }

  /// <summary>
  /// Is the validated object valid
  /// </summary>
  public bool IsValid => !this.Errors.Any() && !this.Missing.Any();

  /// <summary>
  /// Is the validated object valid, but has some warnings
  /// </summary>
  public bool IsWarning => this.IsValid && this.Warnings.Any();

  /// <summary>
  /// Is the validated object invalid
  /// </summary>
  public bool IsError => !this.IsValid;

  /// <summary>
  /// Gets the error for a given property
  /// </summary>
  /// <param name="propertyName">Property Name</param>
  /// <returns>Error for that property or null</returns>
  public string? GetError(string propertyName) =>
    this.Errors.FirstOrDefault(x => x.MemberNames.Contains(propertyName))?.ErrorMessage;

  /// <summary>
  /// Gets the warning for a given property
  /// </summary>
  /// <param name="propertyName">Property Name</param>
  /// <returns>Warning for that property or null</returns>
  public string? GetWarning(string propertyName) =>
    this.Warnings.FirstOrDefault(x => x.MemberNames.Contains(propertyName))?.ErrorMessage;

  /// <summary>
  /// Is the property required and missing
  /// </summary>
  /// <param name="propertyName">Property Name</param>
  /// <returns>Property is required and missing</returns>
  public bool IsMissing(string propertyName) => this.Missing.Any(x => x.MemberNames.Contains(propertyName));

  /// <summary>
  /// Gets the error for a given property
  /// </summary>
  /// <param name="propertyName">Property Name</param>
  /// <returns>Error, Warning and IsRequired for property</returns>
  public (string? error, string? warning, bool isMissing) GetPropertyError(string propertyName)
  {
    var error = this.GetError(propertyName);
    var warning = this.GetWarning(propertyName);
    var isMissing = this.IsMissing(propertyName);

    if (isMissing)
      return (null, null, true);
    else if (error is null)
      return (null, warning, false);
    else
      return (error, null, false);
  }

  public IEnumerable<ValidationResult> EnumerateValidationResults()
  {
    foreach (var missing in this.Missing)
      yield return missing;

    foreach (var warning in this.Warnings)
      yield return warning;

    foreach (var error in this.Errors)
      yield return error;
  }

  internal IEnumerable<IDeepValidationResult> DeepValidationResults
    => this.EnumerateValidationResults().OfType<IDeepValidationResult>();

  private void MergeWith(IDeepValidationResult deepValidation)
  {
    if (deepValidation is WarningValidationResult warning)
      this.Warnings.Add(new WarningValidationResult(warning.ErrorMessage, warning.MemberNames));
    else if (deepValidation is ValidationResult error)
      this.Errors.Add(new ValidationResult(error.ErrorMessage, error.MemberNames));

    var memberName = deepValidation.MemberNames.FirstOrDefault();

    foreach (var child in deepValidation)
    {
      foreach (var error in child.Errors)
        this.Errors.Add(this.WithParent(error, memberName));

      foreach (var childMissing in child.Missing)
        this.Missing.Add(this.WithParent(childMissing, memberName));

      foreach (var childWarning in child.Warnings)
        this.Warnings.Add(this.WithParent(childWarning, memberName));

      foreach (var deepValidationResult in child.DeepValidationResults)
        this.MergeWith(deepValidationResult);
    }
  }

  private ValidationResult WithParent(ValidationResult me, string? parent)
  {
    if (string.IsNullOrEmpty(parent))
      return me;

    var memberNames = me.MemberNames.Select(x => $"{parent}.{x}").ToArray();

    return new ValidationResult(me.ErrorMessage, memberNames);
  }
}