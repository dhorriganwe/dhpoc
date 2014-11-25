using System.Collections.Generic;
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
            query.ViewQueryCommon.ViewName = "Home";
            query.ReleaseVersion = Configurations.ReleaseVersion;

            return View(query);
        }

        public ActionResult Summary()
        {
            var query = new ViewQueryAuditLogSummary();

            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var cnt = auditLogDataService.GetAuditLogSummary();
            query.TotalRecordCount = cnt.TotalRowCount;

            query.ApplicationNames = auditLogDataService.GetApplicationNames();
            query.FeatureNames = auditLogDataService.GetFeatureNames();
            query.Categories = auditLogDataService.GetCategories();
 
            return View(query);
        }

        public ActionResult AuditLog()
        {
            var query = new ViewQueryAuditLog();
            query.ViewQueryCommon.ViewName = "AuditLog";

            var auditLogs = GetAuditLogAll();

            query.List.AuditLogs = auditLogs;

            return View(query);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLog(ViewQueryAuditLog query, string command)
        {
            query.ViewQueryCommon.ViewName = "AuditLog";

            if (command == "Refresh")
            {
                query.List.AuditLogs = GetAuditLogAll();
            }
            else
            {
                query.List.AuditLogs = new List<AuditLog>();
            }

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
            query.ViewQueryCommon.ViewName = "AuditLogById";


            if (command == "Search")
            {
                query.AuditLog = GetAuditLogById(query.AuditLogId);

                var jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string json = jsSerializer.Serialize(query.AuditLog);

                query.Json = json;
            }
            else
            {
                query.AuditLog = new AuditLog();
            }

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
            query.ViewQueryCommon.ViewName = "AuditLogsByEventId";

            if (command == "Search")
            {
                query.List.AuditLogs = GetAuditLogByEventId(query.EventId);
            }
            else
            {
                query.List.AuditLogs = new List<AuditLog>();
            }

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
            query.ViewQueryCommon.ViewName = "AuditLogByApplicationName";

            if (command == "Search")
            {
                query.List.AuditLogs = GetAuditLogByApplicationName(query.ApplicationName);
            }
            else
            {
                query.List.AuditLogs = new List<AuditLog>();
            }

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
            query.ViewQueryCommon.ViewName = "AuditLogByCategory";

            if (command == "Search")
            {
                query.List.AuditLogs = GetAuditLogByCategory(query.Category);
            }
            else
            {
                query.List.AuditLogs = new List<AuditLog>();
            }

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

        private List<AuditLog> GetAuditLogAll()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogAll().ToList());

            return auditLogs;
        }

        private AuditLog GetAuditLogById(string id)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            AuditLog auditLog = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogById(id));

            return auditLog;
        }

        private List<AuditLog> GetAuditLogByEventId(string eventId)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogByEventId(eventId).ToList());

            return auditLogs;
        }

        private List<AuditLog> GetAuditLogByApplicationName(string applicationName)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogByApplicationName(applicationName).ToList());

            return auditLogs;
        }

        private List<AuditLog> GetAuditLogByCategory(string category)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogByCategory(category).ToList());

            return auditLogs;
        }

    }
}
