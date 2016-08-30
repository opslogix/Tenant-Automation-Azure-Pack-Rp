/*globals window,jQuery,Exp,waz*/
(function ($, global, Shell, Exp, undefined) {
    "use strict";
    var _wizard;
    var dataTARunbooks;
    var dataPlans;
    var selectedRow;
    var grid,
        statusIcons = {
            Registered: {
                text: "Registered",
                iconName: "complete"
            },
            Default: {
                iconName: "spinner"
            }
        };

    function onRowSelected(row) {
        selectedRow = row;
    }

    function setCommands() {
        global.Exp.UI.Commands.Contextual.clear();
        global.Exp.UI.Commands.Global.clear();
        global.Exp.UI.Commands.Global.set([
             new global.Exp.UI.Command("ADD RUNBOOK", "ADD RUNBOOK", global.Exp.UI.CommandIconDescriptor.getWellKnown("create"), true, null, AddRunbookWizard),
             new global.Exp.UI.Command("REMOVE RUNBOOK", "REMOVE RUNBOOK", global.Exp.UI.CommandIconDescriptor.getWellKnown("delete"), true, null, RemoveRunbook)
    ]);

        global.Exp.UI.Commands.update();
    }


    function RemoveRunbook() {
        var data = {
            RunbookName: selectedRow.RunbookName,
            RunbookId: selectedRow.RunbookId,
            RunbookTag: selectedRow.RunbookTag,
            PlanName: selectedRow.PlanName,
            PlanId: selectedRow.PlanId
        };
        var addPlanConfirmation = new Shell.UI.Notifications.Confirmation("Are you sure you want to remove runbook: " + data.RunbookName +  " for tenants");
        addPlanConfirmation.setActions(Shell.UI.Notifications.ButtonSets.yesNo(function () {
            $.ajax({
                url: "RunPowerShellAdmin/RemoveTARunbook",
                type: 'POST',
                content: "application/json; charset=utf-8",
                dataType: "json",
                data: data
            })
        },
        function () {
            // do nothing
        }));

        Shell.UI.Notifications.add(addPlanConfirmation);

        return;

    }

    
    function AddRunbookWizard() {
        dataTARunbooks = Shell.Net.ajaxPost({ url: "RunPowerShellAdmin/SMARunbooks" }).done(function (dataTARunbooks) {
            dataPlans = Shell.Net.ajaxPost({ url: "Plan/List" }).done(function (dataPlans) {
               
            

                _wizard = cdm.stepWizard({
                extension: "RunPowerShellAdminExtension",
                steps: [
                    {
                        template: "AddTenantRunbookWiz01",
                        data: dataTARunbooks,
                        // Called when the step is first created
                        onStepCreated: function () {
                            var wizard = this;


                            var options = $("#externaldata1");
                            options.empty();
                            options.append($("<option />"));

                            $.each(dataTARunbooks.data, function () {
                                options.append($("<option />").text(this.RunbookName).val(this.RunbookId));
                            });

                            Shell.UI.Validation.setValidationContainer("#demo-wizard-step-1");
                        },
                        // Called each time the step is displayed
                        //onStepActivate: step1Activate,
                        // Called before the wizard moves to the next step
                        onNextStep: function () {
                            return Shell.UI.Validation.validateContainer("#demo-wizard-step-1");
                        }
                    },
                    {
                        template: "AddTenantRunbookWiz02",
                        data: dataTARunbooks,
                        // Called when the step is first created
                        onStepCreated: function () {
                            var wizard = this;

                            document.getElementById("VMDropDownTextBox").disabled = true;
                            document.getElementById("StringParamTextBox").disabled = true;
                            document.getElementById("IntParamTextBox").disabled = true;
                            document.getElementById("DateParamTextBox").disabled = true;
                            document.getElementById("BoolParamTextBox").disabled = true;
                            document.getElementById("StringArrayParamTextBox").disabled = true;



                            $(function () {
                                $("#VMDropDownCheckBox").click(function () {
                                    if ($(this).is(":checked")) {
                                        document.getElementById("VMDropDownTextBox").disabled = false;
                                        } else {
                                        document.getElementById("VMDropDownTextBox").disabled = true;
                                        }
                                    });
                            });
                            $(function () {
                                $("#StringParamCheckBox").click(function () {
                                    if ($(this).is(":checked")) {
                                        document.getElementById("StringParamTextBox").disabled = false;
                                    } else {
                                        document.getElementById("StringParamTextBox").disabled = true;
                                    }
                                });
                            });
                            $(function () {
                                $("#IntParamCheckBox").click(function () {
                                    if ($(this).is(":checked")) {
                                        document.getElementById("IntParamTextBox").disabled = false;
                                    } else {
                                        document.getElementById("IntParamTextBox").disabled = true;
                                    }
                                });
                            });
                            $(function () {
                                $("#DateParamCheckBox").click(function () {
                                    if ($(this).is(":checked")) {
                                        document.getElementById("DateParamTextBox").disabled = false;
                                    } else {
                                        document.getElementById("DateParamTextBox").disabled = true;
                                    }
                                });
                            });
                            $(function () {
                                $("#BoolParamCheckBox").click(function () {
                                    if ($(this).is(":checked")) {
                                        document.getElementById("BoolParamTextBox").disabled = false;
                                    } else {
                                        document.getElementById("BoolParamTextBox").disabled = true;
                                    }
                                });
                            });
                            $(function () {
                                $("#StringArrayParamCheckBox").click(function () {
                                    if ($(this).is(":checked")) {
                                        document.getElementById("StringArrayParamTextBox").disabled = false;
                                    } else {
                                        document.getElementById("StringArrayParamTextBox").disabled = true;
                                    }
                                });
                            });



                            Shell.UI.Validation.setValidationContainer("#demo-wizard-step-2");
                        },
                        // Called each time the step is displayed
                        //onStepActivate: step1Activate,
                        // Called before the wizard moves to the next step
                        onNextStep: function () {
                            return Shell.UI.Validation.validateContainer("#demo-wizard-step-2");
                        }
                    },
                    {
                        template: "AddTenantRunbookWiz03",
                        data: dataPlans,
                        // Called when the step is first created
                        onStepCreated: function () {
                            var wizard = this;


                            var options = $("#externaldata3");
                            options.empty();
                            options.append($("<option />"));

                            $.each(dataPlans.data, function () {
                                options.append($("<option />").text(this.DisplayName).val(this.DisplayName));
                            });

                            Shell.UI.Validation.setValidationContainer("#demo-wizard-step-3");
                        },
                        // Called each time the step is displayed
                        //onStepActivate: step1Activate,
                        // Called before the wizard moves to the next step
                        onNextStep: function () {
                            return Shell.UI.Validation.validateContainer("#demo-wizard-step-3");
                        }
                    }
                ],
                // Called when the user clicks the "Finish" button on the last step
                onComplete: function () {
                    wizardComplete(dataPlans, dataTARunbooks);
                }
                    
            },
            {
                // Other supported sized include large, medium & small
                size: "mediumplus"
            })
            });
        });
    }

    function wizardComplete(plans,runbooks) {
        var fields = _wizard.gatherFields();

        var strPlanName;
        var strPlanId;
        var strRunbookId;
        var strRunbookName;
        var strRunbookTag;
        var strVMDropDownCheckBox;
        var strVMDropDownTextBox = fields.VMDropDownTextBox;
        var strStringParamCheckBox;
        var strStringParamTextBox = fields.StringParamTextBox;
        var strIntParamCheckBox;
        var strIntParamTextBox = fields.IntParamTextBox;
        var strDateParamCheckBox;
        var strDateParamTextBox = fields.DateParamTextBox;
        var strBoolParamCheckBox;
        var strBoolParamTextBox = fields.BoolParamTextBox;
        var strStringArrayParamCheckBox;
        var strStringArrayParamTextBox = fields.StringArrayParamTextBox;

        //Get the correct data from the wizard for plans
        for (var item in plans.data) {
            var d = plans.data[item];
            if (d.DisplayName == fields.externaldata3)
            {
                strPlanName = d.DisplayName;
                strPlanId = d.id;

            }
        };

        for (var item in runbooks.data) {
            var r = runbooks.data[item];
            if (r.RunbookId == fields.externaldata1) {

                strRunbookName = r.RunbookName;
                strRunbookId = r.RunbookId;
                strRunbookTag = r.RunbookTag;
                
            }
        };

        var data= {
            RunbookName: strRunbookName,
            RunbookId: strRunbookId,
            RunbookTag: strRunbookTag,
            PlanName: strPlanName,
            PlanId: strPlanId,
            VMDropDownCheckBox: strVMDropDownCheckBox,
            ParamVMs: strVMDropDownTextBox,
            StringParamCheckBox: strStringParamCheckBox,
            ParamString: strStringParamTextBox,
            IntParamCheckBox: strIntParamCheckBox,
            ParamInt: strIntParamTextBox,
            DateParamCheckBox: strDateParamCheckBox,
            ParamDate: strDateParamTextBox,
            BoolParamCheckBox: strBoolParamCheckBox,
            ParamBool: strBoolParamTextBox,
            StringArrayParamCheckBox: strStringArrayParamCheckBox,
            ParamStringArray: strStringArrayParamTextBox
        };
        $.ajax({
            url: "RunPowerShellAdmin/AddTARunbook",
            type: 'POST',
            content: "application/json; charset=utf-8",
            dataType: "json",
            data: data
        })

    //}
    }

    function loadTab(extension, renderArea, initData) {
        var localDataSet = {
            url: global.RunPowerShellAdminExtension.Controller.adminTARunbooksUrl,
            dataSetName: global.RunPowerShellAdminExtension.Controller.adminTARunbooksUrl
        },
            columns = [
                { name: "Runbook Name", field: "RunbookName", type: "navigation", navigationField: "RunbookId", sortable: false },
                { name: "Runbook ID", field: "RunbookId", filterable: false, sortable: false },
                { name: "Runbook Tag", field: "RunbookTag", filterable: false, sortable: false },
                { name: "Available in Plan", field: "PlanName", filterable: false, sortable: false },
                

            ];

        grid = renderArea.find(".grid-container")
            .wazObservableGrid("destroy")
            .wazObservableGrid({
                lastSelectedRow: null,
                data: localDataSet,
                keyField: "RunbookName",
                columns: columns,
                gridOptions: {
                    rowSelect: onRowSelected
                },
                emptyListOptions: {
                    extensionName: "RunPowerShellAdminExtension",
                    templateName: "tenantRunbooksTabEmpty"
                }
            });


        setCommands();
    }



    function cleanUp() {
        if (grid) {
            grid.wazObservableGrid("destroy");
            grid = null;
        }
    }

    global.RunPowerShellAdminExtension = global.RunPowerShellAdminExtension || {};
    global.RunPowerShellAdminExtension.TenantRunbooksTab = {
        loadTab: loadTab,
        cleanUp: cleanUp
    };
})(jQuery, this, this.Shell, this.Exp);