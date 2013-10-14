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
        //get the server url
        public static string GetServiceURL()
        {
            return ConfigurationManager.AppSettings["serviceBaseUrl"];
        }
        //get username for users
        public static string GetUsername()
        {
            return ConfigurationManager.AppSettings["userName"];
        }
        //get password for users
        public static string GetPassword()
        {
            return ConfigurationManager.AppSettings["password"];
        }
        //get username for admins
        public static string GetAdminUsername()
        {
            return ConfigurationManager.AppSettings["adminuserName"];
        }
        //get password for users
        public static string GetAdminPassword()
        {
            return ConfigurationManager.AppSettings["adminpassword"];
        }
    }
}
