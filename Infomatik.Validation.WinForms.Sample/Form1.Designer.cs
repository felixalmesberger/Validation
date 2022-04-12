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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
      this.bindingSourceValidator1 = new Infomatik.Validation.WinForms.BindingSourceValidator(this.components);
      this.validationStatusProvider1 = new Infomatik.Validation.WinForms.ValidationStatusProvider(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceValidator1)).BeginInit();
      this.SuspendLayout();
      // 
      // bindingSource
      // 
      this.bindingSource.DataSource = typeof(Infomatik.Validation.WinForms.Sample.User);
      // 
      // textBox1
      // 
      this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Username", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.validationStatusProvider1.SetErrorMessage(this.textBox1, "");
      this.validationStatusProvider1.SetIconAlignment(this.textBox1, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
      this.validationStatusProvider1.SetIconPadding(this.textBox1, 0);
      this.validationStatusProvider1.SetIsMissingValue(this.textBox1, false);
      this.textBox1.Location = new System.Drawing.Point(105, 12);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(200, 23);
      this.textBox1.TabIndex = 0;
      this.validationStatusProvider1.SetWarnMessage(this.textBox1, "");
      // 
      // dateTimePicker1
      // 
      this.dateTimePicker1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSource, "Birthdate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.validationStatusProvider1.SetErrorMessage(this.dateTimePicker1, "");
      this.validationStatusProvider1.SetIconAlignment(this.dateTimePicker1, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
      this.validationStatusProvider1.SetIconPadding(this.dateTimePicker1, 0);
      this.validationStatusProvider1.SetIsMissingValue(this.dateTimePicker1, false);
      this.dateTimePicker1.Location = new System.Drawing.Point(105, 128);
      this.dateTimePicker1.Name = "dateTimePicker1";
      this.dateTimePicker1.Size = new System.Drawing.Size(200, 23);
      this.dateTimePicker1.TabIndex = 1;
      this.validationStatusProvider1.SetWarnMessage(this.dateTimePicker1, "");
      // 
      // label1
      // 
      this.validationStatusProvider1.SetErrorMessage(this.label1, "");
      this.validationStatusProvider1.SetIconAlignment(this.label1, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
      this.validationStatusProvider1.SetIconPadding(this.label1, 0);
      this.validationStatusProvider1.SetIsMissingValue(this.label1, false);
      this.label1.Location = new System.Drawing.Point(7, 12);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(92, 23);
      this.label1.TabIndex = 2;
      this.label1.Text = "Username:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.validationStatusProvider1.SetWarnMessage(this.label1, "");
      // 
      // label2
      // 
      this.validationStatusProvider1.SetErrorMessage(this.label2, "");
      this.validationStatusProvider1.SetIconAlignment(this.label2, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
      this.validationStatusProvider1.SetIconPadding(this.label2, 0);
      this.validationStatusProvider1.SetIsMissingValue(this.label2, false);
      this.label2.Location = new System.Drawing.Point(7, 41);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(92, 23);
      this.label2.TabIndex = 4;
      this.label2.Text = "Password:";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.validationStatusProvider1.SetWarnMessage(this.label2, "");
      // 
      // textBox2
      // 
      this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.validationStatusProvider1.SetErrorMessage(this.textBox2, "");
      this.validationStatusProvider1.SetIconAlignment(this.textBox2, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
      this.validationStatusProvider1.SetIconPadding(this.textBox2, 0);
      this.validationStatusProvider1.SetIsMissingValue(this.textBox2, false);
      this.textBox2.Location = new System.Drawing.Point(105, 41);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(200, 23);
      this.textBox2.TabIndex = 3;
      this.textBox2.UseSystemPasswordChar = true;
      this.validationStatusProvider1.SetWarnMessage(this.textBox2, "");
      // 
      // label3
      // 
      this.validationStatusProvider1.SetErrorMessage(this.label3, "");
      this.validationStatusProvider1.SetIconAlignment(this.label3, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
      this.validationStatusProvider1.SetIconPadding(this.label3, 0);
      this.validationStatusProvider1.SetIsMissingValue(this.label3, false);
      this.label3.Location = new System.Drawing.Point(7, 70);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(92, 23);
      this.label3.TabIndex = 6;
      this.label3.Text = "Name:";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.validationStatusProvider1.SetWarnMessage(this.label3, "");
      // 
      // textBox3
      // 
      this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.validationStatusProvider1.SetErrorMessage(this.textBox3, "");
      this.validationStatusProvider1.SetIconAlignment(this.textBox3, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
      this.validationStatusProvider1.SetIconPadding(this.textBox3, 0);
      this.validationStatusProvider1.SetIsMissingValue(this.textBox3, false);
      this.textBox3.Location = new System.Drawing.Point(105, 70);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new System.Drawing.Size(200, 23);
      this.textBox3.TabIndex = 5;
      this.validationStatusProvider1.SetWarnMessage(this.textBox3, "");
      // 
      // label4
      // 
      this.validationStatusProvider1.SetErrorMessage(this.label4, "");
      this.validationStatusProvider1.SetIconAlignment(this.label4, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
      this.validationStatusProvider1.SetIconPadding(this.label4, 0);
      this.validationStatusProvider1.SetIsMissingValue(this.label4, false);
      this.label4.Location = new System.Drawing.Point(7, 128);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(92, 23);
      this.label4.TabIndex = 7;
      this.label4.Text = "Birthdate:";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.validationStatusProvider1.SetWarnMessage(this.label4, "");
      // 
      // label5
      // 
      this.validationStatusProvider1.SetErrorMessage(this.label5, "");
      this.validationStatusProvider1.SetIconAlignment(this.label5, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
      this.validationStatusProvider1.SetIconPadding(this.label5, 0);
      this.validationStatusProvider1.SetIsMissingValue(this.label5, false);
      this.label5.Location = new System.Drawing.Point(7, 99);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(92, 23);
      this.label5.TabIndex = 9;
      this.label5.Text = "Mail:";
      this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.validationStatusProvider1.SetWarnMessage(this.label5, "");
      // 
      // textBox4
      // 
      this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Mail", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.validationStatusProvider1.SetErrorMessage(this.textBox4, "");
      this.validationStatusProvider1.SetIconAlignment(this.textBox4, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
      this.validationStatusProvider1.SetIconPadding(this.textBox4, 0);
      this.validationStatusProvider1.SetIsMissingValue(this.textBox4, false);
      this.textBox4.Location = new System.Drawing.Point(105, 99);
      this.textBox4.Name = "textBox4";
      this.textBox4.Size = new System.Drawing.Size(200, 23);
      this.textBox4.TabIndex = 8;
      this.validationStatusProvider1.SetWarnMessage(this.textBox4, "");
      // 
      // bindingSourceValidator1
      // 
      this.bindingSourceValidator1.BindingSource = this.bindingSource;
      this.bindingSourceValidator1.ValidationStatusProvider = this.validationStatusProvider1;
      this.bindingSourceValidator1.Validator = objectValidator1;
      // 
      // validationStatusProvider1
      // 
      this.validationStatusProvider1.ErrorIcon = ((System.Drawing.Icon)(resources.GetObject("validationStatusProvider1.ErrorIcon")));
      this.validationStatusProvider1.WarnIcon = ((System.Drawing.Icon)(resources.GetObject("validationStatusProvider1.WarnIcon")));
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
      this.validationStatusProvider1.SetErrorMessage(this, "");
      this.validationStatusProvider1.SetIconAlignment(this, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
      this.validationStatusProvider1.SetIconPadding(this, 0);
      this.validationStatusProvider1.SetIsMissingValue(this, false);
      this.Name = "Form1";
      this.Text = "Sample";
      this.validationStatusProvider1.SetWarnMessage(this, "");
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceValidator1)).EndInit();
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
    private BindingSourceValidator bindingSourceValidator1;
    private ValidationStatusProvider validationStatusProvider1;
  }
}