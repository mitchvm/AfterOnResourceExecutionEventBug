using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AfterOnResourceExecutionEventBug;

internal class AsyncFilterAttribute : Attribute, IAsyncActionFilter, IAsyncResourceFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.Request.Path.Value.EndsWith("action"))
        {
            context.Result = new BadRequestResult();
            return;
        }
        else
        {
            await next();
        }
    }

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        if (context.HttpContext.Request.Path.Value.EndsWith("resource"))
        {
            context.Result = new BadRequestResult();
            return;
        }
        else
        {
            await next();
        }
    }
}