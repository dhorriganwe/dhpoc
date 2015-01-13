using System.Collections.Generic;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryBase
    {
        public ViewQueryBase()
        {
            ApplicationNames = new List<SummaryItem>();
            FeatureNames = new List<SummaryItem>();
            Categories = new List<SummaryItem>();
            Header = new ViewQueryHeader();
        }

        public ViewQueryHeader Header { get; set; }
        public string SessionId { get; set; }
        public string ReleaseVersion { get; set; }

        public List<AuditLog> AuditLogs { get; set; }

        public List<SummaryItem> ApplicationNames { get; set; }
        public List<SummaryItem> FeatureNames { get; set; }
        public List<SummaryItem> Categories { get; set; }
    }
}