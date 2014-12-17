using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CdmsLogFileParser.Models;

namespace CdmsLogFileParser
{
    public class JobResultsAnalyzer
    {
        public void GenerateAverages(JobSummary jobSummary)
        {
            var durations = new List<int>();

            foreach (
                var item in
                    jobSummary.AllRequestItems.Where(i => i.RequestType == CdmsRequestType.AnswerJob_Response.ToString())
                )
            {
                durations.Add(GetCdmsRequestDuration(item));
            }

            jobSummary.AllRequestItems.Where(i => i.RequestType == CdmsRequestType.AnswerJob_Response.ToString())
                .ToList().ForEach(i => durations.Add(GetCdmsRequestDuration(i)));
        }

        private int GetCdmsRequestDuration(CdmsRequestItem item)
        {
            string cdmsPerformance = item.CdmsPerformance;

            int cdmsPerformanceInt;
            if (!int.TryParse(cdmsPerformance, out cdmsPerformanceInt))
                throw new Exception(string.Format("Could not int.Parse '{0}'", cdmsPerformance));

            return cdmsPerformanceInt;
        }
    }
}
