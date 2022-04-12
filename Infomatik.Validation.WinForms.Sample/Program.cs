using System.Globalization;

namespace Infomatik.Validation.WinForms.Sample
{
  internal static class Program
  {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      // To customize application configuration such as set high DPI settings or default font,
      // see https://aka.ms/applicationconfiguration.
      ApplicationConfiguration.Initialize();
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
      
      Application.Run(new Form1());

    }
  }
}