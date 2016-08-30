//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts;
using System.Globalization;
using OpsLogix.WAP.RunPowerShell.Api.ServiceReference.SMAWebservice;
using System.Data.Services.Client;
using System.Net;
using System.Net.Security;
using System.Data.SqlClient;


namespace OpsLogix.WAP.RunPowerShell.Api.Controllers
{
    public class TARunbooksAddController : ApiController
    {        
        public static List<OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts.Runbook> taRunbooks;
        //public static DateTime LatestFileServer;

        static TARunbooksAddController()
        {

        }

        [HttpPost]
        public void TARunbooksAdd(Data data)
        {

            string ParamString = "";
            string ParamInt = "";
            string ParamStringArray = "";
            string ParamDate = "";
            string ParamBool = "";
            string ParamVMs = "";

            string ParamStringChkBox = "0";
            string ParamIntChkBox = "0";
            string ParamStringArrayChkBox = "0";
            string ParamDateChkBox = "0";
            string ParamBoolChkBox = "0";
            string ParamVMsChkBox = "0";

            if (!string.IsNullOrEmpty(data.ParamString)) ParamString = data.ParamString;
            if (!string.IsNullOrEmpty(data.ParamInt)) ParamInt = data.ParamInt;
            if (!string.IsNullOrEmpty(data.ParamStringArray)) ParamStringArray = data.ParamStringArray;
            if (!string.IsNullOrEmpty(data.ParamDate)) ParamDate = data.ParamDate;
            if (!string.IsNullOrEmpty(data.ParamBool)) ParamBool = data.ParamBool;
            if (!string.IsNullOrEmpty(data.ParamVMs)) ParamVMs = data.ParamVMs;

            if (!string.IsNullOrEmpty(data.ParamString)) ParamStringChkBox = "1";
            if (!string.IsNullOrEmpty(data.ParamInt)) ParamIntChkBox = "1";
            if (!string.IsNullOrEmpty(data.ParamStringArray)) ParamStringArrayChkBox = "1";
            if (!string.IsNullOrEmpty(data.ParamDate)) ParamDateChkBox = "1";
            if (!string.IsNullOrEmpty(data.ParamBool)) ParamBoolChkBox = "1";
            if (!string.IsNullOrEmpty(data.ParamVMs)) ParamVMsChkBox = "1";

            System.Configuration.ConnectionStringSettings mySetting = System.Configuration.ConfigurationManager.ConnectionStrings["ResourceProviderDatabase"];

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = mySetting.ConnectionString;
                conn.Open();

                SqlCommand command = new SqlCommand(@"IF NOT EXISTS(SELECT RunbookId,PlanId FROM Runbooks WHERE RunbookId = '" + data.RunbookId + @"' AND PlanId = '" + data.PlanId + @"') INSERT INTO Runbooks VALUES('" + data.RunbookId + 
                                                @"','" + data.RunbookName + 
                                                @"','" + data.RunbookTag +
                                                @"','" + data.PlanId +
                                                @"','" + data.PlanName +
                                                @"','" + ParamString +
                                                @"','" + ParamStringChkBox +
                                                @"','" + ParamInt +
                                                @"','" + ParamIntChkBox +
                                                @"','" + ParamStringArray +
                                                @"','" + ParamStringArrayChkBox +
                                                @"','" + ParamDate +
                                                @"','" + ParamDateChkBox +
                                                @"','" + ParamBool +
                                                @"','" + ParamBoolChkBox +
                                                @"','" + ParamVMs +
                                                @"','" + ParamVMsChkBox +
                                                
                                                @"') ELSE UPDATE Runbooks SET RunbookId='" + data.RunbookId +
                                                @"', RunbookName='" + data.RunbookName +
                                                @"',RunbookTag='" + data.RunbookTag +
                                                @"',PlanId='" + data.PlanId +
                                                @"',PlanName='" + data.PlanName +
                                                @"', ParamString='" + ParamStringChkBox +
                                                @"', ParamStringLabel='" + ParamString +
                                                @"', ParamInt='" + ParamIntChkBox +
                                                @"', ParamIntLabel='" + ParamInt +
                                                @"', ParamStringArray='" + ParamStringArrayChkBox +
                                                @"', ParamStringArrayLabel='" + ParamStringArray +
                                                @"', ParamDate='" + ParamDateChkBox +
                                                @"', ParamDateLabel='" + ParamDate +
                                                @"', ParamBool='" + ParamBoolChkBox +
                                                @"', ParamBoolLabel='" + ParamBool +
                                                @"', ParamVMDropdown='" + ParamVMsChkBox +
                                                @"', ParamVMDropdownLabel='" + ParamVMs +
                                                @"' WHERE RunbookId = '" + data.RunbookId +
                                                @"' AND PlanId = '" + data.PlanId +
                                                @"'", conn);

                command.ExecuteNonQuery();
            }

        }
    }
}