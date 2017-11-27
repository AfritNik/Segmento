using System.Web.Mvc;
using Tweetinvi.Models;

namespace Segmento.WebUI.Infrastructure.Binders
{
    public class UserModelBinder:IModelBinder
    {
        private const string sessionKey = "User";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            IAuthenticatedUser user = null;
            if (controllerContext.HttpContext.Session != null)
            {
                user = controllerContext.HttpContext.Session[sessionKey] as IAuthenticatedUser;
            }
            return user;
        }
    }
}