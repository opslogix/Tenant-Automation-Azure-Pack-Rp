/*globals window,jQuery,Shell,Exp,waz*/

(function (global, $, undefined) {
    "use strict";

    var resources = [],
        runPowerShellExtensionActivationInit,
        navigation,
            selectedContainerId;

    function clearCommandBar() {
        Exp.UI.Commands.Contextual.clear();
        Exp.UI.Commands.Global.clear();
        Exp.UI.Commands.update();
    }

    function onApplicationStart() {
        Exp.UserSettings.getGlobalUserSetting("Admin-skipQuickStart").then(function (results) {
            var setting = results ? results[0] : null;
            if (setting && setting.Value) {
                global.RunPowerShellAdminExtension.settings.skipQuickStart = JSON.parse(setting.Value);
            }
        });

        global.RunPowerShellAdminExtension.settings.skipQuickStart = false;
    }

    function loadQuickStart(extension, renderArea, renderData) {
        clearCommandBar();
        global.RunPowerShellAdminExtension.QuickStartTab.loadTab(renderData, renderArea);
    }

    function loadTenantRunbooksTab(extension, renderArea, renderData) {
        global.RunPowerShellAdminExtension.TenantRunbooksTab.loadTab(renderData, renderArea);
    }

    function loadTenantRunbooksSubTab(extension, renderArea, renderData) {
        clearCommandBar();
        global.RunPowerShellAdminExtension.TenantRunbooksSubTab.loadTab(renderData, renderArea);
    }

    function loadSettingsTab(extension, renderArea, renderData) {
        global.RunPowerShellAdminExtension.SettingsTab.loadTab(renderData, renderArea);
    }


    function onNavigating(context) {
        var destinationItem = context.destination.item;

        // We are navigating to drill downs for a container  
        if (destinationItem) {
            if (destinationItem.type === "Runbook") { // This is the Type property value of JSON object.  
                selectedContainerId = destinationItem.name;
            }
        }
    }

    function loadContainersNavigationItemsDataFunction(data, originalPath, extension) {
        //var items = $.map(global.StorageSampleTenantExtension.Controller.getContainersDataSet().data,
        //      function (value) {
        //          return $.extend(value, {
        //              name: value.ContainerId,  // This is the id of object.
        //              displayName: value.ContainerName,
        //              uniqueId: value.ContainerId,
        //              navigationPath: {
        //                  type: value.Type,     // This is the type of object, you need to set this as a property in JSON data model.
        //                  name: value.ContainerId
        //              }
        //          });
        //      }
        //  );

        //// Note: The following way of finding the subscription id for specific container id is not ideal.
        //// This is done more as a hack. 
        //var i, itemCount;
        //for (i = 0, itemCount = items.length; i < itemCount; i++) {
        //    if (items[i] && items[i].ContainerId == selectedContainerId) {
        //        selectedSubscriptionId = items[i].SubscriptionId;
        //        break;
        //    }
        //}

        return {
            data: [],
            backNavigation: {
                // This should be the id of the tab registered in navigation.
                // Note most of these matching are case-sensitive, yes **SENSITIVE**
                view: "tenantRunbooks"
            }
        };
    }

    navigation = {
        tabs: [
                {
                    id: "quickStart",
                    displayName: "quickStart",
                    template: "quickStartTab",
                    activated: loadQuickStart
                },
				{
				    id: "tenantRunbooks",
				    displayName: "Tenant Runbooks",
				    template: "tenantRunbooksTab",
				    activated: loadTenantRunbooksTab
				},
                {
                    id: "settings",
                    displayName: "settings",
                    template: "settingsTab",
                    activated: loadSettingsTab
                }

        ],

        types: [
          {
              name: "TARunbook", // This is the type name of the object.  
              dataFunction: loadContainersNavigationItemsDataFunction,
              tabs: [
                     {
                         id: "tenantRunbooksSubTab",
                         displayName: "tenantRunbooksSubTab",
                         template: "tenantRunbooksSubTab",
                         activated: function (extension, renderArea, renderData) {
                             loadTenantRunbooksSubTab(extension, renderArea, renderData);
                         }
                     }
              ]
          },
        ]


    };


    global.runPowerShellExtension = global.RunPowerShellAdminExtension || {};

    runPowerShellExtensionActivationInit = function () {
        var runPowerShellExtension = $.extend(this, global.RunPowerShellAdminExtension);

        $.extend(runPowerShellExtension, {
            displayName: "Tenant Automation",
            viewModelUris: [
                global.RunPowerShellAdminExtension.Controller.adminSettingsUrl,
                global.RunPowerShellAdminExtension.Controller.adminTARunbooksUrl
            ],
            menuItems: [ 
                 //{ 
                 //    name: "TARunbook",
                 //    displayName: "Storage Sample", 
                 //    url: "#Workspaces/RunPowerShellAdminExtension",
                 //    preview: "createPreview", 
                 //    isEnabled: function () { 
                 //          return { 
                 //                    enabled: true, 
                 //            description: "Create data storage services" 
                 //        } 
                 //            }, 
                 //    subMenu: [ 
                 //        //getQuickCreateContainerMenuItem() 
                 //    ] 
                 //} 
             ], 

            settings: {
                skipQuickStart: true
            },
            getResources: function () {
                return resources;
            }
        });





        






        runPowerShellExtension.onNavigating = onNavigating;

        runPowerShellExtension.onApplicationStart = onApplicationStart;
        runPowerShellExtension.setCommands = clearCommandBar();

        Shell.UI.Pivots.registerExtension(runPowerShellExtension, function () {
            Exp.Navigation.initializePivots(this, navigation);
        });

        // Need to register types, so that navigation to sub-levels can be enabled for tabs.  
        Exp.TypeRegistry.add(runPowerShellExtension.name, navigation.types);


        // Finally activate runPowerShellExtension 
        $.extend(global.RunPowerShellAdminExtension, Shell.Extensions.activate(runPowerShellExtension));
    };

    Shell.Namespace.define("RunPowerShellAdminExtension", {
        init: runPowerShellExtensionActivationInit
    });

})(this, jQuery, Shell, Exp);