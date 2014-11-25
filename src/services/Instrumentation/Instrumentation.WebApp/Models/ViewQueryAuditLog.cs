
namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLog : ViewQueryBase
    {
        public ViewQueryAuditLog()
        {
            List = new ViewQueryAuditLogList();
        }

        public ViewQueryAuditLogList List { get; set; }
    }
}