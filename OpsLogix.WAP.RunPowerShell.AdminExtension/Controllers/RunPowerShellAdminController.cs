// ---------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Azure.Portal.Configuration;
using OpsLogix.WAP.Base.DataContracts;
using OpsLogix.WAP.RunPowerShell.AdminExtension.Models;
using OpsLogix.WAP.RunPowerShell.ApiClient;
using OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts;
using OpsLogix.WAP.Common;
using OpsLogix.WAP.Base;


namespace OpsLogix.WAP.RunPowerShell.AdminExtension.Controllers
{
    [RequireHttps]
    [OutputCache(Location = OutputCacheLocation.None)]
    [PortalExceptionHandler]
    public sealed class RunPowerShellAdminController : ExtensionController
    {
        private static readonly string adminAPIUri = OnPremPortalConfiguration.Instance.RdfeAdminUri;
        //This model is used to show registered resource provider information
        public EndpointModel RunPowerShellServiceEndPoint { get; set; }

        /// <summary>
        /// Gets the admin settings.
        /// </summary>
        [HttpPost]
        [ActionName("AdminSettings")]
        public async Task<JsonResult> GetAdminSettings()
        {
            try
            {
                var resourceProvider = await ClientFactory.AdminManagementClient.GetResourceProviderAsync
                                                            (RunPowerShellClient.RegisteredServiceName, Guid.Empty.ToString());

                this.RunPowerShellServiceEndPoint = EndpointModel.FromResourceProviderEndpoint(resourceProvider.AdminEndpoint);
                return this.JsonDataSet(this.RunPowerShellServiceEndPoint);
            }
            catch (ManagementClientException managementException)
            {
                // 404 means the Run PowerShell resource provider is not yet configured, return an empty record.
                if (managementException.StatusCode == HttpStatusCode.NotFound)
                {
                    return this.JsonDataSet(new EndpointModel());
                }

                //Just throw if there is any other type of exception is encountered
                throw;
            }
        }

        /// <summary>
        /// Update admin settings => Register Resource Provider
        /// </summary>
        /// <param name="newSettings">The new settings.</param>
        [HttpPost]
        [ActionName("UpdateAdminSettings")]
        public async Task<JsonResult> UpdateAdminSettings(EndpointModel newSettings)
        {
            this.ValidateInput(newSettings);

            ResourceProvider runPowerShellResourceProvider;
            string errorMessage = string.Empty;

            try
            {
                //Check if resource provider is already registered or not
                runPowerShellResourceProvider = await ClientFactory.AdminManagementClient.GetResourceProviderAsync(RunPowerShellClient.RegisteredServiceName, Guid.Empty.ToString());
            }
            catch (ManagementClientException exception)
            {
                // 404 means the Run PowerShell resource provider is not yet configured, return an empty record.
                if (exception.StatusCode == HttpStatusCode.NotFound)
                {
                    runPowerShellResourceProvider = null;
                }
                else
                {
                    //Just throw if there is any other type of exception is encountered
                    throw;
                }
            }

            if (runPowerShellResourceProvider != null)
            {
                //Resource provider already registered so lets update endpoint
                runPowerShellResourceProvider.AdminEndpoint = newSettings.ToAdminEndpoint();
                runPowerShellResourceProvider.TenantEndpoint = newSettings.ToTenantEndpoint();
                runPowerShellResourceProvider.NotificationEndpoint = newSettings.ToNotificationEndpoint();
                runPowerShellResourceProvider.UsageEndpoint = newSettings.ToUsageEndpoint();
            }
            else
            {
                //Resource provider not registered yet so lets register new one now
                runPowerShellResourceProvider = new ResourceProvider()
                {
                    Name = RunPowerShellClient.RegisteredServiceName,
                    DisplayName = "Tenant Automation",
                    InstanceDisplayName = RunPowerShellClient.RegisteredServiceName + " Instance",
                    Enabled = true,
                    PassThroughEnabled = true,
                    AllowAnonymousAccess = false,
                    AdminEndpoint = newSettings.ToAdminEndpoint(),
                    TenantEndpoint = newSettings.ToTenantEndpoint(),
                    NotificationEndpoint = newSettings.ToNotificationEndpoint(),
                    UsageEndpoint = newSettings.ToUsageEndpoint(),
                    MaxQuotaUpdateBatchSize = 3 // Check link http://technet.microsoft.com/en-us/library/dn520926(v=sc.20).aspx
                };
            }

            var testList = new ResourceProviderVerificationTestList()
                               {
                                   new ResourceProviderVerificationTest()
                                   {
                                       TestUri = new Uri(RunPowerShellAdminController.adminAPIUri + RunPowerShellClient.AdminSettings),
                                       IsAdmin = true
                                   }
                               };
            try
            {
                // Resource Provider Verification to ensure given endpoint and username/password is correct
                // Only validate the admin RP since we don't have a tenant subscription to do it.
                var result = await ClientFactory.AdminManagementClient.VerifyResourceProviderAsync(runPowerShellResourceProvider, testList);
                if (result.HasFailures)
                {
                    throw new HttpException("Invalid endpoint or bad username/password");
                }
            }
            catch (ManagementClientException ex)
            {
                throw new HttpException("Invalid endpoint or bad username/password " + ex.Message.ToString());
            }

            //Finally Create Or Update resource provider
            Task<ResourceProvider> rpTask = (string.IsNullOrEmpty(runPowerShellResourceProvider.Name) || String.IsNullOrEmpty(runPowerShellResourceProvider.InstanceId))
                                                ? ClientFactory.AdminManagementClient.CreateResourceProviderAsync(runPowerShellResourceProvider)
                                                : ClientFactory.AdminManagementClient.UpdateResourceProviderAsync(runPowerShellResourceProvider.Name, runPowerShellResourceProvider.InstanceId, runPowerShellResourceProvider);

            try
            {
                await rpTask;
            }
            catch (ManagementClientException e)
            {
                throw e;
            }

            return this.Json(newSettings);
        }

        /// <summary>
        /// Gets all File ATRunbooks.
        /// </summary>
        [HttpPost]
        [ActionName("TARunbooks")]
        public async Task<JsonResult> GetAllTARunbooks()
        {
            try
            {
                var taRunbooks = await ClientFactory.RunPowerShellClient.GetTARunbookListAsync();
                var taRunbooksModel = taRunbooks.Select(d => new TARunbookModel(d)).ToList();
                return this.JsonDataSet(taRunbooksModel);
            }
            catch (HttpRequestException)
            {
                // Returns an empty collection if the HTTP request to the API fails
                return this.JsonDataSet(new RunbookList());
            }
        }

        [HttpPost]
        [ActionName("AddTARunbook")]
        public async Task<JsonResult> AddTARunbook(Data data)
        {
            await ClientFactory.RunPowerShellClient.TARunbooksAddAsync(data);
            return this.Json("Success");

        }

        [HttpPost]
        [ActionName("RemoveTARunbook")]
        public async Task<JsonResult> RemoveTARunbook(Data data)
        {
            await ClientFactory.RunPowerShellClient.TARunbooksRemoveAsync(data);
            return this.Json("Success");

        }



        [HttpPost]
        [ActionName("SMARunbooks")]
        public async Task<JsonResult> GetAllSMARunbooks()
        {
            try
            {
                var smaRunbooks = await ClientFactory.RunPowerShellClient.GetSMARunbookListAsync();
                var smaRunbooksModel = smaRunbooks.Select(d => new SMARunbookModel(d)).ToList();
                return this.JsonDataSet(smaRunbooksModel);
            }
            catch (HttpRequestException)
            {
                // Returns an empty collection if the HTTP request to the API fails
                return this.JsonDataSet(new RunbookList());
            }
        }

       

        private void ValidateInput(EndpointModel newSettings)
        {
            if (newSettings == null)
            {
                throw new ArgumentNullException("newSettings");
            }

            if (String.IsNullOrEmpty(newSettings.EndpointAddress))
            {
                throw new ArgumentNullException("EndpointAddress");
            }

            if (String.IsNullOrEmpty(newSettings.Username))
            {
                throw new ArgumentNullException("Username");
            }

            if (String.IsNullOrEmpty(newSettings.Password))
            {
                throw new ArgumentNullException("Password");
            }
        }
    }

}