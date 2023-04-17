using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation.Mvvm.Tests;

public class Validatable : ValidatableViewModel
{

  private string nonEmpty;

  [Required]
  public string NonEmpty
  {
    get => this.nonEmpty;
    set => this.SetAndValidate(ref this.nonEmpty, value);
  }

  public Validatable()
    : base(TimeSpan.Zero)
  {
    this.SynchronizationContext = null;
  }
}