//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('layoutContent', ['$scope', '$http', '$controller', '$timeout', 'layoutContentService',
  function ($scope, $http, $controller, $timeout, layoutContentService) {

      //extend baseController
      $controller('baseController', { $scope: $scope });
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.service = layoutContentService;

      $scope.formViewTemplate = '/Angular/Views/Admin/LayoutContent/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };

      $scope.loadDataForm = function (data) {

          layoutContentService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.openModal('/Angular/Views/Admin/LayoutContent/edit.html');

          });

      };      

      $scope.loadData();

  }]);

'use strict';
web365app.controller('layoutContentModified', ['$scope', '$http', '$controller', 'utilityServices', 'layoutContentService',
  function ($scope, $http, $controller, utilityServices, layoutContentService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          if ($scope.modified_form.$valid) {

              $scope.dataForm = $(form.target).serialize();

              layoutContentService.modified($scope.dataForm).then(function (res) {

                  $scope.resetList();

                  $scope.cancel();

              });
          } else {

              utilityServices.notificationWarning('Thông báo', 'Bạn cần nhập dữ liệu cần thiết');

          }
      };      

  }]);