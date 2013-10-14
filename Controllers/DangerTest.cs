using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using WIF.SJA.API.Models;

namespace WIF.SJA.API.Tests.Controllers
{
    [TestClass]
    public class DangerTest
    {
        TestExecutor dangerTestExec;

        [TestInitialize]
        public void InitDangerTest()
        {
            dangerTestExec = new TestExecutor();
        }

        //check if Danger web server response is null
        [TestMethod]
        public void Danger_Web_Server_Response_Test()
        {

            var response = dangerTestExec.Get("danger", null);

            Assert.IsNotNull(response, "Web Server did not send a response for Danger");
        }

        //check if returned json is a valid json string
        [TestMethod]
        public void Danger_Valid_Json_Format_Test()
        {           

            try
            {
                JArray dangers = JArray.Parse(dangerTestExec.Get("danger", null));


            }
            catch (JsonException e)
            {

                Assert.Fail("Danger returned a malformed json string");

            }

        }

        //check if json data is in proper format
        [TestMethod]
        public void Danger_Json_Object_Format_Test()
        {
            JObject json_danger = JObject.Parse(dangerTestExec.Get("danger", "(1)"));

            try
            {

                Danger danger = JsonConvert.DeserializeObject<Danger>(json_danger.ToString());
            }
            catch (JsonSerializationException e)
            {

                Assert.Fail("Returned JSON does not in complience with the model for Danger");
            }

        }

        //check if danger organization id and user organization id is the same
        [TestMethod]
        public void DangerOrg_UserOrg_Id_Equality_Test()
        {

            //for all dangers
            JArray dangers = JArray.Parse(dangerTestExec.Get("danger", null));

            List<Danger> danger_list = dangers.ToObject<List<Danger>>();

            JObject user_role_org = JObject.Parse(dangerTestExec.Get("user_role_org", "(1)"));

            User_Role_Org user_org = JsonConvert.DeserializeObject<User_Role_Org>(user_role_org.ToString());


            foreach (Danger danger in danger_list)
            {

                Assert.IsFalse(danger.organization_id != user_org.org_id, "Danger organization and User organization doesn't match in Danger " + danger.id);

            }
        }

        //check if there are null fields

        [TestMethod]
        public void All_Fields_Present_In_Danger_Test()
        {
            //for all tasks
            JArray dangers = JArray.Parse(dangerTestExec.Get("danger", null));

            List<Danger> danger_list = dangers.ToObject<List<Danger>>();

            foreach (Danger danger in danger_list)
            {

                Assert.IsNotNull(danger.id, "ID field has no value in Danger ");
                Assert.IsNotNull(danger.title, "Title field has no value in Danger " + danger.id);
                Assert.IsNotNull(danger.created_by, "Created_by field has no value in Danger " + danger.id);
                Assert.IsNotNull(danger.organization_id, "Organization_id field has no value in Danger " + danger.id);
                Assert.IsNotNull(danger.danger_category, "Danger_category field has no value in Danger " + danger.id);
                Assert.IsNotNull(danger.organization, "Organization field has no value in Danger " + danger.id);
                Assert.IsNotNull(danger.user, "User field has no value in Danger " + danger.id);
                Assert.IsNotNull(danger.description, "Description field has no value in Danger " + danger.id);
                Assert.IsNotNull(danger.relatedTaskIds, "Related_task_id field has no value in Danger " + danger.id);

            }
        }

        //check for unauthorized uri patterns
        [TestMethod]
        public void Danger_Uri_Conventions_Test()
        {
            bool is_filtering_allowed = false;

            JArray dangers_with_filter = JArray.Parse(dangerTestExec.Get("danger", "?$filter=id eq 1 "));

            is_filtering_allowed = (dangers_with_filter != null);

            Assert.IsFalse(is_filtering_allowed, "Dangerous filterings are allowed in Danger");
        }
    }
}
