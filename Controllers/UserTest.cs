using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using WIF.SJA.API.Models;
using Newtonsoft.Json;
using System.IO;

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

        //check for null username
        [TestMethod]
        public void Username_Is_Not_Null()
        {
            JArray jarr = JArray.Parse(userTestExec.Get("user", null));
            List<User> userList = jarr.ToObject<List<User>>();

            foreach (User user in userList)
            {
                Assert.IsNotNull(user.email);
            }
        }

        //check if user created date is valid
        [TestMethod]
        public void Created_Date_Is_Valid()
        {
            DateTime minDate = DateTime.Parse("2013/07/01");
            JArray jarr = JArray.Parse(userTestExec.Get("user", null));
            List<User> userList = jarr.ToObject<List<User>>();

            foreach (User user in userList)
            {
                Assert.IsTrue(user.created_date > minDate);
            }
        }

        //check if username is strong
        [TestMethod]
        public void Username_Is_Strong()
        {
            JArray jarr = JArray.Parse(userTestExec.Get("user", null));
            List<User> userList = jarr.ToObject<List<User>>();

            foreach (User user in userList)
            {
                Assert.IsTrue(user.email.Length >= 6);
            }
        }

        //check if users can view other users
        [TestMethod]
        public void User_Can_View_All_Users_Test()
        {
            JArray jarr = JArray.Parse(userTestExec.Get("user", null));

            if (jarr.Count > 1)
            {
                Assert.Fail("Unauthorized User views allowed for normal user");
            }
        }

        //check if admins can view users of other organizations
        [TestMethod]
        public void Admin_Can_View_Users_In_Other_Organizations_Test()
        {
            JArray jarr = JArray.Parse(userTestExec.AdminGet("user", null));

            JArray admin_user = JArray.Parse(userTestExec.AdminGet("user", "?$filter=email eq 'test'"));

            User admin = JsonConvert.DeserializeObject<User>(admin_user[0].ToString());

            List<User_Role_Org> admin_org = (List<User_Role_Org>)admin.user_role_org;

            int admin_org_id = admin_org[0].org_id;

            List<User> userList = jarr.ToObject<List<User>>();

            foreach (User user in userList)
            {
                JObject user_role_org = JObject.Parse(userTestExec.AdminGet("user_role_org", "(" + user.id + ")"));

                User_Role_Org user_org = JsonConvert.DeserializeObject<User_Role_Org>(user_role_org.ToString());

                int user_org_id = user_org.org_id;

                if (admin_org_id != user_org_id)
                {

                    Assert.Fail("Unauthorized access to Admin to view Users in other Organizations");
                }

            }
        }

        //check if admins/users can view data other than what is defined in the models
        [TestMethod]
        public void User_Pass_And_Salt_Cannot_Access_Test() {


            //test string
          // string user_json= @"[{idxxx: 16,email: ""hashanw@99x.lk"",locale_id: 1,full_name: ""Hashan"",locale: {id: 1,code: ""en"",name: ""English""},user_role_org: [{id: 16,user_id: 16,role_id: 2,org_id: 1,last_accessed: ""0001-01-01T00:00:00"",organization: {id: 1,name: ""Happy Inc"",org_category: [ ]},role: {id: 2,name: ""user""}}]}]";

            JArray admin_users = JArray.Parse(userTestExec.AdminGet("user", "?$filter=email eq 'test'"));

            JArray users = JArray.Parse(userTestExec.Get("user", "?$filter=email eq 'hashanw@99x.lk'"));

            JsonSerializer jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });

            try { 
            
                List<User> admin=jsonSerializer.Deserialize<List<User>>(new JsonTextReader(new StringReader(admin_users.ToString())));
                List<User> user = jsonSerializer.Deserialize<List<User>>(new JsonTextReader(new StringReader(users.ToString())));

                Assert.Fail("Users can access unauthorized content on the json response");
            }
            catch(JsonSerializationException e){ 

            }
        
        }

        //check for unauthorized uri patterns
        [TestMethod]
        public void User_Uri_Conventions_Test()
        {

            bool is_filtering_allowed = false;

            JArray users_with_filter = JArray.Parse(userTestExec.Get("user", "?$filter=id eq 1 "));

            is_filtering_allowed = (users_with_filter != null);

            Assert.IsFalse(is_filtering_allowed, "Dangerous filterings are allowed in User");
        }

    }
}
