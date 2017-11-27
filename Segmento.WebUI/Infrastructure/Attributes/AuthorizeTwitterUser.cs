using Segmento.Domain;
using Segmento.WebUI.Controllers;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Segmento.WebUI.Infrastructure.Attributes
{
    public class AuthorizeTwitterUser : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {            
            if (ReportLikesByPeriodController.GetCurrentUser(httpContext.Session) != null)
                return true;
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "TwitterAuth", Action = "TwitterAuth" }));
        }

        
    }
}