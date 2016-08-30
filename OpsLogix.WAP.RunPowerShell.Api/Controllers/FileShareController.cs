using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data.Services.Client;
using System.Net;
using System.Net.Security;
using System.Threading.Tasks;
using OpsLogix.WAP.RunPowerShell.Api.ServiceReference.SMAWebservice;
using IO = System.IO;
using System.Linq;
using System.Web.Http;
using OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts;
using System.Data.SqlClient;

namespace OpsLogix.WAP.RunPowerShell.Api.Controllers
{
    public class FileShareController : ApiController
    {
        public static List<OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook> fileShares = new List<OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook>();
       
        [HttpGet]
        public List<OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook> ListFileShares(string subscriptionId)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw new ArgumentNullException(subscriptionId);
            }

            //Updates the list of data (Creates fake data)
            fileShares.Clear();
            FileShareController.PopulateFileShareForSubscription(subscriptionId);
            

            var shares = from share in fileShares
                         where string.Equals(share.SubscriptionId, subscriptionId, StringComparison.OrdinalIgnoreCase)
                         select share;

            return shares.ToList();
        }


        [HttpPost]

        [System.Web.Http.ActionName("executerunbook")]

        public void ExecuteRunbook(string subscriptionId, OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.RunbookParameter rbParameter)
        {
            System.Configuration.ConnectionStringSettings url = System.Configuration.ConfigurationManager.ConnectionStrings["SMAUrl"];
            
             /*
            var api = new OrchestratorApi(new Uri("https://sma.lab.local/00000000-0000-0000-0000-000000000000"));*/
            //var api = new OrchestratorApi(new Uri(url.ConnectionString));
            var api = new OrchestratorApi(new Uri(url.ConnectionString));
           
            ((DataServiceContext)api).Credentials = CredentialCache.DefaultCredentials;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });


            var runbook = api.Runbooks.Where(r => r.RunbookName == rbParameter.RunbookName).AsEnumerable().FirstOrDefault();
            if (runbook == null) return;


            var runbookParams = new List<NameValuePair>

            { 

                new NameValuePair() {Name = "Upn", Value = rbParameter.Upn},
                new NameValuePair() {Name = "RunbookName", Value = rbParameter.RunbookName},
                new NameValuePair() {Name = "SubscriptionId", Value = rbParameter.SubscriptionId},
                new NameValuePair() {Name = "SelectedVmId", Value = rbParameter.SelectedVmId},
                new NameValuePair() {Name = "ParamBool", Value = rbParameter.ParamBool}, 
                new NameValuePair() {Name = "ParamString", Value = rbParameter.ParamString}, 
                new NameValuePair() {Name = "ParamInt", Value = rbParameter.ParamInt}, 
                new NameValuePair() {Name = "ParamDate", Value = rbParameter.ParamDate}, 
                new NameValuePair() {Name = "ParamStringArray", Value = rbParameter.ParamStringArray}
            
            

            };



            OperationParameter operationParameters = new BodyOperationParameter("parameters", runbookParams);
            var uriSma = new Uri(string.Concat(api.Runbooks, string.Format("(guid'{0}')/{1}", runbook.RunbookID, "Start")), UriKind.Absolute);
            var jobIdValue = api.Execute<Guid>(uriSma, "POST", true, operationParameters) as QueryOperationResponse<Guid>;
            if (jobIdValue == null) return;


            var jobId = jobIdValue.Single();
            Task.Factory.StartNew(() => QueryJobCompletion(jobId));
        }



        private void QueryJobCompletion(Guid jobId)
        {



        }


        //This code is executed when a subscription for the RUN POWERSHELL resource provider is added to a user
        internal static void PopulateFileShareForSubscription(string subscriptionId)
        {

            System.Configuration.ConnectionStringSettings mySetting = System.Configuration.ConfigurationManager.ConnectionStrings["ResourceProviderDatabase"];

            //taRunbooks = new List<TARunbook>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = mySetting.ConnectionString;
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT Id,RunbookId,RunbookName,RunbookTag,PlanId,PlanName, ParamStringLabel, ParamIntLabel, ParamStringArrayLabel, ParamDateLabel, ParamBoolLabel, ParamVMDropdownLabel FROM Runbooks", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        fileShares.Add(
                    new OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook
                    {
                        RunbookId = reader["RunbookId"].ToString(),
                        RunbookName = reader["RunbookName"].ToString(),
                        RunbookTag = reader["RunbookTag"].ToString(),
                        PlanId = reader["PlanId"].ToString(),
                        PlanName = reader["PlanName"].ToString(),
                        SubscriptionId = subscriptionId,
                         ParamString= reader["ParamStringLabel"].ToString(),
                         ParamInt= reader["ParamIntLabel"].ToString(),
                         ParamStringArray= reader["ParamStringArrayLabel"].ToString(),
                         ParamDate= reader["ParamDateLabel"].ToString(),
                         ParamBool= reader["ParamBoolLabel"].ToString(),
                         ParamVMs= reader["ParamVMDropdownLabel"].ToString()
                    }
                             );
                       // taRunbooks.Add(new TARunbook { RunbookId = reader["RunbookId"].ToString(), RunbookName = reader["RunbookName"].ToString(), RunbookTag = reader["RunbookTag"].ToString(), PlanId = reader["PlanId"].ToString(), PlanName = reader["PlanName"].ToString() });
                    }
                }
            }
            
          
            
        }
    }
}
