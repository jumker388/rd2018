//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('productFilter', ['$scope', '$http', '$controller', '$timeout', 'productFilterService',
  function ($scope, $http, $controller, $timeout, productFilterService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.service = productFilterService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ProductFilter/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };

      $scope.loadDataForm = function (data) {

          productFilterService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/ProductFilter/edit.html');

          });

      };      

      $scope.loadData();

      $scope.loadDataTree();

  }]);

'use strict';
web365app.controller('productFilterModified', ['$scope', '$http', '$controller', 'productFilterService',
  function ($scope, $http, $controller, productFilterService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          productFilterService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };      

  }]);