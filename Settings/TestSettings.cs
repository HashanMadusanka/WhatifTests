using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIF.SJA.API.Tests.Settings
{
    public static class TestSettings
    {
        public static string GetServiceURL()
        {
            return ConfigurationManager.AppSettings["serviceBaseUrl"];
        }
        public static string GetUsername()
        {
            return ConfigurationManager.AppSettings["userName"];
        }
        public static string GetPassword()
        {
            return ConfigurationManager.AppSettings["password"];
        }
    }
}
