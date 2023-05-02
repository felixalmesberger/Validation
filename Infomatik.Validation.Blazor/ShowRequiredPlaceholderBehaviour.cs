//using System.Runtime.Serialization;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Forms;
//using Microsoft.JSInterop;

//namespace Infomatik.Validation.Blazor;

//public class ShowRequiredPlaceholderBehaviour : ComponentBase
//{

//  [Inject]
//  public IJSRuntime JSRuntime { get; set; }

//  [CascadingParameter]
//  private EditContext CurrentEditContext { get; set; } = null!;

//  [Parameter] 
//  public ExtendedValidator Validator { get; set; } = null!;


//  protected override Task OnInitializedAsync()
//  {
//    this.CurrentEditContext.OnValidationStateChanged += this.ValidationStateChanged;
//    return base.OnInitializedAsync();
//  }

//  private void ValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
//  {

//  }

//  private async Task SetPlaceholderAsync(string id, string value)
//  {
//    var js = $"document.getElementById(\"{id}\").setAttribute(\"placeholder\", \"{value}\")";
//    await this.JSRuntime.InvokeVoidAsync(js);
//  }



//}