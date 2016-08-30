// ---------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------

using System.Runtime.Serialization;

namespace OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts
{

    [DataContract(Namespace = Constants.DataContractNamespaces.Default)]
    public sealed class Data
    {
        [DataMember(Order = 1)]
        public string RunbookName { get; set; }

        [DataMember(Order = 2)]
        public string RunbookId { get; set; }

        [DataMember(Order = 3)]
        public string RunbookTag { get; set; }

        [DataMember(Order = 4)]
        public string PlanName { get; set; }

        [DataMember(Order = 5)]
        public string PlanId { get; set; }

        [DataMember(Order = 6)]
        public string ParamString { get; set; }

        [DataMember(Order = 7)]
        public string ParamInt { get; set; }

        [DataMember(Order = 8)]
        public string ParamStringArray { get; set; }

        [DataMember(Order = 9)]
        public string ParamDate { get; set; }

        [DataMember(Order = 10)]
        public string ParamBool { get; set; }

        [DataMember(Order = 11)]
        public string ParamVMs { get; set; }
        
    }
}
