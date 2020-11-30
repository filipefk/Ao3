(function () {
  "use strict";

  angular
    .module("Ao3RentCarsApp")
    .service("VeiculosCadastroService", veiculosCadastroService);

  veiculosCadastroService.$inject = ["$http", "constantes", "helperFactory"];

  function veiculosCadastroService($http, constantes, helper) {
    return {
      incluir: incluir,
      alterar: alterar,
      excluir: excluir,
      buscaMarcas: buscaMarcas,
      buscaModelos: buscaModelos,
    };

    function buscaMarcas() {
      return $http
        .get("http://fipeapi.appspot.com/api/1/carros/marcas.json")
        .then(function (response) {
          return response.data;
        })
        .catch(helper.sendError);
    }

    function buscaModelos(idMarca) {
      return $http
        .get(
          "http://fipeapi.appspot.com/api/1/carros/veiculos/" +
            idMarca +
            ".json"
        )
        .then(function (response) {
          return response.data;
        })
        .catch(helper.sendError);
    }

    function incluir(veiculo) {
      var config = {
        headers: {
          Authorization: "Bearer " + helper.getRootScope("token"),
        },
      };
      return $http
        .post(constantes.URL_BASE + "/Veiculos", veiculo, config)
        .then(function (response) {
          return response.data;
        })
        .catch(helper.sendError);
    }

    function alterar(veiculo) {
      var config = {
        headers: {
          Authorization: "Bearer " + helper.getRootScope("token"),
        },
      };
      var id = veiculo.id;
      return $http
        .put(constantes.URL_BASE + "/Veiculos/" + id, veiculo, config)
        .then(function (response) {
          return response;
        })
        .catch(helper.sendError);
    }

    function excluir(id) {
      var config = {
        headers: {
          Authorization: "Bearer " + helper.getRootScope("token"),
        },
      };
      return $http
        .delete(constantes.URL_BASE + "/Veiculos/" + id, config)
        .then(function (response) {
          return response;
        })
        .catch(helper.sendError);
    }
  }
})();
