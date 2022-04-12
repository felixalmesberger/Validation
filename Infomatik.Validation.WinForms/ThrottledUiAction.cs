using System.Runtime.CompilerServices;

namespace Infomatik.Validation.WinForms;

/// <summary>
/// Throttle actions
/// Several calls will result in only one call every given time span
/// e.g
/// Every change of a property value will lead to a validation call,
/// in this case, it is better to wait until the user finished entering
/// data and then validate the input. This is achieved by thottling the validation call
/// </summary>
internal class ThrottledUiAction
{
  #region fields

  private readonly System.Windows.Forms.Timer timer;
  private readonly Action action;
  private int throttleTimeInMs;

  #endregion

  #region constructors

  public ThrottledUiAction(Action action)
    : this(action, TimeSpan.FromMilliseconds(300))
  {
  }

  public ThrottledUiAction(Action action, TimeSpan delay)
  {
    this.action = action ?? throw new ArgumentNullException(nameof(action));
    this.timer = new System.Windows.Forms.Timer()
    {
      Interval = (int)delay.TotalMilliseconds
    };

    this.timer.Enabled = false;
    this.timer.Tick += this.TimerTick;
  }

  #endregion

  #region properties

  /// <summary>
  /// Gets and sets the minimum time between invocations of the action. If 0 it will not be throttled.
  /// </summary>
  public int ThrottleTimeInMs
  {
    get => this.throttleTimeInMs;
    set
    {
      this.throttleTimeInMs = value;

      if (this.throttleTimeInMs > 0)
        this.timer.Interval = value;
    }
  }

  #endregion

  [MethodImpl(MethodImplOptions.Synchronized)]
  public void Invoke()
  {
    if (this.ThrottleTimeInMs == 0)
    {
      this.action.Invoke();
    }
    else
    {
      this.timer.Stop();
      this.timer.Start();
    }
  }

  private void TimerTick(object? sender, EventArgs e)
  {
    this.timer.Stop();
    this.action.Invoke();
  }
}