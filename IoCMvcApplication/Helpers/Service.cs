using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IoCMvcApplication.Helpers
{
    public class Service : IService
    {
        public string Print()
        {
            return "Successfully injected controller.";
        }
    }
}