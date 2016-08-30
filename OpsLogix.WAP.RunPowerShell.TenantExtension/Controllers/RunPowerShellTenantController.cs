//-----------------------------------------------------------------------
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using OpsLogix.WAP.Common;
using OpsLogix.WAP.RunPowerShell.TenantExtension.Models;
using System;

namespace OpsLogix.WAP.RunPowerShell.TenantExtension.Controllers
{
    [RequireHttps]
    [OutputCache(Location = OutputCacheLocation.None)]
    [PortalExceptionHandler]
    public sealed class RunPowerShellTenantController : ExtensionController
    {

        [HttpPost]
        [ActionName("ExecuteRunbook")]
        //public async Task<JsonResult> ExecuteRunbook(string subscriptionId, RunbookParameterModel rb)
        public async Task<JsonResult> ExecuteRunbook(string Upn, string RunbookName, string SubscriptionId, string SelectedVmId = "", string ParamBool = "", string ParamString = "", string ParamInt = "", string ParamDate = "", string ParamStringArray = "")
        {
            RunbookParameterModel rb = new RunbookParameterModel();
            rb.Upn = Upn;
            rb.RunbookName = RunbookName;
            rb.SubscriptionId = SubscriptionId;
            rb.SelectedVmId = SelectedVmId;
            rb.ParamBool = ParamBool;
            rb.ParamString = ParamString;
            rb.ParamInt = ParamInt;
            rb.ParamDate = ParamDate;
            rb.ParamStringArray = ParamStringArray;

            await ClientFactory.RunPowerShellClient.ExecuteRunbook(SubscriptionId, rb.ToApiObject());

            return this.Json("Success");

        }

        [HttpPost]
        public async Task<JsonResult> ListFileShares(string[] subscriptionIds, string[] planIds)
        {
            var runbooks = new List<RunbookModel>();

            foreach(string plan in planIds)
            {

            var fileSharesFromApi = await ClientFactory.RunPowerShellClient.ListFileSharesAsync(subscriptionIds);

            //Only allow a tenant to run runbooks published to the plan(s) they are subscribed to.
            runbooks.AddRange(fileSharesFromApi.Select(d => new RunbookModel(d)).Where(x => x.PlanId == plan));

            }

            return this.JsonDataSet(runbooks);
        }
      
    }
}