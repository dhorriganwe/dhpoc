
namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogByCategory : ViewQueryBase
    {
        public ViewQueryAuditLogByCategory()
        {
            List = new ViewQueryAuditLogList();
        }

        public string Category { get; set; }
        public ViewQueryAuditLogList List { get; set; }
    }
}