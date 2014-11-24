using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instrumentation.DomainDA.DataServices;
//using Instrumentation.DomainDA.Models;
using Instrumentation.WebApp.Helpers;
using Instrumentation.WebApp.Models;

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

        public ActionResult AuditLog()
        {
            var query = new ViewQueryAuditLog();
            query.ViewQueryCommon.ViewName = "AuditLog";

            var auditLogs = GetAuditLogsAll();

            query.AuditLogs = auditLogs;

            return View(query);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLog(ViewQueryAuditLog query, string command)
        {
            query.ViewQueryCommon.ViewName = "AuditLog";

            if (command == "Refresh")
            {
                query.AuditLogs = GetAuditLogsAll();
            }
            else
            {
                query.AuditLogs = new List<AuditLog>();
            }

            return View(query);
        }

        public ActionResult AuditLogsByEventId(string id)
        {
            var query = new ViewQueryAuditLogsByEventId();
            query.EventId = id;

            return AuditLogsByEventId(query, "Search");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogsByEventId(ViewQueryAuditLogsByEventId query, string command)
        {
            query.ViewQueryCommon.ViewName = "AuditLogsByEventId";

            if (command == "Search")
            {
                query.AuditLogs = GetAuditLogsByEventId(query.EventId);
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

        public ActionResult Summary()
        {
            var query = new ViewQueryAuditLogSummary();
            //query.AuditLogId = id;

            query.TotalRecordCount = 8;

            return View(query);
        }

        public ActionResult AuditLogRow(AuditLog auditLog)
        {
            return View(auditLog);
        }

        private List<AuditLog> GetAuditLogsAll()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogsAll().ToList());

            return auditLogs;
        }

        private AuditLog GetAuditLogById(string id)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            AuditLog auditLog = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogById(id));

            return auditLog;
        }

        private List<AuditLog> GetAuditLogsByEventId(string eventId)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            List<AuditLog> auditLogs = _instrumentationMapper.MapDaToUiAuditLog(auditLogDataService.GetAuditLogsByEventId(eventId).ToList());

            return auditLogs;
        }
    }
}
