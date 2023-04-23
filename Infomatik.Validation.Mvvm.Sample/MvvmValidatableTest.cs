using System.ComponentModel.DataAnnotations;
using System;

namespace Infomatik.Validation.Mvvm.Sample;
public class MvvmValidatableTest : ValidatableViewModel
{

  private string nonEmpty = "HALLO";

  [Required]
  public string NonEmpty
  {
    get => this.nonEmpty;
    set => this.SetAndValidate(ref this.nonEmpty, value);
  }
}