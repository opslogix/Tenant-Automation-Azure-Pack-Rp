//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts;

namespace OpsLogix.WAP.RunPowerShell.TenantExtension.Models
{
    /// <summary>
    /// Data model for domain name tenant view
    /// </summary>    
    public class RunbookModel
    {
        public const string RegisteredStatus = "Registered";

        /// <summary>
        /// Initializes a new instance of the <see cref="RunbookModel" /> class.
        /// </summary>
        public RunbookModel()
        {
        }

                /// <summary>
        /// Initializes a new instance of the <see cref="FileServerModel" /> class.
        /// </summary>
        /// <param name="ProductModel">The domain name from API.</param>
        public RunbookModel(Runbook runbookFromApi)
        {
            this.RunbookId = runbookFromApi.RunbookId;
            this.RunbookName = runbookFromApi.RunbookName;
            this.RunbookTag = runbookFromApi.RunbookTag;
            this.LastJobStatus = runbookFromApi.LastJobStatus;
            this.LastJobStart = runbookFromApi.LastJobStart;
            this.PlanId = runbookFromApi.PlanId;
            this.PlanName = runbookFromApi.PlanName;
            this.SubscriptionId = runbookFromApi.SubscriptionId;
            this.ParamString = runbookFromApi.ParamString;
            this.ParamInt = runbookFromApi.ParamInt;
            this.ParamStringArray = runbookFromApi.ParamStringArray;
            this.ParamDate = runbookFromApi.ParamDate;
            this.ParamBool = runbookFromApi.ParamBool;
            this.ParamVMs = runbookFromApi.ParamVMs;
            this.Type = "Runbook";
        }

        /// <summary>
        /// Covert to the API object.
        /// </summary>
        /// <returns>The API DomainName data contract.</returns>
        public Runbook ToApiObject()
        {
            return new Runbook()
            {
                RunbookId = this.RunbookId,
                RunbookName = this.RunbookName,
                RunbookTag = this.RunbookTag,
                LastJobStatus = this.LastJobStatus,
                LastJobStart = this.LastJobStart,
                PlanId = this.PlanId,
                PlanName = this.PlanName,
                SubscriptionId = this.SubscriptionId,
                ParamString = this.ParamString,
                ParamInt = this.ParamInt,
                ParamStringArray = this.ParamStringArray,
                ParamDate = this.ParamDate,
                ParamBool = this.ParamBool,
                ParamVMs = this.ParamVMs

            };
        }


        public string RunbookId { get; set; }

        public string RunbookName { get; set; }

        public string RunbookTag { get; set; }

        public int LastJobStart { get; set; }

        public string LastJobStatus { get; set; }

        public string PlanId { get; set; }

        public string PlanName { get; set; }

        public string SubscriptionId { get; set; }

        public string ParamString { get; set; }

        public string ParamInt { get; set; }

        public string ParamStringArray { get; set; }

        public string ParamDate { get; set; }

        public string ParamBool { get; set; }

        public string ParamVMs { get; set; }
        public string Type { get; set; }


       
    }
}