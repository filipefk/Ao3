describe("HeaderController", function () {
  var $controller, $rootScope, $httpBackend, $location, service, helper;

  beforeEach(function () {
    angular.mock.Ao3RentCarsMock();
  });

  // beforeEach(inject(function (
  //     _$controller_,
  //     _$rootScope_,
  //     _$httpBackend_,
  //     _$location_,
  //     _Ao3RentCarsService_,
  //     _helperFactory_,
  // ) {
  //     $controller = _$controller_;
  //     $rootScope = _$rootScope_;
  //     $httpBackend = _$httpBackend_;
  //     $location = _$location_;
  //     service = _Ao3RentCarsService_;
  //     helper = _helperFactory_;

  // }));

  beforeEach(inject(function ($injector) {
    $controller = $injector.get("$controller");
    $rootScope = $injector.get("$rootScope");
    $httpBackend = $injector.get("$httpBackend");
    $location = $injector.get("$location");
    service = $injector.get("Ao3RentCarsService");
    helper = $injector.get("helperFactory");
  }));

  afterEach(function () {
    $httpBackend.verifyNoOutstandingExpectation();
    $httpBackend.verifyNoOutstandingRequest();
  });

  beforeEach(function () {
    $httpBackend.whenGET("acesso/login.tpl.html").respond(200, {});
  });

  describe("Testando Controller", function () {
    var vm, scope;

    beforeEach(function () {
      scope = $rootScope.$new();
      vm = $controller("HeaderController", {
        $rootScope: $rootScope,
        $scope: scope,
      });
    });
    // ENTRADA
    // SAÍDA
    // INIT
    // MÉT0DO TESTADO
    it(": expect(vm).toBeDefined()", function () {
      expect(vm).toBeDefined();
      $httpBackend.flush();
    });

    it(": expect($rootScope.userLogged).toEqual(null)", function () {
      // INIT
      // $location.path('/home');
      $rootScope.userLogged = { name: "teste" };
      // MÉT0DO TESTADO
      vm.logout();
      expect($rootScope.userLogged).toEqual(undefined);
      // expect($location.path()).toEqual('/login');
      $httpBackend.flush();
    });
  });
});
