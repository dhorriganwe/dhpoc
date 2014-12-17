using System;
using System.Collections.Generic;
using CdmsLogFileParser.Models;

namespace CdmsLogFileParser
{
    public class LogFileParser
    {
        private List<string> _cdmsRequestTypeIdentifiers = new List<string>();
        public LogFileParser()
        {
            _cdmsRequestTypeIdentifiers.Add("ProductListRequest");
            _cdmsRequestTypeIdentifiers.Add("ProductListResponse");

            _cdmsRequestTypeIdentifiers.Add("LabelCheckMix CheckMixRequest");
            _cdmsRequestTypeIdentifiers.Add("Check Job_Request");
            _cdmsRequestTypeIdentifiers.Add("Check Job_Response");
            _cdmsRequestTypeIdentifiers.Add("LabelCheckMix complete");

            _cdmsRequestTypeIdentifiers.Add("AnswerQuestionRequest");
            _cdmsRequestTypeIdentifiers.Add("Answer request");
            _cdmsRequestTypeIdentifiers.Add("Answer Job_Response");
            _cdmsRequestTypeIdentifiers.Add("AnswerQuestion complete");
            //_cdmsRequestTypeIdentifiers.Add("cdms:");
        }

        public LogFileLineType SetLogFileLineType(LogFileLine logFileLine)
        {
            if (string.IsNullOrEmpty(logFileLine.Text))
            {
                logFileLine.LogFileLineType = LogFileLineType.CR;
                logFileLine.CdmsRequestType = CdmsRequestType.NA;
                return logFileLine.LogFileLineType;
            }

            if (logFileLine.Text == Environment.NewLine)
            {
                logFileLine.LogFileLineType = LogFileLineType.CR;
                logFileLine.CdmsRequestType = CdmsRequestType.NA;
                return logFileLine.LogFileLineType;
            }

            DateTime dateTime;
            if (DateTime.TryParse(logFileLine.Text, out dateTime))
            {
                logFileLine.LogFileLineType = LogFileLineType.TimeStamp;
                logFileLine.CdmsRequestType = CdmsRequestType.NA;
                return logFileLine.LogFileLineType;
            }

            if (logFileLine.Text.IndexOf("correlationId", System.StringComparison.Ordinal) == 0)
            {
                logFileLine.LogFileLineType = LogFileLineType.CorrelationId;
                logFileLine.CdmsRequestType = CdmsRequestType.NA;
                return logFileLine.LogFileLineType;
            }

            if (logFileLine.Text.IndexOf("MachineName", System.StringComparison.Ordinal) == 0)
            {
                logFileLine.LogFileLineType = LogFileLineType.MachineName;
                logFileLine.CdmsRequestType = CdmsRequestType.NA;
                return logFileLine.LogFileLineType;
            }

            if (_cdmsRequestTypeIdentifiers.Contains(logFileLine.Text))
            {
                logFileLine.LogFileLineType = LogFileLineType.CdmsRequestType;
                logFileLine.CdmsRequestType = GetCdmsRequestType(logFileLine.Text);
                return logFileLine.LogFileLineType;
            }

            if (logFileLine.Text.Contains("cdms:"))
            {
                logFileLine.LogFileLineType = LogFileLineType.PerformanceData;
                logFileLine.CdmsRequestType = CdmsRequestType.NA;  
                return logFileLine.LogFileLineType;
            }

            logFileLine.LogFileLineType = LogFileLineType.Content;
            logFileLine.CdmsRequestType = CdmsRequestType.NA;
            
            return LogFileLineType.Content;
        }

        private CdmsRequestType GetCdmsRequestType(string text)
        {
            if (string.IsNullOrEmpty(text))
                return CdmsRequestType.Unknown;

            switch (text)
            {
                case "ProductListRequest":
                    return CdmsRequestType.ProductListRequest;
                    break;
                case "ProductListResponse":
                    return CdmsRequestType.ProductListResponse;
                    break;
                case "LabelCheckMix CheckMixRequest":
                    return CdmsRequestType.LabelCheckMixCheckMixRequest;
                    break;
                case "Check Job_Request":
                    return CdmsRequestType.CheckJob_Request;
                    break;
                case "Check Job_Response":
                    return CdmsRequestType.CheckJob_Response;
                    break;
                case "LabelCheckMix complete":
                    return CdmsRequestType.LabelCheckMixComplete;
                    break;
                case "AnswerQuestionRequest":
                    return CdmsRequestType.AnswerQuestionRequest;
                    break;
                case "Answer request":
                    return CdmsRequestType.AnswerRequest;
                    break;
                case "Answer Job_Response":
                    return CdmsRequestType.AnswerJob_Response;
                    break;
                case "AnswerQuestion complete":
                    return CdmsRequestType.AnswerQuestionComplete;
                    break;
                default:
                    return CdmsRequestType.Unknown;
            }
        }

        public void ParsePerformanceLineToValues(CdmsRequestItem item)
        {
            const string cdmsToken = "cdms:";
            const string pvdrToken = "pvdr:";
            const string msToken = "ms";
            int posCdmsVal = -1;
            int posMs = -1;
            int posPvdrVal = -1;

            posCdmsVal = item.RequestPerfData.IndexOf(cdmsToken, StringComparison.Ordinal) + cdmsToken.Length;
            posMs = item.RequestPerfData.IndexOf(msToken, posCdmsVal, StringComparison.Ordinal);

            item.CdmsPerformance = item.RequestPerfData.Substring(posCdmsVal, posMs - posCdmsVal);

            if (item.RequestPerfData.Contains(pvdrToken))
            {
                posPvdrVal = item.RequestPerfData.IndexOf(pvdrToken, StringComparison.Ordinal) + pvdrToken.Length;
                posMs = item.RequestPerfData.IndexOf(msToken, posPvdrVal, StringComparison.Ordinal);

                item.ProviderPerformance = item.RequestPerfData.Substring(posPvdrVal, posMs - posPvdrVal);
            }
        }
    }
}
