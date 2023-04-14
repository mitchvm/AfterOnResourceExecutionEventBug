using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AfterOnResourceExecutionEventBug;

internal class AsyncResourceFilterAttribute : Attribute, IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
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
}