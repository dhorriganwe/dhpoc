using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogById : ViewQueryBase
    {
        public AuditLog AuditLog { get; set; }
        public string AuditLogId { get; set; }
    }
}