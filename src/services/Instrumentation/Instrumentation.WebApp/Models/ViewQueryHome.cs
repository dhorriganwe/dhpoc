using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.WebApp.Models
{
    public class ViewQueryHome : ViewQueryBase
    {
        public string CurrentServerTime { get; set; }
    }
}