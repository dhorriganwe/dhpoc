using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CdmsLogFileParser.Models;

namespace CdmsLogFileParser
{
    public class CdmsLogFileWorkflow
    {
        private readonly CdmsProviderLogFileParser _parser = new CdmsProviderLogFileParser();
        private readonly List<string> _cdmsPerformanceDataRequestTypes = new List<string>();

        public CdmsLogFileWorkflow()
        {
            // these are the logged items that correspond to 
            // a single CDMS request and its performance data
            _cdmsPerformanceDataRequestTypes.Add("ProductListResponse");
            _cdmsPerformanceDataRequestTypes.Add("Check Job_Response");
            _cdmsPerformanceDataRequestTypes.Add("Answer Job_Response");
        }

        public void ProcessFile(LogFile logFile)
        {
            ReadLogFile(logFile);

            IdentifyLogFileLines(logFile);

            IdentifyCdmsPerfLines(logFile);

            ValidatePerfLinesData(logFile);

            FindCdmsRequestItems(logFile);

            ValidateRequestItems(logFile);

            ParseCdmsPerformanceValues(logFile);
        }

        private void ParseCdmsPerformanceValues(LogFile logFile)
        {
            foreach (var item in logFile.CdmsRequestItems)
            {
                _parser.ParsePerformanceLineToValues(item);
            }

        }

        private void FindCdmsRequestItems(LogFile logFile)
        {
            const string correlationIdToken = "correlationId: ";
            const string machineNameToken = "MachineName: ";

            string fileCorrelationId = "";
            string fileMachineName = "";

            string requestTimeStamp = "";
            string requestType = "";
            string requestPerfData = "";

            foreach (var line in logFile.LogFileLines)
            {
                switch (line.LogFileLineType)
                {
                    case LogFileLineType.MachineName:
                        fileMachineName = line.Text.Substring(machineNameToken.Length);
                        break;
                    case LogFileLineType.CorrelationId:
                        fileCorrelationId = line.Text.Substring(correlationIdToken.Length);
                        break;
                    case LogFileLineType.TimeStamp:
                        requestTimeStamp = line.Text;
                        break;
                    case LogFileLineType.CdmsRequestType:
                        requestType = line.Text;
                        break;
                    case LogFileLineType.PerformanceData:
                        requestPerfData = line.Text;
                        break;
                }

                if (line.LogFileLineType == LogFileLineType.PerformanceData)
                {
                    var requestItem = new CdmsRequestItem();
                    requestItem.FileCorrelationId = fileCorrelationId;
                    requestItem.FileMachineName = fileMachineName;
                    requestItem.RequestTimeStamp = requestTimeStamp;
                    requestItem.RequestType = requestType;
                    requestItem.RequestPerfData = requestPerfData;

                    if (IsCdmsPerformanceData(requestItem))
                        logFile.CdmsRequestItems.Add(requestItem);

                    requestTimeStamp = "";
                    requestPerfData = "";
                    requestType = "";
                }

            }
        }

        private bool IsCdmsPerformanceData(CdmsRequestItem requestItem)
        {
            return _cdmsPerformanceDataRequestTypes.Contains(requestItem.RequestType);
        }

        private void ValidateRequestItems(LogFile logFile)
        {
            foreach (var item in logFile.CdmsRequestItems)
            {
                if (string.IsNullOrEmpty(item.FileCorrelationId)) throw new Exception(string.Format("Did not expect FileCorrelationId to be null."));
                if (string.IsNullOrEmpty(item.FileMachineName)) throw new Exception(string.Format("Did not expect FileMachineName to be null."));
                if (string.IsNullOrEmpty(item.RequestPerfData)) throw new Exception(string.Format("Did not expect RequestPerfData to be null."));
                if (string.IsNullOrEmpty(item.RequestTimeStamp)) throw new Exception(string.Format("Did not expect RequestTimeStamp to be null."));
                if (string.IsNullOrEmpty(item.RequestType)) throw new Exception(string.Format("Did not expect RequestType to be null."));
            }
        }

        private void ValidatePerfLinesData(LogFile logFile)
        {
            foreach (var logFileLine in logFile.LogFileLines)
            {
                if(logFileLine.CdmsRequestType == CdmsRequestType.Unknown)
                    throw new Exception(string.Format("Did not expect a logFileLine with CdmsRequestType.Unknown.  Text:{0}", logFileLine.Text));
                if (logFileLine.LogFileLineType == LogFileLineType.Unknown)
                    throw new Exception(string.Format("Did not expect a logFileLine with LogFileLineType.Unknown.  Text:{0}", logFileLine.Text));
            }
            foreach (var perfLine in logFile.CdmsPerfLines)
            {
                if (perfLine.CdmsRequestType == CdmsRequestType.Unknown)
                    throw new Exception(string.Format("Did not expect a perfLine with CdmsRequestType.Unknown.  Text:{0}", perfLine.Text));
                if (perfLine.LogFileLineType == LogFileLineType.Unknown)
                    throw new Exception(string.Format("Did not expect a perfLine with LogFileLineType.Unknown.  Text:{0}", perfLine.Text));
            }
        }

        private void IdentifyCdmsPerfLines(LogFile logFile)
        {
            logFile.CdmsPerfLines = logFile.LogFileLines.Where(lfl => lfl.LogFileLineType == LogFileLineType.CdmsRequestType).ToList();
        }

        private void IdentifyLogFileLines(LogFile logFile)
        {
            foreach (var line in logFile.Lines)
            {
                var logFileLine = new LogFileLine(line);
                _parser.SetLogFileLineType(logFileLine);
                logFile.LogFileLines.Add(logFileLine);
            }
        }

        private void ReadLogFile(LogFile logFile)
        {
            logFile.Lines = File.ReadLines(logFile.FileInfo.FullName).ToList();
        }
    }
}
