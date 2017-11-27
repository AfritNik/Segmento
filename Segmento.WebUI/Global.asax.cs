using Ninject.Web.Common.WebHost;
using Segmento.Domain;
using Segmento.WebUI.App_Start;
using Segmento.WebUI.Infrastructure.Binders;
using System.Net;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Tweetinvi.Models;
using Ninject;

namespace Segmento.WebUI
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.Add(typeof(IAuthenticatedUser), new UserModelBinder());

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Settings.Initialize("Z450BwWnE34tI0GuVEiTwK5Hu", "Vyxpza3PdQzfJoxIkBiDZn3wupigork4RYC3it3crwWpKxE8tY",
                "927601496631504896-LfYfVpI43JxLgitHgLK2YYoXbVhyGoe", "Z7wh2ygDSybQ8bePjswfGRlKO99psvqGTj2MXT5AWb3ab");
          ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls12
        | SecurityProtocolType.Ssl3;
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        private void RegisterServices(StandardKernel kernel)
        {
            DependencyResolver.SetResolver(new Segmento.WebUI.Infrastructure.NinjectDependencyResolver(kernel));
        }
    }
}
