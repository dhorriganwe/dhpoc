using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLog : ViewQueryBase
    {
        public long Count { get; set; }
        public List<AuditLog> AuditLogs { get; set; }
    }
}