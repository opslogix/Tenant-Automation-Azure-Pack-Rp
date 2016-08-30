//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Azure.Portal.Configuration;
using OpsLogix.WAP.RunPowerShell.ApiClient;

namespace OpsLogix.WAP.RunPowerShell.TenantExtension
{
    public static class ClientFactory
    {
        //Get Service Management API endpoint
        private static Uri tenantApiUri;

        private static BearerMessageProcessingHandler messageHandler;

        //This client is used to communicate with the Run PowerShell resource provider
        private static Lazy<RunPowerShellClient> runPowerShellRestClient = new Lazy<RunPowerShellClient>(
           () => new RunPowerShellClient(tenantApiUri, messageHandler),
           LazyThreadSafetyMode.ExecutionAndPublication);

        static ClientFactory()
        {
            tenantApiUri = new Uri(AppManagementConfiguration.Instance.RdfeUnifiedManagementServiceUri);
            messageHandler = new BearerMessageProcessingHandler(new WebRequestHandler());
        }

        public static RunPowerShellClient RunPowerShellClient
        {
            get
            {
                return runPowerShellRestClient.Value;
            }
        }
    }
}
