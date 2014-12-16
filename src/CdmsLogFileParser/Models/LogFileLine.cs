
namespace CdmsLogFileParser.Models
{
    public class LogFileLine
    {
        public LogFileLine()
        {}

        public LogFileLine(string text)
        {
            Text = text;
        }
        public string Text;
        public string Type;
        public CdmsRequestType CdmsRequestType;
        public LogFileLineType LogFileLineType;
    }
}
