(function () {
  "use strict";

  angular.module("Ao3RentCarsApp").config(routes).run(configDefaults);

  routes.$inject = ["$routeProvider"];
  configDefaults.$inject = ["$rootScope"];

  function routes($routeProvider) {
    $routeProvider
      .when("/", {
        redirectTo: "/home",
      })
      .when("/home", {
        templateUrl: "acesso/home.tpl.html",
      })
      .when("/login", {
        templateUrl: "acesso/login.tpl.html",
      })
      .when("/alugar", {
        templateUrl:
          "veiculos-disponiveis/listar-veiculos-disponiveis.tpl.html",
      })
      .when("/alugar/:id", {
        templateUrl: "veiculos-disponiveis/alugar-veiculo.tpl.html",
      })
      .when("/veiculos/locados", {
        templateUrl: "veiculos-locados/listar-veiculos-locados.tpl.html",
      })
      .when("/gerenciar/categorias/new", {
        templateUrl: "categorias/formulario.tpl.html",
      })
      .when("/gerenciar/categorias/editar/:id", {
        templateUrl: "categorias/formulario.tpl.html",
      })
      .when("/itens", {
        templateUrl: "itens/listar.tpl.html",
      })
      .when("/gerenciar/itens", {
        templateUrl: "itens/itens.tpl.html",
      })
      .when("/gerenciar/itens/new", {
        templateUrl: "itens/formulario.tpl.html",
      })
      .when("/gerenciar/itens/editar/:id", {
        templateUrl: "itens/formulario.tpl.html",
      })
      .otherwise({
        redirectTo: "/home",
      });
  }

  function configDefaults($rootScope) {
    $rootScope.listaMensagens = [];
  }
})();
