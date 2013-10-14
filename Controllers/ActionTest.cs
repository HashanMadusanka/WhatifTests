using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WIF.SJA.API.Models;
using System.Collections.Generic;
namespace WIF.SJA.API.Tests.Controllers
{
    [TestClass]
    public class ActionTest
    {
        TestExecutor actionTestExec;

        [TestInitialize]
        public void InitActionTest()
        {
            actionTestExec = new TestExecutor();
        }

        [TestMethod]
        public void Action_Web_Server_Response_Test()
        {

            var response = actionTestExec.Get("action", null);

            Assert.IsNotNull(response, "Web Server did not send a response for Action");
        }

        [TestMethod]
        public void Action_Valid_Json_Format_Test()
        {
            try
            {
                JArray actions = JArray.Parse(actionTestExec.Get("action", null));


            }
            catch (JsonException e)
            {

                Assert.Fail("Action returned a malformed json string");

            }

        }

        [TestMethod]
        public void Action_Json_Object_Format_Test()
        {


            JObject json_action = JObject.Parse(actionTestExec.Get("action", "(1)"));

            try
            {

                WIF.SJA.API.Models.Action action = JsonConvert.DeserializeObject<WIF.SJA.API.Models.Action>(json_action.ToString());
            }
            catch (JsonSerializationException e)
            {

                Assert.Fail("Returned JSON does not in complience with the model for Action");
            }

        }

        [TestMethod]
        public void ActionOrg_UserOrg_Id_Equality_Test()
        {

            //for all dangers
            JArray actions = JArray.Parse(actionTestExec.Get("action", null));

            List<WIF.SJA.API.Models.Action> action_list = actions.ToObject<List<WIF.SJA.API.Models.Action>>();

            JObject user_role_org = JObject.Parse(actionTestExec.Get("user_role_org", "(1)"));

            User_Role_Org user_org = JsonConvert.DeserializeObject<User_Role_Org>(user_role_org.ToString());


            foreach (WIF.SJA.API.Models.Action action in action_list)
            {

                Assert.IsFalse(action.organization_id != user_org.org_id, "Action organization and User organization doesn't match in Action " + action.id);

            }
        }

        [TestMethod]
        public void All_Fields_Present_In_Action_Test()
        {

            //for all tasks
            JArray actions = JArray.Parse(actionTestExec.Get("action", null));

            List<WIF.SJA.API.Models.Action> action_list = actions.ToObject<List<WIF.SJA.API.Models.Action>>();

            foreach (WIF.SJA.API.Models.Action action in action_list)
            {

                Assert.IsNotNull(action.id, "ID field has no value in Action ");
                Assert.IsNotNull(action.title, "Title field has no value in Action " + action.id);
                Assert.IsNotNull(action.created_by, "Created_by field has no value in Action " + action.id);
                Assert.IsNotNull(action.organization_id, "Organization_id field has no value in Action " + action.id);
                Assert.IsNotNull(action.action_category, "Action_category field has no value in Action " + action.id);
                Assert.IsNotNull(action.organization, "Organization field has no value in Action " + action.id);
                Assert.IsNotNull(action.user, "User field has no value in Action " + action.id);
                Assert.IsNotNull(action.relatedDangerIds, "Related_danger_id field has no value in Action " + action.id);

            }
        }

        [TestMethod]
        public void Action_Uri_Conventions_Test()
        {

            bool is_filtering_allowed = false;

            JArray actions_with_filter = JArray.Parse(actionTestExec.Get("action", "?$filter=id eq 1 "));

            is_filtering_allowed = (actions_with_filter != null);

            Assert.IsFalse(is_filtering_allowed, "Dangerous filterings are allowed in Action");
        }

    }
}
