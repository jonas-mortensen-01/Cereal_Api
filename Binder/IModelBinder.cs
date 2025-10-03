using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

public class JsonQueryModelBinder<T> : IModelBinder
{
    // Checks and validates each action parameter from a reques
    // and figures out how to populate it.
    // This is currently just used to allow RequestContext as the parameter for our "get"
    // where it is passed as a url encrypted json string but will be converted to a model
    // to allow the scalar reference to detect some of the models used
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            bindingContext.Result = ModelBindingResult.Success(default(T));
            return Task.CompletedTask;
        }

        var value = valueProviderResult.FirstValue;
        if (string.IsNullOrEmpty(value))
        {
            bindingContext.Result = ModelBindingResult.Success(default(T));
            return Task.CompletedTask;
        }

        try
        {
            var result = JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            bindingContext.Result = ModelBindingResult.Success(result);
        }
        catch
        {
            bindingContext.ModelState.AddModelError(bindingContext.FieldName, "Invalid JSON.");
        }

        return Task.CompletedTask;
    }
}
