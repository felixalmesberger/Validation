using System;
using System.Runtime.CompilerServices;

namespace Infomatik.Validation.WinForms;

/// <inheritdoc/>
internal class ThrottledWinFormsAction : IThrottledAction
{
  #region fields

  private readonly System.Windows.Forms.Timer timer;
  private readonly Action action;
  private int throttleTimeInMs;

  #endregion

  private bool IsThrottlingEnabled => this.ThrottleTimeInMs > 0;

  public bool IsSuspended { get; set; }

  #region constructors

  public ThrottledWinFormsAction(Action action, int delayInMs = 300)
  {
    this.action = action ?? throw new ArgumentNullException(nameof(action));
    this.timer = new System.Windows.Forms.Timer()
    {
      Interval = (int)delayInMs
    };

    this.timer.Enabled = false;
    this.timer.Tick += this.TimerTick;
  }

  #endregion

  #region properties

  /// <inheritdoc/>
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

  /// <inheritdoc/>
  [MethodImpl(MethodImplOptions.Synchronized)]
  public void Invoke()
  {
    if (this.IsSuspended)
      return;

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