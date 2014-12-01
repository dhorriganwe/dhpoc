using System.Collections.Generic;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.DomainDA.DataServices
{
    public interface IAuditLogDataService
    {
        IList<AuditLog> GetAuditLogByApplicationName(string applicationName, int maxRowCount = -1);
        IList<AuditLog> GetAuditLogByCategory(string category, int maxRowCount = -1);
        IList<AuditLog> GetAuditLogByEventId(string eventid, int maxRowCount = -1);
        IList<AuditLog> GetAuditLogByFeatureName(string featureName, int maxRowCount = -1);
        IList<AuditLog> GetAuditLogByTraceLevel(string travelLevel, int maxRowCount = -1);
        IList<AuditLog> GetAuditLogAll(int maxRowCount = -1);
        AuditLog GetAuditLogById(string id);
        void AddAuditLog(AuditLog auditLog);
        AuditLogSummary GetAuditLogSummary();
        List<ApplicationName> GetApplicationNames();
        List<FeatureName> GetFeatureNames();
        List<Category> GetCategories();
    }
}