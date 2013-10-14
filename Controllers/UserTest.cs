using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using WIF.SJA.API.Models;

namespace WIF.SJA.API.Tests.Controllers
{
    [TestClass]
    public class UserTest
    {
        TestExecutor userTestExec;

        [TestInitialize]
        public void InitOrganizationTest()
        {
            userTestExec = new TestExecutor();
        }

        [TestMethod]
        public void Username_Is_Not_Null()
        {
            JArray jarr = JArray.Parse(userTestExec.Get("user"));
            List<User> userList = jarr.ToObject<List<User>>();

            foreach (User user in userList)
            {
                Assert.IsNotNull(user.email);
            }
        }

        [TestMethod]
        public void Created_Date_Is_Valid()
        {
            DateTime minDate = DateTime.Parse("2013/01/01");
            JArray jarr = JArray.Parse(userTestExec.Get("user"));
            List<User> userList = jarr.ToObject<List<User>>();

            foreach (User user in userList)
            {
                Assert.IsTrue(user.created_date > minDate);
            }
        }

        [TestMethod]
        public void Username_Is_Strong()
        {
            JArray jarr = JArray.Parse(userTestExec.Get("user", null));
            List<User> userList = jarr.ToObject<List<User>>();

            foreach (User user in userList)
            {
                Assert.IsTrue(user.email.Length >=6);
            }
        }


    }
}
