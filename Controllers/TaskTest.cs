using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WIF.SJA.API.Models;
using System.Collections.Generic;

namespace WIF.SJA.API.Tests.Controllers
{
    [TestClass]
    public class TaskTest
    {
        TestExecutor taskTestExec;

        [TestInitialize]
        public void InitTaskTest()
        {
            taskTestExec = new TestExecutor();
        }

        [TestMethod]
        public void Task_Web_Server_Response_Test()
        {

            var response = taskTestExec.Get("action", null);

            Assert.IsNotNull(response, "Web Server did not send a response for Task");
        }

        [TestMethod]
        public void Task_Valid_Json_Format_Test()
        {

            try
            {
                JArray tasks = JArray.Parse(taskTestExec.Get("task", null));


            }
            catch (JsonException e)
            {

                Assert.Fail("Task returned a malformed json string");

            }

        }

        [TestMethod]
        public void Task_Json_Object_Format_Test()
        {


            JObject json_task = JObject.Parse(taskTestExec.Get("task", "(1)"));

            try
            {

                Task task = JsonConvert.DeserializeObject<Task>(json_task.ToString());
            }
            catch (JsonSerializationException e)
            {

                Assert.Fail("Returned JSON does not in complience with the model for Task");
            }

        }

        [TestMethod]
        public void TaskOrg_UserOrg_Id_Equality_Test()
        {

            //for all tasks
            JArray tasks = JArray.Parse(taskTestExec.Get("task", null));

            List<Task> task_list = tasks.ToObject<List<Task>>();

            JObject user_role_org = JObject.Parse(taskTestExec.Get("user_role_org", "(1)"));

            User_Role_Org user_org = JsonConvert.DeserializeObject<User_Role_Org>(user_role_org.ToString());


            foreach (Task task in task_list)
            {

                Assert.IsFalse(task.organization_id != user_org.org_id, "Task organization and User organization doesn't match in task " + task.id);

            }
        }

        [TestMethod]
        public void All_Fields_Present_In_Task_Test()
        {

            //for all tasks
            JArray tasks = JArray.Parse(taskTestExec.Get("task", null));

            List<Task> task_list = tasks.ToObject<List<Task>>();

            foreach (Task task in task_list)
            {
                Assert.IsNotNull(task.id, "ID field has no value in Task");
                Assert.IsNotNull(task.title, "Title field has no value in Task " + task.id);
                Assert.IsNotNull(task.created_by, "Created_by field has no value in Task " + task.id);
                Assert.IsNotNull(task.organization_id, "Organization_id field has no value in Task " + task.id);
                Assert.IsNotNull(task.task_category, "Task_category field has no value in Task " + task.id);
                Assert.IsNotNull(task.organization, "Organization field has no value in Task " + task.id);
                Assert.IsNotNull(task.user, "User field has no value in Task " + task.id);
                Assert.IsNotNull(task.usageCount, "Usage_count field has no value in Task " + task.id);

                // bool is_fields_missing = is_created_by_not_present | is_id_not_present | is_org_id_not_present | is_organization_not_present | is_task_category_not_present | is_title_not_present | is_usage_count_not_present | is_user_not_present;

            }
        }

        [TestMethod]
        public void Task_Uri_Conventions_Test()
        {

            bool is_filtering_allowed = false;
            bool is_where_clause_allowed = false;
            bool is_selection_allowed = false;

            JArray tasks_with_filter = JArray.Parse(taskTestExec.Get("task", "?$filter=id eq 1 "));
            //JArray tasks_with_where = JArray.Parse(taskTestExec.Get("task", "?$ "));
            //JArray tasks_with_selection = JArray.Parse(taskTestExec.Get("task", "?$select=title,id"));

            is_filtering_allowed = (tasks_with_filter != null);
            //is_where_clause_allowed=(tasks_with_where!=null);
            //is_selection_allowed=(tasks_with_selection!=null);

            Assert.IsFalse(is_filtering_allowed | is_where_clause_allowed | is_selection_allowed, "Dangerous filterings are allowed");
        }

    }
}
