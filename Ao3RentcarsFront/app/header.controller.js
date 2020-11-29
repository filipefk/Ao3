(function () {
  "use strict";

  angular
    .module("Ao3RentCarsApp")
    .controller("HeaderController", headerController);

  headerController.$inject = ["Ao3RentCarsService", "helperFactory"];

  function headerController(service, helper) {
    var vm = this;
    /* ***************    INIT VARIÁVEIS    *********************************** */

    /* ***************    FUNÇÕES EXECUTADAS NA VIEW (HMTL)    **************** */
    vm.go = helper.go;
    vm.logout = logout;
    vm.iniciar = iniciar;
    vm.serviceF = serviceF;

    // Podemos criar uma função para ser executada assim que a controller for iniciada
    // então colocamos dentro dessa função o 'vm.go', para que assim que acessar
    // a aplicação a partir de qualquer rota, será usuário o acesso e setado a página na qual estiver
    function iniciar() {
      vm.go();
    }

    function serviceF(_path) {
      return service.exemplo().then(function (response) {
        //console.log("veio da service", response);
      });
    }

    function logout() {
      helper.setRootScope("userLogged", undefined);
      helper.setRootScope("token", undefined);
      helper.path("/login");
    }

    /* ***************    FUNÇÕES INSTERNAS    ******************************** */
  }
})();
