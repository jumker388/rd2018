//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('user', ['$scope', '$http', '$controller', '$timeout', 'userRoleService', 'userService',
  function ($scope, $http, $controller, $timeout, userRoleService, userService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.service = userService;

      $scope.formViewTemplate = '/Angular/Views/Admin/User/detail.html';

      $scope.dataFilter = {
          name: '',
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[0],
          currentPage: 1,
          propertyNameSort: 'UserId',
          descending: true
      };

      $scope.loadDataForm = function (data) {

          userService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.list;

              $scope.openModal('/Angular/Views/Admin/User/edit.html');

          });
      };

      $scope.roleOfUser = function (roleId) {

          userService.getEditForm(roleId).then(function (res) {

              $scope.data = res.data;

              userRoleService.getListForTree().then(function (res) {

                  $scope.listRole = res.list;

                  $scope.openModal('/Angular/Views/Admin/User/role.html');

              });

          });

      };

      $scope.loadData();

  }]);

'use strict';
web365app.controller('userModified', ['$scope', '$http', '$controller', 'userService',
  function ($scope, $http, $controller, userService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          userService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

      $scope.submitRole = function (form) {

          $scope.dataForm = $(form.target).serialize();

          userService.roleForUser($scope.dataForm).then(function (res) {

              $scope.cancel();

          });
      };

  }]);

'use strict';
web365app.controller('userLogin', ['$scope', '$http', '$controller', '$timeout', 'userService',
  function ($scope, $http, $controller, $timeout, userService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.loginModel = {
          userName: '',
          password: '',
          psesistCookie: false
      };

      $scope.login = function () {

          userService.login($scope.loginModel).then(function (res) {

              if (res.result) {
                  window.location.href = '/Admin#/dashboard'
              } else {
                  $('.message-login').html('<div class="alert alert-warning">' +
                                                '<button type="button" class="close" data-dismiss="alert">' +
                                                    '<i class="icon-remove"></i>' +
                                                '</button>' +
                                                '<strong>Cảnh báo!</strong>' +
                                              'Sai thông tin đăng nhập' +
                                              '<br>' +
                                          '</div>');
              }

          });

      };

  }]);

'use strict';
web365app.controller('profile', ['$scope', '$http', '$controller', '$timeout', 'userService',
  function ($scope, $http, $controller, $timeout, userService) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));

      userService.getDetailForm().then(function (res) {

          $scope.data = res.data;

      });

      $scope.changePass = function (form) {

          $scope.dataForm = $(form.target).serialize();

          userService.changePass($scope.dataForm).then(function (res) {

              alert(res.result);

          });

      };

  }]);