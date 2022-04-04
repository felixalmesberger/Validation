namespace Infomatik.Validation.WinForms.Sample
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      this.InitializeComponent();
      this.bindingSource.DataSource = new User();
    }
  }
}