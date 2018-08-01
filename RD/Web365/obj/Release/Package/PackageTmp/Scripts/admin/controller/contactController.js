//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('contact', ['$scope', '$http', '$controller', '$timeout', 'contactService',
  function ($scope, $http, $controller, $timeout, contactService) {

      //extend baseController
      $controller('baseController', { $scope: $scope });
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.service = contactService;

      $scope.formViewTemplate = '/Angular/Views/Admin/Contact/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };

      $scope.loadDataForm = function (data) {

          contactService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/Contact/edit.html');

          });

      };      

      $scope.loadData();

  }]);

'use strict';
web365app.controller('contactModified', ['$scope', '$http', '$controller', 'contactService',
  function ($scope, $http, $controller, contactService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          if ($scope.modified_form.$valid) {

              $scope.dataForm = $(form.target).serialize();

              contactService.modified($scope.dataForm).then(function (res) {

                  $scope.resetList();

                  $scope.cancel();

              });

          }          
      };      

  }]);