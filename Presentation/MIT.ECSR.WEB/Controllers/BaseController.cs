using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Web.Helper;

namespace MIT.ECSR.WEB.Controllers
{
    public class BaseController<T> : Controller
    {
        private ILogger<T> _loggerInstance;
        protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
        protected TokenUserObject Token;
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = HelperClient.GetToken(HttpContext.Request);
            if (token.Success)
            {
                ViewBag.User = token.Result.User;
                Token = token.Result.User;
            }
            
            await base.OnActionExecutionAsync(context, next);
        }
    }
}