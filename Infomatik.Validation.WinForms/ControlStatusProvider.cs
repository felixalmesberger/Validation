using System.ComponentModel;
using Infomatik.Validation.WinForms.i18n;
using Infomatik.Validation.WinForms.Properties;

namespace Infomatik.Validation.WinForms;

/// <summary>
/// Capable of showing an error / warn icon or setting
/// the cue banner for required errors
/// </summary>
[ProvideProperty("IconPadding", typeof(Control))]
[ProvideProperty("IconAlignment", typeof(Control))]
[ProvideProperty("RequiredError", typeof(Control))]
[ProvideProperty("ErrorMessage", typeof(Control))]
[ProvideProperty("WarnMessage", typeof(Control))]

public class ControlStatusProvider : Component, IExtenderProvider
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

  private readonly RequiredErrorProvider requiredProvider = new();

  #endregion

  #region constructors

  public ControlStatusProvider()
  {
    this.SetErrorProviderIcon(this.errorProvider, (Bitmap)Resources.Error.Clone());
    this.SetErrorProviderIcon(this.warnProvider, (Bitmap)Resources.Warning.Clone());
  }

  private void SetErrorProviderIcon(ErrorProvider provider, Bitmap icon)
  {
    provider.Icon = Icon.FromHandle(new Bitmap(icon).GetHicon());
  }

  public ControlStatusProvider(IContainer? container)
    : this()
  {
    container?.Add(this);
  }

  #endregion

  public void SetErrorIconPadding(Control control, int padding)
  {
    this.errorProvider.SetIconPadding(control, padding);
    this.warnProvider.SetIconPadding(control, padding);
  }

  public int GetErrorIconPadding(Control control)
  {
    return this.errorProvider.GetIconPadding(control);
  }

  public void SetErrorIconAlignment(Control control, ErrorIconAlignment alignment)
  {
    this.errorProvider.SetIconAlignment(control, alignment);
    this.warnProvider.SetIconAlignment(control, alignment);
  }

  public ErrorIconAlignment GetErrorIconAlignment(Control control)
  {
    return this.errorProvider.GetIconAlignment(control);
  }

  public void Clear(Control control)
  {
    this.SetRequiredError(control, false);
    this.SetErrorMessage(control, null);
  }

  public string RequiredFallbackErrorMessage => Strings.Required;

  public void SetRequiredError(Control control, bool showRequired)
  {
    this.InitializeControlIconAlignment(control);

    var canUseRequiredProvider = this.requiredProvider.CanExtend(control);
    var errorProviderShowingRequiredMessage = this.errorProvider.GetError(control) == RequiredFallbackErrorMessage;

    if (!showRequired && errorProviderShowingRequiredMessage)
      this.errorProvider.SetError(control, null);
    else if (!showRequired && canUseRequiredProvider)
      this.requiredProvider.SetShowRequiredMessage(control, false);
    else if (showRequired && canUseRequiredProvider)
      this.requiredProvider.SetShowRequiredMessage(control, true);
    else if (showRequired && !canUseRequiredProvider)
      this.errorProvider.SetError(control, RequiredFallbackErrorMessage);
  }

  public void SetErrorMessage(Control control, string? message)
  {
    this.InitializeControlIconAlignment(control);

    if (message is not null)
      this.warnProvider.SetError(control, null);

    this.errorProvider.SetError(control, message);
  }

  public void SetWarnMessage(Control control, string? message)
  {
    this.InitializeControlIconAlignment(control);

    if (message is not null)
      this.errorProvider.SetError(control, null);
    this.warnProvider.SetError(control, message);
  }

  public string GetWarnMessage(Control control)
  {
    return this.warnProvider.GetError(control);
  }

  public bool GetRequiredError(Control control)
  {
    return this.requiredProvider.GetShowRequiredMessage(control);
  }

  public string GetErrorMessage(Control control)
  {
    return this.errorProvider.GetError(control);
  }

  public bool CanExtend(object extendee)
  {
    return extendee is Control;
  }

  private void InitializeControlIconAlignment(Control control)
  {
    this.errorProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
    this.errorProvider.SetIconPadding(control, 5);

    this.warnProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
    this.warnProvider.SetIconPadding(control, 5);
  }
}