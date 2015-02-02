
namespace Instrumentation.WebApp.Models
{
    public class ViewQueryAuditLogById : ViewQueryBase
    {
        public string AuditLogId { get; set; }
        public AuditLog AuditLog { get; set; }
    }
}