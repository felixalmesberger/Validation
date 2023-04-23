using System.ComponentModel;
using Xunit;

namespace Infomatik.Validation.Mvvm.Tests
{
  public class ValidatableViewModelTests
  {
    [Fact]
    public void RaisesErrorChanged()
    {
      var instance = new MvvmValidatableTest();
      var evt = Assert.Raises<DataErrorsChangedEventArgs>(h => instance.ErrorsChanged += h,
        h => instance.ErrorsChanged -= h, () => instance.NonEmpty = "");

      Assert.NotNull(evt);
      Assert.Equal("NonEmpty", evt.Arguments.PropertyName);
    }
  }
}