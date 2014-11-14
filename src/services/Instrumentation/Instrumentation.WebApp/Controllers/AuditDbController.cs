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


        [HttpPost, ValidateInput(false)]
        public ActionResult AuditLogById(ViewQueryAuditLogById query, string command)
        {
            query.ViewQueryCommon.ViewName = "AuditLogById";


            if (command == "Search")
            {
                var auditLogs = GetAuditLogById(query.AuditLogId);
                query.AuditLog = auditLogs;
            }
            else
            {
                query.AuditLog = new AuditLog();
            }

            return View(query);
        }

        //public ActionResult AuditLogById(ViewQueryAuditLogById query)
        //{
        //    query.ViewQueryCommon.ViewName = "AuditLogById";
        //    query.AuditLog = new AuditLog();

        //    return View(query);
        //}

        public ActionResult AuditLogById(string id)
        {
            var query = new ViewQueryAuditLogById();
            query.AuditLogId = id;

            return AuditLogById(query, "Search");
        }

        private List<AuditLog> GetAuditLogsAll()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            List<AuditLog> auditLogs = auditLogDataService.GetAuditLogsAll().ToList();

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
