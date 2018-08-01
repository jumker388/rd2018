//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('menu', ['$scope', '$http', '$controller', '$timeout', 'menuService',
  function ($scope, $http, $controller, $timeout, menuService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = menuService;

      $scope.formViewTemplate = '/Angular/Views/Admin/Menu/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          menuService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/Menu/edit.html');

          });
      };      

      $scope.loadData();

      $scope.loadDataTree();

  }]);

'use strict';
web365app.controller('menuModified', ['$scope', '$http', '$controller', 'menuService',
  function ($scope, $http, $controller, menuService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          menuService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);