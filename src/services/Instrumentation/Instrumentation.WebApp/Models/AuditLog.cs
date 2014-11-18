
namespace Instrumentation.WebApp.Models
{
    public class AuditLog
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string ApplicationName { get; set; }
        public string FeatureName { get; set; }
        public string Category { get; set; }
        public string MessageCode { get; set; }
        public string Messages { get; set; }
        public string TraceLevel { get; set; }
        public string LoginName { get; set; }
        public string AuditedOn { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
