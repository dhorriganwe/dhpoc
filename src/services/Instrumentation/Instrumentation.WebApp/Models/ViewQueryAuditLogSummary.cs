using System.Collections.Generic;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogSummary : ViewQueryBase
    {
        public long TotalRecordCount { get; set; }
    }
}