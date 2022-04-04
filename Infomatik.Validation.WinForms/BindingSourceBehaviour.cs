using System.ComponentModel;

namespace Infomatik.Validation.WinForms;

public class BindingSourceBehaviour : Component
{

  #region fields

  private BindingSource? bindingSource;

  #endregion

  #region properties

  protected IEnumerable<Binding> Bindings => this.bindingSource?.CurrencyManager.Bindings.OfType<Binding>() ?? Enumerable.Empty<Binding>();
  protected IEnumerable<Control> BoundControls => this.Bindings.Select(x => x.Control);

  public BindingSource? BindingSource
  {
    get => this.bindingSource;
    set
    {
      if (value == this.BindingSource)
        return;

      this.DetachFromBindingSource();
      this.bindingSource = value;
      this.AttachToBindingSource();
    }
  }

  #endregion

  #region constructors

  public BindingSourceBehaviour()
  {
  }

  public BindingSourceBehaviour(IContainer? container)
  {
    container?.Add(this);
  }

  #endregion

  

  protected virtual bool AttachToBindingSource()
  {
    if (this.BindingSource is null)
      return false;

    this.BindingSource.BindingComplete += this.BindingComplete;
    this.BindingSource.DataSourceChanged += this.DataSourceChanged;

    return true;
  }

  protected virtual bool DetachFromBindingSource()
  {
    if (this.BindingSource is null)
      return false;

    this.BindingSource.BindingComplete -= this.BindingComplete;
    this.BindingSource.DataSourceChanged -= this.DataSourceChanged;

    return true;
  }

  protected virtual void DataSourceChanged(object? sender, EventArgs e)
  {
  }

  protected virtual void BindingComplete(object? sender, BindingCompleteEventArgs e)
  {
  }
}