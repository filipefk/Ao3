(function () {
  "use strict";

  angular
    .module("Ao3RentCarsApp")
    .controller("VeiculosDisponiveisController", veiculosDisponiveisController);

  veiculosDisponiveisController.$inject = [
    "helperFactory",
    "VeiculosDisponiveisService",
    "$routeParams",
  ];

  function veiculosDisponiveisController(helper, service, $routeParams) {
    var vm = this;
    /* ***************    INIT VARIÁVEIS    *********************************** */

    /* ***************    FUNÇÕES EXECUTADAS NA VIEW (HMTL)    **************** */
    vm.go = helper.go;
    vm.iniciar = iniciar;

    function iniciar() {
      return listarVeiculosDisponiveis();
    }

    /* ***************    FUNÇÕES INSTERNAS    ******************************** */
    function listarVeiculosDisponiveis() {
      return service.listar().then(salvarVeiculosDisponiveis);

      function salvarVeiculosDisponiveis(_listaVeiculosDisponiveis) {
        vm.listaVeiculosDisponiveis = _listaVeiculosDisponiveis;
      }
    }
  }
})();
