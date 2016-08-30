//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts;
using System.Globalization;
using OpsLogix.WAP.RunPowerShell.Api.ServiceReference.SMAWebservice;
using System.Data.Services.Client;
using System.Net;
using System.Net.Security;
using System.Data.SqlClient;


namespace OpsLogix.WAP.RunPowerShell.Api.Controllers
{
    public class TARunbooksController : ApiController
    {        
        public static List<OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook> taRunbooks;
        //public static DateTime LatestFileServer;

        static TARunbooksController()
        {

          
            
        }



        [HttpGet]
        public List<OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook> GetTARunbookList()
        {

            System.Configuration.ConnectionStringSettings mySetting = System.Configuration.ConfigurationManager.ConnectionStrings["ResourceProviderDatabase"];

            taRunbooks = new List<OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString =  mySetting.ConnectionString;
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT Id,RunbookId,RunbookName,RunbookTag,PlanId,PlanName FROM Runbooks", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        taRunbooks.Add(new OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook { RunbookId = reader["RunbookId"].ToString(), RunbookName = reader["RunbookName"].ToString(), RunbookTag = reader["RunbookTag"].ToString(), PlanId = reader["PlanId"].ToString(), PlanName = reader["PlanName"].ToString() });
                    }
                }
            }

            return taRunbooks;
        }


        [HttpPut]
        public void UpdateTARunbook(OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook tarunbook)
        {
            if (tarunbook == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.FileServerEmpty);
            }

            var taRunbook = (from s in taRunbooks where s.RunbookId == tarunbook.RunbookId select s).FirstOrDefault();

            if (taRunbook != null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.FileServerNotFound, tarunbook.RunbookName);
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, message);
            }
            else
            {
                taRunbook.RunbookName = tarunbook.RunbookName;
                taRunbook.RunbookId = tarunbook.RunbookId;
                taRunbook.RunbookTag = tarunbook.RunbookTag;
                taRunbook.PlanId = tarunbook.PlanId;
                taRunbook.PlanName = tarunbook.PlanName;
            }
        }

    }
}