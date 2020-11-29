(function () {
  "use strict";

  angular
    .module("Ao3RentCarsApp")
    .service("Ao3RentCarsService", Ao3RentCarsService);

  Ao3RentCarsService.$inject = ["$http", "constantes", "helperFactory"];

  function Ao3RentCarsService($http, constantes, helper) {
    // this.name = name;

    // function name(params) {
    //     // implementar
    // }

    return {
      logar: logar,
    };

    // ======================================

    function logar(_params) {
      return $http
        .post(constantes.URL_BASE + "/Login", _params)
        .then(function (response) {
          return response.data;
        })
        .catch(helper.sendError);
    }
  }
})();
