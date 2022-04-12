namespace Infomatik.Validation.WinForms;

/// <summary>
/// Interface for controls implementing their own status visualization
/// </summary>
public interface IVisualizeStatus
{
  void SetErrorMessage(string? error);
  void SetWarnMessage(string? warning);
  void SetIsMissingValue(bool showRequired);

  string? GetErrorMessage();
  string? GetWarnMessage();
  bool GetRequiredError();
}