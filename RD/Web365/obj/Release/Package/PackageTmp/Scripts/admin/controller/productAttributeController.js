//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('productAttribute', ['$scope', '$http', '$controller', '$timeout', 'productAttributeService',
  function ($scope, $http, $controller, $timeout, productAttributeService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.service = productAttributeService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ProductAttribute/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };

      $scope.loadDataForm = function (data) {

          productAttributeService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listGroup;

              $scope.openModal('/Angular/Views/Admin/ProductAttribute/edit.html');

          });

      };      

      $scope.loadData();

      $scope.loadDataTree();

  }]);

'use strict';
web365app.controller('productAttributeModified', ['$scope', '$http', '$controller', 'productAttributeService',
  function ($scope, $http, $controller, productAttributeService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          productAttributeService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };      

  }]);