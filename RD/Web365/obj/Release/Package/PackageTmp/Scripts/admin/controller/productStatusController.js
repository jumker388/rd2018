//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('productStatus', ['$scope', '$http', '$controller', '$timeout', 'productStatusService',
  function ($scope, $http, $controller, $timeout, productStatusService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = productStatusService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ProductStatus/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          productStatusService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/ProductStatus/edit.html');

          });
      };      

      $scope.loadData();

  }]);

'use strict';
web365app.controller('productStatusModified', ['$scope', '$http', '$controller', 'productStatusService',
  function ($scope, $http, $controller, productStatusService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          productStatusService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);