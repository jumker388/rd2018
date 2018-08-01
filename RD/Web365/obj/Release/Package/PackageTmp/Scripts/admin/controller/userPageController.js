//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('userPage', ['$scope', '$http', '$controller', '$timeout', 'userPageService',
  function ($scope, $http, $controller, $timeout, userPageService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = userPageService;

      $scope.formViewTemplate = '/Angular/Views/Admin/UserPage/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          userPageService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listTree;

              $scope.openModal('/Angular/Views/Admin/UserPage/edit.html');

          });
      };      

      $scope.loadData();

  }]);

'use strict';
web365app.controller('userPageModified', ['$scope', '$http', '$controller', 'userPageService',
  function ($scope, $http, $controller, userPageService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          userPageService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);