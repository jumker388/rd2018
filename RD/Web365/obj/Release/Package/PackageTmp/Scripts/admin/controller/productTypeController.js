//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('productType', ['$scope', '$http', '$controller', '$timeout', 'productTypeService',
  function ($scope, $http, $controller, $timeout, productTypeService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.service = productTypeService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ProductType/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };

      $scope.loadDataForm = function (data) {

          productTypeService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.listGroup = res.listGroup;

              $scope.openModal('/Angular/Views/Admin/ProductType/edit.html');

          });

      };      

      $scope.loadData();

      $scope.loadDataTree();

  }]);

'use strict';
web365app.controller('productTypeModified', ['$scope', '$http', '$controller', 'productTypeService',
  function ($scope, $http, $controller, productTypeService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          productTypeService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };      

  }]);