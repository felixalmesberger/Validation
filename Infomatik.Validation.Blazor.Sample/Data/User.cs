using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation.Blazor.Sample.Data;

public class User : IValidatableObject
{
  [Required]
  public string Username { get; set; }

  [Required]
  public string Password { get; set; }

  [Required]
  public string ConfirmPassword { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    if (this.Password != this.ConfirmPassword)
      yield return this.CreateError("Password and Confirmed Password do not match", nameof(Password),
        nameof(ConfirmPassword));
  }
}