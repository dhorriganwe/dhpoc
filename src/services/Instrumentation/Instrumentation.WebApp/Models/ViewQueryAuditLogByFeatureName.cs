
namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogByFeatureName : ViewQueryBase
    {
        public ViewQueryAuditLogByFeatureName()
        {
            List = new ViewQueryAuditLogList();
        }

        public string FeatureName { get; set; }
        public ViewQueryAuditLogList List { get; set; }
    }
}