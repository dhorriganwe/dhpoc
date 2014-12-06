
using System.Collections.Generic;

namespace Instrumentation.DomainDA.Models
{
    public class AuditLogSummary
    {
        public List<SummaryItem> FeatureNames;
        public List<SummaryItem> Categories; 
        public long TotalRowCount;
    }
}
