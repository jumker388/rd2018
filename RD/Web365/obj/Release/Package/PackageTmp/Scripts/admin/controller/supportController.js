//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('support', ['$scope', '$http', '$controller', '$timeout', 'supportService',
  function ($scope, $http, $controller, $timeout, supportService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = supportService;

      $scope.formViewTemplate = '/Angular/Views/Admin/Support/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          supportService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listType = res.listType;

              $scope.openModal('/Angular/Views/Admin/Support/edit.html');

          });
      };      

      $scope.loadData();

  }]);

'use strict';
web365app.controller('supportModified', ['$scope', '$http', '$controller', 'supportService',
  function ($scope, $http, $controller, supportService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          supportService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);