using Instrumentation.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instrumentation.WebApp.Helpers
{
    public class InstrumentationMapper
    {
        public AuditLog MapDaToUiAuditLog(Instrumentation.DomainDA.Models.AuditLog auditLogDa)
        {
            var auditLogUi = new AuditLog();
            auditLogUi.Id = auditLogDa.Id;
            auditLogUi.EventId = auditLogDa.EventId;
            auditLogUi.ApplicationName = auditLogDa.ApplicationName;
            auditLogUi.FeatureName = auditLogDa.FeatureName;
            auditLogUi.Category = auditLogDa.Category;

            auditLogUi.TraceLevel = auditLogDa.TraceLevel;
            auditLogUi.MessageCode = auditLogDa.MessageCode;
            auditLogUi.AdditionalInfo = auditLogDa.AdditionalInfo;
            auditLogUi.LoginName = auditLogDa.LoginName;
            auditLogUi.AuditedOn = auditLogDa.AuditedOn;

            auditLogUi.Messages = auditLogDa.Messages;

            return auditLogUi;
        }

        public List<AuditLog> MapDaToUiAuditLog(List<Instrumentation.DomainDA.Models.AuditLog> auditLogDas)
        {
            List<AuditLog> auditLogUis = new List<AuditLog>();

            foreach(var alDa in auditLogDas)
            {
                auditLogUis.Add(MapDaToUiAuditLog(alDa));
            }

            return auditLogUis;
        }

    }
}