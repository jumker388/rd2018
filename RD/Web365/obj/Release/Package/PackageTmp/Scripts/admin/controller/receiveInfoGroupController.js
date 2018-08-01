//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('receiveInfoGroup', ['$scope', '$http', '$controller', '$timeout', 'receiveInfoGroupService',
  function ($scope, $http, $controller, $timeout, receiveInfoGroupService) {

      //extend baseController
      $controller('baseController', { $scope: $scope });
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.service = receiveInfoGroupService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ReceiveInfoGroup/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };

      $scope.loadDataForm = function (data) {

          receiveInfoGroupService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/ReceiveInfoGroup/edit.html');

          });

      };      

      $scope.loadData();

  }]);

'use strict';
web365app.controller('receiveInfoGroupModified', ['$scope', '$http', '$controller', 'receiveInfoGroupService',
  function ($scope, $http, $controller, receiveInfoGroupService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          if ($scope.modified_form.$valid) {

              $scope.dataForm = $(form.target).serialize();

              receiveInfoGroupService.modified($scope.dataForm).then(function (res) {

                  $scope.resetList();

                  $scope.cancel();

              });

          }          
      };      

  }]);