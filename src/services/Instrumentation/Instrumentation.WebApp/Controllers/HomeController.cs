using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Instrumentation.WebApp.Helpers;
using Instrumentation.WebApp.Models;

namespace Instrumentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var query = new AuditLogViewModel();

            query.ReleaseVersion = Configurations.ReleaseVersion;
            query.CurrentServerTime = System.DateTime.Now.ToString();

            query.DbKeyList = InitDbKeySelectList();

            return View(query);
        }

        public ActionResult Header(AuditLogViewModel query = null)
        {
            query = query ?? new AuditLogViewModel();

            return View(query);
        }

        private SelectList InitDbKeySelectList()
        {
            var dbKeys = Configurations.GetDbKeysFromConfig();
            return new SelectList(dbKeys, "Value", "Description");
        }

    }
}
