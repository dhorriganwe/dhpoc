using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Instrumentation.DomainDA.DataServices;
using Instrumentation.WebApp.Helpers;
using Instrumentation.WebApp.Models;
using AuditLog = Instrumentation.WebApp.Models.AuditLog;

namespace Instrumentation.WebApp.Controllers
{
    public class AuditDbController : Controller
    {
        private InstrumentationMapper _instrumentationMapper = new InstrumentationMapper();

        public ActionResult Index()
        {
            var query = new ViewQueryBase();
            query.ViewName = "Home";
            query.ReleaseVersion = Configurations.ReleaseVersion;

            return View(query);
        }

        public ActionResult Summary()
        {
            var query = new ViewQueryAuditLogSummary();

            return Summary(query, "Refresh");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Summary(ViewQueryAuditLogSummary query, string command)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            if (command == "Refresh")
            {
                var summary = auditLogDataService.GetAuditLogSummary();
                query.TotalRecordCount = summary.TotalRowCount;

                query.ApplicationNames = auditLogDataService.GetApplicationNames();
                query.FeatureNames = auditLogDataService.GetFeatureNames();
                query.Categories = auditLogDataService.GetCategories();
            }

            InitDbOptionSelectList(query);

            return View(query);
        }

        public ActionResult AuditLog()
        {
            var query = new ViewQueryAuditLog();

            return AuditLog(query, "Refresh");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLog(ViewQueryAuditLog query, string command)
        {
            query.ViewName = "AuditLog";

            if (command == "Refresh")
            {
                query.List.AuditLogs = GetAuditLogAll(query.DbKey);
            }
            else
            {
                query.List.AuditLogs = new List<AuditLog>();
            }

            InitDbOptionSelectList(query);

            return View(query);
        }

        public ActionResult AuditLogById(string id)
        {
            var query = new ViewQueryAuditLogById();
            query.AuditLogId = id;

            return AuditLogById(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogById(ViewQueryAuditLogById query, string command)
        {
            query.ViewName = "AuditLogById";


            if (command == "Search")
            {
                query.AuditLog = GetAuditLogById(query.AuditLogId, query.DbKey);

                var jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string json = jsSerializer.Serialize(query.AuditLog);

                query.Json = json;
            }
            else
            {
                query.AuditLog = new AuditLog();
            }

            InitDbOptionSelectList(query);

            return View(query);
        }

        public ActionResult AuditLogByEventId(string id)
        {
            var query = new ViewQueryAuditLogByEventId();
            query.EventId = id;

            return AuditLogByEventId(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByEventId(ViewQueryAuditLogByEventId query, string command)
        {
            query.ViewName = "AuditLogsByEventId";

            if (command == "Search")
            {
                query.List.AuditLogs = GetAuditLogByEventId(query.EventId, query.DbKey);
            }
            else
            {
                query.List.AuditLogs = new List<AuditLog>();
            }

            InitDbOptionSelectList(query);

            return View(query);
        }

        public ActionResult AuditLogByApplicationName(string id)
        {
            var query = new ViewQueryAuditLogByApplicationName();
            query.ApplicationName = id;

            return AuditLogByApplicationName(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByApplicationName(ViewQueryAuditLogByApplicationName query, string command)
        {
            query.ViewName = "AuditLogByApplicationName";

            if (command == "Search")
            {
                query.List.AuditLogs = GetAuditLogByApplicationName(query.ApplicationName, query.DbKey);
            }
            else
            {
                query.List.AuditLogs = new List<AuditLog>();
            }

            InitDbOptionSelectList(query);

            return View(query);
        }

        public ActionResult AuditLogByCategory(string id)
        {
            var query = new ViewQueryAuditLogByCategory();
            query.Category = id;

            return AuditLogByCategory(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByCategory(ViewQueryAuditLogByCategory query, string command)
        {
            query.ViewName = "AuditLogByCategory";

            if (command == "Search")
            {
                query.List.AuditLogs = GetAuditLogByCategory(query.Category, query.DbKey);
            }
            else
            {
                query.List.AuditLogs = new List<AuditLog>();
            }

            InitDbOptionSelectList(query);

            return View(query);
        }

        public ActionResult AuditLogByFeatureName(string id)
        {
            var query = new ViewQueryAuditLogByFeatureName();
            query.FeatureName = id;

            return AuditLogByFeatureName(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByFeatureName(ViewQueryAuditLogByFeatureName query, string command)
        {
            query.ViewName = "AuditLogByFeatureName";

            if (command == "Search")
            {
                query.List.AuditLogs = GetAuditLogByFeatureName(query.FeatureName, query.DbKey);
            }
            else
            {
                query.List.AuditLogs = new List<AuditLog>();
            }

            InitDbOptionSelectList(query);

            return View(query);
        }

        public ActionResult AuditLogRecord(AuditLog auditLog)
        {
            return View(auditLog);
        }

        public ActionResult AuditLogGrid(ViewQueryAuditLogList list)
        {
            return View(list);
        }

        public ActionResult DbSelect(ViewQueryBase query)
        {
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

        private List<AuditLog> GetAuditLogAll(string dbKey)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(dbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogAll().ToList());

            return auditLogs;
        }

        private AuditLog GetAuditLogById(string id, string dbKey)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(dbKey);

            AuditLog auditLog = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogById(id));

            return auditLog;
        }

        private List<AuditLog> GetAuditLogByEventId(string eventId, string dbKey)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(dbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogByEventId(eventId).ToList());

            return auditLogs;
        }

        private List<AuditLog> GetAuditLogByApplicationName(string applicationName, string dbKey)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(dbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogByApplicationName(applicationName).ToList());

            return auditLogs;
        }

        private List<AuditLog> GetAuditLogByCategory(string category, string dbKey)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(dbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogByCategory(category).ToList());

            return auditLogs;
        }

        private List<AuditLog> GetAuditLogByFeatureName(string featureName, string dbKey)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(dbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogByFeatureName(featureName).ToList());

            return auditLogs;
        }

    }
}
