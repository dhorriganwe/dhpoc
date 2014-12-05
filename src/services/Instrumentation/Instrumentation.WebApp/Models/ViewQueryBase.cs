
using System.Collections.Generic;
using System.Web.Mvc;
using Instrumentation.DomainDA.Helpers;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryBase
    {
        public ViewQueryBase()
        {
            MaxRowCount = Configurations.MaxRowCountDefault;
            ApplicationNames = new List<ApplicationName>();
            FeatureNames = new List<FeatureName>();
            Categories = new List<Category>();
        }

        public string ViewName { get; set; }
        public string ViewTitle { get; set; }
        public string SessionId { get; set; }
        public string ReleaseVersion { get; set; }

        public int MaxRowCount { get; set; }
        public string DbKey { get; set; }
        public SelectList DbOptionSelectList { get; set; }

        public List<AuditLog> AuditLogs { get; set; }

        public List<ApplicationName> ApplicationNames { get; set; }
        public List<FeatureName> FeatureNames { get; set; }
        public List<Category> Categories { get; set; }
    }
}