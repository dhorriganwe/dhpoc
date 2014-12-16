using System;
using CdmsLogFileParser.Helpers;

namespace CdmsLogFileParser.Tests
{
    public class TestBase
    {
        protected string NL = Environment.NewLine;

        protected string SampleDataText(string textFileName)
        {
            const string folder = "CdmsLogFileParser.Tests.SampleData";

            var rdr = new ResourceReader();
            return rdr.GetResourceString(string.Format("{0}.{1}", folder, textFileName), this.GetType().Assembly);
        }

    }
}
