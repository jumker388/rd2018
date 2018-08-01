//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('userRole', ['$scope', '$http', '$controller', '$timeout', 'userRoleService', 'userPageService',
  function ($scope, $http, $controller, $timeout, userRoleService, userPageService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = userRoleService;

      $scope.formViewTemplate = '/Angular/Views/Admin/UserRole/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'RoleId',
          descending: true
      };            

      $scope.loadDataForm = function (data) {

          userRoleService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.list;

              $scope.openModal('/Angular/Views/Admin/UserRole/edit.html');

          });
      };

      $scope.pageOfRole = function (roleId) {
          
          userRoleService.getEditForm(roleId).then(function (res) {

              $scope.data = res.data;

              userPageService.getListForTree().then(function (res) {

                  $scope.listPage = res.list;

                  $scope.openModal('/Angular/Views/Admin/UserRole/page.html');

              });

          });         

      };

      $scope.loadData();

  }]);

'use strict';
web365app.controller('userRoleModified', ['$scope', '$http', '$controller', 'userRoleService',
  function ($scope, $http, $controller, userRoleService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          userRoleService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

      $scope.submitPage = function (form) {

          $scope.dataForm = $(form.target).serialize();

          userRoleService.pageForRole($scope.dataForm).then(function (res) {

              $scope.cancel();

          });
      };

  }]);