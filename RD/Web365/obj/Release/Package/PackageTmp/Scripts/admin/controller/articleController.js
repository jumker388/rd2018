//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('article', ['$scope', '$http', '$controller', '$timeout', 'articleService',
  function ($scope, $http, $controller, $timeout, articleService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = articleService;

      $scope.formViewTemplate = '/Angular/Views/Admin/Article/detail.html';

      $scope.dataFilter = {
          name: '',
          typeId: null,
          groupId: null,
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          articleService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.listArticleGroup = res.listGroup;

              $scope.openModal('/Angular/Views/Admin/Article/edit.html');

          });
      };      

      $scope.loadPropertyFilter();
      $scope.loadData();

  }]);

'use strict';
web365app.controller('articleModified', ['$scope', '$http', '$controller', 'articleService',
  function ($scope, $http, $controller, articleService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          articleService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();
          });
      };

  }]);