using System;
using System.Data.Entity.Validation;
using System.Text;

public static class DbEntityValidationExceptionExtensions
{
	#region Public Methods

	/// <summary>
	/// Provides a formatted message of error information for each property that failed validation.
	/// </summary>
	/// <param name="self"></param>
	/// <returns></returns>
	public static string GetFriendlyMessage(this DbEntityValidationException self)
	{
		var errorMessage = new StringBuilder();
		foreach (DbEntityValidationResult validationErrors in self.EntityValidationErrors)
		{
			foreach (DbValidationError validationError in validationErrors.ValidationErrors)
			{
				errorMessage.Append($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage} {Environment.NewLine}");
			}
		}

		return errorMessage.ToString();
	}

	#endregion Public Methods
}
