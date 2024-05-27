using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UI.Controllers;

namespace UI.ActionFilters
{
    public class AuthActionFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("loggedUser")==null)
                context.Result = new RedirectResult("/Home/Login");
            int id = int.Parse(context.HttpContext.Session.GetString("loggedUser"));
        }
    }
}
