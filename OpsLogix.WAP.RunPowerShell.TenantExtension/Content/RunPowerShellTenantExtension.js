/// <reference path="scripts/runPowerShellTenant.createwizard.js" />
/// <reference path="scripts/runPowerShellTenant.controller.js" />
/*globals window,jQuery,Shell, RunPowerShellTenantExtension, Exp*/

(function ($, global, undefined) {
    "use strict";

    var resources = [],
        RunPowerShellTenantExtensionActivationInit,
        navigation,
        serviceName = "runPowerShell";

    function onNavigateAway() {
        Exp.UI.Commands.Contextual.clear();
        Exp.UI.Commands.Global.clear();
        Exp.UI.Commands.update();
    }

    function loadSettingsTab(extension, renderArea, renderData) {
        global.RunPowerShellTenantExtension.SettingsTab.loadTab(renderData, renderArea);
    }


    function fileSharesTab(extension, renderArea, renderData) {
        global.RunPowerShellTenantExtension.FileSharesTab.loadTab(renderData, renderArea);
    }

    function vmsTab(extension, renderArea, renderData) {
        global.RunPowerShellTenantExtension.vmsTab.loadTab(renderData, renderArea);
    }

    function RunPowerShellTabContainer(extension, renderArea, renderData) {
        global.RunPowerShellTenantExtension.RunPowerShellTabContainer.loadTab(renderData, renderArea);
    }

    global.RunPowerShellTenantExtension = global.RunPowerShellTenantExtension || {};

    navigation = {
        tabs: [
            
{
    id: "RunPowerShellTabContainer",
    displayName: "Runbooks",
    template: "RunPowerShellTabContainer",
    activated: RunPowerShellTabContainer
}
        ],
        types: [
        ]
    };

    RunPowerShellTenantExtensionActivationInit = function () {
        var subs = Exp.Rdfe.getSubscriptionList(),
            subscriptionRegisteredToService = global.Exp.Rdfe.getSubscriptionsRegisteredToService("runpowershell"),
            runPowerShellExtension = $.extend(this, global.RunPowerShellTenantExtension);

        // Don't activate the extension if user doesn't have a plan that includes the service.
        if (subscriptionRegisteredToService.length === 0) {
            return false; // Don't want to activate? Just bail
        }

        $.extend(runPowerShellExtension, {
            viewModelUris: [runPowerShellExtension.Controller.userInfoUrl],
            displayName: "Automation",
            navigationalViewModelUri: {
                uri: runPowerShellExtension.Controller.listFileSharesUrl,
                ajaxData: function () {
                    return global.Exp.Rdfe.getSubscriptionIdsRegisteredToService(serviceName);
                }
            },
            displayStatus: global.waz.interaction.statusIconHelper(global.RunPowerShellTenantExtension.FileSharesTab.statusIcons, "Status"),
            menuItems: [
                //{
                //    name: "FileShares",
                //    displayName: "Run PowerShell",
                //    url: "#Workspaces/RunPowerShellTenantExtension",
                //    preview: "createPreview",
                //    subMenu: [
                //        {
                //            name: "Create",
                //            displayName: "Create File Share",
                //            description: "Quickly Create File Share on a File Server",
                //            template: "CreateFileShare",
                //            label: "Create",
                //            subMenu: [
                //                {
                //                    name: "QuickCreate",
                //                    displayName: "FileFile",
                //                    template: "CreateFileShare"
                //                }
                //            ]
                //        }
                //    ]
                //}
            ],
            getResources: function () {
                return resources;
            }
        });

        runPowerShellExtension.onNavigateAway = onNavigateAway;
        runPowerShellExtension.navigation = navigation;

        Shell.UI.Pivots.registerExtension(runPowerShellExtension, function () {
            Exp.Navigation.initializePivots(this, this.navigation);
        });

        // Finally activate and give "the" runPowerShellExtension the activated extension since a good bit of code depends on it
        $.extend(global.RunPowerShellTenantExtension, Shell.Extensions.activate(runPowerShellExtension));
    };

    function getQuickCreateFileShareMenuItem() {
        return {
            name: "QuickCreate",
            displayName: "Create File Share",
            description: "Create new file share",
            template: "quickCreateWithRdfe",
            label: resources.CreateMenuItem,

            opening: function () {
                AccountsAdminExtension.AccountsTab.renderListOfHostingOffers(offersListSelector);
            },

            open: function () {
                // Enables As-You-Type validation experience on a container specified
                Shell.UI.Validation.setValidationContainer(valContainerSelector);
                // Enables password complexity feedback experience on a container specified
                Shell.UI.PasswordComplexity.parse(valContainerSelector);
            },

            ok: function (object) {
                var dialogFields = object.fields,
                    isValid = validateAccount();

                if (isValid) {
                    createAccountWithRdfeCore(dialogFields);
                }
                return false;
            },

            cancel: function (dialogFields) {
                // you can return false to cancel the closing
            }
        };
    }

    Shell.Namespace.define("RunPowerShellTenantExtension", {
        serviceName: serviceName,
        init: RunPowerShellTenantExtensionActivationInit,
        getQuickCreateFileShareMenuItem: getQuickCreateFileShareMenuItem
    });
})(jQuery, this);
