using PagedRequestBuilder.Models;
using System;
using System.Runtime.CompilerServices;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace PagedRequestBuilder.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PagedRequestKeyAttribute : Attribute
{
    public string RequestKey { get; }
    public string? TargetProperty { get; }
    public PagedRequestKeyAttribute(string key, [CallerMemberName] string? property = null)
    {
        RequestKey = key;
        TargetProperty = property;
    }
}

public class PagedRequestModelBinder : IModelBinder
{
    public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
    {
        bindingContext.Model = new PagedRequestBase();
        return true;
    }
}
