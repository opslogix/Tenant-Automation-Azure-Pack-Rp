(function (global, undefined) {
    "use strict";

    var extensions = [{
        name: "RunPowerShellTenantExtension",
        displayName: "Run PowerShell",
        iconUri: "/Content/RunPowerShellTenant/RunPowerShellTenant.png",
        iconShowCount: false,
        iconTextOffset: 11,
        iconInvertTextColor: true,
        displayOrderHint: 2 // Display it right after WebSites extension (order 1)
    }];

    global.Shell.Internal.ExtensionProviders.addLocal(extensions);
})(this);