using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation;

public class DeepValidationErrorResult : ValidationResult, IDeepValidationResult
{
  protected DeepValidationErrorResult(ValidationResult validationResult) : base(validationResult)
  {
  }

  public DeepValidationErrorResult(string? errorMessage) : base(errorMessage)
  {
  }

  public DeepValidationErrorResult(string? errorMessage, IEnumerable<string>? memberNames) : base(errorMessage, memberNames)
  {
  }

  public IList<ObjectValidationResult> Children { get; set; } = new List<ObjectValidationResult>();
  public IEnumerator<ObjectValidationResult> GetEnumerator() => this.Children.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}