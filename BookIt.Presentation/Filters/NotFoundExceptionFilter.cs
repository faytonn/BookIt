using BookIt.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookIt.Presentation.Filters;

public class NotFoundExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not NotFoundException)
            return;

        context.Result = new NotFoundResult();
        context.ExceptionHandled = true;
    }
}
