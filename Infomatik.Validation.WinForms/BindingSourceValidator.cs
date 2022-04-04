using System.ComponentModel;

namespace Infomatik.Validation.WinForms;

/// <summary>
/// A component that validates Bindings on a given control.
/// For this the DataSource if the Binding must implement the
/// IValidatableObject interface. Errors will be visualized using
/// an error provider
/// </summary>
public class BindingSourceValidator : BindingSourceBehaviour, ISupportInitialize
{
  #region events

  public event EventHandler? IsValidChanged;
  private void OnIsValidChanged()
  {
    this.IsValidChanged?.Invoke(this, EventArgs.Empty);
  }

  #endregion

  #region fields

  private bool isValid;
  private readonly ThrottledUiAction throttledValidation;

  #endregion

  #region constructors

  public BindingSourceValidator()
  {
    this.throttledValidation = new(this.ValidateBindings);
  }

  public BindingSourceValidator(IContainer container)
    : base(container)
  {
    this.throttledValidation = new(this.ValidateBindings);
  }

  #endregion

  #region properties

  /// <summary>
  /// Error provider that will be used to show errors
  /// </summary>
  public ControlStatusProvider? ControlStatusProvider { get; set; }

  public IObjectValidator Validator { get; set; } = ObjectValidator.Instance;

  public bool IsValid
  {
    get => this.isValid;
    set { this.isValid = value; this.OnIsValidChanged(); }
  }

  #endregion

  public bool SuspendValidation { get; set; }

  protected override void BindingComplete(object? sender, BindingCompleteEventArgs e)
  {
    var hasBindingError = e.Exception != null;
    if (hasBindingError)
    {
      this.ShowBindingError(e);
    }
    else
    {
      this.throttledValidation.Invoke();
    }
  }

  protected override void DataSourceChanged(object? sender, EventArgs e)
  {
    this.throttledValidation.Invoke();
  }

  private void ShowBindingError(BindingCompleteEventArgs e)
  {
    if (e.Binding?.Control is null)
      return;

    this.ControlStatusProvider?.SetErrorMessage(e.Binding.Control, e.ErrorText);
  }

  private void ValidateBindings()
  {
    if (this.SuspendValidation)
      return;

    if (this.BindingSource?.DataSource is null)
      return;

    if (this.ControlStatusProvider is null)
      return;

    var validationResult = this.Validator.Validate(this.BindingSource.DataSource, cancelOnFirstError: false);

    foreach (var binding in this.Bindings)
    {
      if (binding.Control is null)
        continue;

      var boundProperty = binding.BindingMemberInfo.BindingMember;

      var (error, warning, isRequired) = validationResult.GetPropertyError(boundProperty);

      // if already showing required error, forget the others
      if (!isRequired || string.IsNullOrEmpty(error))
        this.ControlStatusProvider.SetErrorMessage(binding.Control, error);

      this.ControlStatusProvider.SetWarnMessage(binding.Control, warning);
      this.ControlStatusProvider.SetRequiredError(binding.Control, isRequired);
    }

    this.IsValid = validationResult.IsValid;
  }

  private bool TryFindControlStatusProvider(out ControlStatusProvider? errorProvider)
  {
    errorProvider = this.Container?.Components.OfType<ControlStatusProvider>().FirstOrDefault();
    return errorProvider is not null;
  }

  public void BeginInit()
  {
  }

  public void EndInit()
  {
    if (this.TryFindControlStatusProvider(out var errorProvider) && this.ControlStatusProvider is null)
      this.ControlStatusProvider = errorProvider;
  }

  public void Validate()
  {
    this.ValidateBindings();
  }
}