using Infomatik.Validation.WinForms.Properties;

namespace Infomatik.Validation.WinForms;

public static class DefaultIcons
{
  
  // create a control to get access to DeviceDpi
  private static readonly Control control = new();

  private static int DeviceDpi
  {
    get
    {
      if(!control.IsHandleCreated)
        control.CreateControl();

      return control.DeviceDpi;
    }
  }

  private static Icon GetScaledIcon(Icon icon16, Icon icon32)
  {
    if (DeviceDpi == 96)
      return icon16;
    else if (DeviceDpi == 192)
      return icon32;

    var scaling = DeviceDpi / 192.0;

    using var iconBitmap = Bitmap.FromHicon(icon32.Handle);

    var newSize = new Size((int) (iconBitmap.Width * scaling), (int) (iconBitmap.Height * scaling));
    var resizedIconBitmap = new Bitmap(iconBitmap, newSize);
    return Icon.FromHandle(resizedIconBitmap.GetHicon());
  }

  public static Icon Warning => GetScaledIcon(Resources.Warn16, Resources.Warn32);
  public static Icon Error => GetScaledIcon(Resources.Error16, Resources.Error32);
}