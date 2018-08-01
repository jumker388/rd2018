//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('order', ['$scope', '$http', '$controller', '$timeout', 'orderService',
  function ($scope, $http, $controller, $timeout, orderService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = orderService;

      $scope.formViewTemplate = '/Angular/Views/Admin/Order/detail.html';

      $scope.dataFilter = {
          id: null,
          customerName: '',
          email: '',
          phone: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true,
          isViewed: null
      };            

      $scope.loadDataForm = function (data) {
          orderService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listOrderStatus = [
                  { ID: 1, Name: 'Đang chờ' },
                  { ID: 2, Name: 'Đã hủy' },
                  { ID: 3, Name: 'Hoàn thành' }
              ];

              $scope.openModal('/Angular/Views/Admin/Order/edit.html');

          });
      };      

      $scope.loadData();

  }]);

'use strict';
web365app.controller('orderModified', ['$scope', '$http', '$controller', 'orderService',
  function ($scope, $http, $controller, orderService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          orderService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);