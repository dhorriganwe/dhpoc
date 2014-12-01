
using System.Collections.Generic;
using System.Web.Mvc;
using Instrumentation.DomainDA.Helpers;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryBase
    {
        public ViewQueryBase()
        {
            MaxRowCount = Configurations.MaxRowCountDefault;
        }

        public string ViewName { get; set; }
        public string ViewTitle { get; set; }
        public string SessionId { get; set; }
        public string ReleaseVersion { get; set; }

        public int MaxRowCount { get; set; }
        public string DbKey { get; set; }
        public SelectList DbOptionSelectList { get; set; }

        public List<AuditLog> AuditLogs { get; set; }
    }
}