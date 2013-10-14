using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WIF.SJA.API.Models;

namespace WIF.SJA.API.Tests.Controllers
{
    [TestClass]
    public class OrganizationTest
    {
        TestExecutor organizationTestExec;

        [TestInitialize]
        public void InitOrganizationTest()
        {
            organizationTestExec= new TestExecutor();
        }

        [TestMethod]
        public void Organization_Count_Test()
        {
            JArray jarr = JArray.Parse(organizationTestExec.Get("organization", null));
            //List<Organization> orgList = jarr.ToObject<List<Organization>>();

            Assert.AreEqual(4, jarr.Count);
        }

        [TestMethod]
        public void Organization_Name_Not_Null()
        {
            JObject jobj = JObject.Parse(organizationTestExec.Get("organization", "(1)"));
            Organization org = JsonConvert.DeserializeObject<Organization>(jobj.ToString());
            Assert.IsNotNull(org.name);
        }

    }
}
