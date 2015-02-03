using Instrumentation.WebApp.Models;
using System.Collections.Generic;

namespace Instrumentation.WebApp.Helpers
{
    public class InstrumentationMapper
    {
        public AuditLogItem MapDaToUiAuditLog(Instrumentation.DomainDA.Models.AuditLog auditLogDa)
        {
            var auditLogUi = new AuditLogItem();
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


            if (!string.IsNullOrEmpty(auditLogUi.Id))
                auditLogUi.Id = auditLogUi.Id.Trim();
            if (!string.IsNullOrEmpty(auditLogUi.EventId))
                auditLogUi.EventId = auditLogUi.EventId.Trim();
            if (!string.IsNullOrEmpty(auditLogUi.ApplicationName))
                auditLogUi.ApplicationName = auditLogUi.ApplicationName.Trim();
            if (!string.IsNullOrEmpty(auditLogUi.FeatureName))
                auditLogUi.FeatureName = auditLogUi.FeatureName.Trim();
            if (!string.IsNullOrEmpty(auditLogUi.Category))
                auditLogUi.Category = auditLogUi.Category.Trim();
            if (!string.IsNullOrEmpty(auditLogUi.TraceLevel))
                auditLogUi.TraceLevel = auditLogUi.TraceLevel.Trim();
            if (!string.IsNullOrEmpty(auditLogUi.MessageCode))
                auditLogUi.MessageCode = auditLogUi.MessageCode.Trim();

            return auditLogUi;
        }

        public List<AuditLogItem> MapDaToUiAuditLog(List<Instrumentation.DomainDA.Models.AuditLog> auditLogDas)
        {
            List<AuditLogItem> auditLogUis = new List<AuditLogItem>();

            foreach(var alDa in auditLogDas)
            {
                auditLogUis.Add(MapDaToUiAuditLog(alDa));
            }

            return auditLogUis;
        }

    }
}