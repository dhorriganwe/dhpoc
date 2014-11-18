﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogById : ViewQueryBase
    {
        public string AuditLogId { get; set; }
        public AuditLog AuditLog { get; set; }
        public string Json { get; set; }
    }
}