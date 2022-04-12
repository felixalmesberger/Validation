using System.ComponentModel;
using Infomatik.Validation.WinForms.i18n;
using Infomatik.Validation.WinForms.Properties;

namespace Infomatik.Validation.WinForms;

/// <summary>
/// Capable of showing an error / warn icon or setting
/// the cue banner for required errors
/// </summary>
[ToolboxBitmap(typeof(ErrorProvider))]
[ProvideProperty("IconPadding", typeof(Control))]
[ProvideProperty("IconAlignment", typeof(Control))]
[ProvideProperty("IsMissingValue", typeof(Control))]
[ProvideProperty("ErrorMessage", typeof(Control))]
[ProvideProperty("WarnMessage", typeof(Control))]
public class ValidationStatusProvider : Component, IExtenderProvider
{
  #region fields

  private readonly ErrorProvider errorProvider = new()
  {
    BlinkStyle = ErrorBlinkStyle.NeverBlink
  };

  private readonly ErrorProvider warnProvider = new()
  {
    BlinkStyle = ErrorBlinkStyle.NeverBlink
  };

  private readonly IsMissingStatusProvider isMissingStatusProvider = new();

  #endregion

  #region constructors

  public ValidationStatusProvider()
  {
    this.warnProvider.Icon = DefaultIcons.Warning;
    this.errorProvider.Icon = DefaultIcons.Error;
  }

  public ValidationStatusProvider(IContainer? container)
    : this()
  {
    container?.Add(this);
  }

  #endregion

  #region properties

  public string RequiredFallbackErrorMessage => Strings.Required;

  #endregion

  #region properties

  /// <summary>
  /// Gets or sets the Icon that displayed next to a control when a warn has been set for the control.
  /// </summary>
  [Category("ValidationStatusProvider")]
  [Description("Gets or sets the Icon that displayed next to a control when a warn has been set for the control.")]
  public Icon? WarnIcon
  {
    get => this.warnProvider.Icon;
    set
    {
      if (value is null)
        return;

      this.warnProvider.Icon = value;
    }
  }


  /// <summary>
  /// Gets or sets the Icon that displayed next to a control when an error has been set for the control.
  /// </summary>
  [Category("ValidationStatusProvider")]
  [Description("Gets or sets the Icon that displayed next to a control when an error has been set for the control.")]
  public Icon? ErrorIcon
  {
    get => this.errorProvider.Icon;
    set
    {
      if (value is null)
        return;

      this.errorProvider.Icon = value;
    }
  }

  #endregion

  #region provided properties

  public void SetIconPadding(Control control, int padding)
  {
    this.errorProvider.SetIconPadding(control, padding);
    this.warnProvider.SetIconPadding(control, padding);
  }

  public int GetIconPadding(Control control)
  {
    return this.errorProvider.GetIconPadding(control);
  }

  public void SetIconAlignment(Control control, ErrorIconAlignment alignment)
  {
    this.errorProvider.SetIconAlignment(control, alignment);
    this.warnProvider.SetIconAlignment(control, alignment);
  }

  public ErrorIconAlignment GetIconAlignment(Control control)
  {
    return this.errorProvider.GetIconAlignment(control);
  }

  public void Clear(Control control)
  {
    this.SetIsMissingValue(control, false);
    this.SetErrorMessage(control, null);
  }

  #endregion

  public void SetIsMissingValue(Control control, bool isMissing)
  {
    // ReSharper disable once SuspiciousTypeConversion.Global
    if (control is IVisualizeStatus visualizer)
    {
      visualizer.SetIsMissingValue(isMissing);
      return;
    }

    this.InitializeDefaultControlIconAlignment(control);

    var canUseRequiredProvider = this.isMissingStatusProvider.CanExtend(control);
    var errorProviderShowingRequiredMessage = this.errorProvider.GetError(control) == RequiredFallbackErrorMessage;

    if (!isMissing && errorProviderShowingRequiredMessage)
      this.errorProvider.SetError(control, null);
    else if (!isMissing && canUseRequiredProvider)
      this.isMissingStatusProvider.SetShowRequiredMessage(control, false);
    else if (isMissing && canUseRequiredProvider)
      this.isMissingStatusProvider.SetShowRequiredMessage(control, true);
    else if (isMissing && !canUseRequiredProvider)
      this.errorProvider.SetError(control, RequiredFallbackErrorMessage);
  }

  public void SetErrorMessage(Control control, string? message)
  {
    // ReSharper disable once SuspiciousTypeConversion.Global
    if (control is IVisualizeStatus visualizer)
    {
      visualizer.SetErrorMessage(message);
      return;
    }

    this.InitializeDefaultControlIconAlignment(control);

    if (message is not null)
      this.warnProvider.SetError(control, null);

    this.errorProvider.SetError(control, message);
  }

  public void SetWarnMessage(Control control, string? message)
  {
    // ReSharper disable once SuspiciousTypeConversion.Global
    if (control is IVisualizeStatus visualizer)
    {
      visualizer.SetWarnMessage(message);
      return;
    }

    this.InitializeDefaultControlIconAlignment(control);

    if (message is not null)
      this.errorProvider.SetError(control, null);
    this.warnProvider.SetError(control, message);
  }

  /// <summary>
  /// Visualize a warn message
  /// </summary>
  [Category("ValidationStatusProvider")]
  [Description("Visualize a warn message")]
  public string? GetWarnMessage(Control control)
  {
    // ReSharper disable once SuspiciousTypeConversion.Global
    if (control is IVisualizeStatus visualizer)
      return visualizer.GetWarnMessage();

    return this.warnProvider.GetError(control);
  }


  /// <summary>
  /// Visualize, that this control is bound to a required property
  /// </summary>
  [Category("ValidationStatusProvider")]
  [Description("Visualize that this is a required field")]
  public bool GetIsMissingValue(Control control)
  {
    // ReSharper disable once SuspiciousTypeConversion.Global
    if (control is IVisualizeStatus visualizer)
      return visualizer.GetRequiredError();

    return this.isMissingStatusProvider.GetShowRequiredMessage(control);
  }

  /// <summary>
  /// Visualize an error message
  /// </summary>
  [Category("ValidationStatusProvider")]
  [Description("Visualize an error message")]
  public string? GetErrorMessage(Control control)
  {
    // ReSharper disable once SuspiciousTypeConversion.Global
    if (control is IVisualizeStatus visualizer)
      return visualizer.GetErrorMessage();

    return this.errorProvider.GetError(control);
  }

  /// <summary>
  /// Is the control supported by the ValidationStatusProvider
  /// </summary>
  public bool CanExtend(object extendee)
  {
    return extendee is Control;
  }

  /// <summary>
  /// Sets the default icon alignment to middle right with padding 5
  /// </summary>
  /// <param name="control"></param>
  private void InitializeDefaultControlIconAlignment(Control control)
  {
    // if padding != 0, padding already set so return
    if (this.errorProvider.GetIconPadding(control) != 0)
      return;

    this.errorProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
    this.errorProvider.SetIconPadding(control, 5);

    this.warnProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
    this.warnProvider.SetIconPadding(control, 5);
  }
}