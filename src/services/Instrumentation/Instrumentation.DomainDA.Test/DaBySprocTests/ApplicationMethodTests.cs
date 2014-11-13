using System;
using System.Collections.Generic;
using System.Linq;
using Instrumentation.DomainDA.DataServices;
using Instrumentation.DomainDA.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instrumentation.DomainDA.Test.DaBySprocTests
{
    [TestClass]
    public class ApplicationMethodTests
    {
        [TestMethod]
        public void GetApplicationMethods_sproc()
        {
            // select rt.getallapplicationmethods()

            IApplicationMethodDataService applicationMethodDataService = new ApplicationMethodDataService();

            List<ApplicationMethod> applicationMethods = applicationMethodDataService.GetAllApplicationMethods_sproc().ToList();

            applicationMethods.ForEach(al => Console.WriteLine(string.Format("{0}  {1}", 
                al.Id, 
                al.Title)));
        }
    }
}
