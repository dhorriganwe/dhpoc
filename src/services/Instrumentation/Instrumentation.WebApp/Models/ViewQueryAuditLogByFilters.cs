using System.Web.Mvc;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogByFilters : ViewQueryBase
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string CorrelationIdSearchStr { get; set; }
        public string MessageSearchStr { get; set; }
        public string AdditionalInfoSearchStr { get; set; }
        public string ApplicationName { get; set; }
        public string FeatureName { get; set; }
        public string Category { get; set; }
        public string TraceLevel { get; set; }

        public SelectList ApplicationNameList { get; set; }
        public SelectList FeatureNameList { get; set; }
        public SelectList CategoryList { get; set; }
        public SelectList TraceLevelList { get; set; }
    }
}