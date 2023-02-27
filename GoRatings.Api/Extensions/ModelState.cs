using System.Text;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GoRatings.Api;

public static partial class Extensions
{
	public static string GetErrors(this ModelStateDictionary modelState)
	{
		if (modelState.IsValid)
			return string.Empty;

		var sb = new StringBuilder();

		foreach (ModelStateEntry mse in modelState.Values.Where(mse => mse != null))
			sb.AppendLine(string.Join(", ", mse.Errors.Select(e => e.ErrorMessage)));

		return sb.ToString();
	}
}
