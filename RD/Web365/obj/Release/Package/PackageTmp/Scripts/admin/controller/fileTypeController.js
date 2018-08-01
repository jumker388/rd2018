//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('fileType', ['$scope', '$http', '$controller', '$timeout', 'fileTypeService',
  function ($scope, $http, $controller, $timeout, fileTypeService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = fileTypeService;

      $scope.formViewTemplate = '/Angular/Views/Admin/FileType/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          fileTypeService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/FileType/edit.html');

          });
      };      

      $scope.loadData();

      $scope.loadDataTree();

  }]);

'use strict';
web365app.controller('fileTypeModified', ['$scope', '$http', '$controller', 'fileTypeService',
  function ($scope, $http, $controller, fileTypeService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          fileTypeService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);