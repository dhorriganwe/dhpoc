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

            query.AuditLogs = auditLogs;

            return View(query);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLog(ViewQueryAuditLog query, string command)
        {
            query.ViewQueryCommon.ViewName = "AuditLog";

            if (command == "Refresh")
            {
                query.AuditLogs = GetAuditLogAll();
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
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
                query.AuditLogs = GetAuditLogByEventId(query.EventId);
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
            }

            return View(query);
        }

        public ActionResult AuditLogByApplicationName()
        {
            var query = new ViewQueryAuditLogByApplicationName();
            query.ViewQueryCommon.ViewName = "AuditLogByApplicationName";

            return View(query);
        }

        public ActionResult AuditLogByCategory()
        {
            var query = new ViewQueryAuditLogByApplicationName();
            query.ViewQueryCommon.ViewName = "AuditLogByCategory";

            return View(query);
        }

        public ActionResult AuditLogRow(AuditLog auditLog)
        {
            return View(auditLog);
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

    }
}
