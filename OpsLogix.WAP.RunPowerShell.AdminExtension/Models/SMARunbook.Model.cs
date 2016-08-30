// ---------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------

using OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts;

namespace OpsLogix.WAP.RunPowerShell.AdminExtension.Models
{    
    /// <summary>
    /// This is a model class which contains data contract we send to Controller which then shows up in UI
    /// FileServerModel contains data contract of data which shows up in "File Servers" tab inside RunPowerShell Admin Extension
    /// </summary>
    public class SMARunbookModel
    {
        public string RunbookId { get; set; }

        /// <summary>
        /// Name of the file server
        /// </summary>
        public string RunbookName { get; set; }

        /// <summary>
        /// Total space in File Server (KB, MB, GB) 
        /// </summary>
        public string RunbookTag { get; set; }

        public SMARunbookModel(Runbook smaRunbook)
        {
            this.RunbookId = smaRunbook.RunbookId;
            this.RunbookName = smaRunbook.RunbookName;
            this.RunbookTag = smaRunbook.RunbookTag;
        }
    }
}
