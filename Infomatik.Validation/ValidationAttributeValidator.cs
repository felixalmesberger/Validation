using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Infomatik.Validation;

/// <summary>
/// Validator for ValidationAttributes
/// To avoid unneccessary reflection calls for each validation,
/// this class is static and all Properties and Validation Attributes for a type are cached.
/// </summary>
internal static class ValidationAttributeValidator
{
  private static readonly Dictionary<Type, TypeAttributeValidation> cache = new();

  /// <summary>
  /// Validate an object using ValidationAttributes,
  /// exceptions in validation attribute will be wrapped in a ValidationResult
  /// </summary>
  /// <param name="instance">Validated object</param>
  /// <param name="context">Validation Context</param>
  /// <param name="cancelOnFirstError">Cancel after first error</param>
  /// <returns></returns>
  public static IEnumerable<ValidationResult> Validate(object instance, ValidationContext context, bool cancelOnFirstError)
  {
    var typeAttributeValidation = GetTypeAttributeValidation(instance);
    return typeAttributeValidation.EnumerateValidationAttributes(instance, context, cancelOnFirstError);
  }

  [MethodImpl(MethodImplOptions.Synchronized)]
  private static TypeAttributeValidation GetTypeAttributeValidation(object instance)
  {
    var type = instance.GetType();
    if (cache.TryGetValue(type, out var result))
      return result;

    var typeAttributeValidation = new TypeAttributeValidation(type);
    cache.Add(type, typeAttributeValidation);
    return typeAttributeValidation;
  }

  /// <summary>
  /// Validate an object by ValidationAttributes
  /// </summary>
  private class TypeAttributeValidation
  {
    private readonly List<PropertyAttributeValidation> propertyValidations;

    public TypeAttributeValidation(Type type)
    {
      if (type is null)
        throw new ArgumentNullException(nameof(type));

      this.propertyValidations = type.GetProperties()
                                     .Where(PropertyAttributeValidation.IsValidatable)
                                     .Select(x => new PropertyAttributeValidation(x, type))
                                     .Where(x => x.HasValidationAttributes)
                                     .ToList();
    }

    public IEnumerable<ValidationResult> EnumerateValidationAttributes(object instance, ValidationContext context,
      bool cancelOnFirstError)
    {
      foreach (var propertyValidation in this.propertyValidations)
      {
        var results = propertyValidation.EnumerateValidationResults(instance, context, cancelOnFirstError);

        foreach (var result in results)
        {
          yield return result;

          if (cancelOnFirstError)
            yield break;
        }
      }
    }
  }

  /// <summary>
  /// Validate a property by ValidationAttributes
  /// </summary>
  private class PropertyAttributeValidation
  {
    public static bool IsValidatable(PropertyInfo propertyInfo)
    {
      if (!propertyInfo.CanRead)
        return false;

      if (!propertyInfo.CanWrite)
        return false;

      return true;
    }

    public PropertyAttributeValidation(PropertyInfo propertyInfo, Type objectType)
    {
      this.objectType = objectType ?? throw new ArgumentNullException(nameof(objectType));
      this.propertyInfo = propertyInfo ?? throw new ArgumentNullException();
      this.validationAttributes = propertyInfo.GetCustomAttributes<ValidationAttribute>().ToList();
    }

    #region fields

    private readonly Type objectType;
    private readonly PropertyInfo propertyInfo;
    public string Name => this.propertyInfo.Name;
    private readonly List<ValidationAttribute> validationAttributes;

    #endregion
    public bool HasValidationAttributes => this.validationAttributes.Any();

    public IEnumerable<ValidationResult> EnumerateValidationResults(object instance, ValidationContext context, bool cancelOnFirstError)
    {
      if (!this.TryGetPropertyValue(instance, out var value))
        yield break;

      foreach (var attribute in this.validationAttributes)
      {
        var result = this.SafeValidateAttribute(value, attribute, context);

        if (result is null)
          continue;

        if (attribute is RequiredAttribute)
          yield return new RequiredValidationResult(result);
        else
          yield return result;

        if (cancelOnFirstError)
          yield break;
      }
    }

    private ValidationResult? SafeValidateAttribute(object? value, ValidationAttribute attribute, ValidationContext context)
    {
      try
      {
        var propertyContext = new ValidationContext(context.ObjectInstance, context, context.Items)
        {
          MemberName = this.propertyInfo.Name
        };

        return attribute.GetValidationResult(value, propertyContext);
      }
      catch (Exception ex)
      {
        return new ValidationResult(SR.ExceptionInValidation(attribute, this.propertyInfo, this.objectType, ex), new[] { this.propertyInfo.Name });
      }
    }

    private bool TryGetPropertyValue(object? instance, out object? value)
    {
      if (instance is null)
      {
        value = default;
        return false;
      }

      try
      {
        value = this.propertyInfo.GetValue(instance);
        return true;
      }
      catch
      {
        value = default;
        return false;
      }
    }
    private static class SR
    {
      public static string ExceptionInValidation(ValidationAttribute attribute, PropertyInfo property, Type type, Exception ex)
        => $"Exception while validating '{property.Name}' in type '{type}', failed attribute: '{attribute.GetType()}'.\r\nException: {ex.ToString()}";
    }
  }
}
