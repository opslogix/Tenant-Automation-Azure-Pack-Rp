using OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts;
using System;

public class RunbookParameterModel
{

    public string Upn { get; set; }
    public string RunbookName { get; set; }

    public string SubscriptionId { get; set; }

    public string SelectedVmId { get; set; }

    public string ParamBool { get; set; }

    public string ParamString { get; set; }

    public string ParamInt { get; set; }

    public string ParamDate { get; set; }

    public string ParamStringArray { get; set; }

    

    



    public RunbookParameter ToApiObject()
    {

        return new RunbookParameter()

        {
            Upn = this.Upn,
            RunbookName = this.RunbookName,
            SubscriptionId = this.SubscriptionId,
            SelectedVmId = this.SelectedVmId,
            ParamBool = this.ParamBool,
            ParamString = this.ParamString,
            ParamInt = this.ParamInt,
            ParamDate = this.ParamDate,
            ParamStringArray = this.ParamStringArray

        };

    }

}
