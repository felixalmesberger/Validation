using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

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

  public static bool TryValidateWith<TAttribute>(this IValidatableObject validatable,
    object? value, 
    string memberName,
    ValidationContext context,
    out ValidationResult? validationResult)
  where TAttribute : ValidationAttribute, new()
  {
    var attribute = new TAttribute();

    var propertyContext = new ValidationContext(context.ObjectInstance, context, context.Items)
    {
      MemberName = memberName
    };

    validationResult = attribute.GetValidationResult(value, propertyContext);
    return validationResult == ValidationResult.Success;
  }

  public static bool TryValidateWith<TAttribute,TValue>(this IValidatableObject validatable,
    Expression<Func<TValue>>  accessor,
    ValidationContext context,
    out ValidationResult? validationResult)
    where TAttribute : ValidationAttribute, new()
  {
    if (accessor?.Body is not MemberExpression memberExpression)
      throw new ArgumentException("Accessor is not a valid expression");

    var memberName = memberExpression.Member.Name;
    var value = accessor.Compile()();

    return validatable.TryValidateWith<TAttribute>(value, memberName, context, out validationResult);
  }
}