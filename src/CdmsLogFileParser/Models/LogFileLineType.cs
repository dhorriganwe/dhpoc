using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdmsLogFileParser.Models
{
    public enum LogFileLineType
    {
        Unknown,
        CR,
        Date,
        CorrelationId,
        MachineName,
        CdmsRequestType,

    }
}
