using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using Timer = System.Timers.Timer;

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
  private readonly ThrottledAction throttledValidationAction;

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

  public ValidatableViewModel()
    : this(TimeSpan.FromMilliseconds(300))
  {
  }

  public ValidatableViewModel(TimeSpan throttledValidationTimespan)
  {
    this.throttledValidationAction = new(this.Validate, throttledValidationTimespan);
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
        if(!result.ContainsKey(member))
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
        {
          yield return entry;
          continue;
        }
      }
    }
  }

  protected void SetAndValidate<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
  {
    field = value;
    this.OnPropertyChanged(propertyName);
    this.throttledValidationAction.Trigger();

  }

  public IEnumerable GetErrors(string? propertyName)
  {
    if (this.errors.TryGetValue(propertyName, out var errors))
      return errors;

    return Enumerable.Empty<object>();
  }
}

public class ThrottledAction
{
  private readonly Action action;
  private readonly Timer timer;


  public bool IsThrottlingEnabled { get; }
  public TimeSpan ThrottleTime { get; }
  public SynchronizationContext? SynchronizationContext { get; set; } = SynchronizationContext.Current;

  public ThrottledAction(Action action, TimeSpan throttleTime)
  {
    this.action = action;
    this.ThrottleTime = throttleTime;
    this.IsThrottlingEnabled = throttleTime.TotalMilliseconds > 0;

    if (this.IsThrottlingEnabled)
    {
      this.timer = new Timer(throttleTime.TotalMilliseconds);
      this.timer.Elapsed += this.TriggerThrottledAction;
      this.timer.Enabled = false;
    }
  }

  private void TriggerThrottledAction(object? sender, ElapsedEventArgs e)
  {
    this.timer.Stop();
    this.ExecuteActionOnSynchronizationContext();
  }

  private void ExecuteActionOnSynchronizationContext()
  {
    if (this.SynchronizationContext is null)
    {
      this.action();
    }
    else
    {
      this.SynchronizationContext.Post(_ =>
      {
        this.action();
      }, null);
    }
  }

  public void Trigger()
  {
    if (this.IsThrottlingEnabled)
    {
      this.timer.Stop();
      this.timer.Start();
    }
    else
    {
      this.ExecuteActionOnSynchronizationContext();
    }
  }
}