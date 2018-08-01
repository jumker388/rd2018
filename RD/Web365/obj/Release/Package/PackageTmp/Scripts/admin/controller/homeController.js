//create by Duongnt19
//25-07-2014 
//control view app page
'use strict';
web365app.controller('dashboard', ['$scope', '$http', '$controller', '$timeout',
  function ($scope, $http, $controller, $timeout) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

  }]);