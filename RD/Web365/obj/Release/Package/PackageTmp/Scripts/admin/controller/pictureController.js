//create by BienlV
//08-08-2014 
//control view product type page
'use strict';
web365app.controller('picture', ['$scope', '$http', '$controller', '$timeout', '$modal', 'pictureService', 'utilityServices',
  function ($scope, $http, $controller, $timeout, $modal, pictureService, utilityServices) {

      //extend baseController
      $.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.service = pictureService;

      $scope.dataFilter = {
          name: '',
          fileName: '',
          typePictureId: null,
          currentRecord: 0,
          numberRecord: $scope.listNumberRow[1],
          currentPage: 1,
          propertyNameSort: 'ID',
          descending: true,
          isShow: null
      };      

      $scope.loadDataForm = function (data) {

          pictureService.getEditForm(data).then(function (res) {

              $scope.data = res.data;

              $scope.listTreeDataForm = res.listType;

              $scope.openModal('/Angular/Views/Admin/Picture/edit.html');

          });

      };      

      $scope.uploadImages = function () {

          $scope.modalUploadPicture = $modal.open({
              templateUrl: 'Angular/Views/Admin/Picture/upload-images.html',
              scope: $scope
          });
          
      };      

      $scope.checkedPicture = function ($event, id) {

          var _self = $event.target.localName == 'i' ? $($event.target) : $($event.target).find('i');

          _self.toggleClass('icon-check-empty').toggleClass('icon-check');

          $scope.changeListId(id);

      };

      $scope.resetListPicture = function () {

          $scope.listData = [];

          $scope.dataFilter.currentRecord = 0;

          $scope.dataFilter.currentPage = 1;

          $scope.loadData();

      };

	  $scope.loadPropertyFilter();
      $scope.loadData();

  }]);

'use strict';
web365app.controller('pictureModified', ['$scope', '$http', '$controller', 'pictureService',
  function ($scope, $http, $controller, pictureService) {

      //extend baseController
      //$.extend(this, $controller('baseController', { $scope: $scope }));

      $scope.submit = function (form) {

          $scope.dataForm = $(form.target).serialize();

          pictureService.modified($scope.dataForm).then(function (res) {

              $scope.resetList();

              $scope.cancel();

          });
      };

  }]);