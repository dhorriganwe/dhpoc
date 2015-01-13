using System.Web.Mvc;
using Instrumentation.DomainDA.Helpers;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryHeader
    {
        public ViewQueryHeader()
        {
            MaxRowCount = Configurations.MaxRowCountDefault;
        }
        public string DbKey { get; set; }
        public string ViewName { get; set; }
        public string ViewTitle { get; set; }
        public int MaxRowCount { get; set; }

        public SelectList DbOptionSelectList { get; set; }
    }
}
