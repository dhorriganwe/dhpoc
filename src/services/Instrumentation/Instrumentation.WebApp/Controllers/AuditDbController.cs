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

        public ActionResult Summary(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogSummary();
            query.DbKey = dbkey;

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

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLog(string id, string dbkey = null)
        {
            var query = new ViewQueryBase();
            query.DbKey = dbkey;

            return AuditLog(query, "Refresh");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLog(ViewQueryBase query, string command)
        {
            query.ViewName = "AuditLog";

            if (command == "Refresh")
            {
                query.AuditLogs = GetAuditLogAll(query);
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
            }

            InitDbOptionSelectList(query);

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogById(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogById();
            query.AuditLogId = id;
            query.DbKey = dbkey;

            return AuditLogById(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogById(ViewQueryAuditLogById query, string command)
        {
            query.ViewName = "AuditLogById";


            if (command == "Search")
            {
                query.AuditLog = GetAuditLogById(query);

                var jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string json = jsSerializer.Serialize(query.AuditLog);

                query.Json = json;
            }
            else
            {
                query.AuditLog = new AuditLog();
            }

            InitDbOptionSelectList(query);

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByEventId(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogByEventId();
            query.EventId = id;
            query.DbKey = dbkey;

            return AuditLogByEventId(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByEventId(ViewQueryAuditLogByEventId query, string command)
        {
            query.ViewName = "AuditLogsByEventId";

            if (command == "Search")
            {
                query.AuditLogs = GetAuditLogByEventId(query);
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
            }

            InitDbOptionSelectList(query);

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByApplicationName(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogByApplicationName();
            query.ApplicationName = id;
            query.DbKey = dbkey;

            return AuditLogByApplicationName(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByApplicationName(ViewQueryAuditLogByApplicationName query, string command)
        {
            query.ViewName = "AuditLogByApplicationName";

            if (command == "Search")
            {
                query.AuditLogs = GetAuditLogByApplicationName(query);
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
            }

            InitDbOptionSelectList(query);

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByCategory(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogByCategory();
            query.Category = id;
            query.DbKey = dbkey;

            return AuditLogByCategory(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByCategory(ViewQueryAuditLogByCategory query, string command)
        {
            query.ViewName = "AuditLogByCategory";

            if (command == "Search")
            {
                query.AuditLogs = GetAuditLogByCategory(query);
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
            }

            InitDbOptionSelectList(query);

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByFeatureName(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogByFeatureName();
            query.FeatureName = id;
            query.DbKey = dbkey;

            return AuditLogByFeatureName(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByFeatureName(ViewQueryAuditLogByFeatureName query, string command)
        {
            query.ViewName = "AuditLogByFeatureName";

            if (command == "Search")
            {
                query.AuditLogs = GetAuditLogByFeatureName(query);
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
            }

            InitDbOptionSelectList(query);

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogRecord(AuditLog auditLog)
        {
            return View(auditLog);
        }

        public ActionResult AuditLogGrid(ViewQueryBase query)
        {
            return View(query);
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

        private List<AuditLog> GetAuditLogAll(ViewQueryBase query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogAll(query.MaxRowCount).ToList());

            return auditLogs;
        }

        private AuditLog GetAuditLogById(ViewQueryAuditLogById query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            AuditLog auditLog = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogById(query.AuditLogId));

            return auditLog;
        }

        private List<AuditLog> GetAuditLogByEventId(ViewQueryAuditLogByEventId query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(
                                            auditLogDataService.GetAuditLogByEventId(
                                                query.EventId, 
                                                query.MaxRowCount)
                                            .ToList());

            return auditLogs;
        }

        private List<AuditLog> GetAuditLogByApplicationName(ViewQueryAuditLogByApplicationName query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(
                                            auditLogDataService.GetAuditLogByApplicationName(
                                                query.ApplicationName, 
                                                query.MaxRowCount)
                                            .ToList());

            return auditLogs;
        }

        private List<AuditLog> GetAuditLogByCategory(ViewQueryAuditLogByCategory query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(
                                            auditLogDataService.GetAuditLogByCategory(
                                                query.Category, 
                                                query.MaxRowCount)
                                            .ToList());

            return auditLogs;
        }

        private List<AuditLog> GetAuditLogByFeatureName(ViewQueryAuditLogByFeatureName query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(
                                            auditLogDataService.GetAuditLogByFeatureName(
                                                query.FeatureName,
                                                query.MaxRowCount)
                                            .ToList());

            return auditLogs;
        }

    }
}
