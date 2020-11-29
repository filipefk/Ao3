(function () {
  // angular.module('ngRoute', []);
  angular.module("moduloExterno", []); // mock

  angular.mock.Ao3RentCarsMock = function ($routeProvider) {
    module("Ao3RentCarsApp");
    module(function ($provide) {
      $provide.service("Ao3RentCarsService", function () {
        return {
          exemplo: function () {
            return {};
          },
        };
      });
    });
  };
})();
