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

  /// <summary>
  /// Treat missing (properties with Required attribute) as errors when field changed
  /// It can be useful to set this to false, when required fields are displayed differently
  /// With a placeholder for example
  /// NOTE: This has no effect, when the whole model is being validated. 
  /// </summary>
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
  }

  // if validiation is requested always treat missing as errors
  private void ValidationRequested(object? sender, ValidationRequestedEventArgs e)
    => this.Validate(true);

  private void FieldChanged(object? sender, FieldChangedEventArgs e)
    => this.Validate(this.TreatMissingAsErrors);

  internal void Validate(bool treatMissingAsErrors)
  {
    this.lastValidationResult = this.objectValidator.Validate(this.editContext.Model, false);

    this.messages.Clear();

    foreach (var error in this.lastValidationResult.Errors)
      foreach (var memberName in error.MemberNames)
        this.messages.Add(new FieldIdentifier(this.editContext.Model, memberName), error.ErrorMessage!);

    if (treatMissingAsErrors)
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