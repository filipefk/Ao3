(function () {
  "use strict";

  angular
    .module("Ao3RentCarsApp")
    .service("VeiculosLocadosService", veiculosLocadosService);

  veiculosLocadosService.$inject = ["$http", "constantes", "helperFactory"];

  function veiculosLocadosService($http, constantes, helper) {
    return {
      listar: listar,
      encerrarLocacao: encerrarLocacao,
    };

    function listar() {
      var config = {
        headers: {
          Authorization: "Bearer " + helper.getRootScope("token"),
        },
      };
      return $http
        .get(constantes.URL_BASE + "/Veiculos/Locados", config)
        .then(function (response) {
          return response.data;
        })
        .catch(helper.sendError);
    }

    function encerrarLocacao(id) {
      var config = {
        headers: {
          Authorization: "Bearer " + helper.getRootScope("token"),
        },
      };
      return $http
        .put(
          constantes.URL_BASE + "/Veiculos/" + id + "/Encerrar",
          null,
          config
        )
        .then(function (response) {
          return response;
        })
        .catch(helper.sendError);
    }
  }
})();
