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
        IList<AuditLog> GetAuditLogByFilters(int maxRowCount, string startTime, string endTime, string travelLevel, string applicationName);

        IList<AuditLog> GetByILikeEventId(int maxRowCount, string startTime, string endTime, string eventIdSearchStr);
        IList<AuditLog> GetByILikeMessage(int maxRowCount, string startTime, string endTime, string messageSearchStr);
        IList<AuditLog> GetByILikeAdditionalInfo(int maxRowCount, string startTime, string endTime, string additionalInfoSearchStr);
        IList<AuditLog> GetByILikeLoginName(int maxRowCount, string startTime, string endTime, string loginNameSearchStr);

        AuditLog GetAuditLogById(string id);
        void AddAuditLog(AuditLog auditLog);
        AuditLogSummary GetAuditLogRowCount();
        List<SummaryItem> GetApplicationNameCounts();
        List<SummaryItem> GetFeatureNameCounts();
        List<SummaryItem> GetCategoryCounts();

        List<string> GetApplicationNames();
        List<string> GetFeatureNames();
        List<string> GetCategories();
        List<string> GetTraceLevels();

        List<SummaryItem> GetSummaryItemsByApplicationName(string summaryType, string applicationName);
        List<SummaryItem> GetSummaryItemsByFeatureName(string summaryType, string featureName);
        List<SummaryItem> GetSummaryItemsByCategory(string summaryType, string category);
    }
}