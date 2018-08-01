//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('receiveInfo', ['$scope', '$http', '$controller', '$timeout', 'receiveInfoService',
  function ($scope, $http, $controller, $timeout, receiveInfoService) {

      //extend baseController
      $controller('baseController', { $scope: $scope });
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.service = receiveInfoService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ReceiveInfo/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };

      $scope.loadDataForm = function (data) {

          receiveInfoService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listGroup;

              $scope.openModal('/Angular/Views/Admin/ReceiveInfo/edit.html');

          });

      };      

      $scope.loadData();

  }]);

'use strict';
web365app.controller('receiveInfoModified', ['$scope', '$http', '$controller', 'receiveInfoService',
  function ($scope, $http, $controller, receiveInfoService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          if ($scope.modified_form.$valid) {

              $scope.dataForm = $(form.target).serialize();

              receiveInfoService.modified($scope.dataForm).then(function (res) {

                  $scope.resetList();

                  $scope.cancel();

              });

          }          
      };      

  }]);