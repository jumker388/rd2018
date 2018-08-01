//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('productVariant', ['$scope', '$http', '$controller', '$timeout', 'productVariantService',
  function ($scope, $http, $controller, $timeout, productVariantService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.service = productVariantService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ProductVariant/detail.html';

      $scope.dataFilter = {
          name: '',
          code: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };

      $scope.loadDataForm = function (data) {

          productVariantService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listProduct = res.listProduct;

              $scope.openModal('/Angular/Views/Admin/ProductVariant/edit.html');

          });

      };      

      $scope.loadPropertyFilter();
      $scope.loadData();

  }]);

'use strict';
web365app.controller('productVariantModified', ['$scope', '$http', '$controller', 'productVariantService',
  function ($scope, $http, $controller, productVariantService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          productVariantService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };      

  }]);