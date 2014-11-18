using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogSummary : ViewQueryBase
    {
        public long TotalRecordCount { get; set; }
        public List<string> ApplicationNames { get; set; }
        public List<string> FeatureNames { get; set; }
        public List<string> Categories { get; set; }
    }
}