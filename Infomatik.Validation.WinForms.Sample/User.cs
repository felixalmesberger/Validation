using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation.WinForms.Sample;

public class User : IValidatableObject
{
  [Required]
  public string? Username { get; set; }

  [Required]
  public string? Name { get; set; }

  [Required]
  [EmailAddress]
  public string? Mail { get; set; }

  [Required]
  public string? Password { get; set; }

  public DateTime Birthdate { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    if (this.Birthdate < new DateTime(1900, 1, 1))
      yield return this.CreateError("Birthdate must be after 1/1/1900", nameof(this.Birthdate));

    if (this.Username?.Length > 20)
      yield return this.CreateWarning("Username is very long", nameof(this.Username));

    var specialCharacters = "!$%&/()=?.:,;-_+*~#'{[]}\\§";
    if (this.Password?.Any(x => specialCharacters.Contains(x)) == false)
      yield return this.CreateError("Password must contain special character", nameof(this.Password));
  }
}