// ---------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------

using System.Runtime.Serialization;

namespace OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts
{
    [CollectionDataContract(Name = "Runbooks", ItemName = "Runbook", Namespace = Constants.DataContractNamespaces.Default)]
    public class RunbookList : IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets the structure that contains extra data.
        /// </summary>
        public ExtensionDataObject ExtensionData { get; set; }
    }
}
