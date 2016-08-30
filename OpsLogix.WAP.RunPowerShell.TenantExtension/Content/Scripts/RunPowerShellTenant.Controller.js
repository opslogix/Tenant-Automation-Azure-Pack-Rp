/*globals window,jQuery,cdm,RunPowerShellTenantExtension,waz,Exp*/
(function ($, global, undefined) {
    "use strict";

    var baseUrl = "/RunPowerShellTenant",
        listFileSharesUrl = baseUrl + "/ListFileShares",
        domainType = "RunPowerShell";

    function executeRunbook(Upn, RunbookName, SubscriptionId, SelectedVmId, ParamBool, ParamString, ParamInt, ParamDate, ParamStringArray) {

        return Shell.Net.ajaxPost({

            data: {
                    Upn: Upn,
                    RunbookName: RunbookName,
                    SubscriptionId: SubscriptionId,
                    SelectedVmId: SelectedVmId,
                    ParamBool: ParamBool,
                    ParamString: ParamString,
                    ParamInt: ParamInt,
                    ParamDate: ParamDate,
                    ParamStringArray: ParamStringArray
            },

            url: baseUrl + "/ExecuteRunbook"

        });

    }

 
    function navigateToListView() {
        Shell.UI.Navigation.navigate("#Workspaces/{0}/runpowershell".format(RunPowerShellTenantExtension.name));
    }

    function getFileShares(subscriptionIds) {
        return makeAjaxCall(listFileSharesUrl, { subscriptionIds: subscriptionIds }).data;
    }

    function makeAjaxCall(url, data) {
        return Shell.Net.ajaxPost({
            url: url,
            data: data
        });
    }

    function getLocalPlanDataSet() {
        return Exp.Data.getLocalDataSet(planListUrl);
    }

    function getfileSharesData(subscriptionId) {
        return Exp.Data.getData("fileshare{0}".format(subscriptionId), {
            ajaxData: {
                subscriptionIds: subscriptionId
            },
            url: listFileSharesUrl,
            forceCacheRefresh: true
        });
    }

    // TODO: Can we use the waz.dataWrapper in the sample?
    function createFileShare(subscriptionId, fileShareName, size, fileServerName) {
        return new waz.dataWrapper(Exp.Data.getLocalDataSet(listFileSharesUrl))
            .add(
            {
                Name: fileShareName,
                SubscriptionId: subscriptionId,
                Size: size,
                FileServerName: fileServerName
            },
            Shell.Net.ajaxPost({
                data: {
                    subscriptionId: subscriptionId,
                    Name: fileShareName,
                    Size: size,
                    FileServerName: fileServerName
                },
                url: baseUrl + "/CreateFileShare"
            })
        );
    }

    global.RunPowerShellTenantExtension = global.RunPowerShellTenantExtension || {};
    global.RunPowerShellTenantExtension.Controller = {
        createFileShare: createFileShare,
        listFileSharesUrl: listFileSharesUrl,
        getFileShares: getFileShares,
        getLocalPlanDataSet: getLocalPlanDataSet,
        getfileSharesData: getfileSharesData,
        navigateToListView: navigateToListView,
        executeRunbook: executeRunbook
    };
})(jQuery, this);
