using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation;

public class DeepValidationWarningResult : WarningValidationResult, IDeepValidationResult
{

  #region constructors
  public DeepValidationWarningResult(string errorMessage) : base(errorMessage)
  {
  }

  public DeepValidationWarningResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames)
  {
  }

  public DeepValidationWarningResult(ValidationResult validationResult) : base(validationResult)
  {
  }

  #endregion

  public IList<ObjectValidationResult> Children { get; set; } = new List<ObjectValidationResult>();
  public IEnumerator<ObjectValidationResult> GetEnumerator() => this.Children.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}