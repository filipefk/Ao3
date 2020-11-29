(function () {
  "use strict";

  angular
    .module("Ao3RentCarsApp")
    .factory("Ao3RentCarsFactory", Ao3RentCarsFactory);

  Ao3RentCarsFactory.$inject = [];

  function Ao3RentCarsFactory() {
    return {
      name: name,
    };

    // ======================================
    function name(params) {
      // implementar
    }
  }
})();
