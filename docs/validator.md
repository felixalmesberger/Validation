# Validator

The ObjectValidator validates an object evaluating all validation attributes and the [IValidatableObject.Validate](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.ivalidatableobject.validate#system-componentmodel-dataannotations-ivalidatableobject-validate(system-componentmodel-dataannotations-validationcontext)) method. It returns an ObjectValidationResult which contains Errors, Warnings and MissingValues.

## What are missing values

Missing values are ValidationResults created by the \[Requried\] Attribute or **RequiredValidationResult**s, indicating, that a value is required but empty. In a lot of cases those errors are visualized differently, so they are stored seperatly.

## How to create a warning

To create a warning return a **WarningValidationResult** instead of a **ValidationResult**.

## How to use 

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
