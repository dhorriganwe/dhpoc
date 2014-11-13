using System.Collections.Generic;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryBase
    {
        public ViewQueryBase()
        {
            ViewQueryCommon = new ViewQueryCommon();
        }
        public string SessionId { get; set; }
        public ViewQueryCommon ViewQueryCommon { get; set; }
        public string ReleaseVersion { get; set; }
        public string RequestDuration { get; set; }
        public string FailMessage { get; set; }
        public List<FailDetail> FailDetails { get; set; }
        public string AjaxUrlRoot { get; set; }
        public bool QueryProduction { get; set; }
    }
}