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
            try
            {
                IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

                if (command == "Refresh")
                {
                    var summary = auditLogDataService.GetAuditLogRowCount();
                    query.TotalRecordCount = summary.TotalRowCount;

                    query.ApplicationNames = auditLogDataService.GetApplicationNameCounts();
                    query.FeatureNames = auditLogDataService.GetFeatureNameCounts();
                    query.Categories = auditLogDataService.GetCategoryCounts();
                }
            }
            catch (Exception ex)
            {
                query.Header.ErrorMessage = ex.ToString();
            }

            InitQueryBase(query);

            ModelState.Clear();

            return View(query);
        }

        private void InitQueryBase(ViewQueryBase query)
        {
            query.DbKeyList = InitDbKeySelectList();
            query.MaxRowCount = 100;
            
        }
        public ActionResult AuditLog(string id, string dbkey = null)
        {
            var query = new ViewQueryBase();

            query.DbKey = dbkey;

            InitQueryBase(query);

            return AuditLog(query, "Refresh");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLog(ViewQueryBase query, string command)
        {
            try
            {
                query.ViewName = "AuditLog";

                query.AuditLogs = command == "Refresh" ? GetAuditLogAll(query) : new List<AuditLog>();

            }
            catch (Exception ex)
            {
                query.Header.ErrorMessage = ex.ToString();
            }

            InitQueryBase(query);

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogById(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogById();
            query.AuditLogId = id;
            query.DbKey = dbkey;

            InitQueryBase(query);

            return AuditLogById(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogById(ViewQueryAuditLogById query, string command)
        {
            query.ViewName = "AuditLogById";

            if (command == "Search" || command == "Refresh")
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

            query.DbKeyList = InitDbKeySelectList();

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByEventId(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogByEventId();
            query.EventId = id;
            query.DbKey = dbkey;

            InitQueryBase(query);

            return AuditLogByEventId(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByEventId(ViewQueryAuditLogByEventId query, string command)
        {
            query.ViewName = "AuditLogsByEventId";

            query.AuditLogs = command == "Search" ? GetAuditLogByEventId(query) : new List<AuditLog>();

            query.DbKeyList = InitDbKeySelectList();

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByFilters(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogByFilters();

            query.DbKey = dbkey;

            InitializeQuery(query);

            return AuditLogByFilters(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByFilters(ViewQueryAuditLogByFilters query, string command)
        {
            query.ViewName = "AuditLogByFilters";

            try
            {
                if (command == "Reset")
                {
                    InitializeQuery(query);
                    query = GetAuditLogByFilters(query);
                }
                else if (command == "Search")
                {
                    query = GetAuditLogByFilters(query);
                }
                else
                {
                    query.AuditLogs = new List<AuditLog>();
                }
            }
            catch (Exception ex)
            {
                query.Header.ErrorMessage = ex.ToString();
            }

            InitializeSelectLists(query);

            ModelState.Clear();

            return View(query);
        }

        private ViewQueryAuditLogByFilters GetAuditLogByFilters(ViewQueryAuditLogByFilters query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetAuditLogByFilters(
                query.MaxRowCount,
                query.StartDate,
                query.EndDate,
                query.TraceLevel);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            return query;
        }

        public ActionResult AuditLogByApplicationName(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogByApplicationName();
            query.ApplicationName = id;
            query.DbKey = dbkey;

            InitQueryBase(query);

            return AuditLogByApplicationName(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByApplicationName(ViewQueryAuditLogByApplicationName query, string command)
        {
            query.ViewName = "AuditLogByApplicationName";

            if (command == "Search" || command == "Refresh")
            {
                query = GetAuditLogByApplicationName(query);
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
            }


            //InitDbOptionSelectList(query)
            //InitDbOptionSelectList(query);
            query.DbKeyList = InitDbKeySelectList();

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByFeatureName(string id, string dbkey = null, string name = null)
        {
            var query = new ViewQueryAuditLogByFeatureName();
            query.FeatureName = name;
            query.DbKey = dbkey;

            InitQueryBase(query);

            return AuditLogByFeatureName(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByFeatureName(ViewQueryAuditLogByFeatureName query, string command)
        {
            query.ViewName = "AuditLogByFeatureName";

            if (command == "Search" || command == "Refresh")
            {
                query.FeatureName = HttpUtility.UrlDecode(query.FeatureName);

                query = GetAuditLogByFeatureName(query);
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
            }

            InitDbKeySelectList();

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByCategory(string id, string dbkey = null)
        {
            var query = new ViewQueryAuditLogByCategory();
            query.Category = id;
            query.DbKey = dbkey;

            InitQueryBase(query);

            return AuditLogByCategory(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByCategory(ViewQueryAuditLogByCategory query, string command)
        {
            query.ViewName = "AuditLogByCategory";

            if (command == "Search" || command == "Refresh")
            {
                query = GetAuditLogByCategory(query);
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
            }

            query.DbKeyList = InitDbKeySelectList();

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
        
        private void InitializeQuery(ViewQueryAuditLogByFilters query)
        {
            query = query ?? new ViewQueryAuditLogByFilters();

            query.MaxRowCount = 100;
            query.StartDate = DateTime.Now.AddDays(-7).ToString();
            query.EndDate = DateTime.Now.ToString();
        }

        private void InitializeSelectLists(ViewQueryAuditLogByFilters query)
        {
            if (query == null)
                query = new ViewQueryAuditLogByFilters();

            query.ApplicationNameList = InitApplicationNameSelectList(query);
            query.FeatureNameList = InitFeatureNameSelectList(query);
            query.CategoryList = InitCategorySelectList(query);
            query.TraceLevelList = InitTraceLevelSelectList(query);
            query.DbKeyList = InitDbKeySelectList();
        }

        private SelectList InitApplicationNameSelectList(ViewQueryBase query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);
            List<string> appNames = auditLogDataService.GetApplicationNames();

            var applications = new List<LookupItem>();

            appNames.ForEach(an => applications.Add(new LookupItem { Value = an, Description = an }));

            var applicationSelectList = new SelectList(applications, "Value", "Description");

            return applicationSelectList;
        }

        private SelectList InitFeatureNameSelectList(ViewQueryBase query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);
            List<string> featureNames = auditLogDataService.GetFeatureNames();

            var features = new List<LookupItem>();

            featureNames.ForEach(fn => features.Add(new LookupItem { Value = fn, Description = fn }));
            
            var featureSelectList = new SelectList(features, "Value", "Description");

            return featureSelectList;
        }

        private SelectList InitCategorySelectList(ViewQueryBase query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);
            List<string> categoryNames = auditLogDataService.GetCategories();

            var categories = new List<LookupItem>();

            categoryNames.ForEach(cn => categories.Add(new LookupItem { Value = cn, Description = cn }));

            var categorySelectList = new SelectList(categories, "Value", "Description");

            return categorySelectList;
        }

        private SelectList InitTraceLevelSelectList(ViewQueryBase query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);
            List<string> levelNames = auditLogDataService.GetTraceLevels();

            var traceLevels = new List<LookupItem>();

            levelNames.ForEach(ln => traceLevels.Add(new LookupItem { Value = ln, Description = ln }));
            
            var traceLevelSelectList = new SelectList(traceLevels, "Value", "Description");

            return traceLevelSelectList;
        }

        private SelectList InitDbKeySelectList()
        {
            var dbKeys = GetDbOptionsFromConfig();
            return new SelectList(dbKeys, "Value", "Description");
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

        private ViewQueryAuditLogByApplicationName GetAuditLogByApplicationName(ViewQueryAuditLogByApplicationName query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetAuditLogByApplicationName(
                query.ApplicationName,
                query.MaxRowCount);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            query.FeatureNames = auditLogDataService.GetSummaryItemsByApplicationName("FeatureName", query.ApplicationName);
            query.Categories = auditLogDataService.GetSummaryItemsByApplicationName("Category", query.ApplicationName);
            
            return query;
        }

        private ViewQueryAuditLogByCategory GetAuditLogByCategory(ViewQueryAuditLogByCategory query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetAuditLogByCategory(
                query.Category,
                query.MaxRowCount);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            query.FeatureNames = auditLogDataService.GetSummaryItemsByCategory("FeatureName", query.Category);
            query.ApplicationNames = auditLogDataService.GetSummaryItemsByCategory("ApplicationName", query.Category);

            return query;
        }

        private ViewQueryAuditLogByFeatureName GetAuditLogByFeatureName(ViewQueryAuditLogByFeatureName query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetAuditLogByFeatureName(
                query.FeatureName,
                query.MaxRowCount);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            query.ApplicationNames = auditLogDataService.GetSummaryItemsByFeatureName("ApplicationName", query.FeatureName);
            query.Categories = auditLogDataService.GetSummaryItemsByFeatureName("Category", query.FeatureName);

            return query;
        }

    }
}
