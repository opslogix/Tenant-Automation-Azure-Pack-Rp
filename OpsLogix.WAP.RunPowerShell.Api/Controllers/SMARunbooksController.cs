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
    public class SMARunbooksController : ApiController
    {        
        public static List<OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook> smaRunbooks;
        //public static DateTime LatestFileServer;

        static SMARunbooksController()
        {

        }

        /// <summary>
        /// Get all the runbooks from SMA
        /// </summary>
        [HttpGet]
        public List<OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook> GetSMARunbookList()
        {
            smaRunbooks = new List<OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook>();
            System.Configuration.ConnectionStringSettings url = System.Configuration.ConfigurationManager.ConnectionStrings["SMAUrl"];
            var api = new OrchestratorApi(new Uri(url.ConnectionString));
            //var api = new OrchestratorApi(new Uri(url.ConnectionString));
            /*
            var api = new OrchestratorApi(new Uri("https://sma.lab.local/00000000-0000-0000-0000-000000000000"));*/
            ((DataServiceContext)api).Credentials = CredentialCache.DefaultCredentials;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

            //var runbooks = api.Runbooks.Where(r => r.Tags.Contains("TenantRunbook")).AsEnumerable();
            var runbooks = api.Runbooks.AsEnumerable();

            foreach (OpsLogix.WAP.RunPowerShell.Api.ServiceReference.SMAWebservice.Runbook runbook in runbooks)
            {
                smaRunbooks.Add(new OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook { RunbookId = runbook.RunbookID.ToString(), RunbookName = runbook.RunbookName, RunbookTag = runbook.Tags });
            }

            return smaRunbooks;
        }

        /// <summary>
        /// Update the list of all the runbooks from SMA
        /// </summary>
        [HttpPut]
        public void UpdateTARunbook(OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook smarunbook)
        {
            if (smarunbook == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.FileServerEmpty);
            }

            var smaRunbook = (from s in smaRunbooks where s.RunbookId == smarunbook.RunbookId select s).FirstOrDefault();

            if (smaRunbook != null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.FileServerNotFound, smarunbook.RunbookName);
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, message);
            }
            else
            {
               smaRunbook.RunbookName = smarunbook.RunbookName;
                smaRunbook.RunbookId = smarunbook.RunbookId;
                smaRunbook.RunbookTag = smarunbook.RunbookTag;
            }
        }

    }
}