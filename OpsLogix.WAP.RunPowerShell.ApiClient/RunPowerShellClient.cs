// ---------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts;

namespace OpsLogix.WAP.RunPowerShell.ApiClient
{
    /// <summary>
    /// This is client of Run PowerShell Resource Provider 
    /// This client is used by admin and tenant extensions to make call to Run PowerShell Resource Provider
    /// In real world you should have seperate clients of admin and tenant extensions    
    /// </summary>
    public class RunPowerShellClient
    {        
        public const string RegisteredServiceName = "runpowershell";
        public const string RegisteredPath = "services/" + RegisteredServiceName;
        public const string AdminSettings = RegisteredPath + "/settings";
        public const string AdminProducts = RegisteredPath + "/products";
        public const string AdminFileServers = RegisteredPath + "/fileservers";
        public const string AdminTARunbooks = RegisteredPath + "/tarunbooks";
        public const string AdminTARunbooksAdd = RegisteredPath + "/tarunbooksadd";
        public const string AdminTARunbooksRemove = RegisteredPath + "/tarunbooksremove";
        public const string AdminSMARunbooks = RegisteredPath + "/smarunbooks";
        public const string AdminTASubscriptions = RegisteredPath + "/tasubscriptions";
        public const string FileShares = "{0}/" + RegisteredPath + "/fileshares";


        public const string TenantExecuteRunbook = "{0}/" + RegisteredPath + "/executerunbook";

        public Uri BaseEndpoint { get; set; }
        public HttpClient httpClient;


        public async Task ExecuteRunbook(string subscriptionId, RunbookParameter rb)
        {
            var requestUrl =
           this.CreateRequestUri(
           string.Format(CultureInfo.InvariantCulture,
            TenantExecuteRunbook, subscriptionId));
            await this.PostAsync<RunbookParameter>(requestUrl, rb);
        }


        /// <summary>
        /// This constructor takes BearerMessageProcessingHandler which reads token as attach to each request
        /// </summary>
        /// <param name="baseEndpoint"></param>
        /// <param name="handler"></param>
        public RunPowerShellClient(Uri baseEndpoint, MessageProcessingHandler handler)
        {
            if (baseEndpoint == null) 
            {
                throw new ArgumentNullException("baseEndpoint"); 
            }

            this.BaseEndpoint = baseEndpoint;

            this.httpClient = new HttpClient(handler);
        }

        public RunPowerShellClient(Uri baseEndpoint, string bearerToken, TimeSpan? timeout = null)
        {
            if (baseEndpoint == null) 
            { 
                throw new ArgumentNullException("baseEndpoint"); 
            }

            this.BaseEndpoint = baseEndpoint;

            this.httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            if (timeout.HasValue)
            {
                this.httpClient.Timeout = timeout.Value;
            }
        }
       
        #region Admin APIs
        /// <summary>
        /// GetAdminSettings returns Run PowerShell Resource Provider endpoint information if its registered with Admin API
        /// </summary>
        /// <returns></returns>
        public async Task<AdminSettings> GetAdminSettingsAsync()
        {
            var requestUrl = this.CreateRequestUri(RunPowerShellClient.AdminSettings);

            // For simplicity, we make a request synchronously.
            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<AdminSettings>();
        }

        /// <summary>
        /// UpdateAdminSettings registers Run PowerShell Resource Provider endpoint information with Admin API
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAdminSettingsAsync(AdminSettings newSettings)
        {
            var requestUrl = this.CreateRequestUri(RunPowerShellClient.AdminSettings);
            var response = await this.httpClient.PutAsJsonAsync<AdminSettings>(requestUrl.ToString(), newSettings);
            response.EnsureSuccessStatusCode();
        }

        public async Task TARunbooksAddAsync(Data data)
        {
            var requestUrl = this.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, RunPowerShellClient.AdminTARunbooksAdd));
            await this.PostAsync<Data>(requestUrl, data);
        }

        public async Task TARunbooksRemoveAsync(Data data)
        {
            var requestUrl = this.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, RunPowerShellClient.AdminTARunbooksRemove));
            await this.PostAsync<Data>(requestUrl, data);
        }
        
        public async Task<List<Subscription>> GetSubscriptionListAsync()
        {
            var requestUrl = this.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, RunPowerShellClient.AdminTASubscriptions));

            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Subscription>>();
        }

        public async Task<List<Runbook>> GetTARunbookListAsync()
        {
            var requestUrl = this.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, RunPowerShellClient.AdminTARunbooks));

            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Runbook>>();
        }

        public async Task<List<Runbook>> GetSMARunbookListAsync()
        {
            var requestUrl = this.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, RunPowerShellClient.AdminSMARunbooks));

            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Runbook>>();
        }

    
        public async Task UpdateTARunbookAsync(Runbook taRunbook)
        {
            var requestUrl = this.CreateRequestUri(RunPowerShellClient.AdminTARunbooks);
            var response = await this.httpClient.PutAsJsonAsync<Runbook>(requestUrl.ToString(), taRunbook);
            response.EnsureSuccessStatusCode();
        }


        #endregion

        #region Tenant APIs

        public async Task<List<Runbook>> ListFileSharesAsync(string[] subscriptionId = null)
        {
            List<Runbook> listfileshare = new List<Runbook>();

            var requestUrl = this.CreateRequestUri(RunPowerShellClient.CreateUri(subscriptionId[0]));
                try
                {
                    var tmp = await this.GetAsync<List<Runbook>>(requestUrl);
                    foreach (Runbook f in tmp)
                    {
                        listfileshare.Add(f);
                    }
                }
                catch (Exception u)
                {

                }
            return listfileshare;

        }
        
        #endregion

        #region Private Methods
        /// <summary>
        /// Common method for making GET calls
        /// </summary>        
        private async Task<T> GetAsync<T>(Uri requestUrl)
        {         
            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<T>();
        }

        /// <summary>
        /// Common method for making POST calls
        /// </summary>        
        private async Task PostAsync<T>(Uri requestUrl, T content)
        {            
            var response = await this.httpClient.PostAsXmlAsync<T>(requestUrl.ToString(), content);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Common method for making PUT calls
        /// </summary>        
        private async Task PutAsync<T>(Uri requestUrl, T content)
        {            
            var response = await this.httpClient.PutAsJsonAsync<T>(requestUrl.ToString(), content);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Common method for making Request Uri's
        /// </summary>        
        private Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(this.BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }

        private static string CreateUri(string subscriptionId)
        {
            return string.Format(CultureInfo.InvariantCulture, RunPowerShellClient.FileShares, subscriptionId);
        }
        #endregion
    }
}
