using System.Web.Mvc;
using Instrumentation.DomainDA.Helpers;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryFilters
    {
        public ViewQueryFilters()
        {
            MaxRowCount = Configurations.MaxRowCountDefault;
            StartDate = "1/1/2015";
            StartDate = "12/1/2015";
        }
        public int MaxRowCount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public SelectList ApplicationSelectList { get; set; }
        public SelectList FeatureSelectList { get; set; }
        public SelectList CategorySelectList { get; set; }
    }
}
