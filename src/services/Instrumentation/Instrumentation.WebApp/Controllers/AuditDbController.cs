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
            query.ReleaseVersion = Configurations.ReleaseVersion;

            return View(query);
        }

        public ActionResult Summary(string id, string dbkey = null, int? maxrowcount = null)
        {
            var query = new ViewQueryBase();
            query.DbKey = dbkey;
            query.MaxRowCount = maxrowcount.GetValueOrDefault();

            return Summary(query, "Refresh");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Summary(ViewQueryBase query, string command)
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
                query.ErrorMessage = ex.ToString();
            }

            query.DbKeyList = InitDbKeySelectList();

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLog(string id, string dbkey = null, int? maxrowcount = null)
        {
            var query = new ViewQueryBase();

            query.DbKey = dbkey;
            query.MaxRowCount = maxrowcount.GetValueOrDefault();

            query.DbKeyList = InitDbKeySelectList();

            return AuditLog(query, "Refresh");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLog(ViewQueryBase query, string command)
        {
            try
            {
                query.AuditLogs = command == "Refresh" ? GetAuditLogAll(query) : new List<AuditLog>();
            }
            catch (Exception ex)
            {
                query.ErrorMessage = ex.ToString();
            }

            query.DbKeyList = InitDbKeySelectList();

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogById(string id, string dbkey = null, int? maxrowcount = null)
        {
            var query = new ViewQueryAuditLogById();
            query.AuditLogId = id;
            query.DbKey = dbkey;
            query.MaxRowCount = maxrowcount.GetValueOrDefault();

            query.DbKeyList = InitDbKeySelectList();

            return AuditLogById(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogById(ViewQueryAuditLogById query, string command)
        {
            try
            {
                if (command == "Search" || command == "Refresh")
                {
                    query.AuditLog = GetAuditLogById(query);
                }
                else
                {
                    query.AuditLog = new AuditLog();
                }

                query.DbKeyList = InitDbKeySelectList();
            }
            catch (Exception ex)
            {
                query.ErrorMessage = ex.ToString();
            }

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByEventId(string id, string dbkey = null, int? maxrowcount = null)
        {
            var query = new ViewQueryBase();
            query.EventId = id;
            query.DbKey = dbkey;
            query.MaxRowCount = maxrowcount.GetValueOrDefault();

            query.DbKeyList = InitDbKeySelectList();

            return AuditLogByEventId(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByEventId(ViewQueryBase query, string command)
        {
            try
            {
                query.AuditLogs = GetAuditLogByEventId(query);

                query.DbKeyList = InitDbKeySelectList();
            }
            catch (Exception ex)
            {
                query.ErrorMessage = ex.ToString();
            }

            ModelState.Clear();

            return View(query);
        }

        public ActionResult     AuditLogByFilters(string id, string dbkey = null, int? maxrowcount = null)
        {
            var query = new ViewQueryAuditLogByFilters();

            query.DbKey = dbkey;
            query.MaxRowCount = maxrowcount.GetValueOrDefault();

            InitQuery(query);
            InitSelectLists(query);

            return AuditLogByFilters(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByFilters(ViewQueryAuditLogByFilters query, string command)
        {
            try
            {
                switch (command)
                {
                    case "Search":
                        query = GetAuditLogByFilters(query);
                        break;
                    case "iLikeCorrelationId":
                        query = GetByILikeEventId(query);
                        break;
                    case "iLikeMessage":
                        query = GetByILikeMessage(query);
                        break;
                    case "iLikeAdditionalInfo":
                        query = GetByILikeAdditionalInfo(query);
                        break;
                    case "iLikeLoginName":
                        query = GetByILikeLoginName(query);
                        break;
                    default:
                        break;
                }

                InitSelectLists(query);
            }
            catch (Exception ex)
            {
                query.ErrorMessage = ex.ToString();
            }

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByApplicationName(string id, string dbkey = null, int? maxrowcount = null)
        {
            var query = new ViewQueryBase();
            query.ApplicationName = id;
            query.DbKey = dbkey;
            query.MaxRowCount = maxrowcount.GetValueOrDefault();

            query.DbKeyList = InitDbKeySelectList();

            return AuditLogByApplicationName(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByApplicationName(ViewQueryBase query, string command)
        {
            try
            {
                if (command == "Search" || command == "Refresh")
                {
                    query = GetAuditLogByApplicationName(query);
                }
                else
                {
                    query.AuditLogs = new List<AuditLog>();
                }

                query.DbKeyList = InitDbKeySelectList();
            }
            catch (Exception ex)
            {
                query.ErrorMessage = ex.ToString();
            }

            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByFeatureName(string id, string name = null, string dbkey = null, int? maxrowcount = null)
        {
            var query = new ViewQueryBase();
            query.FeatureName = name;
            query.DbKey = dbkey;
            query.MaxRowCount = maxrowcount.GetValueOrDefault();

            query.DbKeyList = InitDbKeySelectList();

            return AuditLogByFeatureName(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByFeatureName(ViewQueryBase query, string command)
        {
            try
            {
                if (command == "Search" || command == "Refresh")
                {
                    query.FeatureName = HttpUtility.UrlDecode(query.FeatureName);

                    query = GetAuditLogByFeatureName(query);
                }
                else
                {
                    query.AuditLogs = new List<AuditLog>();
                }

                query.DbKeyList = InitDbKeySelectList();
            }
            catch (Exception ex)
            {
                query.ErrorMessage = ex.ToString();
            }
            ModelState.Clear();

            return View(query);
        }

        public ActionResult AuditLogByCategory(string id, string dbkey = null, int? maxrowcount = null)
        {
            var query = new ViewQueryBase();
            query.Category = id;
            query.DbKey = dbkey;
            query.MaxRowCount = maxrowcount.GetValueOrDefault();

            query.DbKeyList = InitDbKeySelectList();

            return AuditLogByCategory(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogByCategory(ViewQueryBase query, string command)
        {
            try
            {
                if (command == "Search" || command == "Refresh")
                {
                    query = GetAuditLogByCategory(query);
                }
                else
                {
                    query.AuditLogs = new List<AuditLog>();
                }

                query.DbKeyList = InitDbKeySelectList();
            }
            catch (Exception ex)
            {
                query.ErrorMessage = ex.ToString();
            }

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
        
        private void InitQuery(ViewQueryAuditLogByFilters query)
        {
            query = query ?? new ViewQueryAuditLogByFilters();

            query.StartDate = DateTime.Now.AddDays(-21).ToString();
            query.EndDate = DateTime.Now.ToString();
        }

        private void InitSelectLists(ViewQueryAuditLogByFilters query)
        {
            if (query == null)
                query = new ViewQueryAuditLogByFilters();

            query.ApplicationNameList = InitApplicationNameSelectList(query.DbKey);
            query.TraceLevelList = InitTraceLevelSelectList(query.DbKey);
            query.DbKeyList = InitDbKeySelectList();
        }

        private SelectList InitApplicationNameSelectList(string dbKey)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(dbKey);
            List<string> appNames = auditLogDataService.GetApplicationNames();

            var applications = new List<LookupItem>();
            applications.Add(new LookupItem { Value = "*", Description = "*" });

            appNames.ForEach(an => applications.Add(new LookupItem { Value = an, Description = an }));

            var applicationSelectList = new SelectList(applications, "Value", "Description");

            return applicationSelectList;
        }

        private SelectList InitFeatureNameSelectList(string dbKey)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(dbKey);
            List<string> featureNames = auditLogDataService.GetFeatureNames();

            var features = new List<LookupItem>();
            features.Add(new LookupItem { Value = "*", Description = "*" });

            featureNames.ForEach(fn => features.Add(new LookupItem { Value = fn, Description = fn }));
            
            var featureSelectList = new SelectList(features, "Value", "Description");

            return featureSelectList;
        }

        private SelectList InitCategorySelectList(string dbKey)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(dbKey);
            List<string> categoryNames = auditLogDataService.GetCategories();

            var categories = new List<LookupItem>();
            categories.Add(new LookupItem { Value = "*", Description = "*" });

            categoryNames.ForEach(cn => categories.Add(new LookupItem { Value = cn, Description = cn }));

            var categorySelectList = new SelectList(categories, "Value", "Description");

            return categorySelectList;
        }

        private SelectList InitTraceLevelSelectList(string dbKey)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(dbKey);
            List<string> levelNames = auditLogDataService.GetTraceLevels();

            var traceLevels = new List<LookupItem>();
            traceLevels.Add(new LookupItem { Value = "*", Description = "*" });

            levelNames.ForEach(ln => traceLevels.Add(new LookupItem { Value = ln, Description = ln }));
            
            var traceLevelSelectList = new SelectList(traceLevels, "Value", "Description");

            return traceLevelSelectList;
        }

        private SelectList InitDbKeySelectList()
        {
            var dbKeys = Configurations.GetDbKeysFromConfig();
            return new SelectList(dbKeys, "Value", "Description");
        }

        private ViewQueryAuditLogByFilters GetAuditLogByFilters(ViewQueryAuditLogByFilters query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);
            var traceLevel = "";
            if (string.IsNullOrEmpty(query.TraceLevel))
                traceLevel = "";
            else if (query.TraceLevel != "*")
                traceLevel = query.TraceLevel;

            var applicationName = "";
            if (query.ApplicationName != "*")
                applicationName = query.ApplicationName;

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetAuditLogByFilters(
                query.MaxRowCount,
                query.StartDate,
                query.EndDate,
                traceLevel,
                applicationName);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            return query;
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

        private List<AuditLog> GetAuditLogByEventId(ViewQueryBase query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(
                                            auditLogDataService.GetAuditLogByEventId(
                                                query.EventId,
                                                query.MaxRowCount)
                                            .ToList());

            return auditLogs;
        }

        private ViewQueryAuditLogByFilters GetByILikeEventId(ViewQueryAuditLogByFilters query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);
            if (string.IsNullOrEmpty(query.CorrelationIdSearchStr))
                return query;

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetByILikeEventId(
                query.MaxRowCount,
                query.StartDate,
                query.EndDate,
                query.CorrelationIdSearchStr);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            return query;
        }

        private ViewQueryAuditLogByFilters GetByILikeMessage(ViewQueryAuditLogByFilters query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);
            if (string.IsNullOrEmpty(query.MessageSearchStr))
                return query;

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetByILikeMessage(
                query.MaxRowCount,
                query.StartDate,
                query.EndDate,
                query.MessageSearchStr);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            return query;
        }

        private ViewQueryAuditLogByFilters GetByILikeAdditionalInfo(ViewQueryAuditLogByFilters query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);
            if (string.IsNullOrEmpty(query.AdditionalInfoSearchStr))
                return query;

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetByILikeAdditionalInfo(
                query.MaxRowCount,
                query.StartDate,
                query.EndDate,
                query.AdditionalInfoSearchStr);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            return query;
        }

        private ViewQueryAuditLogByFilters GetByILikeLoginName(ViewQueryAuditLogByFilters query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);
            if (string.IsNullOrEmpty(query.LoginNameSearchStr))
                return query;

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetByILikeLoginName(
                query.MaxRowCount,
                query.StartDate,
                query.EndDate,
                query.LoginNameSearchStr);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            return query;
        }

        private ViewQueryBase GetAuditLogByApplicationName(ViewQueryBase query)
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

        private ViewQueryBase GetAuditLogByCategory(ViewQueryBase query)
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

        private ViewQueryBase GetAuditLogByFeatureName(ViewQueryBase query)
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
