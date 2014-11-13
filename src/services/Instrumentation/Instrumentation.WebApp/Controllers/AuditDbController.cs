using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instrumentation.DomainDA.DataServices;
using Instrumentation.DomainDA.Models;
using Instrumentation.WebApp.Helpers;
using Instrumentation.WebApp.Models;

namespace Instrumentation.WebApp.Controllers
{
    public class AuditDbController : Controller
    {
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
            query.ReleaseVersion = Configurations.ReleaseVersion;

            var auditLogs = GetAuditLogsAll();

            query.Count = auditLogs.Count;
            query.AuditLogs = auditLogs;

            return View(query);
        }

        public ActionResult AuditLogById(ViewQueryAuditLogById viewQueryAuditLogById)
        {
            var query = new ViewQueryAuditLogById();
            query.ViewQueryCommon.ViewName = "AuditLogById";
            query.ReleaseVersion = Configurations.ReleaseVersion;

            var auditLogs = GetAuditLogById("xyz");

            //query.Count = auditLogs.Count;
            query.AuditLog = auditLogs;

            return View(query);
        }

        private List<AuditLog> GetAuditLogsAll()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            List<AuditLog> auditLogs = auditLogDataService.GetAuditLogsAll_sproc().ToList();

            return auditLogs;
        }

        private AuditLog GetAuditLogById(string id)
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            AuditLog auditLog = auditLogDataService.GetAuditLogById(id);

            return auditLog;
        }
    }
}
