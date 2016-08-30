/// <reference path="RunPowerShelladmin.controller.js" />
/*globals,jQuery,trace,cdm,RunPowerShellAdminExtension,waz,Exp*/
(function ($, global, Shell, Exp, undefined) {
    "use strict";

    var commandsEnabled,
        passwordChanged = false;

    function renderPage(adminSettings) {
        // This is a placeholder password that is used for rendering only.
        // We want to populate the password textbox to indicate that there's already a password there even though the API hides it.
        if (adminSettings != null && adminSettings != undefined) {
            adminSettings.Password = "dummy4RenderingOnly";
        }

        $("#rps-endpointUrl").val(adminSettings.EndpointAddress ? adminSettings.EndpointAddress : null);
        $("#rps-username").val(adminSettings.Username ? adminSettings.Username : null);
        $("#rps-password").val(adminSettings.Password ? adminSettings.Password : null);
    }

    function onSettingChanged() {
        updateContextualCommands(true);
    }

    function updateContextualCommands(hasPendingChanges) {
        if (commandsEnabled !== hasPendingChanges) {
            Exp.UI.Commands.Contextual.clear();
            if (hasPendingChanges) {
                Exp.UI.Commands.Contextual.add(new Exp.UI.Command("saveSettings", "Save", Exp.UI.CommandIconDescriptor.getWellKnown("save"), true, null, onSaveSettings));
                Exp.UI.Commands.Contextual.add(new Exp.UI.Command("discardSettings", "Discard", Exp.UI.CommandIconDescriptor.getWellKnown("reset"), true, null, onDiscardSettings));
                Shell.UI.Navigation.setConfirmNavigateAway("If you leave this page then your unsaved changes will be lost.");
                commandsEnabled = true;
            } else {
                Shell.UI.Navigation.removeConfirmNavigateAway();
                commandsEnabled = false;
            }
            Exp.UI.Commands.update();
        }
    }

    // Command handlers
    function onSaveSettings() {
        var progressOperation, newSettings;

        progressOperation = new Shell.UI.ProgressOperation("Updating settings...", null /* call back */, false /*isDeterministic */);
        Shell.UI.ProgressOperations.add(progressOperation);

        newSettings = $.extend(true, {}, global.RunPowerShellAdminExtension.Controller.getCurrentAdminSettings());
        newSettings.EndpointAddress = $("#rps-endpointUrl").val();
        newSettings.Username = $("#rps-username").val();
        newSettings.Password = passwordChanged ? $("#rps-password").val() : null;

        // newSettings.ResourceProviderEndpoint = null; // backend will skip the ResourceProvider update call if not specified
        // newSettings.ResellerEndpoint = {
        //    EndpointAddress: $("#rps-endpointUrl").val(),
        //    Username: $("#rps-username").val(),
        //    Password: passwordChanged ? $("#rps-password").val() : null
        // };

        newSettings.LoadBalancerIPAddress = $("#dm-loadBalancerIP").val();

        global.RunPowerShellAdminExtension.Controller.updateAdminSettings(newSettings)
            .done(function (data, textStatus, jqXHR) {
                progressOperation.complete("Successfully updated settings.", Shell.UI.InteractionSeverity.information);
                global.RunPowerShellAdminExtension.Controller.invalidateAdminSettingsCache();
                updateContextualCommands(false);
                passwordChanged = false;
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                var message = "Update settings failed.";
                global.Exp.Utilities.failProgressOperation(progressOperation, message, Exp.Utilities.getXhrError(jqXHR));
            });
    }

    function onDiscardSettings() {
        renderPage(global.RunPowerShellAdminExtension.Controller.getCurrentAdminSettings());
        updateContextualCommands(false);
    }

    // Public
    function loadTab(renderData, container) {
        commandsEnabled = false;

        // Intialize the local data update event handler
        global.RunPowerShellAdminExtension.Controller.invalidateAdminSettingsCache()
            .done(function (url, dataSet) {
                $(dataSet.data).off("propertyChange").on("propertyChange", function () {
                    renderPage(dataSet.data);
                });
                $(dataSet.data).trigger("propertyChange");
            });

        Shell.UI.Validation.setValidationContainer("#rps-settings");  // Initialize validation container for subsequent calls to Shell.UI.Validation.validateContainer.
        $("#rps-settings").on("change.fxcontrol", onSettingChanged);

        $("#rps-password").on("keyup change", function () {
            passwordChanged = true;
        });
    }

    function cleanup() {
    }

    global.RunPowerShellAdminExtension = global.RunPowerShellAdminExtension || {};
    global.RunPowerShellAdminExtension.SettingsTab = {
        loadTab: loadTab,
        cleanup: cleanup
    };
})(jQuery, this, this.Shell, this.Exp);