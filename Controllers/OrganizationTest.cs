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

        //check for organization count
        [TestMethod]
        public void Organization_Count_Test()
        {

            JArray jarr = JArray.Parse(organizationTestExec.Get("organization", null));
            //List<Organization> orgList = jarr.ToObject<List<Organization>>();

            Assert.AreEqual(4, jarr.Count);
        }

        //check if organization name is not null

        [TestMethod]
        public void Organization_Name_Not_Null()
        {
            JObject jobj = JObject.Parse(organizationTestExec.Get("organization", "(1)"));
            Organization org = JsonConvert.DeserializeObject<Organization>(jobj.ToString());
            Assert.IsNotNull(org.name);
        }

        //check if admin can access other organizations
        [TestMethod]
        public void Admin_Cannot_Access_Other_Organizations_Test() {
            //currently one admin can have only one organization
            JArray organizations = JArray.Parse(organizationTestExec.AdminGet("organization", null));

            if (organizations.Count > 1) {

                Assert.Fail("Admin has unauthorized access to other organizations");
            
            }
           
        
        }

        //check if users can access other organizations
        [TestMethod]
        public void User_Cannot_Access_Other_Organizations_Test()
        {
            //currently one user can have only one organization

            JArray organizations = JArray.Parse(organizationTestExec.Get("organization", null));

            if (organizations.Count > 1)
            {

                Assert.Fail("User has unauthorized access to other organizations");

            }


        }

        //check for unauthorized uri patterns
        [TestMethod]
        public void Organization_Uri_Conventions_Test()
        {

            bool is_filtering_allowed = false;

            JArray orgs_with_filter = JArray.Parse(organizationTestExec.Get("organization", "?$filter=id eq 1 "));

            is_filtering_allowed = (orgs_with_filter != null);

            Assert.IsFalse(is_filtering_allowed, "Dangerous filterings are allowed in Action");
        }

    }
}
