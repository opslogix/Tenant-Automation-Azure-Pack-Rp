(function (global, undefined) {
    "use strict";

    var extensions = [{
        name: "RunPowerShellAdminExtension",
        displayName: "Run PowerShell",
        iconUri: "/Content/RunPowerShellAdmin/TestTeam.png",
        iconShowCount: false,
        iconTextOffset: 11,
        iconInvertTextColor: true,
        displayOrderHint: 51
    }];

    global.Shell.Internal.ExtensionProviders.addLocal(extensions);
})(this);