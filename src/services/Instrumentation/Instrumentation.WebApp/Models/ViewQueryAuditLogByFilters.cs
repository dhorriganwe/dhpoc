using System.Collections.Generic;
using System.Web.Mvc;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogByFilters : ViewQueryBase
    {
        public ViewQueryAuditLogByFilters()
        {
            // provide default values for SelectList members so that UI can display errors
            ApplicationNameList = new SelectList(new List<LookupItem> { new LookupItem { Value = "*", Description = "*" } }, "Value", "Description");
            TraceLevelList = new SelectList(new List<LookupItem> { new LookupItem { Value = "*", Description = "*" } }, "Value", "Description");
        }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string CorrelationIdSearchStr { get; set; }
        public string MessageSearchStr { get; set; }
        public string AdditionalInfoSearchStr { get; set; }
        public string LoginNameSearchStr { get; set; }
        public string ApplicationName { get; set; }
        public string TraceLevel { get; set; }

        public SelectList ApplicationNameList { get; set; }
        public SelectList TraceLevelList { get; set; }
    }
}