// ---------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------

using System.Web.Http;

namespace OpsLogix.WAP.RunPowerShell.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "AdminSettings",
               routeTemplate: "admin/settings",
               defaults: new { controller = "AdminSettings" });

            config.Routes.MapHttpRoute(
                name: "AdminProducts",
                routeTemplate: "admin/products",
                defaults: new { controller = "Products" });

            config.Routes.MapHttpRoute(
                name: "AdminFileServers",
                routeTemplate: "admin/fileservers",
                defaults: new { controller = "FileServers" });

            config.Routes.MapHttpRoute(
               name: "AdminTARunbooks",
               routeTemplate: "admin/tarunbooks",
               defaults: new { controller = "TARunbooks" });

            config.Routes.MapHttpRoute(
               name: "AdminTARunbooksAdd",
               routeTemplate: "admin/tarunbooksadd",
               defaults: new { controller = "TARunbooksAdd" });

            config.Routes.MapHttpRoute(
               name: "AdminTARunbooksRemove",
               routeTemplate: "admin/tarunbooksremove",
               defaults: new { controller = "TARunbooksRemove" });

            config.Routes.MapHttpRoute(
               name: "AdminSMARunbooks",
               routeTemplate: "admin/smarunbooks",
               defaults: new { controller = "SMARunbooks" });

            config.Routes.MapHttpRoute(
               name: "RunPowerShellQuota",
               routeTemplate: "admin/quota",
               defaults: new { controller = "Quota" });

            config.Routes.MapHttpRoute(
               name: "RunPowerShellDefaultQuota",
               routeTemplate: "admin/defaultquota",
               defaults: new { controller = "Quota" });

            config.Routes.MapHttpRoute(
               name: "TASubscriptions",
               routeTemplate: "admin/tasubscriptions",
               defaults: new { controller = "TASubscriptions" });

            config.Routes.MapHttpRoute(
               name: "FileShares",
               routeTemplate: "subscriptions/{subscriptionId}/fileshares",
               defaults: new { controller = "FileShare" });

            config.Routes.MapHttpRoute(
               name: "Usage",
               routeTemplate: "usage",
               defaults: new { controller = "Usage" });



            config.Routes.MapHttpRoute(
               name: "ExecuteRunbook",
               routeTemplate: "subscriptions/{subscriptionId}/executerunbook",
               defaults: new { controller = "FileShare" });

        }
    }
}
