using System.Collections.Generic;
using Instrumentation.DomainDA.Helpers;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.DomainDA.DataServices
{
    public interface IAuditLogDataService
    {
        IList<AuditLog> GetAuditLogByApplicationName(string applicationName, int rowcount = Constants.ROWCOUNT);
        IList<AuditLog> GetAuditLogByCategory(string category, int rowcount = Constants.ROWCOUNT);
        IList<AuditLog> GetAuditLogByEventId(string eventid, int rowcount = Constants.ROWCOUNT);
        IList<AuditLog> GetAuditLogByFeatureName(string featureName, int rowcount = Constants.ROWCOUNT);
        IList<AuditLog> GetAuditLogByTraceLevel(string travelLevel, int rowcount = Constants.ROWCOUNT);
        IList<AuditLog> GetAuditLogAll(int rowcount = Constants.ROWCOUNT);
        AuditLog GetAuditLogById(string id);
        void AddAuditLog(AuditLog auditLog);
        AuditLogSummary GetAuditLogSummary();
        List<ApplicationName> GetApplicationNames();
        List<FeatureName> GetFeatureNames();
        List<Category> GetCategories();
    }
}