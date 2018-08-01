//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('productLabel', ['$scope', '$http', '$controller', '$timeout', 'productLabelService',
  function ($scope, $http, $controller, $timeout, productLabelService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.service = productLabelService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ProductLabel/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };

      $scope.loadDataForm = function (data) {

          productLabelService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/ProductLabel/edit.html');

          });

      };      

      $scope.loadData();

      $scope.loadDataTree();

  }]);

'use strict';
web365app.controller('productLabelModified', ['$scope', '$http', '$controller', 'productLabelService',
  function ($scope, $http, $controller, productLabelService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          productLabelService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };      

  }]);