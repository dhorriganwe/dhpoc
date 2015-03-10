using System.Web.Mvc;
using Instrumentation.WebApp.Helpers;
using Instrumentation.WebApp.Models;

namespace Instrumentation.WebApp.Controllers
{
    public class InstrumentationSvcController : Controller
    {
        public ActionResult Index()
        {
            //var query = new AuditLogViewModel();

            //query.DbKey = dbkey ?? Configurations.DbKeyDefault;
            //query.MaxRowCount = maxrowcount ?? Configurations.MaxRowCountDefault;
            //query.ReleaseVersion = Configurations.ReleaseVersion;
            //query.CurrentServerTime = System.DateTime.UtcNow.ToString();

            //query.DbKeyList = InitDbKeySelectList();

            return View();
        }

        private SelectList InitDbKeySelectList()
        {
            var dbKeys = Configurations.GetDbKeysFromConfig();
            return new SelectList(dbKeys, "Value", "Description");
        }

    }
}
