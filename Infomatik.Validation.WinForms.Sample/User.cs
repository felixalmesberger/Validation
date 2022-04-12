using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

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
    if (this.Birthdate < new DateTime(2000, 1, 1))
      yield return new ValidationResult("Must be born before 2000", new[] { nameof(Birthdate) });

    var specialCharacters = "!$%&/()=?.:,;-_+*~#'{[]}\\§";
    if (this.Password?.Any(x => specialCharacters.Contains(x)) == false)
      yield return this.CreateWarning("Password should contain special character", nameof(this.Password));
  }

  public class MustBeBornBefore : ValidationAttribute
  {
    private readonly int year;

    public MustBeBornBefore(int year)
    {
      this.year = year;
      this.ErrorMessage = $"Must be born before {year}";
    }

    public override bool IsValid(object? value)
    {
      if (value is not DateTime dateTime)
        throw new ArgumentException("Invalid attribute usage: value is not of type 'DateTime'");

      return dateTime < new DateTime(this.year, 1, 1);
    }
  }
}