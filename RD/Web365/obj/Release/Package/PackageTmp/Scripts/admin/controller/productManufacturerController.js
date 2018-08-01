//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('productManufacturer', ['$scope', '$http', '$controller', '$timeout', 'productManufacturerService',
  function ($scope, $http, $controller, $timeout, productManufacturerService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = productManufacturerService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ProductManufacturer/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          productManufacturerService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/ProductManufacturer/edit.html');

          });
      };      

      $scope.loadData();

  }]);

'use strict';
web365app.controller('productManufacturerModified', ['$scope', '$http', '$controller', 'productManufacturerService',
  function ($scope, $http, $controller, productManufacturerService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          productManufacturerService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);