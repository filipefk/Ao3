(function () {
  "use strict";

  angular
    .module("Ao3RentCarsApp")
    .service("VeiculosDisponiveisService", veiculosDisponiveisService);

  veiculosDisponiveisService.$inject = ["$http", "constantes", "helperFactory"];

  function veiculosDisponiveisService($http, constantes, helper) {
    return {
      listar: listar,
      consultar: consultar,
      locar: locar,
    };

    function listar() {
      // var config = {
      //   headers: {
      //     Authorization: "Bearer " + helper.getRootScope("token"),
      //   },
      // };
      return $http
        .get(constantes.URL_BASE + "/Veiculos/Disponiveis")
        .then(function (response) {
          return response.data;
        })
        .catch(helper.sendError);
    }

    function consultar(id) {
      // var config = {
      //   headers: {
      //     Authorization: "Bearer " + helper.getRootScope("token"),
      //   },
      // };
      return $http
        .get(constantes.URL_BASE + "/Veiculos/" + id)
        .then(function (response) {
          return response.data;
        })
        .catch(helper.sendError);
    }

    function locar(_locacaoCliente) {
      // var config = {
      //   headers: {
      //     Authorization: "Bearer " + helper.getRootScope("token"),
      //   },
      // };
      return $http
        .post(constantes.URL_BASE + "/Locacoes/Locar", _locacaoCliente)
        .then(function (response) {
          return response.data;
        })
        .catch(helper.sendError);
    }
  }
})();
