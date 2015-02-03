using System.Collections.Generic;
using System.Web.Mvc;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.WebApp.Models
{
    public class AuditLogViewModel
    {
        public AuditLogViewModel()
        {
            // provide default values for SelectList members so that UI can display errors
            ApplicationNameList = new SelectList(new List<LookupItem> { new LookupItem { Value = "*", Description = "*" } }, "Value", "Description");
            TraceLevelList = new SelectList(new List<LookupItem> { new LookupItem { Value = "*", Description = "*" } }, "Value", "Description");
            DbKeyList = new SelectList(new List<LookupItem> { new LookupItem { Value = "*", Description = "*" } }, "Value", "Description");
            AuditLogs = new List<AuditLogItem>();
        }

        // meta info
        public string CurrentServerTime { get; set; }
        public string ReleaseVersion { get; set; }
        public string ViewTitle { get; set; }
        public string ErrorMessage { get; set; }

        // db data info
        public long TotalRecordCount { get; set; }
        public int MaxRowCount { get; set; }
        public string DbKey { get; set; }
        public SelectList DbKeyList { get; set; }
        public List<AuditLogItem> AuditLogs { get; set; }

        // browse info
        public string AuditLogId { get; set; }
        public AuditLogItem AuditLog { get; set; }
        public string ApplicationName { get; set; }
        public string Category { get; set; }
        public string EventId { get; set; }
        public string FeatureName { get; set; }
        public string TraceLevel { get; set; }

        public List<SummaryItem> ApplicationNames { get; set; }
        public List<SummaryItem> FeatureNames { get; set; }
        public List<SummaryItem> Categories { get; set; }

        // search info
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CorrelationIdSearchStr { get; set; }
        public string MessageSearchStr { get; set; }
        public string AdditionalInfoSearchStr { get; set; }
        public string LoginNameSearchStr { get; set; }

        public SelectList ApplicationNameList { get; set; }
        public SelectList TraceLevelList { get; set; }
    }
}