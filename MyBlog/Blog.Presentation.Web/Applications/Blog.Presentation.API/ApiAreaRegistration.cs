using System.Web.Mvc;

namespace Blog.Presentation.API
{

    public class ApiAreaRegistration : AreaRegistration
    {

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "API_default",
                "API/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Blog.Presentation.API.Controllers" }
                );
        }

        public override string AreaName => "API";

    }

}