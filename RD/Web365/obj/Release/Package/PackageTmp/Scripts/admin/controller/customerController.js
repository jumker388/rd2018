//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('customer', ['$scope', '$http', '$controller', '$timeout', 'customerService', 'utilityServices',
  function ($scope, $http, $controller, $timeout, customerService, utilityServices) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));      

      $scope.service = customerService;

      $scope.formViewTemplate = '/Angular/Views/Admin/Customer/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true
      };

      $scope.dataMail = {
          To: ''
      };

      $scope.loadDataForm = function (data) {
          customerService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/Customer/edit.html');

          });
      };

      $scope.formSendMail = function (data) {

          angular.forEach($scope.listData, function (v, i) {

              angular.forEach($scope.listId, function (v1, i1) {
                  
                  if(v.ID == v1){
                      $scope.dataMail.To += ($scope.dataMail.To == '' ? '' : ';') + v.Email;
                  }

              });

          });

          $scope.openModal('/Angular/Views/Admin/Customer/send-mail.html');
      };

      $scope.sendMail = function (form) {

          customerService.sendMail($(form.target).serialize()).then(function (res) {

              $scope.cancel();

              utilityServices.notificationSuccess('Thông báo', 'Gửi mail thành công.');

          }, function (error) {

              utilityServices.notificationWarning('Thông báo', 'Có lỗi xảy ra không thể gửi mail, bạn hãy thử lại.');

          });
      };

      $scope.loadData();

      $scope.loadDataTree();

  }]);

'use strict';
web365app.controller('customerModified', ['$scope', '$http', '$controller', 'customerService',
  function ($scope, $http, $controller, customerService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          customerService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);