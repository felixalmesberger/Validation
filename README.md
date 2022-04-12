# Extended Valdidation for .NET & Windows Forms

This library aims to replace the static [Validator](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validator) and offer extended functionality:

1. Supports full object validation using attributes and the IValidateObject.Validate method. Other than the .NET Validator, the Validate method will always be evaluated, even when errors are found  during attribute-based validation. This allows to mix the attribute and IValidatableObject base mechanisms.
2. Supports errors, warnings, and missing property values.  
3. Not static: The validator implements an interface, that can be exchanged in tests. 
4. Easy usage: No creating and passing of ValidationContexts etc. One call thats all.

[more info](docs/validator.d)

## UI Visualization

If you want to visualize the validation results on a Windows Forms user control check out the BindingSourceValidator.

![sample](docs/sample.png)

[more info](docs/bindingsourcevalidato.mdr)
