using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

// Non-generic binder for RequestContext
public class FromJsonQueryRequestContextAttribute : ModelBinderAttribute
{
    public FromJsonQueryRequestContextAttribute() 
        : base(typeof(JsonQueryModelBinder<RequestContext>)) // closed generic type
    {
        BindingSource = BindingSource.Query;
    }
}