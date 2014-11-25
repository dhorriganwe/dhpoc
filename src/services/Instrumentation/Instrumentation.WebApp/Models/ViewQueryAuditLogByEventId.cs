
namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogByEventId : ViewQueryBase
    {
        public ViewQueryAuditLogByEventId()
        {
            List = new ViewQueryAuditLogList();
        }

        public string EventId { get; set; }
        public ViewQueryAuditLogList List { get; set; }
    }
}