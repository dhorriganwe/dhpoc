using System;
using System.Linq;
using CdmsLogFileParser.Models;

namespace CdmsLogFileParser
{
    public class JobResultsAnalyzer
    {
        public void GenerateAverages(JobSummary jobSummary)
        {
            var productListSummary = new RequestTypeSummary();
            var checkSummary = new RequestTypeSummary();
            var answerSummary = new RequestTypeSummary();

            var productListRequests = jobSummary.AllRequestItems.Where(i => i.RequestType == "ProductListResponse").Select(GetCdmsRequestDuration).ToList();
            productListSummary.AverageDuration = (int)productListRequests.Average();
            productListSummary.Count = productListRequests.Count;
            jobSummary.RequestTypeSummaries.Add("ProductListResponse", productListSummary);

            var checkRequests = jobSummary.AllRequestItems.Where(i => i.RequestType == "Check Job_Response").Select(GetCdmsRequestDuration).ToList();
            checkSummary.AverageDuration = (int)checkRequests.Average();
            checkSummary.Count = checkRequests.Count;
            jobSummary.RequestTypeSummaries.Add("Check Job_Response", checkSummary);

            var answerRequests = jobSummary.AllRequestItems.Where(i => i.RequestType == "Answer Job_Response").Select(GetCdmsRequestDuration).ToList();
            answerSummary.AverageDuration = (int)answerRequests.Average();
            answerSummary.Count = answerRequests.Count;
            jobSummary.RequestTypeSummaries.Add("Answer Job_Response", answerSummary);
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
