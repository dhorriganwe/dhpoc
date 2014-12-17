using System;
using System.Linq;
using CdmsLogFileParser.Models;

namespace CdmsLogFileParser
{
    public class JobResultsAnalyzer
    {
        public void GenerateAverages(JobSummary jobSummary)
        {
            var allDurations = jobSummary.AllRequestItems.Where(i => i.RequestType == "ProductListResponse").Select(GetCdmsRequestDuration).ToList();
            var average = (int)allDurations.Average();
            jobSummary.Averages.Add("ProductListResponse", average);

            allDurations = jobSummary.AllRequestItems.Where(i => i.RequestType == "Check Job_Response").Select(GetCdmsRequestDuration).ToList();
            average = (int)allDurations.Average();
            jobSummary.Averages.Add("Check Job_Response", average);

            allDurations = jobSummary.AllRequestItems.Where(i => i.RequestType == "Answer Job_Response").Select(GetCdmsRequestDuration).ToList();
            average = (int)allDurations.Average();
            jobSummary.Averages.Add("Answer Job_Response", average);
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
