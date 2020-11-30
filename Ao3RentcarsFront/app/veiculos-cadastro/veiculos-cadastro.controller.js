(function () {
  "use strict";

  angular
    .module("Ao3RentCarsApp")
    .controller("VeiculosCadastroController", veiculosCadastroController);

  veiculosCadastroController.$inject = [
    "helperFactory",
    "VeiculosCadastroService",
    "$routeParams",
  ];

  function veiculosCadastroController(helper, service, $routeParams) {
    var vm = this;
    /* ***************    INIT VARIÁVEIS    *********************************** */

    vm.veiculo = {
      modelo: "",
      marca: "",
      placa: "",
      anoModelo: 0,
      anoFabricacao: 0,
    };

    /* ***************    FUNÇÕES EXECUTADAS NA VIEW (HMTL)    **************** */
    vm.go = helper.go;
    vm.iniciar = iniciar;
    vm.preencheModelos = preencheModelos;
    vm.salvarNovoVeiculo = salvarNovoVeiculo;

    function iniciar() {
      limparCampos();
      return service.buscaMarcas().then(salvarMarcas);

      function salvarMarcas(_listaMarcas) {
        vm.listaMarcas = _listaMarcas;
      }
    }

    function preencheModelos() {
      helper.clearMsg();
      return service.buscaModelos(vm.marca.id).then(salvarModelos);

      function salvarModelos(_listaModelos) {
        vm.listaModelos = _listaModelos;
      }
    }

    /* ***************    FUNÇÕES INSTERNAS    ******************************** */
    function salvarNovoVeiculo() {
      vm.veiculo.marca = vm.marca.name;
      vm.veiculo.modelo = vm.modelo.name;
      return service.incluir(vm.veiculo).then(function (_veiculo) {
        tratarResposta(_veiculo);
      });
    }

    function tratarResposta(_veiculo) {
      if (_veiculo.id > 0) {
        limparCampos();
        helper.addMsg("Veículo salvo!", "success");
      } else {
        helper.addMsg("Erro ao tentar cadastrar o veículo", "danger");
      }
    }

    function limparCampos() {
      vm.marca = undefined;
      vm.modelo = undefined;
      vm.veiculo = {
        modelo: "",
        marca: "",
        placa: "",
        anoModelo: 0,
        anoFabricacao: 0,
      };
      helper.clearMsg();
    }
  }
})();
