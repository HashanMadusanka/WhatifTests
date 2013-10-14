﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WIF.SJA.API.Tests.Settings;

namespace WIF.SJA.API.Tests
{
    public class TestExecutor
    {

        public string ResponseGenerator(HttpWebRequest request)
        {
            string responsejson = null;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream webStream = response.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        responsejson = responseReader.ReadToEnd();

                    }
                }
            }

            return responsejson;
        }

        public string Get(string entity, string filterParams)
        {            
            filterParams = filterParams ?? "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TestSettings.GetServiceURL() + entity + "s/" + filterParams);

            string userName=TestSettings.GetUsername();
            string password=TestSettings.GetPassword();
            string username_password=userName+":"+password;
            byte [] credentials=Encoding.UTF8.GetBytes(username_password.ToCharArray());
            request.Headers.Add("Authorization","BASIC "+Convert.ToBase64String(credentials));
            
            return ResponseGenerator(request);           
        }

        public string Get(string entity)
        {         
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TestSettings.GetServiceURL() + entity + "s/");

            string userName = TestSettings.GetUsername();
            string password = TestSettings.GetPassword();
            string username_password = userName + ":" + password;
            byte[] credentials = Encoding.UTF8.GetBytes(username_password.ToCharArray());
            request.Headers.Add("Authorization", "BASIC" + Convert.ToBase64String(credentials));
            
            
            return ResponseGenerator(request);
        }

    }
}
