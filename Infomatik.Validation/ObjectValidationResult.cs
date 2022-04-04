using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation;

/// <summary>
/// Contains the results of an object validation
/// </summary>
public class ObjectValidationResult
{

  public ObjectValidationResult(IList<ValidationResult>? errors,
                                IList<ValidationResult>? warnings,
                                IList<ValidationResult>? required)
  {
    this.Warnings = warnings ?? new List<ValidationResult>();
    this.Errors = errors ?? new List<ValidationResult>();
    this.Required = required ?? new List<ValidationResult>();
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
  public IList<ValidationResult> Required { get; }

  /// <summary>
  /// Is the validated object valid
  /// </summary>
  public bool IsValid => !this.Errors.Any();

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
  /// <returns>Property is required or missing</returns>
  public bool IsRequired(string propertyName) => this.Required.Any(x => x.MemberNames.Contains(propertyName));

  /// <summary>
  /// Gets the error for a given property
  /// </summary>
  /// <param name="propertyName">Property Name</param>
  /// <returns>Error, Warning and IsRequired for property</returns>
  public (string? error, string? warning, bool isRequired) GetPropertyError(string propertyName)
  {
    var error = this.GetError(propertyName);
    var warning = this.GetWarning(propertyName);
    var isRequired = this.IsRequired(propertyName);

    if (isRequired)
      return (null, null, true);
    else if (error is null)
      return (null, warning, false);
    else
      return (error, null, false);
  }
}

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
