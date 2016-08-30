navigation = {
    tabs: [
      {
          id: "domains",
          displayName: "domains",
          template: "domainsTab",
          activated: loadDomainsTab
      }
    ]
}
cdm.stepWizard({
    extension: "RunbookModalTemplate",
    steps: [{
        template: "createStep1",
        data: data,
        onStepCreated: function () {
            wizard = this;
        },
        onStepActivate: step1Activate,
        onNextStep: function () {
            return Shell.UI.Validation.validateContainer("#dm-create-step1");
        }
    }]
},
  { size: "mediumplus" });