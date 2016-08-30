/*globals window,jQuery,cdm, RunPowerShellAdminExtension*/
(function ($, global, undefined) {
    "use strict";

    var baseUrl = "/RunPowerShellAdmin",
        adminSettingsUrl = baseUrl + "/AdminSettings",
        adminProductsUrl = baseUrl + "/Products",
        adminFileServersUrl = baseUrl + "/FileServers",
        adminTARunbooksUrl = baseUrl + "/TARunbooks",
        adminTARunbooksAddUrl = baseUrl + "/TARunbooksAdd",
    adminSubscriptionsUrl = baseUrl + "/TASubscriptions";

    function makeAjaxCall(url, data) {
        return Shell.Net.ajaxPost({
            url: url,
            data: data
        });
    }

    function updateAdminSettings(newSettings) {
        return makeAjaxCall(baseUrl + "/UpdateAdminSettings", newSettings);
    }

    function invalidateAdminSettingsCache() {
        return global.Exp.Data.getData({
            url: global.RunPowerShellAdminExtension.Controller.adminSettingsUrl,
            dataSetName: RunPowerShellAdminExtension.Controller.adminSettingsUrl,
            forceCacheRefresh: true
        });
    }

    function getCurrentAdminSettings() {
        return makeAjaxCall(global.RunPowerShellAdminExtension.Controller.adminSettingsUrl);
    }

    function isResourceProviderRegistered() {
        global.Shell.UI.Spinner.show();
        global.RunPowerShellAdminExtension.Controller.getCurrentAdminSettings()
        .done(function (response) {
            if (response && response.data.EndpointAddress) {
                return true;
            }
            else {
                return false;
            }
        })
         .always(function () {
             global.Shell.UI.Spinner.hide();
         });
    }

    // Public
    global.RunPowerShellAdminExtension = global.RunPowerShellAdminExtension || {};
    global.RunPowerShellAdminExtension.Controller = {
        adminSettingsUrl: adminSettingsUrl,
        adminProductsUrl: adminProductsUrl,
        adminFileServersUrl: adminFileServersUrl,
        adminTARunbooksUrl: adminTARunbooksUrl,
        adminSubscriptionsUrl: adminSubscriptionsUrl,
        updateAdminSettings: updateAdminSettings,
        getCurrentAdminSettings: getCurrentAdminSettings,
        invalidateAdminSettingsCache: invalidateAdminSettingsCache,
        isResourceProviderRegistered: isResourceProviderRegistered
    };
})(jQuery, this);
