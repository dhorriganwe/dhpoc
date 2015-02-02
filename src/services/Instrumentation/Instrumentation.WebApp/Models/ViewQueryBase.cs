﻿using System.Collections.Generic;
using System.Web.Mvc;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryBase
    {
        public ViewQueryBase()
        {
            Header = new ViewQueryHeader();

            // provide default values for SelectList members so that UI can display errors
            DbKeyList = new SelectList(new List<LookupItem> { new LookupItem { Value = "*", Description = "*" } }, "Value", "Description");
            AuditLogs = new List<AuditLog>();
        }

        public ViewQueryHeader Header { get; set; }
        public string ReleaseVersion { get; set; }
        public string ViewTitle { get; set; }

        public int MaxRowCount { get; set; }
        public string DbKey { get; set; }
        public SelectList DbKeyList { get; set; }
        public List<AuditLog> AuditLogs { get; set; }

        public List<SummaryItem> ApplicationNames { get; set; }
        public List<SummaryItem> FeatureNames { get; set; }
        public List<SummaryItem> Categories { get; set; }
    }
}