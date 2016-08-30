using System;
using System.Runtime.Serialization;


namespace OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts
{
    [DataContract(Namespace = Constants.DataContractNamespaces.Default)]
    public class RunbookParameter
    {
        [DataMember(Order = 1)]
        public string Upn { get; set; }

        [DataMember(Order = 2)]
        public string RunbookName { get; set; }

        [DataMember(Order = 3)]
        public string SubscriptionId { get; set; }

        [DataMember(Order = 4)]
        public string SelectedVmId { get; set; }

        [DataMember(Order = 5)]
        public string ParamBool { get; set; }

        [DataMember(Order = 6)]
        public string ParamString { get; set; }

        [DataMember(Order = 7)]
        public string ParamInt { get; set; }

        [DataMember(Order = 8)]
        public string ParamDate { get; set; }

        [DataMember(Order = 9)]
        public string ParamStringArray { get; set; }

    } 

}
