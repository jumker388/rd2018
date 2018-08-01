//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('productDistributor', ['$scope', '$http', '$controller', '$timeout', 'productDistributorService',
  function ($scope, $http, $controller, $timeout, productDistributorService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = productDistributorService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ProductDistributor/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          productDistributorService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/ProductDistributor/edit.html');

          });
      };      

      $scope.loadData();

  }]);

'use strict';
web365app.controller('productDistributorpModified', ['$scope', '$http', '$controller', 'productDistributorService',
  function ($scope, $http, $controller, productDistributorService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          productDistributorService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);