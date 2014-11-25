using System.Collections.Generic;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogSummary : ViewQueryBase
    {
        public ViewQueryAuditLogSummary()
        {
            ApplicationNames = new List<ApplicationName>();
            FeatureNames = new List<FeatureName>();
        }
        public long TotalRecordCount { get; set; }
        public List<ApplicationName> ApplicationNames { get; set; }
        public List<FeatureName> FeatureNames { get; set; }
        public List<Category> Categories { get; set; }
    }
}