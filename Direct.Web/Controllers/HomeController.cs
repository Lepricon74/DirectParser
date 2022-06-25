using Microsoft.AspNetCore.Mvc;
using Vostok.Logging.Abstractions;

namespace Direct.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILog log;

        public HomeController(ILog _log)
        {
            log = _log;
        }

        public IActionResult Index()
        {
            log.Info("GET-MAIN-APP-REQUEST"
                     + " HostAddress: " + (Request.HttpContext.Connection.RemoteIpAddress) + ","
                     + " UserAgent: "+ (Request.Headers["User-Agent"])+","
                     + " Platform: " + Request.Headers["sec-ch-ua-platform"]);
            return View();
        }
    }
}
