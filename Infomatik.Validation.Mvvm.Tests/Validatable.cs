﻿using System.ComponentModel.DataAnnotations;

namespace Infomatik.Validation.Mvvm.Tests;

public class MvvmValidatableTest : ValidatableViewModel
{

  private string nonEmpty = "HALLO";

  [Required]
  public string NonEmpty
  {
    get => this.nonEmpty;
    set => this.SetAndValidate(ref this.nonEmpty, value);
  }

  public MvvmValidatableTest()
    : base(TimeSpan.Zero)
  {
    this.SynchronizationContext = null;
  }
}