using System.Web.Mvc;
using Instrumentation.WebApp.Helpers;
using Instrumentation.WebApp.Models;

namespace Instrumentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string id, string dbkey = null, int? maxrowcount = null)
        {
            var query = new AuditLogViewModel();

            query.DbKey = dbkey ?? Configurations.DbKeyDefault;
            query.MaxRowCount = maxrowcount ?? Configurations.MaxRowCountDefault;
            query.ReleaseVersion = Configurations.ReleaseVersion;
            query.CurrentServerTime = System.DateTime.UtcNow.ToString();

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
