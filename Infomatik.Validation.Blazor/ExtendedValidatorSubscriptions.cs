using Infomatik.Validation.i18n;
using Microsoft.AspNetCore.Components.Forms;

namespace Infomatik.Validation.Blazor;

public class ExtendedValidatorSubscriptions : IDisposable
{
  private readonly EditContext editContext;
  private readonly IServiceProvider serviceProvider;
  private readonly ValidationMessageStore messages;

  private readonly IObjectValidator objectValidator;

  private ObjectValidationResult? lastValidationResult;

  public bool TreatMissingAsErrors { get; set; }

  public ExtendedValidatorSubscriptions(EditContext editContext, IServiceProvider? serviceProvider)
  {
    this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    this.editContext = editContext ?? throw new ArgumentNullException(nameof(editContext));
    this.editContext.OnFieldChanged += this.FieldChanged;
    this.editContext.OnValidationRequested += this.ValidationRequested;

    this.messages = new ValidationMessageStore(this.editContext);

    this.objectValidator = new ObjectValidator()
    {
      ServiceProvider = this.serviceProvider
    };

    this.Validate(this.TreatMissingAsErrors);
  }

  private void ValidationRequested(object? sender, ValidationRequestedEventArgs e)
    => this.Validate(addMissingsToErrors: this.TreatMissingAsErrors);

  private void FieldChanged(object? sender, FieldChangedEventArgs e)
    => this.Validate(addMissingsToErrors: this.TreatMissingAsErrors);

  private void Validate(bool addMissingsToErrors)
  {
    this.lastValidationResult = this.objectValidator.Validate(this.editContext.Model, false);

    this.messages.Clear();

    foreach (var error in this.lastValidationResult.Errors)
      foreach (var memberName in error.MemberNames)
        this.messages.Add(new FieldIdentifier(this.editContext.Model, memberName), error.ErrorMessage!);

    if (addMissingsToErrors)
    {
      foreach (var error in this.lastValidationResult.Missing)
        foreach (var memberName in error.MemberNames)
          this.messages.Add(new FieldIdentifier(this.editContext.Model, memberName), error.ErrorMessage!);
    }

    this.editContext.NotifyValidationStateChanged();
  }

  public string GetRequiredPlaceholder(string fieldName)
  {
    if (this.lastValidationResult is null)
      return "";

    return this.lastValidationResult.Missing.Any(x => x.MemberNames.Contains(fieldName))
      ? SR.Required()
      : "";
  }

  public void Dispose()
  {
    this.editContext.OnFieldChanged -= this.FieldChanged;
    this.editContext.OnValidationRequested -= this.ValidationRequested;
  }
}