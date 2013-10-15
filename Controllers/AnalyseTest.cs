using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WIF.SJA.API.Models;
using System.Collections.Generic;

namespace WIF.SJA.API.Tests.Controllers
{
    [TestClass]
    public class AnalyseTest
    {
        TestExecutor analyseTestExec;

        [TestInitialize]
        public void InitAnalyseTest()
        {
            analyseTestExec = new TestExecutor();
        }

        //check if Analyse web server response is null
        [TestMethod]
        public void Analyse_Web_Server_Response_Test()
        {

            var response = analyseTestExec.Get("analyse", null);

            Assert.IsNotNull(response, "Web Server did not send a response for Analyse");
        }

        //check if returned json is a valid json string
        [TestMethod]
        public void Analyse_Valid_Json_Format_Test()
        {

            try
            {
                JArray analyses = JArray.Parse(analyseTestExec.Get("analyse", null));

            }
            catch (JsonException e)
            {

                Assert.Fail("Analyse returned a malformed json string");

            }

        }

        //check if json data is in proper format
        [TestMethod]
        public void Analyse_Json_Object_Format_Test()
        {

            JObject json_analyse = JObject.Parse(analyseTestExec.Get("analyse", "(1)"));

            try
            {

                Analysis analyse = JsonConvert.DeserializeObject<Analysis>(json_analyse.ToString());
            }
            catch (JsonSerializationException e)
            {

                Assert.Fail("Returned JSON does not in complience with the model for Analyse");
            }

        }

        //check if user can view analyses that are not assigned to him
        [TestMethod]
        public void AssignedTo_Id_User_Id_Equality_Test()
        {
            //for all analyses

            JArray analyses = JArray.Parse(analyseTestExec.Get("analyse", null));

            List<Analysis> analyse_list = analyses.ToObject<List<Analysis>>();
                       
            int user_id = 16;

            if (analyse_list.Count > 0)
            {

                foreach (Analysis analyse in analyse_list)
                {

                    Assert.IsFalse(analyse.assigned_to != user_id, "User can view unassigned Analysis in " + analyse.id);

                }
            }
        }

        //check if admins can view analyses that originated out of his organization
        [TestMethod]
        public void AdminOrg_Id_AnalyseOrg_Id_Equality_Test()
        {
            //for all analyses

            JArray analyses = JArray.Parse(analyseTestExec.AdminGet("analyse", null));

            List<Analysis> analyse_list = analyses.ToObject<List<Analysis>>();

            int org_id = 1;

            if (analyse_list.Count > 0)
            {

                foreach (Analysis analyse in analyse_list)
                {
                    List<User_Role_Org> analyse_org = (List<User_Role_Org>)analyse.user.user_role_org;
                    Assert.IsFalse(analyse_org[0].org_id != org_id, "Admin can view unauthorized Analyse " + analyse.id);

                }
            }
        }


        //check if there are null fields
        [TestMethod]
        public void All_Fields_Present_In_Analyse_Test()
        {
            //for all analyses
            JArray analyses = JArray.Parse(analyseTestExec.AdminGet("analyse", null));

            List<Analysis> analyse_list = analyses.ToObject<List<Analysis>>();

            foreach (Analysis analyse in analyse_list)
            {

                Assert.IsNotNull(analyse.id, "ID field has no value in Analyse ");
                Assert.IsNotNull(analyse.probability_new, "Probablity_new field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.probability_old, "Probablity_old field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.consequence_new, "Consequence_new field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.consequence_old, "Consequence_old field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.status_id, "Status_id field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.created_by, "Created_by field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.assigned_to, "Assigned_to field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.category_id, "Category_id field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.created_date, "Created_ate field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.updated_date, "Updated_date field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.analysis_status, "Analysis_status field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.analysis_action, "Analysis_action field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.analysis_danger, "Analysis_danger field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.analysis_participant, "Analysis_participant field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.analysis_task, "Analysis_task field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.analysis_users, "Analysis_users field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.user, "User field has no value in Analyse " + analyse.id);
                Assert.IsNotNull(analyse.category, "Category field has no value in Analyse " + analyse.id);

            }
        }

        //check for unauthorized uri patterns
        [TestMethod]
        public void Analyse_Uri_Conventions_Test()
        {

            bool is_filtering_allowed = false;

            JArray analyses_with_filter = JArray.Parse(analyseTestExec.Get("analyse", "?$filter=id eq 1 "));

            is_filtering_allowed = (analyses_with_filter != null);

            Assert.IsFalse(is_filtering_allowed, "Dangerous filterings are allowed in Analyses");
        }

    }
}
