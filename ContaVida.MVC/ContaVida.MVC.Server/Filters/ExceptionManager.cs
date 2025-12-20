using ContaVida.MVC.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ContaVida.MVC.Server.Filters
{
    public class ExceptionManager : ExceptionFilterAttribute
    {
        //https://gist.github.com/jppampin/beac17c279cd63a565518a93409e6751
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                context.Result = new JsonResult(new { Message = context.Exception.Message });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            if (context.Exception is IncorrectPasswordException)
            {
                context.Result = new JsonResult(new { Message = context.Exception.Message });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }

            base.OnException(context);
        }
    }
}
