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
            ViewQueryHome query = new ViewQueryHome();
            query.ViewName = "Home";
            query.ReleaseVersion = Configurations.ReleaseVersion;
            query.CurrentServerTime = System.DateTime.Now.ToString();

            InitDbOptionSelectList(query);

            return View(query);
        }

        public ActionResult Header(ViewQueryBase query = null)
        {
            query = query ?? new ViewQueryBase();

            return View(query);
        }

        private void InitDbOptionSelectList(ViewQueryBase query)
        {
            query.DbOptionSelectList = new SelectList(GetDbOptionsFromConfig(), "Value", "Description");
        }
        private List<LookupItem> GetDbOptionsFromConfig()
        {
            var keys = Configurations.DbKeys;
            var splits = keys.Split(';');
            if (splits == null || splits.Length == 0)
                throw new ConfigurationErrorsException("DBKeys configuration should have 1 or more dbkeys.");

            List<LookupItem> dbOptions = new List<LookupItem>();
            foreach (var split in splits)
            {
                dbOptions.Add(new LookupItem { Value = split, Description = split });
            }

            return dbOptions;
        }
    }
}
