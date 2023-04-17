using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Infomatik.Validation.Tests;

public class DeepValidationAttributeTest
{
  [Fact]
  public void Test()
  {
    var instance = new DeepPropertyTestObject();
    var result = ObjectValidator.Default.Validate(instance);

    Assert.False(result.IsValid);
  }

}

public class DeepPropertyTestObject
{
  [DeepValidation]
  public DeepProperty DeepProperty { get; set; } = new();

}

public class DeepProperty
{
  [Required]
  public string NotEmpty { get; set; }
}