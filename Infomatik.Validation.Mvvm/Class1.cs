using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Infomatik.Validation.Mvvm;

public class ValidatableViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
  #region events

  public event PropertyChangedEventHandler? PropertyChanged;

  protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

  protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = null)
  {
    this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
  }

  #endregion

  #region fields

  private readonly Dictionary<string, IEnumerable> errors = new();
  private bool hasErrors;

  #endregion

  #region properties

  public bool HasErrors
  {
    get => this.hasErrors;
    private set => this.Set(ref this.hasErrors, value);
  }

  public IObjectValidator Validator { get; set; } = ObjectValidator.Default;

  #endregion

  protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
  {
    field = value;
    this.OnPropertyChanged(propertyName);
  }


  protected void Validate()
  {
    var result = this.Validator.Validate(this, false);
    this.HasErrors = result.IsError;

    this.errors

  }



  protected void SetAndValidate<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
  {
    field = value;
    this.OnPropertyChanged(propertyName);

  }

  public IEnumerable GetErrors(string? propertyName)
  {
    if (this.errors.TryGetValue(propertyName, out var errors))
      return errors;

    return Enumerable.Empty<object>();
  }

}