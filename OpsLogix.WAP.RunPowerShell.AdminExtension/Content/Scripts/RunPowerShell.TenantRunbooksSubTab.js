/*globals window,jQuery,Exp,waz*/
(function ($, global, Shell, Exp, undefined) {
    "use strict";

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

    function dateFormatter(value) {
        return $.datepicker.formatDate("m/d/yy", value);
    }

    function onRowSelected(row) {
    }





    function loadTab(extension, renderArea, initData) {

        getData().done(function (x) {
            for (var item in x.data) {
                var d = x.data[item];
                x.data.splice(0, 1);
            };
            var columns = [
                    { name: "DisplayName", field: "DisplayName" },
                { name: "Status", field: "Status" },
                { name: "ConfigStateDisplayName", field: "ConfigStateDisplayName" },
                { name: "PopularityIndex", field: "PopularityIndex" },
                { name: "SubscriptionCount", field: "SubscriptionCount" },
                { name: "UniqueId", field: "UniqueId" }

            ];


            var control = $(".grid-container")
            control.wazDataGrid("destroy")
                               .wazDataGrid({
                                   columns: columns,
                                   data: x.data,
                                   selectable: true,
                                   rowSelect: function (e, rows) {
                                       onRowSelected(rows.selected ? rows.selected.dataItem : null);
                                   }
                               });

        });
    }


    function makeAjaxCall(url, data) {
        return Shell.Net.ajaxPost({
            url: url,
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
        });
    }


    function getData() {
        //Get all the plan data
        return makeAjaxCall("Plan/List", null);
    }


    function cleanUp() {
        if (grid) {
            grid.wazObservableGrid("destroy");
            grid = null;
        }
    }

    global.RunPowerShellAdminExtension = global.RunPowerShellAdminExtension || {};
    global.RunPowerShellAdminExtension.TenantRunbooksSubTab = {
        loadTab: loadTab,
        cleanUp: cleanUp
    };

})(jQuery, this, this.Shell, this.Exp);