using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleJwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApiDemo.Controllers
{
    public class AuthenticateAttribute : ActionFilterAttribute
    {
        //TODO implement protection https://github.com/cuongle/Hmac.WebApi/blob/master/Hmac.Api/Filters/AuthenticateAttribute.cs
        //TODO implement protection https://stackoverflow.com/questions/11775594/how-to-secure-an-asp-net-web-api
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // do something before the action executes
            var keys = context.HttpContext.Request.Headers["Token"];

            if (!JwtHelper.VerifyToken(keys, LoginController.Key, LoginController.Issuer))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }
}