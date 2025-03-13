using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CLIMFinders.Web.ServiceExtension
{
    public static class ModelStateHelper
    {
        public static void AddGlobalErrors(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                var errors = modelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                if (errors.Count != 0)
                {
                    string errorMessage = string.Join("\n", errors);
                    modelState.AddModelError(string.Empty, errorMessage);
                }
            }
        }
    }
}
