//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('pictureType', ['$scope', '$http', '$controller', '$timeout', 'pictureTypeService',
  function ($scope, $http, $controller, $timeout, pictureTypeService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = pictureTypeService;

      $scope.formViewTemplate = '/Angular/Views/Admin/PictureType/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          pictureTypeService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/PictureType/edit.html');

          });
      };      

      $scope.loadData();

      $scope.loadDataTree();

  }]);

'use strict';
web365app.controller('pictureTypeModified', ['$scope', '$http', '$controller', 'pictureTypeService',
  function ($scope, $http, $controller, pictureTypeService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          pictureTypeService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);