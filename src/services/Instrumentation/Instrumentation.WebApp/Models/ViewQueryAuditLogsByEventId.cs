using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogsByEventId : ViewQueryBase
    {
        public string EventId { get; set; }
        public List<AuditLog> AuditLogs { get; set; }
        public string Json { get; set; }
    }
}