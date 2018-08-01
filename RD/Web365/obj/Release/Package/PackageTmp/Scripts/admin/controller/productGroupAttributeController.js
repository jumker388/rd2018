//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('productGroupAttribute', ['$scope', '$http', '$controller', '$timeout', 'productGroupAttributeService',
  function ($scope, $http, $controller, $timeout, productGroupAttributeService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.service = productGroupAttributeService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ProductGroupAttribute/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };

      $scope.loadDataForm = function (data) {

          productGroupAttributeService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listProductType;

              $scope.openModal('/Angular/Views/Admin/ProductGroupAttribute/edit.html');

          });

      };      

      $scope.loadData();

  }]);

'use strict';
web365app.controller('productGroupAttributeModified', ['$scope', '$http', '$controller', 'productGroupAttributeService',
  function ($scope, $http, $controller, productGroupAttributeService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          productGroupAttributeService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };      

  }]);