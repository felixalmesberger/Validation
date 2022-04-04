namespace Infomatik.Validation.WinForms.Sample
{
  partial class Form1
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      Infomatik.Validation.ObjectValidator objectValidator1 = new Infomatik.Validation.ObjectValidator();
      this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.textBox4 = new System.Windows.Forms.TextBox();
      this.bindingSourceValidator = new Infomatik.Validation.WinForms.BindingSourceValidator(this.components);
      this.controlStatusProvider = new Infomatik.Validation.WinForms.ControlStatusProvider(this.components);
      this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceValidator)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
      this.SuspendLayout();
      // 
      // bindingSource
      // 
      this.bindingSource.DataSource = typeof(Infomatik.Validation.WinForms.Sample.User);
      // 
      // textBox1
      // 
      this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Username", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.controlStatusProvider.SetErrorMessage(this.textBox1, "");
      this.textBox1.Location = new System.Drawing.Point(105, 12);
      this.textBox1.Name = "textBox1";
      this.controlStatusProvider.SetRequiredError(this.textBox1, false);
      this.textBox1.Size = new System.Drawing.Size(200, 23);
      this.textBox1.TabIndex = 0;
      this.controlStatusProvider.SetWarnMessage(this.textBox1, "");
      // 
      // dateTimePicker1
      // 
      this.dateTimePicker1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSource, "Birthdate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.controlStatusProvider.SetErrorMessage(this.dateTimePicker1, "");
      this.dateTimePicker1.Location = new System.Drawing.Point(105, 128);
      this.dateTimePicker1.Name = "dateTimePicker1";
      this.controlStatusProvider.SetRequiredError(this.dateTimePicker1, false);
      this.dateTimePicker1.Size = new System.Drawing.Size(200, 23);
      this.dateTimePicker1.TabIndex = 1;
      this.controlStatusProvider.SetWarnMessage(this.dateTimePicker1, "");
      // 
      // label1
      // 
      this.controlStatusProvider.SetErrorMessage(this.label1, "");
      this.label1.Location = new System.Drawing.Point(7, 12);
      this.label1.Name = "label1";
      this.controlStatusProvider.SetRequiredError(this.label1, false);
      this.label1.Size = new System.Drawing.Size(92, 23);
      this.label1.TabIndex = 2;
      this.label1.Text = "Username:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.controlStatusProvider.SetWarnMessage(this.label1, "");
      // 
      // label2
      // 
      this.controlStatusProvider.SetErrorMessage(this.label2, "");
      this.label2.Location = new System.Drawing.Point(7, 41);
      this.label2.Name = "label2";
      this.controlStatusProvider.SetRequiredError(this.label2, false);
      this.label2.Size = new System.Drawing.Size(92, 23);
      this.label2.TabIndex = 4;
      this.label2.Text = "Password:";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.controlStatusProvider.SetWarnMessage(this.label2, "");
      // 
      // textBox2
      // 
      this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.controlStatusProvider.SetErrorMessage(this.textBox2, "");
      this.textBox2.Location = new System.Drawing.Point(105, 41);
      this.textBox2.Name = "textBox2";
      this.controlStatusProvider.SetRequiredError(this.textBox2, false);
      this.textBox2.Size = new System.Drawing.Size(200, 23);
      this.textBox2.TabIndex = 3;
      this.textBox2.UseSystemPasswordChar = true;
      this.controlStatusProvider.SetWarnMessage(this.textBox2, "");
      // 
      // label3
      // 
      this.controlStatusProvider.SetErrorMessage(this.label3, "");
      this.label3.Location = new System.Drawing.Point(7, 70);
      this.label3.Name = "label3";
      this.controlStatusProvider.SetRequiredError(this.label3, false);
      this.label3.Size = new System.Drawing.Size(92, 23);
      this.label3.TabIndex = 6;
      this.label3.Text = "Name:";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.controlStatusProvider.SetWarnMessage(this.label3, "");
      // 
      // textBox3
      // 
      this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.controlStatusProvider.SetErrorMessage(this.textBox3, "");
      this.textBox3.Location = new System.Drawing.Point(105, 70);
      this.textBox3.Name = "textBox3";
      this.controlStatusProvider.SetRequiredError(this.textBox3, false);
      this.textBox3.Size = new System.Drawing.Size(200, 23);
      this.textBox3.TabIndex = 5;
      this.controlStatusProvider.SetWarnMessage(this.textBox3, "");
      // 
      // label4
      // 
      this.controlStatusProvider.SetErrorMessage(this.label4, "");
      this.label4.Location = new System.Drawing.Point(7, 128);
      this.label4.Name = "label4";
      this.controlStatusProvider.SetRequiredError(this.label4, false);
      this.label4.Size = new System.Drawing.Size(92, 23);
      this.label4.TabIndex = 7;
      this.label4.Text = "Birthdate:";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.controlStatusProvider.SetWarnMessage(this.label4, "");
      // 
      // label5
      // 
      this.controlStatusProvider.SetErrorMessage(this.label5, "");
      this.label5.Location = new System.Drawing.Point(7, 99);
      this.label5.Name = "label5";
      this.controlStatusProvider.SetRequiredError(this.label5, false);
      this.label5.Size = new System.Drawing.Size(92, 23);
      this.label5.TabIndex = 9;
      this.label5.Text = "Mail:";
      this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.controlStatusProvider.SetWarnMessage(this.label5, "");
      // 
      // textBox4
      // 
      this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Mail", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.controlStatusProvider.SetErrorMessage(this.textBox4, "");
      this.textBox4.Location = new System.Drawing.Point(105, 99);
      this.textBox4.Name = "textBox4";
      this.controlStatusProvider.SetRequiredError(this.textBox4, false);
      this.textBox4.Size = new System.Drawing.Size(200, 23);
      this.textBox4.TabIndex = 8;
      this.controlStatusProvider.SetWarnMessage(this.textBox4, "");
      // 
      // bindingSourceValidator
      // 
      this.bindingSourceValidator.BindingSource = this.bindingSource;
      this.bindingSourceValidator.ControlStatusProvider = this.controlStatusProvider;
      this.bindingSourceValidator.IsValid = false;
      this.bindingSourceValidator.SuspendValidation = false;
      objectValidator1.ServiceProvider = null;
      this.bindingSourceValidator.Validator = objectValidator1;
      // 
      // errorProvider1
      // 
      this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.errorProvider1.ContainerControl = this;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(374, 169);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.textBox4);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.textBox3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.textBox2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.dateTimePicker1);
      this.Controls.Add(this.textBox1);
      this.controlStatusProvider.SetErrorMessage(this, "");
      this.Name = "Form1";
      this.controlStatusProvider.SetRequiredError(this, false);
      this.Text = "Form1";
      this.controlStatusProvider.SetWarnMessage(this, "");
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceValidator)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private BindingSource bindingSource;
    private TextBox textBox1;
    private DateTimePicker dateTimePicker1;
    private Label label1;
    private Label label2;
    private TextBox textBox2;
    private Label label3;
    private TextBox textBox3;
    private Label label4;
    private Label label5;
    private TextBox textBox4;
    private ControlStatusProvider controlStatusProvider;
    private BindingSourceValidator bindingSourceValidator;
    private ErrorProvider errorProvider1;
  }
}