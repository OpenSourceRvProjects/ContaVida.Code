using Microsoft.AspNetCore.Mvc.Filters;

namespace ContaVida.MVC.Server.Filters
{
    public class LoggedUserDataFilter : ActionFilterAttribute, IActionFilter, IAsyncActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            //if (context.HttpContext.Session.GetString("userID") == null)
            //{
            var tokenHeader = context.HttpContext.Request.Headers["Authorization"];
            var bearerToken = tokenHeader.FirstOrDefault();

            var token = bearerToken?.Split("Bearer ")[1];
            var tokenDataDecoded = FilterHelper.GetTokenDataByStringValue(token);

            var userID = tokenDataDecoded.Claims.Where(W => W.Type == "userID").FirstOrDefault().Value;
            context.HttpContext.Session.SetString("userID", userID);

            //}
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            this.OnActionExecuting(context);
            var resultContext = await next();
            this.OnActionExecuted(resultContext);
        }
    }
}
