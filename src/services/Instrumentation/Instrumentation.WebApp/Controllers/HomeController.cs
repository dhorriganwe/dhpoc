using System.Web.Mvc;
using Instrumentation.WebApp.Helpers;
using Instrumentation.WebApp.Models;

namespace Instrumentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewQueryHome query = new ViewQueryHome();
            query.ViewQueryCommon.ViewName = "Home";
            query.ReleaseVersion = Configurations.ReleaseVersion;
            query.CurrentServerTime = System.DateTime.Now.ToString();

            return View(query);
        }

        public ActionResult Header(ViewQueryCommon query = null)
        {
            query = query ?? new ViewQueryCommon();

            return View(query);
        }
    }
}
