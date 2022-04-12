using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Infomatik.Validation.WinForms;

/// <summary>
/// A component that validates Bindings on a given control.
/// For this the DataSource if the Binding must implement the
/// IValidatableObject interface. Errors will be visualized using
/// an error provider
/// </summary>
[ToolboxBitmap(typeof(ErrorProvider))]
public class BindingSourceValidator : Component, ISupportInitialize
{
  #region events

  public event EventHandler? IsValidChanged;
  private void OnIsValidChanged()
  {
    this.IsValidChanged?.Invoke(this, EventArgs.Empty);
  }

  #endregion

  #region fields

  private readonly IContainer? container;
  private bool isValid;
  private readonly ThrottledUiAction throttledValidation;
  private BindingSource? bindingSource;

  #endregion

  #region constructors

  public BindingSourceValidator()
  {
    this.throttledValidation = new(this.ValidateBindings)
    {
      ThrottleTimeInMs = 300
    };
  }

  public BindingSourceValidator(IContainer container)
    : this()
  {
    this.container = container;
    container?.Add(this);
  }

  #endregion

  #region properties

  /// <summary>
  /// ValidationStatusProvider used to show control validation results
  /// </summary>
  [Category("BindingSourceValidator")]
  [Description]
  public ValidationStatusProvider? ValidationStatusProvider { get; set; }

  /// <summary>
  /// Used validator
  /// </summary>
  [Category("BindingSourceValidator")]
  [Description("Used validator")]
  public IObjectValidator Validator { get; set; } = ObjectValidator.Default;

  /// <summary>
  /// Gets the validation state of monitored object
  /// </summary>
  public bool IsValid
  {
    get => this.isValid;
    private set { this.isValid = value; this.OnIsValidChanged(); }
  }

  /// <summary>
  /// Gets or sets, whether the validation is suspended
  /// </summary>
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  [Category("BindingSourceValidator")]
  public bool SuspendValidation { get; set; }

  [DefaultValue(300)]
  [Category("BindingSourceValidator")]
  public int ThrottleTimeInMs
  {
    get => this.throttledValidation.ThrottleTimeInMs;
    set => this.throttledValidation.ThrottleTimeInMs = value;
  }

  /// <summary>
  /// All bindings belonging to <see cref="BindingSource"/>
  /// </summary>
  private IEnumerable<Binding> Bindings => this.bindingSource?.CurrencyManager.Bindings.OfType<Binding>() ?? Enumerable.Empty<Binding>();


  /// <summary>
  /// Gets or sets the monitored binding source
  /// </summary>
  public BindingSource? BindingSource
  {
    get => this.bindingSource;
    set
    {
      if (value == this.BindingSource)
        return;

      this.DetachFromBindingSource();
      this.bindingSource = value;
      this.AttachToBindingSource();
    }
  }

  #endregion

  protected virtual bool AttachToBindingSource()
  {
    if (this.BindingSource is null)
      return false;

    this.BindingSource.BindingComplete += this.BindingComplete;
    this.BindingSource.DataSourceChanged += this.DataSourceChanged;

    return true;
  }

  protected virtual bool DetachFromBindingSource()
  {
    if (this.BindingSource is null)
      return false;

    this.BindingSource.BindingComplete -= this.BindingComplete;
    this.BindingSource.DataSourceChanged -= this.DataSourceChanged;

    return true;
  }
  private void BindingComplete(object? sender, BindingCompleteEventArgs e)
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

  private void DataSourceChanged(object? sender, EventArgs e)
  {
    this.throttledValidation.Invoke();
  }

  private void ShowBindingError(BindingCompleteEventArgs e)
  {
    if (e.Binding?.Control is null)
      return;

    this.ValidationStatusProvider?.SetErrorMessage(e.Binding.Control, e.ErrorText);
  }

  private void ValidateBindings()
  {
    if (this.SuspendValidation)
      return;

    if (this.BindingSource?.DataSource is null)
      return;

    if (this.ValidationStatusProvider is null)
      return;

    var validationResult = this.Validator.Validate(this.BindingSource.DataSource, breakOnFirstError: false);

    foreach (var binding in this.Bindings)
    {
      if (binding.Control is null)
        continue;

      var boundProperty = binding.BindingMemberInfo.BindingMember;

      var (error, warning, isRequired) = validationResult.GetPropertyError(boundProperty);

      // if already showing required error, forget the others
      if (!isRequired || string.IsNullOrEmpty(error))
        this.ValidationStatusProvider.SetErrorMessage(binding.Control, error);

      this.ValidationStatusProvider.SetWarnMessage(binding.Control, warning);
      this.ValidationStatusProvider.SetIsMissingValue(binding.Control, isRequired);
    }

    this.IsValid = validationResult.IsValid;
  }

  private bool TryFindControlStatusProvider(out ValidationStatusProvider? errorProvider)
  {
    errorProvider = this.container?.Components.OfType<ValidationStatusProvider>().FirstOrDefault();
    return errorProvider is not null;
  }

  public void BeginInit()
  {
  }

  public void EndInit()
  {
    if (this.TryFindControlStatusProvider(out var errorProvider) && this.ValidationStatusProvider is null)
      this.ValidationStatusProvider = errorProvider;
  }

  /// <summary>
  /// Manually fires a validation
  /// </summary>
  public void Validate()
  {
    this.ValidateBindings();
  }
}