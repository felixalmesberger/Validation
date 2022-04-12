# Extended Valdidation for .NET & Windows Forms

This library aims to replace the static [Validator]( https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validator) and offer extended functionality:

1. Supports full object validation using attributes and the IValidateObject.Validate method. Other than the .NET Validator, the Validate method will always be evaluated, even when errors happened during attribute-based validation. This allows to mix the attribute and IValidatableObject base mechanisms.
2. Supports errors, warnings, and missing property values.  
3. Not static: The validator implements an interface, that can be exchanged in tests. 
4. Easy usage: No creating and passing of ValidationContexts etc. One call thats all.

## UI Visualization

![image info](https://raw.githubusercontent.com/felixalmesberger/Validation/main/docs/sample.png?token=GHSAT0AAAAAABLD7QBWYNIXMTH7FIMPZFKCYSVNJWQ)

It also includes a Windows Forms library making it very easy to visualize the status of an object.

Example

    // create a new validator instance
    var validator = new ObjectValidator()
    {
      ServiceProvider = myServiceProvider
    };

	// or use default instance
	ObjectValidator.Default.ServiceProvider = myServiceProvider;

	// define a data class
	public class ValidatableObject : IValidatableObject 
	{
		[Required]
		public string RequiredProperty { get; set; }
		
		[Range(0,1000)]
		public int Min {get; set;}
		
		public int InBetweenMinAndMax { get; set; }
		
		[Range(0,1000)]
		public int Max { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext context)
		{
			if(this.Min > this.InBetweenMinAndMax || this.Max < this.InBetweenMinAndMax)
			{
				var message = $"InBetweenMinAndMax must be between {this.Min} and {this.Max}";
				yield return this.CreateError(message, nameof(this.InBetweenMinAndMax));
			}
		}
    }

	// do validation
	var obj = new ValidatableObject();
	var validationResult = validator.Validate(obj);
