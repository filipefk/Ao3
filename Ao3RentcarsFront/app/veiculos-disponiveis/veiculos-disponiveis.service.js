(function () {
  "use strict";

  angular
    .module("Ao3RentCarsApp")
    .service("VeiculosDisponiveisService", veiculosDisponiveisService);

  veiculosDisponiveisService.$inject = ["$http", "constantes", "helperFactory"];

  function veiculosDisponiveisService($http, constantes, helper) {
    return {
      listar: listar,
    };

    function listar() {
      var config = {
        headers: {
          Authorization: "Bearer " + helper.getRootScope("token"),
        },
      };
      return $http
        .get(constantes.URL_BASE + "/Veiculos/Disponiveis", config)
        .then(function (response) {
          return response.data;
        })
        .catch(helper.sendError);
    }
  }
})();
