using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instrumentation.DomainDA.DataServices;
using Instrumentation.WebApp.Helpers;
using Instrumentation.WebApp.Models;
using AuditLog = Instrumentation.WebApp.Models.AuditLog;
using System;

namespace Instrumentation.WebApp.Controllers
{
    public class AuditDbController : Controller
    {
        private InstrumentationMapper _instrumentationMapper = new InstrumentationMapper();

        public ActionResult Index()
        {
            var query = new ViewQueryBase();
            query.Header.ViewName = "Home";
            query.ReleaseVersion = Configurations.ReleaseVersion;

            return View(query);
        }

        public ActionResult Summary(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogSummary();
            query.Header.DbKey = dbkey;

            return Summary(query, "Refresh");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Summary(ViewQueryAuditLogSummary query, string command)
        {
            try
            {
                IAuditLogDataService auditLogDataService = new AuditLogDataService(query.Header.DbKey);

                if (command == "Refresh")
                {
                    var summary = auditLogDataService.GetAuditLogSummary();
                    query.TotalRecordCount = summary.TotalRowCount;

                    query.ApplicationNames = auditLogDataService.GetApplicationNames();
                    query.FeatureNames = auditLogDataService.GetFeatureNames();
                    query.Categories = auditLogDataService.GetCategories();
                }
            }
            catch (Exception ex)
            {
                query.Header.ErrorMessage = ex.ToString();
            }

            InitDbOptionSelectList(query);

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLog(string id, string dbkey = null)
        {
            var query = new ViewQueryBase();
            query.Header.DbKey = dbkey;

            return AuditLog(query, "Refresh");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLog(ViewQueryBase query, string command)
        {
            query.Header.ViewName = "AuditLog";

            query.AuditLogs = command == "Refresh" ? GetAuditLogAll(query) : new List<AuditLog>();

            InitDbOptionSelectList(query);

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogById(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogById();
            query.AuditLogId = id;
            query.Header.DbKey = dbkey;

            return AuditLogById(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogById(ViewQueryAuditLogById query, string command)
        {
            query.Header.ViewName = "AuditLogById";


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
            query.Header.DbKey = dbkey;

            return AuditLogByEventId(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByEventId(ViewQueryAuditLogByEventId query, string command)
        {
            query.Header.ViewName = "AuditLogsByEventId";

            query.AuditLogs = command == "Search" ? GetAuditLogByEventId(query) : new List<AuditLog>();

            InitDbOptionSelectList(query);

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByApplicationName(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogByApplicationName();
            query.ApplicationName = id;
            query.Header.DbKey = dbkey;

            return AuditLogByApplicationName(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByApplicationName(ViewQueryAuditLogByApplicationName query, string command)
        {
            query.Header.ViewName = "AuditLogByApplicationName";

            if (command == "Search")
            {
                query = GetAuditLogByApplicationName(query);
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
            query.Header.DbKey = dbkey;

            return AuditLogByCategory(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByCategory(ViewQueryAuditLogByCategory query, string command)
        {
            query.Header.ViewName = "AuditLogByCategory";

            if (command == "Search")
            {
                query = GetAuditLogByCategory(query);
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
            }

            InitDbOptionSelectList(query);

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByFeatureName(string id, string dbkey = null, string name = null)
        {
            var query = new ViewQueryAuditLogByFeatureName();
            query.FeatureName = name;
            query.Header.DbKey = dbkey;

            return AuditLogByFeatureName(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByFeatureName(ViewQueryAuditLogByFeatureName query, string command)
        {
            query.Header.ViewName = "AuditLogByFeatureName";

            if (command == "Search")
            {
                query.FeatureName = HttpUtility.UrlDecode(query.FeatureName);

                query = GetAuditLogByFeatureName(query);
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
            }

            InitDbOptionSelectList(query);

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogDetail(ViewQueryBase query)
        {
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

        public ActionResult DbSelect(ViewQueryHeader query)
        {
            return View(query);
        }

        public ActionResult SummaryFeatureNames(ViewQueryBase query)
        {
            return View(query);
        }

        public ActionResult SummaryApplicationNames(ViewQueryBase query)
        {
            return View(query);
        }

        public ActionResult SummaryCategoryNames(ViewQueryBase query)
        {
            return View(query);
        }
        
        private void InitDbOptionSelectList(ViewQueryBase query)
        {
            query.Header.DbOptionSelectList = new SelectList(GetDbOptionsFromConfig(), "Value", "Description");
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
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.Header.DbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogAll(query.Header.MaxRowCount).ToList());

            return auditLogs;
        }

        private AuditLog GetAuditLogById(ViewQueryAuditLogById query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.Header.DbKey);

            AuditLog auditLog = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogById(query.AuditLogId));

            return auditLog;
        }

        private List<AuditLog> GetAuditLogByEventId(ViewQueryAuditLogByEventId query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.Header.DbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(
                                            auditLogDataService.GetAuditLogByEventId(
                                                query.EventId,
                                                query.Header.MaxRowCount)
                                            .ToList());

            return auditLogs;
        }

        private ViewQueryAuditLogByApplicationName GetAuditLogByApplicationName(ViewQueryAuditLogByApplicationName query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.Header.DbKey);

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetAuditLogByApplicationName(
                query.ApplicationName,
                query.Header.MaxRowCount);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            query.FeatureNames = auditLogDataService.GetSummaryItemsByApplicationName("FeatureName", query.ApplicationName);
            query.Categories = auditLogDataService.GetSummaryItemsByApplicationName("Category", query.ApplicationName);
            
            return query;
        }

        private ViewQueryAuditLogByCategory GetAuditLogByCategory(ViewQueryAuditLogByCategory query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.Header.DbKey);

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetAuditLogByCategory(
                query.Category,
                query.Header.MaxRowCount);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            query.FeatureNames = auditLogDataService.GetSummaryItemsByCategory("FeatureName", query.Category);
            query.ApplicationNames = auditLogDataService.GetSummaryItemsByCategory("ApplicationName", query.Category);

            return query;
        }

        private ViewQueryAuditLogByFeatureName GetAuditLogByFeatureName(ViewQueryAuditLogByFeatureName query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.Header.DbKey);

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetAuditLogByFeatureName(
                query.FeatureName,
                query.Header.MaxRowCount);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            query.ApplicationNames = auditLogDataService.GetSummaryItemsByFeatureName("ApplicationName", query.FeatureName);
            query.Categories = auditLogDataService.GetSummaryItemsByFeatureName("Category", query.FeatureName);

            return query;
        }

    }
}
