using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Infomatik.Validation.WinForms.Tests;

public class BindingSourceValidatorTests
{

  private readonly ObservableValidator validator = new();
  private readonly BindingSource bindingSource = new();
  private readonly ValidationStatusProvider validationStatusProvider = new();

  private readonly BindingSourceValidator bindingSourceValidator;

  public BindingSourceValidatorTests()
  {
    this.bindingSourceValidator = new()
    {
      ThrottleTimeInMs = 0,
      BindingSource = this.bindingSource,
      Validator = this.validator,
      ValidationStatusProvider = this.validationStatusProvider
    };
  }

  [StaFact]
  public void SetDataSourceOfConnectedBindingSource_TriggersValidate()
  {
    bindingSource.DataSource = new InvalidObject();
    Assert.Equal(1, this.validator.NumberOfValidationInvocations);
  }
}

public class ObservableValidator : IObjectValidator
{

  public int NumberOfValidationInvocations { get; private set; } = 0;

  public ObjectValidationResult Validate(object instance, bool breakOnFirstError, ValidationContext? context = null)
  {
    this.NumberOfValidationInvocations++;
    return ObjectValidator.Default.Validate(instance, breakOnFirstError);
  }
}

public class InvalidObject
{
  [Required]
  public string Property { get; set; }
}