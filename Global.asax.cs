using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Http.WebHost;

namespace MyWebAPIDeal
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);   
        }

        //public override void Init()
        //{
        //    this.PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
        //    base.Init();
        //}
        //void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        //{
        //    System.Web.HttpContext.Current.SetSessionStateBehavior(
        //        SessionStateBehavior.Required);
        //}

        //public class SessionHttpControllerHandler : HttpControllerHandler, IRequiresSessionState
        //{
        //    public SessionHttpControllerHandler(RouteData routeData)
        //        : base(routeData)
        //    {
        //    }
        //}

        //public class SessionHttpControllerRouteHandler : HttpControllerRouteHandler
        //{
        //    protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        //    {
        //        return new SessionHttpControllerHandler(requestContext.RouteData);
        //    }
        //}
    }
}