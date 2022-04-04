using System.ComponentModel;
using System.Runtime.InteropServices;
using Infomatik.Validation.WinForms.i18n;

namespace Infomatik.Validation.WinForms;

/// <summary>
/// Showing a required message as cue banner in
/// TextBox and ComboBox instead if showing an error icon
/// </summary>
[ProvideProperty("ShowRequiredMessage", typeof(Control))]
public class RequiredErrorProvider : Component, IExtenderProvider
{
  private readonly IDictionary<Control, bool> controlShowRequiredMessageMap = new Dictionary<Control, bool>();

  public RequiredErrorProvider()
  {
  }

  public RequiredErrorProvider(IContainer? container)
  {
    container?.Add(this);
  }

  public bool CanExtend(object extendee)
  {
    return extendee switch
    {
      TextBox => true,
      ComboBox => true,
      _ => false
    };
  }

  public void SetShowRequiredMessage(Control control, bool value)
  {
    if (this.controlShowRequiredMessageMap.ContainsKey(control))
      this.controlShowRequiredMessageMap[control] = value;
    else
      this.controlShowRequiredMessageMap.Add(control, value);

    this.ApplyCueBanner(control);
  }

  public bool GetShowRequiredMessage(Control control)
  {
    if (this.controlShowRequiredMessageMap.TryGetValue(control, out var result))
      return result;

    return false;
  }

  private void ApplyCueBanner(Control control)
  {
    if (!control.IsHandleCreated)
    {
      control.HandleCreated += this.ControlHandleCreated;
      return;
    }

    var message = this.GetShowRequiredMessage(control)
      ? SR.Required
      : "";

    this.SetCueBanner(control, message);
  }

  private void ControlHandleCreated(object? sender, EventArgs e)
  {
    if (!(sender is Control control))
      return;

    control.HandleCreated -= this.ControlHandleCreated;
    this.ApplyCueBanner(control);
  }

  private void SetCueBanner(Control control, string message)
  {
    if (control is TextBox)
      NativeMethods.SendMessage(control.Handle, NativeMethods.EM_SETCUEBANNER, 0, message);
    else if (control is ComboBox)
      NativeMethods.SendMessage(control.Handle, NativeMethods.CB_SETCUEBANNER, 0, message);
  }

  private static class SR
  {
    public static string Required => Strings.Required;
  }

  private static class NativeMethods
  {
    // ReSharper disable twice InconsistentNaming
    public const int EM_SETCUEBANNER = 0x1501;

    public const int CB_SETCUEBANNER = 0x1703;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern long SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
  }
}