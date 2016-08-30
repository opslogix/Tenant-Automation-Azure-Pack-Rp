/// <reference path="RunPowerShelltenant.createwizard.js" />
/// <reference path="RunPowerShelltenant.controller.js" />
/*globals window,jQuery,RunPowerShellTenantExtension,Exp,waz,cdm*/
(function ($, global, undefined) {
    "use strict";

    var grid,
        selectedRow,
        statusIcons = {
            Registered: {
                text: "Registered",
                iconName: "complete"
            },
            Default: {
                iconName: "spinner"
            }
        };






    //------------------------------------------------------

    var selectors = {
        container: ".aux-manageAdministratorsContainer",
        coAdminName: "#coAdminUsername",
        coAdminNameTextValue: "input[id=coAdminUsername]",
        subscriptions: "#aux-manageCoadminSubscriptions",
        subscriptionCheckbox: "#aux-manageCoadminSubscriptions .coadmin-subscription-checkbox{0}",
        checkedSubscriptionCheckboxes: "#aux-manageCoadminSubscriptions input[type='checkbox']:checked",
        header: ".aux-manageAdministratorsContainer .aux-dialogHeader",
        subHeader: ".aux-manageAdministratorsContainer .aux-dialogSubHeader",
        description: ".aux-manageAdministratorsContainer .aux-dialogDescription"
    },
        checkboxTemplateString = "<div class='users-checkbox-container'>" +
                                "    <label><input class='coadmin-subscription-checkbox{{>id}}' type='checkbox' {{>isChecked}} {{>isDisabled}}/>{{>subscriptionName}}</label>" +
                                "</div>",
        checkboxTemplate = $.templates(checkboxTemplateString),
        dialog,
        manageCoAdministratorInput,
        resources = global.Resources.getResources("Microsoft.WindowsAzure.Server.CommonPortalStrings.AccountsTenantResources"),
        observableGrid,
        selectedRow,
        selectedSubscriptions,
        commandButtons,
        isEditMode,
        columns = [
            { name: resources.FriendlyNameColumn, field: "DisplayName", type: "text" },
            { name: resources.SubscriptionNameColumn, field: "SubscriptionName", type: "text" },
            { name: resources.SubscriptionIdColumn, field: "SubscriptionId", type: "text" },
            { name: resources.RoleColumnName, field: "Role", type: "text" }
        ];

    function onRowSelected(row) {
        selectedRow = row;
        setCommands(row);
    }

    function checkboxFormatter(value, options) {
        var item = options.dataItem || {},
            data = {
                id: item.subscriptionId,
                isChecked: item.isChecked ? " checked='checked'" : "",
                isDisabled: item.isDisabled ? " disabled='disabled'" : "",
                subscriptionName: item.subscriptionName
            };
        return checkboxTemplate.render(data);
    }

    function onInit(root) {
        var columns = [];
        //   columns.push({ name: resources.SubscriptionNameColumn, field: "subscriptionName", formatter: checkboxFormatter });
        //   columns.push({ name: resources.SubscriptionIdColumn, field: "subscriptionId", type: "text" });
        //   columns.push({ name: resources.RoleColumnName, field: "role", type: "text" });

        if (isEditMode) {
            // we are in edit mode
            $(selectors.header).text(resources.EditCoAdminHeader);
            $(selectors.subHeader).text(resources.EditCoAdminSubHeader);
            $(selectors.description).text(resources.EditCoAdminDescription);
            // disable the email input field
            $(selectors.coAdminName).attr("disabled", "true");
        } else {
            // we are in add mode
            $(selectors.header).text("Run Runbook with parameter");
            //  $(selectors.subHeader).text(resources.AddCoAdminSubHeader);
            //  $(selectors.description).text(resources.AddCoAdminDescription);

            // Update regex to include required things.
            $(selectors.coAdminName)
                .on("change keyup drop paste", function () {
                    // Remove white space characters in the coAdminName field
                    var $input = $(this),
                        currentVal = $input.val(),
                        trimmedVal = $.trim(currentVal);

                    if (currentVal !== trimmedVal) {
                        $input.val(trimmedVal);
                    }
                });
        }

        $(selectors.subscriptions)
            .wazDataGrid("destroy")
            .wazDataGrid({
                columns: columns,
                data: manageCoAdministratorInput.subscriptions,
                selectable: false,
                selectFirstRowByDefault: false,
                maxHeight: 110
            })
            .on("change", updateOkButton);

        $(selectors.coAdminName)
            .on("keyup.okButtonUpdate", updateOkButton)
            .on("paste.okButtonUpdate", updateOkButton)
            .on("cut.okButtonUpdate", updateOkButton)
            .on("drop.okButtonUpdate", updateOkButton);

        global.Shell.UI.Validation.setValidationContainer(selectors.container);
    }

    function showManageCoAdminDialog() {
        var dialogDeferred = $.Deferred(),
            updatingDeferred = $.Deferred(),
            i,
            len,
            subscriptions,
            resourceStrings,
            isChecked;

        manageCoAdministratorInput = {
            manageCoAdministratorPromise: null,
            subscriptions: [],
            coAdminName: selectedRow ? selectedRow.DisplayName : null,
            accountType: null,
            showSelectAccount: false
        };

        subscriptions = global.Exp.Rdfe.getUnfilteredSubscriptionList();
        selectedSubscriptions = [];

        for (i = 0, len = subscriptions.length; i < len; i++) {
            isChecked = true;

            // filter out disabled and not owned subscriptions
            if (subscriptions[i].SubscriptionStatus.toLowerCase() !== "active" || subscriptions[i].AccountAdminLiveEmailId.toLowerCase() !== Shell.Environment.getUserEmailAddress().toLowerCase()) {
                continue;
            }

            if (isEditMode) {
                if ($.inArray(manageCoAdministratorInput.coAdminName, subscriptions[i].CoAdminNames) > -1) {
                    selectedSubscriptions.push(subscriptions[i].id);
                    isChecked = true;
                }
            }

            manageCoAdministratorInput.subscriptions.push({
                isChecked: isChecked,
                isDisabled: false,
                subscriptionId: subscriptions[i].SubscriptionID,
                subscriptionName: subscriptions[i].OfferFriendlyName,
                role: resources.CoAdminRole
            });
        }

        global.Shell.UI.DialogPresenter.show({
            extension: global.MyAccountExtension.name,
            name: "manageCoAdmin",
            template: "manageCoAdminDialog",
            data: manageCoAdministratorInput,
            init: function (root) {
                dialog = this;

                // the dialog box is hard-coded to 400px, following is a way to overwrite it
                $(dialog.element).css("width", "600px");
                $(selectors.subHeader).css("width", "575px");
                $(selectors.description).css("width", "575px");

                if (isEditMode) {
                    $(selectors.coAdminNameTextValue).val(manageCoAdministratorInput.coAdminName);
                }

                onInit(root);
                updateOkButton();
            },
            ok: function () {
                var i,
                    subscriptions = manageCoAdministratorInput.subscriptions,
                    updatedCoAdmins,
                    addedSubscriptions = [],
                    deletedSubscriptions = [],
                    subscriptionIds = [];

                manageCoAdministratorInput.coAdminName = $(selectors.coAdminName).val();




                ////----------------------------------------------

                var runbookName = "Test-Runbook-With_String";
                var subscriptionId = global.Exp.Rdfe.getSubscriptionsRegisteredToService("runpowershell")[0].id;

                var parameter = name = $(selectors.coAdminName).val();

                var promise = RunPowerShellTenantExtension.Controller.executeRunbook(runbookName, subscriptionId, parameter);

                //number, stringArray, date, sayGoodbye);

                global.waz.interaction.showProgress(

                promise,

                {

                    initialText: "Executing runbook...",

                    successText: "Runbook launched successfully.",

                    failureText: "Failed to execute runbook."

                }

                );

                promise.done(function () {

                });


                ////----------------------------------------------


                if (!global.Shell.UI.Validation.validateContainer(selectors.container)) {
                    return false;
                } else {
                    dialogDeferred.resolve(updatingDeferred.promise());

                    resourceStrings = buildNotificationResourceStrings(manageCoAdministratorInput.coAdminName);
                    if (isEditMode) {
                        for (i = 0; i < subscriptions.length; i++) {
                            if (subscriptions[i].isChecked) {
                                if ($.inArray(subscriptions[i].subscriptionId, selectedSubscriptions) > -1) {
                                    continue;
                                } else {
                                    addedSubscriptions.push(subscriptions[i].subscriptionId);
                                }
                            } else {
                                if ($.inArray(subscriptions[i].subscriptionId, selectedSubscriptions) > -1) {
                                    deletedSubscriptions.push(subscriptions[i].subscriptionId);
                                } else {
                                    continue;
                                }
                            }
                        }

                        updatedCoAdmins = { coAdminName: manageCoAdministratorInput.coAdminName, addedSubscriptions: addedSubscriptions, deletedSubscriptions: deletedSubscriptions };
                    } else {
                        for (i = 0; i < subscriptions.length; i++) {
                            if (subscriptions[i].isChecked) {
                                subscriptionIds.push(subscriptions[i].subscriptionId);
                            }
                        }

                        updatedCoAdmins = { coAdminName: manageCoAdministratorInput.coAdminName, subscriptionIds: subscriptionIds };
                    }

                    manageCoAdministrators(updatedCoAdmins, resourceStrings)
                        .done(function (result) {
                            updatingDeferred.resolve(result);
                        })
                        .fail(function (result) {
                            updatingDeferred.reject(result);
                        });
                    $(selectors.container).submit();
                    return true;
                }

                return true;
            }
        });

        return dialogDeferred.promise();
    }

    function updateOkButton() {
        var i, len,
            subscriptions = manageCoAdministratorInput.subscriptions,
            subscriptionsSelected,
            subscriptionSelector;

        // update the list of subscription selections
        for (i = 0, len = subscriptions.length; i < len; i++) {
            subscriptionSelector = $(selectors.subscriptionCheckbox.format(subscriptions[i].subscriptionId));
            if (subscriptionSelector && subscriptionSelector.length === 1) {
                subscriptions[i].isChecked = !!subscriptionSelector.attr("checked");
            }
            if (subscriptions[i].isChecked) {
                subscriptionsSelected = true;
            }
        }

        if ($(selectors.coAdminName).val() && (isEditMode || subscriptionsSelected)) {
            dialog.enableOkButton();
        } else {
            dialog.disableOkButton();
        }

        if (isEditMode && !subscriptionsSelected) {
            showAlert({
                title: resources.AlertRemoveCoAdminTitle,
                content: resources.AlertRemoveCoAdminDescription
            });
        } else {
            hideAlert();
        }
    }



    function showAlert(toastOptions) {
        if (dialog) {
            $(dialog.element).dialogalertbox(toastOptions).dialogalertbox("open");
        }
    }

    function hideAlert() {
        if (dialog) {
            $(dialog.element).dialogalertbox("close");
        }
    }


    function administrator_performAddCommand() {
        isEditMode = false;
        var promise = showManageCoAdminDialog();

        return promise;
    }



    function getMenuItem() {
        return {
            name: "AddCoAdministrator",
            displayName: resources.AddCoAdminHeader,
            description: resources.AddCoAdminDescription,
            isEnabled: function () {
                var subscriptions = global.MyAccountExtension.getListOfSubscriptionsToDisplay();
                return {
                    enabled: subscriptions.length > 0
                };
            },
            execute: function () {
                administrator_performAddCommand().done(function () {
                    global.MyAccountExtension.Controller.navigateToCoAdministratorsListView();
                });
            }
        };
    }



    function destroy() {
        observableGrid.wazObservableGrid("destroy");
        observableGrid = null;
    }

    function cleanup() {
        if (observableGrid) {
            destroy();
        }
    }



    commandButtons = {
        Add: {
            command: "Add",
            executeCommand: administrator_performAddCommand
        },
        Edit: {
            //   command: "Edit",
            //   executeCommand: administrator_performEditCommand
        },
        Remove: {
            //   command: "Remove",
            //   executeCommand: administrator_performRemoveCommand
        }
    };


    //------------------------------------------------------





    function dateFormatter(value) {
        try {
            if (value) {
                return $.datepicker.formatDate("m/d/yy", value);
            }
        }
        catch (err) { }  // Display "-" if the date is in an unrecoginzed format.

        return "-";
    }

    function onRowSelected(row) {
        if (row) {
            selectedRow = row;
            updateContextualCommands(row);
        }
    }

    function updateContextualCommands(domain) {
        Exp.UI.Commands.Contextual.clear();
        Exp.UI.Commands.Contextual.add(new Exp.UI.Command("viewDomainInfo", "Info", Exp.UI.CommandIconDescriptor.getWellKnown("viewinfo"), true, null, onViewInfo));
        Exp.UI.Commands.Contextual.add(new Exp.UI.Command("Execute", "Execute", Exp.UI.CommandIconDescriptor.getWellKnown("Start"), true, null, administrator_performAddCommand));
        Exp.UI.Commands.update();
    }

    function onExecuteRunbook() {

        var subscriptionId =

           global.Exp.Rdfe.getSubscriptionsRegisteredToService("runpowershell")[0].id, parameter = "test";

        //name = "test",

        //number = 100,

        //stringArray = new Array("value1", "value2", "value3"),

        //date = new Date(),

        //sayGoodbye = true;



        var promise = RunPowerShellTenantExtension.Controller.executeRunbook(runbookName, subscriptionId, parameter);

        //number, stringArray, date, sayGoodbye);

        global.waz.interaction.showProgress(

        promise,

        {

            initialText: "Executing runbook...",

            successText: "Runbook launched successfully.",

            failureText: "Failed to execute runbook."

        }

        );

        promise.done(function () {

        });

    }


    function onViewInfo(item) {
        cdm.stepWizard({
            extension: "RunPowerShellTenantExtension",
            steps: [{
                template: "vmsTab",
                data: data,
                // Called when the step is first created
                onStepCreated: function () {
                    wizard = this;
                },
                // Called each time the step is displayed
                onStepActivate: step1Activate,
                // Called before the wizard moves to the next step
                onNextStep: function () {
                    return Shell.UI.Validation.validateContainer("#dm-create-step1");
                }
            }],
            // Called when the user clicks the "Finish" button on the last step
            onComplete: function () {
                var newPassword, newResellerPortalUrl;
                newPassword = $("#dm-password").val();
                newResellerPortalUrl = registerReseller ? $("#dm-portalUrl").val() : null;
                // Call whatever backend function we need to. In our example, it returns a promise
                promise = callback(newPassword, newResellerPortalUrl);

                // Create a new Progress Operation object
                var progressOperation = new Shell.UI.ProgressOperation(
                  // Title of operation
                  "Registering endpoint...",
                  // Initial status. null = default
                  null,
                  // Is indeterministic? (Does it NOT provide a % complete)
                  true);

                // This adds the progress operation we set up earlier to the visible list of PrOp's
                Shell.UI.ProgressOperations.add(progressOperation);

                promise
                  .done(function () {
                      // When the operation succeeds, complete the progress operation
                      progressOperation.complete(
                        "Successfully registered the endpoint.",
                        Shell.UI.InteractionSeverity.information);
                  })
                  .fail(function () {
                      // When the operation fails, complete the progress operation
                      progressOperation.complete(
                        "Failed to register the endpoint.",
                        Shell.UI.InteractionSeverity.error,
                        Shell.UI.InteractionBehavior.ok);
                  });
            }
        },
  {
      // Other supported sized include large, medium & small
      size: "mediumplus"
  });

    }


    // Load the grid with all the available runbooks
    function loadTab(extension, renderArea, initData) {

        var subs = Exp.Rdfe.getSubscriptionList();

        var subscriptionIds = [];
        var planIds = [];

        for (var item in subs) {
            var element = subs[item];
            //Test if PlanId is not null, if it is not null add it to the array
            if (element.PlanId) {
                planIds.push(element.PlanId);
                subscriptionIds.push(element.id);
            };

        };

        var subscriptionRegisteredToService = global.Exp.Rdfe.getSubscriptionsRegisteredToService("runpowershell"),
      localDataSet = {
          dataSetName: global.RunPowerShellTenantExtension.Controller.listFileSharesUrl,
          ajaxData: {
              subscriptionIds: subscriptionIds,
              planIds: planIds,
          },
          url: global.RunPowerShellTenantExtension.Controller.listFileSharesUrl
      },
      columns = [
              { name: "Runbook Name", field: "RunbookName", sortable: false },
              { name: "Runbook Tag", field: "RunbookTag", filterable: false, sortable: false },
              { name: "Runbook Id", field: "RunbookId", filterable: false, sortable: false },
              { name: "In Plan", field: "PlanName", filterable: false, sortable: false }

      ];

        grid = renderArea.find(".gridContainer")
            .wazObservableGrid("destroy")
            .wazObservableGrid({
                lastSelectedRow: null,
                data: localDataSet,
                keyField: "name",
                columns: columns,
                gridOptions: {
                    rowSelect: onRowSelected
                },
                emptyListOptions: {
                    extensionName: "RunPowerShellTenantExtension",
                    templateName: "FileSharesTabEmpty"
                }
            });
    }

    function cleanUp() {
        if (grid) {
            grid.wazObservableGrid("destroy");
            grid = null;
        }
    }

    global.RunPowerShellTenantExtension = global.RunPowerShellTenantExtension || {};
    global.RunPowerShellTenantExtension.vmsTab = {
        loadTab: loadTab,
        cleanUp: cleanUp,
        statusIcons: statusIcons
    };
})(jQuery, this);
