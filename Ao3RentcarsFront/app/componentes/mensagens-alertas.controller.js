(function () {
  "use strict";

  angular
    .module("Ao3RentCarsApp")
    .controller("MensagensAlertasController", mensagensAlertasController);

  mensagensAlertasController.$inject = ["$rootScope"];

  function mensagensAlertasController($rootScope) {
    var vm = this;
    /* ***************    INIT VARIÁVEIS    *********************************** */
    vm.teste = "um texto qualquer";

    /* ***************    FUNÇÕES EXECUTADAS NA VIEW (HMTL)    **************** */
    vm.ctrl = {
      funcao: funcao,
    };

    // vm.params

    function funcao() {}
    /* ***************    FUNÇÕES INSTERNAS    ******************************** */
  }
})();
