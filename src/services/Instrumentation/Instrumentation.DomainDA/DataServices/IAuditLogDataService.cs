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
        List<SummaryItem> GetApplicationNames();
        List<SummaryItem> GetFeatureNames();
        List<SummaryItem> GetCategories();

        //List<SummaryItem> GetSummaryItems();

        //IList<AuditLog> GetAuditLogByApplicationName(string applicationName, int maxRowCount = -1);
        List<SummaryItem> GetSummaryItemsByApplicationName(string summaryType, string applicationName);
    }
}