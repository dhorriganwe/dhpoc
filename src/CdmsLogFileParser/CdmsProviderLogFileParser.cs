using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CdmsLogFileParser.Models;

namespace CdmsLogFileParser
{
    public class CdmsProviderLogFileParser
    {
        public CdmsProviderLogFileParser()
        {
            CdmsRequestTypeIdentifiers.Add("ProductListRequest");
            CdmsRequestTypeIdentifiers.Add("ProductListResponse");

            CdmsRequestTypeIdentifiers.Add("LabelCheckMix CheckMixRequest");
            CdmsRequestTypeIdentifiers.Add("Check Job_Request");
            CdmsRequestTypeIdentifiers.Add("Check Job_Response");
            CdmsRequestTypeIdentifiers.Add("LabelCheckMix complete");

            CdmsRequestTypeIdentifiers.Add("AnswerQuestionRequest");
            CdmsRequestTypeIdentifiers.Add("Answer request");
            CdmsRequestTypeIdentifiers.Add("Answer Job_Response");
            CdmsRequestTypeIdentifiers.Add("AnswerQuestion complete");
        }


        public LogFileLineType SetLogFileLineType(LogFileLine logFileLine)
        {
            if (string.IsNullOrEmpty(logFileLine.Text))
            {
                logFileLine.LogFileLineType = LogFileLineType.CR;
                return logFileLine.LogFileLineType;
            }

            if (logFileLine.Text == Environment.NewLine)
            {
                logFileLine.LogFileLineType = LogFileLineType.CR;
                return logFileLine.LogFileLineType;
            }

            DateTime dateTime;
            if (DateTime.TryParse(logFileLine.Text, out dateTime))
            {
                logFileLine.LogFileLineType = LogFileLineType.Date;
                return logFileLine.LogFileLineType;
            }

            if (logFileLine.Text.IndexOf("correlationId", System.StringComparison.Ordinal) == 0)
            {
                logFileLine.LogFileLineType = LogFileLineType.CorrelationId;
                return logFileLine.LogFileLineType;
            }

            if (logFileLine.Text.IndexOf("MachineName", System.StringComparison.Ordinal) == 0)
            {
                logFileLine.LogFileLineType = LogFileLineType.MachineName;
                return logFileLine.LogFileLineType;
            }

            if (CdmsRequestTypeIdentifiers.Contains(logFileLine.Text))
            {
                logFileLine.LogFileLineType = LogFileLineType.CdmsRequestType;

                logFileLine.CdmsRequestType = GetCdmsRequestType(logFileLine.Text);
                return logFileLine.LogFileLineType;
            }

            return LogFileLineType.Unknown;
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

        public List<string> CdmsRequestTypeIdentifiers = new List<string>();
    }
}
