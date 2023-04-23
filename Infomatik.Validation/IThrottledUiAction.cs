using System;

namespace Infomatik.Validation;

/// <summary>
/// Throttle actions
/// Several calls will result in only one call every given time span
/// e.g
/// Every change of a property value will lead to a validation call,
/// in this case, it is better to wait until the user finished entering
/// data and then validate the input. This is achieved by thottling the validation call
/// </summary>
public interface IThrottledAction
{
  /// <summary>
  /// Gets and sets the minimum time between invocations of the action. If 0 it will not be throttled.
  /// </summary>
  int ThrottleTimeInMs { get; set; }

  /// <summary>
  /// Invoke the action
  /// </summary>
  void Invoke();

  bool IsSuspended { get; set; }
}

public static class ThrottledActionExtensions
{
  public static IDisposable Suspend(this IThrottledAction me) 
    => new ThrottledActionSuspender(me);
}

public class ThrottledActionSuspender : IDisposable
{
  private readonly IThrottledAction action;

  public ThrottledActionSuspender(IThrottledAction action)
  {
    this.action = action;
    this.action.IsSuspended = true;
  }

  public void Dispose()
  {
    this.action.IsSuspended = false;
    this.action.Invoke();
  }
}