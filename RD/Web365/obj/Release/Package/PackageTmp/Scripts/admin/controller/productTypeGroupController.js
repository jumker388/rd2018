//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('productTypeGroup', ['$scope', '$http', '$controller', '$timeout', 'productTypeGroupService', 'productTypeService',
  function ($scope, $http, $controller, $timeout, productTypeGroupService, productTypeService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = productTypeGroupService;

      $scope.formViewTemplate = '/Angular/Views/Admin/ProductTypeGroup/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          productTypeGroupService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.openModal('/Angular/Views/Admin/ProductTypeGroup/edit.html');

          });
      };

      $scope.typeOfGrroup = function (id, name) {
          productTypeService.getListOfGroup(id).then(function (res) {

              $scope.listType = res.list

              $scope.dataGroupType = {
                  id: id,
                  name: name,
                  arrTypeId: []
              };

              angular.forEach($scope.listType, function (v, k) {
                  $scope.dataGroupType.arrTypeId.push(v.ID);
              });

              $scope.openModal('/Angular/Views/Admin/ProductTypeGroup/type-of-group.html');

          });
      };

      $scope.loadData();

  }]);

'use strict';
web365app.controller('productTypeGroupModified', ['$scope', '$http', '$controller', 'productTypeGroupService', 'utilityServices',
  function ($scope, $http, $controller, productTypeGroupService, utilityServices) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          if ($scope.modified_form.$valid) {

              $scope.dataForm = $(form.target).serialize();

              productTypeGroupService.modified($scope.dataForm).then(function (res) {

                  $scope.resetList();

                  $scope.cancel();

              });

          } else {

              utilityServices.notificationWarning('Thông báo', 'Bạn cần nhập dữ liệu cần thiết');

          }          
      };

      $scope.submitTypeOfGroup = function (form) {

          $scope.dataForm = $(form.target).serialize();

          productTypeGroupService.addTypeForGroup($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

              utilityServices.notificationSuccess('Thông báo', 'Cập nhật dữ liệu thành công');

          });
      };

      $scope.removeType = function ($event) {

          $($event.target).closest('tr').remove();

          $('input:hidden[name="typeId"]').val($('.list-sort-able').sortable('toArray', { attribute: 'data-id' }));

      };

  }]);