(function () {
  "use strict";

  angular
    .module("Ao3RentCarsApp")
    .controller("VeiculosLocadosController", veiculosLocadosController);

  veiculosLocadosController.$inject = [
    "helperFactory",
    "VeiculosLocadosService",
    "$routeParams",
  ];

  function veiculosLocadosController(helper, service, $routeParams) {
    var vm = this;
    /* ***************    INIT VARIÁVEIS    *********************************** */

    /* ***************    FUNÇÕES EXECUTADAS NA VIEW (HMTL)    **************** */
    vm.go = helper.go;
    vm.iniciar = iniciar;
    vm.encerrarLocacao = encerrarLocacao;

    function iniciar() {
      return listarVeiculosLocados();
    }

    /* ***************    FUNÇÕES INSTERNAS    ******************************** */
    function encerrarLocacao(id) {
      return service.encerrarLocacao(id).then(function (_encerrar) {
        tratarResposta(_encerrar);
      });
    }

    function tratarResposta(_encerrar) {
      if (_encerrar.status == 204) {
        listarVeiculosLocados();
        helper.addMsg("Locação encerrada com sucesso!", "success");
      } else {
        helper.addMsg("Erro ao tentar encerrar a locação", "danger");
      }
    }

    function listarVeiculosLocados() {
      return service.listar().then(salvarVeiculosLocados);

      function salvarVeiculosLocados(_listaVeiculosLocados) {
        vm.listaVeiculosLocados = _listaVeiculosLocados;
      }
    }
  }
})();
