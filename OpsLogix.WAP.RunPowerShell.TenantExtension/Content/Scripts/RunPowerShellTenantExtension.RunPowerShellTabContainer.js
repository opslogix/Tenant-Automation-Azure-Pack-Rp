/// <reference path="RunPowerShelltenant.createwizard.js" />
/// <reference path="RunPowerShelltenant.controller.js" />
/*globals window,jQuery,RunPowerShellTenantExtension,Exp,waz,cdm*/
(function ($, global, undefined) {
    "use strict";

    var _wizard;
    var dataTARunbooks;
    var dataTARunbooks2;
    var dataPlans;
    var grid, selectedRow;
    var Subscription;
    var upn;


    function onRowSelected(row) {
        if (row) {
            selectedRow = row;
            updateContextualCommands(row);
        }
    }

    function updateContextualCommands(domain) {

        global.Exp.UI.Commands.Contextual.clear();
        global.Exp.UI.Commands.Global.clear();
        global.Exp.UI.Commands.Global.set([
             new global.Exp.UI.Command("START RUNBOOK", "START RUNBOOK", global.Exp.UI.CommandIconDescriptor.getWellKnown("start"), true, null, AddRunbookWizard)
        ]);

        global.Exp.UI.Commands.update();

    }

    

    function AddRunbookWizard() {
        dataTARunbooks = getData().done(function (dataTARunbooks) {
            dataTARunbooks2 = getData2().done(function (dataTARunbooks2) {

                var ParamsForRunbookElement;
                for (var item in dataTARunbooks2.data) {
                    var element = dataTARunbooks2.data[item];
                    //Make sure we have the right parameter fields for the runbook
                    if (element.RunbookId == selectedRow.RunbookId) {
                        ParamsForRunbookElement = item;
                        break;
                    };

                };

            _wizard = cdm.stepWizard({
                extension: "RunPowerShellTenantExtension",
                steps: [
                    {
                        template: "RunbookWiz01",
                        data: {"dataTARunbooks": dataTARunbooks,
                            "runbookname": selectedRow.runbookName},
                        // Called when the step is first created
                        onStepCreated: function () {
                            var wizard = this;


                            //Show or hide the VMs drop down
                            if (dataTARunbooks2.data[ParamsForRunbookElement].ParamVMs != "") {

                                document.getElementById("ParamVMsLabel").innerHTML = dataTARunbooks2.data[ParamsForRunbookElement].ParamVMs;

                                var VMlist = $("#VMParamDropDown");
                                $.each(dataTARunbooks.data, function () {
                                    VMlist.append(new Option(this.cloudService, this.id));
                                });
                            }

                            else {

                                document.getElementById("ParamVMsDiv").removeNode(true);
                            }

                            //Show or hide the bool drop down
                            if (dataTARunbooks2.data[ParamsForRunbookElement].ParamBool != "") {

                                document.getElementById("BoolParamLabel").innerHTML = dataTARunbooks2.data[ParamsForRunbookElement].ParamBool;

                                var BoolList = $("#BoolParamDropDown");
                                BoolList.append(new Option("true", "true"));
                                BoolList.append(new Option("false", "false"));
                            }

                            else {

                                document.getElementById("BoolParamDiv").removeNode(true);
                            }


                            //Show or hide the string text box
                            if (dataTARunbooks2.data[ParamsForRunbookElement].ParamString != "") {

                                document.getElementById("StringParamLabel").innerHTML = dataTARunbooks2.data[ParamsForRunbookElement].ParamString;
                            }

                            else {

                                document.getElementById("StringParamDiv").removeNode(true);
                            }

                            //Show or hide the Int text box
                            if (dataTARunbooks2.data[ParamsForRunbookElement].ParamInt != "") {

                                document.getElementById("IntParamLabel").innerHTML = dataTARunbooks2.data[ParamsForRunbookElement].ParamInt;
                            }

                            else {

                                document.getElementById("IntParamDiv").removeNode(true);
                            }

                            //Show or hide the Date text box
                            if (dataTARunbooks2.data[ParamsForRunbookElement].ParamDate != "") {

                                document.getElementById("DateParamLabel").innerHTML = dataTARunbooks2.data[ParamsForRunbookElement].ParamDate;
                                $(function () {

                                    $("#DateParamTextBox").datepicker();

                                });
                            }

                            else {

                                document.getElementById("DateParamDiv").removeNode(true);
                            }

                            //Show or hide the StringArray text box
                            if (dataTARunbooks2.data[ParamsForRunbookElement].ParamStringArray != "") {

                                document.getElementById("StringArrayParamLabel").innerHTML = dataTARunbooks2.data[ParamsForRunbookElement].ParamStringArray;
                            }

                            else {

                                document.getElementById("StringArrayParamDiv").removeNode(true);
                            }
                            




                            Shell.UI.Validation.setValidationContainer("#demo-wizard-step-1");
                        },
                        // Called each time the step is displayed
                        //onStepActivate: step1Activate,
                        // Called before the wizard moves to the next step
                        onNextStep: function () {
                            return Shell.UI.Validation.validateContainer("#demo-wizard-step-1");
                        }
                    },
                ],
                // Called when the user clicks the "Finish" button on the last step
                onComplete: function () {

                    var fields = _wizard.gatherFields();

                    //var runbookName = "Test-Runbook-With_String";
                    var runbookName = selectedRow.RunbookName;

                    var subscriptionId = global.Exp.Rdfe.getSubscriptionsRegisteredToService("runpowershell")[0].id;

                    var promise = RunPowerShellTenantExtension.Controller.executeRunbook(upn, runbookName, subscriptionId, fields.VMParamDropDown, fields.BoolParamDropDown, fields.StringParamTextBox, fields.IntParamTextBox, fields.DateParamTextBox, fields.StringArrayParamTextBox);

                    //RunbookName, SubscriptionId, SelectedVmId, ParamBool, ParamString, ParamInt, PatamDate, ParamStringArray

                    global.waz.interaction.showProgress(

                    promise,

                    {

                        initialText: "Executing runbook...",

                        successText: "Runbook launched successfully.",

                        failureText: "Failed to execute runbook."

                    }

                    );
                }

            },
        {
            // Other supported sized include large, medium & small
            size: "mediumplus"
        })
        })
            });

    }


    function getData() {
        //Get all the plan data
        var data = { "SubscriptionDetailLevel": "filtered", "SubscriptionPool": [{ "SubscriptionId": selectedRow.SubscriptionId, "ContinuationToken": "", "Features": [], "Services": [] }] };
        var v = makeAjaxCall("VM/ListVMsAndVMRoles", data);
     
        return v;
    }

    function getData2() {
        var data2 = { "subscriptionIds": selectedRow.SubscriptionId, "planIds": selectedRow.PlanId, "skip": 0, "take": 25 };

        //var dataTAR = Shell.Net.ajaxPost({ url: global.RunPowerShellTenantExtension.Controller.listFileSharesUrl, data: data }).done(function (dataTARunbooks) { });
        var w = makeAjaxCall(global.RunPowerShellTenantExtension.Controller.listFileSharesUrl, data2);

        return w;
    }

    function makeAjaxCall(url, data) {
        return Shell.Net.ajaxPost({
            url: url,
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
        });
    }

 

    // Load the grid with all the available runbooks
    function loadTab(extension, renderArea, initData) {

        var subs = Exp.Rdfe.getSubscriptionList();
        upn = subs[0].AccountAdminLiveEmailId;

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
    global.RunPowerShellTenantExtension.RunPowerShellTabContainer = {
        loadTab: loadTab,
        cleanUp: cleanUp,
        //statusIcons: statusIcons
    };
})(jQuery, this);
