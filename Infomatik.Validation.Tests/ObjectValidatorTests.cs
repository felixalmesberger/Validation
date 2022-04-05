using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Infomatik.Validation.Tests
{
  public class ObjectValidatorTests
  {

    private readonly ObjectValidator validator = new();

    [Fact]
    public void Validate_ObjectWithValidationAttributeAndImplementingIValidatableObject_FullyValidated()
    {
      var instance = new ObjectWithValidationAttributeAndImplementingIValidatableObject();
      var result = validator.Validate(instance, false);
      Assert.Equal(2, result.Errors.Count);
    }

    [Fact]
    public void Validate_PropertyAnnotatedWithErrorAttribute_ReturnsErrorResult()
    {
      var instance = new PropertyAnnotatedWithErrorAttribute_TestClass();

      var result = this.validator.Validate(instance, false);

      Assert.Equal(1, result.Errors.Count);
      var memberName = result.Errors[0].MemberNames.ToList()[0];
      Assert.Equal(nameof(PropertyAnnotatedWithErrorAttribute_TestClass.Property), memberName);
    }

    [Fact]
    public void Validate_PropertyAnnotatedWithWarningAttribute_ReturnsWarningResult()
    {
      var instance = new PropertyAnnotatedWithWarningAttribute_TestClass();

      var result = this.validator.Validate(instance, false);

      Assert.Equal(1, result.Warnings.Count);
      var memberName = result.Warnings[0].MemberNames.ToList()[0];
      Assert.Equal(nameof(PropertyAnnotatedWithErrorAttribute_TestClass.Property), memberName);
    }

    [Fact]
    public void Validate_PropertyAnnotatedWithRequiredAttribute_ReturnsRequiredValidationResult()
    {
      var instance = new PropertyAnnotatedWithRequiredAttribute_TestClass();

      var result = this.validator.Validate(instance, false);

      Assert.Equal(1, result.Required.Count);

      var memberName = result.Required[0].MemberNames.ToList()[0];
      Assert.Equal(nameof(PropertyAnnotatedWithRequiredAttribute_TestClass.Property), memberName);
    }

    [Fact]
    public void Validate_CancelOnFirstErrorOnlyAttributes_CancelsOnFirstError()
    {
      var instance = new CancelOnFirstErrorOnlyAttributes_TestClass();
      var result = this.validator.Validate(instance, true);

      Assert.Equal(1, result.Required.Count);
      var memberName = result.Required[0].MemberNames.ToList()[0];
      Assert.Equal(nameof(CancelOnFirstErrorOnlyAttributes_TestClass.Property1), memberName);
    }

    [Fact]
    public void Validate_CancelOnFirstErrorOnlyIValidatable_CancelOnFirstError()
    {
      var instance = new CancelOnFirstErrorOnlyIValidatable_TestClass();
      var result = this.validator.Validate(instance, true);

      Assert.Equal(1, result.Errors.Count);
    }

    [Fact]
    public void Validate_CancelOnFirstErrorAttributeAndIValidatable_CancelOnFirstError()
    {
      var instance = new CancelOnFirstErrorAttributeAndIValidatable_TestClass();

      var result = this.validator.Validate(instance, true);

      Assert.Equal(1, result.Errors.Count);
    }

    [Fact]
    public void Validate_ExceptionInValidationAttribute_WrappedInValidationResult()
    {
      var instance = new ExceptionInValidationAttributeWrappedInValidationResult_TestClass();
      var result = this.validator.Validate(instance, false);
      Assert.Equal(1, result.Errors.Count);
    }
  }

  public class PropertyAnnotatedWithWarningAttribute_TestClass
  {
    [Warning("Property")]
    public string Property { get; set; }
  }


  public class PropertyAnnotatedWithErrorAttribute_TestClass 
  {
    [Error("Property")]
    public string Property { get; set; }
  }

  public class ObjectWithValidationAttributeAndImplementingIValidatableObject : IValidatableObject
  {
    [Error("Property")]
    public string Property1 { get; set; }

    public string Property2 { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      yield return new ValidationResult(nameof(Property2), new[] { nameof(Property2) });
    }
  }

  public class PropertyAnnotatedWithRequiredAttribute_TestClass
  {

    [Required]
    public string Property { get; set; }
  }

  public class CancelOnFirstErrorOnlyAttributes_TestClass
  {
    [Required]
    public string Property1 { get; set; }

    [Required]
    public string Property2 { get; set; }
  }

  public class CancelOnFirstErrorOnlyIValidatable_TestClass : IValidatableObject
  {
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      yield return new ValidationResult("Error1");
      yield return new ValidationResult("Error2");
    }
  }

  public class CancelOnFirstErrorAttributeAndIValidatable_TestClass : IValidatableObject
  {
    [Error("Error")]
    public string Property { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      yield return new ValidationResult("Error");
    }
  }

  public class ErrorAttribute : ValidationAttribute
  {
    public ErrorAttribute(string errorMessage)
    {
      this.ErrorMessage = errorMessage;
    }

    public override bool IsValid(object? value)
    {
      return false;
    }
  }

  public class WarningAttribute: ValidationAttribute
  {
    public WarningAttribute(string errorMessage)
    {
      this.ErrorMessage = errorMessage;
    }

    public override bool IsValid(object? value)
    {
      return false;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      return new WarningValidationResult(base.IsValid(value, validationContext));
    }
  }


  public class ExceptionInValidationAttributeWrappedInValidationResult_TestClass
  {
    [FaultyValidation]
    public string Property { get; set; }
  }

  public class FaultyValidationAttribute : ValidationAttribute
  {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      throw new Exception("Faulty");
    }
  }
}