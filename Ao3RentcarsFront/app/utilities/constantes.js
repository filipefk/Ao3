(function () {
  "use strict";

  angular.module("Ao3RentCarsApp").constant("constantes", {
    URL_BASE: "https://localhost:44390/api",
    MENSAGENS: {
      ERRO_GERAL: "Ocorreu algum problema. Tente novamente mais tarde.",
      SEM_ACESSO: "Você não tem acesso a esta página.",
    },
    MSG_ERRO: "Ocorreu algum problema. Tente novamente mais tarde.",
  });
})();
