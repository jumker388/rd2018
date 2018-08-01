//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('supportType', ['$scope', '$http', '$controller', '$timeout', 'supportTypeService',
  function ($scope, $http, $controller, $timeout, supportTypeService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = supportTypeService;

      $scope.formViewTemplate = '/Angular/Views/Admin/SupportType/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          supportTypeService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/SupportType/edit.html');

          });
      };      

      $scope.loadData();

  }]);

'use strict';
web365app.controller('supportTypeModified', ['$scope', '$http', '$controller', 'supportTypeService',
  function ($scope, $http, $controller, supportTypeService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          supportTypeService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);