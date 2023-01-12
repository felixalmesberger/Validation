using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Infomatik.Validation.Tests;

public class DeepValidationAttributeTest
{
  [Fact]
  public void Test()
  {

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