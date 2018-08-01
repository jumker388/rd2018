//create by BienLV
//14-09-2014 
//use for layout
'use strict';
web365app.controller('layout', ['$scope', '$http', '$controller', '$timeout', 'userService', 'userPageService',
  function ($scope, $http, $controller, $timeout, userService, userPageService) {

      $scope.listPage = [];

      userPageService.getPageOfUser().then(function (res) {
          
          $scope.listPage = res.list;

      });

      $scope.logout = function () {

          userService.logout().then(function (res) {

              window.location.href = '/Admin/Login';

          });

      };

      $scope.parserDate = function (date) {

          return date.toString().replace(/\/|Date\(|\)/gi,'');

      };

  }]);