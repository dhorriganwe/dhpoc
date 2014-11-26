using System;
using Instrumentation.WebApp.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instrumentation.WebApp.Tests.Helpers
{
    [TestClass]
    public class InstrumentationMapperTests
    {
        InstrumentationMapper _mapper = new InstrumentationMapper();

        //[TestMethod]
        //public void UrlEncodeWorks()
        //{
        //    string applicationName = "WilburEllis.RisingTide.AdvancedRec.Services";
        //    Console.WriteLine(applicationName);

        //    var encoded = _mapper.UrlEncode(applicationName);
        //    Console.WriteLine(encoded);

        //    var decoded = _mapper.UrlEncode(encoded);
        //    Console.WriteLine(decoded);

        //    Assert.AreEqual(applicationName, decoded);
        //}
    }
}
