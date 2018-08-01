//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('product', ['$scope', '$http', '$controller', '$timeout', 'productService', 'productFilterService', 'productAttributeService', 'productGroupAttributeService',
  function ($scope, $http, $controller, $timeout, productService, productFilterService, productAttributeService, productGroupAttributeService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = productService;

      $scope.formViewTemplate = '/Angular/Views/Admin/Product/detail.html';

      $scope.dataFilter = {
          name: '',
          serial: '',
          typeId: null,
          manuId: null,
          distributorId: null,
          statusId: null,
          labelId: null,
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };            

      $scope.loadDataForm = function (data) {
          productService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.listManufacturer = res.listManufacturer;

              $scope.listDistributor = res.listDistributor;

              $scope.listStatus = res.listStatus;

              $scope.listLabel = res.listLabel;

              $scope.openModal('/Angular/Views/Admin/Product/edit.html');

          });
      };

      $scope.loadFilterForm = function (id) {
          productFilterService.getListForTree().then(function (res) {

              $scope.listFilter = res.list;

              productService.getEditFormFilter(id).then(function (r) {

                  $scope.dataFilter = r.data                  

                  $scope.openModal('/Angular/Views/Admin/Product/filter.html');
              });              

          });
      };

      $scope.loadAttributeForm = function (productId, productName, typeId) {

          $scope.dataFormAttribute = {
              id: productId,
              name: productName
          };

          productGroupAttributeService.getListByProductType(typeId).then(function (r) {

              $scope.listGroupAttribute = r.list;

              productAttributeService.getListByProductType(typeId, productId).then(function (r1) {

                  $scope.listAttribute = r1.list;

                  $scope.openModal('/Angular/Views/Admin/Product/attribute.html');

              });

          });
      };

      $scope.loadPropertyFilter();
      $scope.loadData();

  }]);

'use strict';
web365app.controller('productModified', ['$scope', '$http', '$controller', 'utilityServices', 'productService', 'productAttributeService',
  function ($scope, $http, $controller, utilityServices, productService, productAttributeService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          if ($scope.modified_form.$valid) {

              $scope.dataForm = $(form.target).serialize();

              productService.modified($scope.dataForm).then(function (res) {

                  $scope.resetList();

                  $scope.cancel();
              });
          } else {

              utilityServices.notificationWarning('Thông báo', 'Bạn cần nhập dữ liệu cần thiết');

          }
      };

      $scope.submitFilter = function (form) {

          $scope.dataForm = $(form.target).serialize();

          productService.addFilter($scope.dataForm).then(function (res) {

              $scope.cancel();

          });
      };

      $scope.submitAttribute = function (form) {

          $scope.dataForm = $(form.target).serialize();

          productAttributeService.addProductAttribute($scope.dataForm).then(function (res) {
              utilityServices.notificationSuccess('Lưu thành công', 'Đã lưu thông tin thuộc tính thành công');
          });
      };

  }]);