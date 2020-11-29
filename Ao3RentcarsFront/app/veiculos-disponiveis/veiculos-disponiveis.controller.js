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

    vm.locacaoCliente = {
      idUsuario: 0,
      idVeiculo: 0,
      nome: "",
      cpf: "",
      dataInicio: "",
      dataFimPrevisto: "",
    };

    /* ***************    FUNÇÕES EXECUTADAS NA VIEW (HMTL)    **************** */
    vm.go = helper.go;
    vm.iniciar = iniciar;
    vm.submitLocacao = submitLocacao;

    function iniciar() {
      var userLogged = helper.setRootScope("userLogged");
      if (userLogged) {
        vm.locacaoCliente.idUsuario = userLogged.id;
      }
      if ($routeParams.id) {
        return consultaVeiculo($routeParams.id);
      } else {
        return listarVeiculosDisponiveis();
      }
    }

    /* ***************    FUNÇÕES INSTERNAS    ******************************** */
    function submitLocacao() {
      return service.locar(vm.locacaoCliente).then(function (_locacao) {
        tratarResposta(_locacao);
      });
    }

    function tratarResposta(_locacao) {
      if (_locacao.id) {
        helper.path("/alugar");
        helper.addMsg("Reserva concluída com sucesso!", "success");
      } else {
        console.log(_locacao);
        helper.addMsg("Erro ao tentar concluir a reserva", "danger");
      }
    }

    function listarVeiculosDisponiveis() {
      return service.listar().then(salvarVeiculosDisponiveis);

      function salvarVeiculosDisponiveis(_listaVeiculosDisponiveis) {
        vm.listaVeiculosDisponiveis = _listaVeiculosDisponiveis;
      }
    }

    function consultaVeiculo(id) {
      return service.consultar(id).then(salvarVeiculo);

      function salvarVeiculo(_veiculo) {
        vm.veiculo = _veiculo;
        vm.locacaoCliente.idVeiculo = vm.veiculo.id;
      }
    }
  }
})();
