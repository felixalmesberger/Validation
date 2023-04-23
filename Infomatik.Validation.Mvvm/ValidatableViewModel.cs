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

  private Dictionary<string, IList<string>> errors = new();
  private bool hasErrors;
  private readonly SynchronizedThrottledAction throttledValidationAction;

  #endregion

  #region properties

  public bool HasErrors
  {
    get => this.hasErrors;
    private set => this.Set(ref this.hasErrors, value);
  }

  public IObjectValidator Validator { get; set; } = ObjectValidator.Default;

  public SynchronizationContext? SynchronizationContext
  {
    get => this.throttledValidationAction.SynchronizationContext;
    set => this.throttledValidationAction.SynchronizationContext = value;
  }

  #endregion

  #region constructors
  
  public ValidatableViewModel(int validationThrottleTimeInMs = 300)
  {
    this.throttledValidationAction = new(this.Validate, validationThrottleTimeInMs);
  }

  #endregion

  protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
  {
    field = value;
    this.OnPropertyChanged(propertyName);
  }


  protected void Validate()
  {
    var current = new Dictionary<string, IList<string>>(this.errors);
    var result = this.Validator.Validate(this, false);
    this.HasErrors = result.IsError;
    this.errors = this.FlattenValidationResult(result);

    foreach (var changed in this.EnumerateChangedProperties(current, this.errors))
      this.OnErrorsChanged(changed);
  }

  private Dictionary<string, IList<string>> FlattenValidationResult(ObjectValidationResult validationResult)
  {
    var result = new Dictionary<string, IList<string>>();

    var errorOrMissing = validationResult.Errors.Concat(validationResult.Missing);

    foreach (var error in errorOrMissing)
    {
      foreach (var member in error.MemberNames)
      {
        if (!result.ContainsKey(member))
          result.Add(member, new List<string>());

        result[member].Add(error.ErrorMessage!);
      }
    }

    return result;
  }

  private IEnumerable<string> EnumerateChangedProperties(IDictionary<string, IList<string>> current,
    IDictionary<string, IList<string>> changed)
  {
    foreach (var currentEntry in current.Keys.ToList())
    {
      if (!changed.ContainsKey(currentEntry))
      {
        yield return currentEntry;
        current.Remove(currentEntry);
      }
    }

    foreach (var changedEntry in changed.Keys.ToList())
    {
      if (!current.ContainsKey(changedEntry))
      {
        yield return changedEntry;
        changed.Remove(changedEntry);
      }
    }

    foreach (var entry in current.Keys.ToList())
    {
      var currentValues = current[entry].OfType<object>().ToList();
      var changedValues = changed[entry].OfType<object>().ToList();

      if (currentValues.Count != changedValues.Count)
      {
        yield return entry;
        continue;
      }

      for (var i = 0; i < currentValues.Count; i++)
      {
        if (currentValues[i] != changedValues[i])
          yield return entry;
      }
    }
  }

  public IDisposable SuspendValidation() => this.throttledValidationAction.Suspend();

  protected void SetAndValidate<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
  {
    field = value;
    this.OnPropertyChanged(propertyName);
    this.throttledValidationAction.Invoke();
  }

  public IEnumerable GetErrors(string? propertyName)
  {
    if (propertyName is null)
      return Enumerable.Empty<object>();

    if (this.errors.TryGetValue(propertyName, out var errors))
      return errors;

    return Enumerable.Empty<object>();
  }
}