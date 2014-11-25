
namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogByApplicationName : ViewQueryBase
    {
        public ViewQueryAuditLogByApplicationName()
        {
            List = new ViewQueryAuditLogList();
        }

        public string ApplicationName { get; set; }
        public ViewQueryAuditLogList List { get; set; }
    }
}