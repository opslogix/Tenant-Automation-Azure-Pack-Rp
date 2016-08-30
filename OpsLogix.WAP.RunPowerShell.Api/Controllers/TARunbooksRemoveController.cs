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
    public class TARunbooksRemoveController : ApiController
    {        
        public static List<OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook> taRunbooks;
        //public static DateTime LatestFileServer;

        static TARunbooksRemoveController()
        {

            
            
        }



        [HttpPost]
        public void TARunbooksRemove(Data data)
        {
            if (data == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.FileServerEmpty);
            }

            System.Configuration.ConnectionStringSettings mySetting = System.Configuration.ConfigurationManager.ConnectionStrings["ResourceProviderDatabase"];


            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = mySetting.ConnectionString;
                conn.Open();

                  SqlCommand command = new SqlCommand("DELETE FROM Runbooks WHERE RunbookId = '" + data.RunbookId + @"' AND PlanId = '" + data.PlanId + @"'", conn);

                  command.ExecuteNonQuery();
            }

        }
    }
}