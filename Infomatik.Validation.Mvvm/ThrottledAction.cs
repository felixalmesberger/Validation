using System.Timers;
using Timer = System.Timers.Timer;

namespace Infomatik.Validation.Mvvm;

internal class SynchronizedThrottledAction : IThrottledAction
{
  private readonly Action action;
  private readonly Timer timer;

  private bool IsThrottlingEnabled => this.ThrottleTimeInMs > 0;
  public SynchronizationContext? SynchronizationContext { get; set; } = SynchronizationContext.Current;
  public bool IsSuspended { get; set; }

  public SynchronizedThrottledAction(Action action, int delayInMs)
  {
    this.action = action;

    this.ThrottleTimeInMs = delayInMs;

    // set delay to something greater zero to initialize timer
    // the timer will never be triggered when delay is zero
    if (!this.IsThrottlingEnabled)
      delayInMs = 1;
    
    this.timer = new Timer(delayInMs);
    this.timer.Elapsed += this.TriggerThrottledAction;
    this.timer.Enabled = false;
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

  public void Invoke()
  {
    if (this.IsSuspended)
      return;

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


  public int ThrottleTimeInMs
  {
    get => (int)this.timer.Interval;
    set
    {
      if (this.IsThrottlingEnabled)
        this.timer.Interval = value;
      else
        this.timer.Stop();
    }
  }

}