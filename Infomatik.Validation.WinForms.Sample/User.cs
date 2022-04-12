using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Infomatik.Validation.WinForms.Sample;

public class User : IValidatableObject
{
  //[Required]
  public string? Username { get; set; }

  //[Required]
  public string? Name { get; set; }

  //[Required]
  //[EmailAddress]
  public string? Mail { get; set; }

  //[Required]
  public string? Password { get; set; }

  //[MustBeBornBefore(2000)]
  public DateTime Birthdate { get; set; }


  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    if (string.IsNullOrEmpty(this.Username))
      yield return new ValidationResult("Username is required", new[] { nameof(this.Username) });

    if (string.IsNullOrEmpty(this.Name))
      yield return new ValidationResult("Name is required", new[] { nameof(this.Name) });

    if (string.IsNullOrEmpty(this.Mail))
      yield return new ValidationResult("Mail is required", new[] { nameof(this.Mail) });

    if (string.IsNullOrEmpty(this.Password))
      yield return new ValidationResult("Password is required", new[] { nameof(this.Password) });

    if (string.IsNullOrEmpty(this.Mail) || !MailAddress.TryCreate(this.Mail, out _))
      yield return new ValidationResult("Mail is invalid", new[] { nameof(this.Mail) });

    if (this.Birthdate < new DateTime(2000, 1, 1))
      yield return new ValidationResult("Must be born before 2000", new[] { nameof(Birthdate) });
  }

  //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  //{
  //  if (this.Birthdate < new DateTime(1900, 1, 1))
  //    yield return this.CreateError("Birthdate must be after 1/1/1900", nameof(this.Birthdate));

  //  if (this.Username?.Length > 20)
  //    yield return this.CreateWarning("Username is very long", nameof(this.Username));

  //  var specialCharacters = "!$%&/()=?.:,;-_+*~#'{[]}\\§";
  //  if (this.Password?.Any(x => specialCharacters.Contains(x)) == false)
  //    yield return this.CreateError("Password must contain special character", nameof(this.Password));
  //}

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