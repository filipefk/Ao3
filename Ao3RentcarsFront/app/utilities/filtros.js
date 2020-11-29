(function () {
  "use strict";

  angular.module("Ao3RentCarsApp").filter("capitalize", function () {
    return function (input) {
      return angular.isString(input) && input.length > 0
        ? input.charAt(0).toUpperCase() + input.substr(1).toLowerCase()
        : input;
    };
  });
})();
