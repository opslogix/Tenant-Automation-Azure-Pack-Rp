//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Azure.Portal.Configuration;
using OpsLogix.WAP.RunPowerShell.ApiClient;
using OpsLogix.WAP.Base;

namespace OpsLogix.WAP.RunPowerShell.AdminExtension
{
    public static class ClientFactory
    {
        //Get Service Management API endpoint
        private static Uri adminApiUri;

        private static BearerMessageProcessingHandler messageHandler;

        //This client is used to communicate with the Run PowerShell resource provider
        private static Lazy<RunPowerShellClient> runPowerShellRestClient = new Lazy<RunPowerShellClient>(
           () => new RunPowerShellClient(adminApiUri, messageHandler),
           LazyThreadSafetyMode.ExecutionAndPublication);

        //This client is used to communicate with the Admin API
        private static Lazy<AdminManagementClient> adminApiRestClient = new Lazy<AdminManagementClient>(
            () => new AdminManagementClient(adminApiUri, messageHandler),
            LazyThreadSafetyMode.ExecutionAndPublication);

        static ClientFactory()
        {
            adminApiUri = new Uri(OnPremPortalConfiguration.Instance.RdfeAdminUri);
            messageHandler = new BearerMessageProcessingHandler(new WebRequestHandler());
        }

        public static RunPowerShellClient RunPowerShellClient
        {
            get
            {
                return runPowerShellRestClient.Value;
            }
        }

        public static AdminManagementClient AdminManagementClient
        {
            get
            {
                return adminApiRestClient.Value;
            }
        }
    }
}
