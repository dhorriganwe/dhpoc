
using System.Web.Mvc;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryBase
    {
        public string ViewName { get; set; }
        public string ViewTitle { get; set; }
        public string SessionId { get; set; }
        public string ReleaseVersion { get; set; }

        public string DbKey { get; set; }
        public SelectList DbOptionSelectList { get; set; }
    }
}