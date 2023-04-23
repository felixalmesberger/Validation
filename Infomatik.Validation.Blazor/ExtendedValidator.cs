using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Infomatik.Validation.Blazor;


public class ExtendedValidator : ComponentBase, IDisposable
{
  private ExtendedValidatorSubscriptions? subscriptions;

  [CascadingParameter] 
  private EditContext CurrentEditContext { get; set; } = null!;

  [Inject] 
  public IServiceProvider? ServiceProvider { get; set; } 

  protected override void OnInitialized()
  {
    this.subscriptions = new ExtendedValidatorSubscriptions(this.CurrentEditContext, this.ServiceProvider);
  }

  public string GetRequiredPlaceholderMessage(string fieldName)
    => this.subscriptions!.GetRequiredPlaceholder(fieldName);


  public string GetRequiredPlaceholderMessage<T>(Expression<Func<T>> expression) =>
    this.GetRequiredPlaceholderMessage(FieldIdentifier.Create(expression).FieldName);

  protected virtual void Dispose(bool disposing)
  {
  }

  void IDisposable.Dispose()
  {
    this.subscriptions?.Dispose();
    this.subscriptions = null;

    this.Dispose(disposing: true);
  }
}