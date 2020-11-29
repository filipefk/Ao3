(function () {
  "use strict";

  angular
    .module("Ao3RentCarsApp")
    .controller("AcessoController", acessoController);

  acessoController.$inject = ["helperFactory", "Ao3RentCarsService"];

  function acessoController(helper, service) {
    var vm = this;
    /* ***************    INIT VARIÁVEIS    *********************************** */
    // Array do exemplo do uso do 'ngOptions'
    // vm.tiposEmail = [
    //     { id: 1, desc: '@hotmail.com', disable: false, tipo: 'geral' },
    //     { id: 2, desc: '@outlook.com', disable: false, tipo: 'geral' },
    //     { id: 3, desc: '@gmail.com', disable: false, tipo: 'geral' },
    //     { id: 4, desc: '@filipekanitz.com', disable: false, tipo: 'corporativo' },
    //     { id: 5, desc: '@yahoo.com.br', disable: true, tipo: 'geral' },
    //     { id: 6, desc: '@empresa.com.br', disable: false, tipo: 'corporativo' },
    //     { id: 7, desc: '@teste.com', disable: true, tipo: 'corporativo' },
    // ];

    /* ***************    FUNÇÕES EXECUTADAS NA VIEW (HMTL)    **************** */
    vm.go = helper.go;
    vm.logar = logar;

    function logar() {
      return service.logar(vm.login).then(function (_resp) {
        if (_resp.error) {
          helper.addMsg(_resp.msg, "danger");
        } else {
          helper.setRootScope("userLogged", _resp.usuario);
          helper.setRootScope("token", _resp.token);
          helper.path("/home");
        }
      });
    }

    /* ***************    FUNÇÕES INSTERNAS    ******************************** */
  }
})();
