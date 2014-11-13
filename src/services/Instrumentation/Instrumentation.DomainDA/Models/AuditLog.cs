
namespace Instrumentation.DomainDA.Models
{
    public class AuditLog
    {
        public string Id;
        public string EventId;
        public string ApplicationName;
        public string FeatureName;
        public string Category;
        public string MessageCode;
        public string Messages;
        public string TraceLevel;
        public string LoginName;
        public string AuditedOn;
        public string AdditionalInfo;
    }
}
