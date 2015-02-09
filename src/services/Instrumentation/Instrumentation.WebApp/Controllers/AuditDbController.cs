using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instrumentation.DomainDA.DataServices;
using Instrumentation.WebApp.Helpers;
using Instrumentation.WebApp.Models;
using AuditLog = Instrumentation.WebApp.Models.AuditLogItem;
using System;

namespace Instrumentation.WebApp.Controllers
{
    public class AuditDbController : Controller
    {
        private InstrumentationMapper _instrumentationMapper = new InstrumentationMapper();

        public ActionResult Index()
        {
            var query = new AuditLogViewModel();
            query.ReleaseVersion = Configurations.ReleaseVersion;

            return View(query);
        }

        public ActionResult Summary(string id, string dbkey = null, int? maxrowcount = null)
        {
            var query = new AuditLogViewModel();
            query.DbKey = dbkey ?? Configurations.DbKeyDefault;
            query.MaxRowCount = maxrowcount ?? Configurations.MaxRowCountDefault;

            return Summary(query, "Refresh");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Summary(AuditLogViewModel query, string command)
        {
            try
            {
                IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

                if (command == "Refresh")
                {
                    query.TotalRecordCount = auditLogDataService.GetTotalRowCount();
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

        public ActionResult Recent(string id, string dbkey = null, int? maxrowcount = null)
        {
            var query = new AuditLogViewModel();

            query.DbKey = dbkey ?? Configurations.DbKeyDefault;
            query.MaxRowCount = maxrowcount ?? Configurations.MaxRowCountDefault;

            query.DbKeyList = InitDbKeySelectList();

            return Recent(query, "Refresh");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Recent(AuditLogViewModel query, string command)
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

        public ActionResult Search(string id, string dbkey = null, int? maxrowcount = null)
        {
            var query = new AuditLogViewModel();

            query.DbKey = dbkey ?? Configurations.DbKeyDefault;
            query.MaxRowCount = maxrowcount ?? Configurations.MaxRowCountDefault;

            try
            {
                InitQuery(query);
                InitSelectLists(query);
            }
            catch (Exception ex)
            {
                query.ErrorMessage = ex.ToString();
            }

            return Search(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Search(AuditLogViewModel query, string command)
        {
            try
            {
                switch (command)
                {
                    case "Search":
                        query = GetByAppNameAndTraceLevel(query);
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
                        throw new Exception("Unexpected command: " + command);
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

        public ActionResult AuditLogById(string id, string dbkey = null, int? maxrowcount = null)
        {
            var query = new AuditLogViewModel();
            query.AuditLogId = id;
            query.DbKey = dbkey ?? Configurations.DbKeyDefault;
            query.MaxRowCount = maxrowcount ?? Configurations.MaxRowCountDefault;

            query.DbKeyList = InitDbKeySelectList();

            return AuditLogById(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogById(AuditLogViewModel query, string command)
        {
            try
            {
                if (command == "Search" || command == "Refresh")
                {
                    query.AuditLog = GetById(query);
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
                query.AuditLog = new AuditLogItem();
            }

            ModelState.Clear();

            return View(query);
        }

        public ActionResult Browse(string id, string name = null, string dbkey = null, int? maxrowcount = null, string browsemode = null)
        {
            var query = new AuditLogViewModel();

            // Browse by FeatureName most be UrlEncoded and Decoded so it uses name param
            query.BrowseId = id ?? HttpUtility.UrlDecode(name);

            query.DbKey = dbkey ?? Configurations.DbKeyDefault;
            query.MaxRowCount = maxrowcount ?? Configurations.MaxRowCountDefault;
            query.BrowseMode = browsemode ?? Constants.BrowseModeDefault;

            query.DbKeyList = InitDbKeySelectList();

            return Browse(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Browse(AuditLogViewModel query, string command)
        {
            try
            {
                if (command == "Search" || command == "Refresh")
                {
                    query = GetAuditLogByBrowseId(query);
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

        public ActionResult AuditLogDetail(AuditLogViewModel query)
        {
            return View(query);
        }

        public ActionResult AuditLogGrid(AuditLogViewModel query)
        {
            return View(query);
        }

        public ActionResult SummaryFeatureNames(AuditLogViewModel query)
        {
            return View(query);
        }

        public ActionResult SummaryApplicationNames(AuditLogViewModel query)
        {
            return View(query);
        }

        public ActionResult SummaryCategoryNames(AuditLogViewModel query)
        {
            return View(query);
        }

        private void InitQuery(AuditLogViewModel query)
        {
            query = query ?? new AuditLogViewModel();

            query.StartDate = DateTime.Now.AddDays(-21).ToString();
            query.EndDate = DateTime.Now.AddDays(1).ToString();
        }

        private void InitSelectLists(AuditLogViewModel query)
        {
            if (query == null)
                query = new AuditLogViewModel();

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

        private AuditLogViewModel GetByAppNameAndTraceLevel(AuditLogViewModel query)
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

            IList<DomainDA.Models.AuditLog> auditLogsDa = auditLogDataService.GetByAppNameAndTraceLevel(
                query.MaxRowCount,
                query.StartDate,
                query.EndDate,
                traceLevel,
                applicationName);

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            return query;
        }

        private List<AuditLog> GetAuditLogAll(AuditLogViewModel query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAll(query.MaxRowCount).ToList());

            return auditLogs;
        }

        private AuditLog GetById(AuditLogViewModel query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);

            AuditLog auditLog = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetById(query.AuditLogId));

            return auditLog;
        }

        private AuditLogViewModel GetByILikeEventId(AuditLogViewModel query)
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

        private AuditLogViewModel GetByILikeMessage(AuditLogViewModel query)
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

        private AuditLogViewModel GetByILikeAdditionalInfo(AuditLogViewModel query)
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

        private AuditLogViewModel GetByILikeLoginName(AuditLogViewModel query)
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

        private AuditLogViewModel GetAuditLogByBrowseId(AuditLogViewModel query)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService(query.DbKey);
            IList<DomainDA.Models.AuditLog> auditLogsDa = null;
            switch (query.BrowseMode)
            {
                case BrowseMode.ApplicationName:
                    auditLogsDa = auditLogDataService.GetByApplicationName(
                        query.BrowseId,
                        query.MaxRowCount);
                    query.FeatureNames = auditLogDataService.GetSummaryItemsByApplicationName("FeatureName", query.BrowseId);
                    query.Categories = auditLogDataService.GetSummaryItemsByApplicationName("Category", query.BrowseId);
                    break;
                case BrowseMode.FeatureName:
                    auditLogsDa = auditLogDataService.GetByFeatureName(
                        query.BrowseId,
                        query.MaxRowCount);
                    query.ApplicationNames = auditLogDataService.GetSummaryItemsByFeatureName("ApplicationName", query.BrowseId);
                    query.Categories = auditLogDataService.GetSummaryItemsByFeatureName("Category", query.BrowseId);
                    break;
                case BrowseMode.Category:
                    auditLogsDa = auditLogDataService.GetByCategory(
                        query.BrowseId,
                        query.MaxRowCount);
                    query.FeatureNames = auditLogDataService.GetSummaryItemsByCategory("FeatureName", query.BrowseId);
                    query.ApplicationNames = auditLogDataService.GetSummaryItemsByCategory("ApplicationName", query.BrowseId);
                    break;
                case BrowseMode.EventId:
                    auditLogsDa = auditLogDataService.GetByEventId(
                        query.BrowseId,
                        query.MaxRowCount);
                    break;
            }

            query.AuditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogsDa.ToList());

            return query;
        }
    }
}
