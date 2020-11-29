(function () {
  "use strict";

  angular.module("Ao3RentCarsApp").component("mensagensAlertas", {
    templateUrl: "componentes/mensagens-alertas.tpl.html",
    controller: "MensagensAlertasController",
    controllerAs: "vm",
    bindings: {
      ctrl: "=",
      params: "<",
      name: "=",
    },
  });
})();
