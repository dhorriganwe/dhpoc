
using System.Collections.Generic;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogByApplicationName : ViewQueryBase
    {
        public string ApplicationName { get; set; }
    }
}