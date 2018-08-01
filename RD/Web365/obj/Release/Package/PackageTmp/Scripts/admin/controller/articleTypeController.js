//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('articleType', ['$scope', '$http', '$controller', '$timeout', 'articleTypeService',
  function ($scope, $http, $controller, $timeout, articleTypeService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = articleTypeService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ArticleType/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          articleTypeService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.listGroup = res.listGroup;

              $scope.openModal('/Angular/Views/Admin/ArticleType/edit.html');

          });
      };      

      $scope.loadData();

      $scope.loadDataTree();

  }]);

'use strict';
web365app.controller('articleTypeModified', ['$scope', '$http', '$controller', 'articleTypeService',
  function ($scope, $http, $controller, articleTypeService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          articleTypeService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);