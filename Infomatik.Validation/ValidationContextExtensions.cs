using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation;

internal static class ValidationContextExtensions
{
  public static bool TryGetObjectValidator(this ValidationContext me, out IObjectValidator? validator)
  {
    validator = null;

    if (me.Items.TryGetValue(ObjectValidator.ValidatorKey, out var match))
      validator = match as IObjectValidator;

    return validator is not null;
  }

  public static bool TryGetObjectVisited(this ValidationContext me, out HashSet<object>? visited)
  {
    visited = null;

    if (me.Items.TryGetValue(ObjectValidator.VisitedKey, out var match))
      visited = match as HashSet<object>;

    return visited is not null;
  }
}