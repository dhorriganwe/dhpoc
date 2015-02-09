using System.Collections.Generic;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.DomainDA.DataServices
{
    public interface IAuditLogDataService
    {
        IList<AuditLog> GetAll(int maxRowCount = -1);
        IList<AuditLog> GetByApplicationName(string applicationName, int maxRowCount = -1);
        IList<AuditLog> GetByCategory(string category, int maxRowCount = -1);
        IList<AuditLog> GetByEventId(string eventid, int maxRowCount = -1);
        IList<AuditLog> GetByFeatureName(string featureName, int maxRowCount = -1);
        IList<AuditLog> GetByTraceLevel(string travelLevel, int maxRowCount = -1);

        IList<AuditLog> GetByAppNameAndTraceLevel(int maxRowCount, string startTime, string endTime, string travelLevel, string applicationName);
        IList<AuditLog> GetByILikeEventId(int maxRowCount, string startTime, string endTime, string eventIdSearchStr);
        IList<AuditLog> GetByILikeMessage(int maxRowCount, string startTime, string endTime, string messageSearchStr);
        IList<AuditLog> GetByILikeAdditionalInfo(int maxRowCount, string startTime, string endTime, string additionalInfoSearchStr);
        IList<AuditLog> GetByILikeLoginName(int maxRowCount, string startTime, string endTime, string loginNameSearchStr);

        AuditLog GetById(string id);
        void AddAuditLog(AuditLog auditLog);
        long GetTotalRowCount();
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