// ---------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------

using OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts;

namespace OpsLogix.WAP.RunPowerShell.AdminExtension.Models
{    
    public class TARunbookModel
    {
        public string RunbookId { get; set; }
        public string RunbookName { get; set; }
        public string RunbookTag { get; set; }
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public string Type {get;set; }

        public TARunbookModel(Runbook taRunbook)
        {
            this.RunbookId = taRunbook.RunbookId;
            this.RunbookName = taRunbook.RunbookName;
            this.RunbookTag = taRunbook.RunbookTag;
            this.PlanId = taRunbook.PlanId;
            this.PlanName = taRunbook.PlanName;
            this.Type = "TARunbook";
        }
    }
}
