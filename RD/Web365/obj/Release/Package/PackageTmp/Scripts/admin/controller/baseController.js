//create by BienLV2
//09-07-2014
//init properties, functions of all controller
web365app.controller('baseController', ['$rootScope', '$scope', '$http', '$sce', '$timeout', '$modal', 'utilityServices',
  function ($rootScope, $scope, $http, $sce, $timeout, $modal, utilityServices) {

      $scope.service = $scope.service != null ? $scope.service : {};

      $scope.formViewTemplate = $scope.formViewTemplate != null ? $scope.formViewTemplate : '';

      $scope.modalInstance = $scope.modalInstance != null ? $scope.modalInstance : null;

      $scope.data = $scope.data != null ? $scope.data : {};

      $scope.propertyFilter = $scope.propertyFilter != null ? $scope.propertyFilter : {};

      $scope.isLoadMore = $scope.isLoadMore != null ? $scope.isLoadMore : false;

      $scope.listId = [];

      $scope.listNumberRow = Web365Config.listNumberRow;

      $scope.listData = [];

      $scope.listTreeData = [];

      $scope.total = 0;      

      /*function data*/

      $scope.showAll = function () {

          $scope.service.showAll($scope.listId).then(function (response) {
              $scope.resetList();
          }, function (err) {
              console.log(err.error_description);
          });
      };

      $scope.hideAll = function () {

          $scope.service.hideAll($scope.listId).then(function (response) {
              $scope.resetList();
          }, function (err) {
              console.log(err.error_description);
          });
      };

      $scope.deleteAll = function () {

          $scope.service.deleteAll($scope.listId).then(function (response) {
              $scope.resetList();
          }, function (err) {
              console.log(err.error_description);
          });
      };

      $scope.showItem = function (id, name) {

          $scope.service.showAll(id).then(function (response) {

              utilityServices.notificationSuccess('Hiển thị thành công', 'Đã hiển thị ' + name);

              $scope.cancelShow();

              $scope.resetList();

          }, function (err) {
              console.log(err.error_description);
          });

      };

      $scope.hideItem = function (id, name) {

          $scope.service.hideAll(id).then(function (response) {

              utilityServices.notificationSuccess('Ẩn thành công', 'Đã ẩn ' + name);

              $scope.cancelHide();

              $scope.resetList();

          }, function (err) {
              console.log(err.error_description);
          });

      };

      $scope.deleteItem = function (id, name) {

          $scope.service.deleteAll(id).then(function (response) {

              utilityServices.notificationSuccess('Xóa thành công', 'Đã xóa ' + name);

              $scope.cancelDelete();

              $scope.resetList();

          }, function (err) {
              console.log(err.error_description);
          });

      };

      $scope.loadData = function (data) {

          $scope.dataFilter.currentRecord = ($scope.dataFilter.currentPage - 1) * $scope.dataFilter.numberRecord;

          //get data from services
          $scope.service.getList($scope.dataFilter).then(function (response) {

              $timeout(function () {

                  if(!$scope.isLoadMore){

                      $scope.total = response.total;

                      $scope.listData = response.list;

                  } else {

                      $scope.listData = $scope.listData.concat(response.list);

                  }

              }, 50);

          }, function (err) {
              console.log(err.error_description);
          });
      };

      $scope.loadPropertyFilter = function () {

          //get data from services
          $scope.service.getPropertyFilter().then(function (res) {

              $timeout(function () {

                  $scope.propertyFilter = res;

              }, 50);

          }, function (err) {
              console.log(err.error_description);
          });
      };

      $scope.loadDataTree = function (data) {

          //get data from services
          $scope.service.getListForTree().then(function (response) {

              $scope.listTreeData = response.list;

          }, function (err) {
              console.log(err.error_description);
          });
      };

      /*end function data*/

      /*event page list*/

      $scope.pageChanged = function () {

          $scope.resetList();
      };

      $scope.changeNumberRecord = function () {
          $scope.resetList();
      };

      $scope.loadMore = function () {

          $scope.dataFilter.currentPage += 1;

          $scope.isLoadMore = true;

          $scope.resetList();
      };

      $scope.filterList = function () {
          $scope.resetList();
      };      

      /*end event page list*/

      $scope.openModal = function (templateUrl) {

          $scope.modalInstance = $modal.open({
              templateUrl: templateUrl,
              scope: $scope
          });

      };

      $scope.view = function (data) {

          //get data from services
          $scope.service.getDetailForm(data).then(function (response) {

              $scope.data = response.data;

              $scope.modalInstance = $modal.open({
                  templateUrl: $scope.formViewTemplate,
                  scope: $scope
              });

          }, function (err) {
              console.log(err.error_description);
          });          

      };

      $scope.add = function (data) {

          $scope.data = null;

          $scope.loadDataForm(data);

      };

      $scope.edit = function (data) {

          $scope.cancel();

          $scope.loadDataForm(data);

      };

      $scope.delete = function (type, name, data) {

          $scope.modalDelete = $modal.open({
              template: '<div class="modal-header">' +
                            '<h3 class="modal-title">Xóa ' + type + '</h3>' +
                        '</div>' +
                        '<div class="modal-body">' +
                        '<p>Bạn có muốn xóa ' + name + '</p>' +
                      '</div>' +
                      '<div class="modal-footer">' +
                          '<button class="btn btn-primary" ng-click="deleteItem(' + data + ', \'' + name + '\')">Xóa</button>' +
                          '<button class="btn btn-warning" ng-click="cancelDelete()">Thoát</button>' +
                      '</div>',
              scope: $scope
          });

      };

      $scope.show = function (type, name, data) {

          $scope.modalShow = $modal.open({
              template: '<div class="modal-header">' +
                            '<h3 class="modal-title">Hiển thị ' + type + '</h3>' +
                        '</div>' +
                        '<div class="modal-body">' +
                        '<p>Bạn có muốn hiển thị ' + name + '</p>' +
                      '</div>' +
                      '<div class="modal-footer">' +
                          '<button class="btn btn-primary" ng-click="showItem(' + data + ', \'' + name + '\')">Hiển thị</button>' +
                          '<button class="btn btn-warning" ng-click="cancelShow()">Thoát</button>' +
                      '</div>',
              scope: $scope
          });

      };

      $scope.hide = function (type, name, data) {

          $scope.modalHide = $modal.open({
              template: '<div class="modal-header">' +
                            '<h3 class="modal-title">Ẩn ' + type + '</h3>' +
                        '</div>' +
                        '<div class="modal-body">' +
                        '<p>Bạn có muốn ẩn ' + name + '</p>' +
                      '</div>' +
                      '<div class="modal-footer">' +
                          '<button class="btn btn-primary" ng-click="hideItem(' + data + ', \'' + name + '\')">Ẩn</button>' +
                          '<button class="btn btn-warning" ng-click="cancelHide()">Thoát</button>' +
                      '</div>',
              scope: $scope
          });

      };

      $scope.cancel = function () {

          $scope.data = null;

          if ($scope.modalInstance != null) {

              $scope.modalInstance.dismiss('cancel');

              $scope.modalInstance = null;

          }          

      };

      $scope.cancelDelete = function () {

          $scope.modalDelete.dismiss('cancel');

      };

      $scope.cancelShow = function () {

          $scope.modalShow.dismiss('cancel');

      };

      $scope.cancelHide = function () {

          $scope.modalHide.dismiss('cancel');

      };

      $scope.sortList = function ($event) {

          var self = $($event.target);

          $scope.dataFilter.propertyNameSort = self.attr('propertyNameSort');

          self.addClass('currentSorting');

          self.parent().find('.sorting_asc').not('.currentSorting').addClass('sorting').removeClass('sorting_asc');
          self.parent().find('.sorting_desc').not('.currentSorting').addClass('sorting').removeClass('sorting_desc');

          if (self.hasClass('sorting') || self.hasClass('sorting_asc')) {
              self.addClass('sorting_desc');
              self.removeClass('sorting sorting_asc currentSorting');
              $scope.dataFilter.descending = true;
          } else if (self.hasClass('sorting_desc')) {
              self.addClass('sorting_asc');
              self.removeClass('sorting sorting_desc currentSorting');
              $scope.dataFilter.descending = false;
          }

          $scope.resetList();
      };

      $scope.checkedAll = function ($event) {

          $scope.listId = [];

          $($event.target).closest('table').find('tr > td:first-child input:checkbox').each(function (i) {

              this.checked = $($event.target).is(':checked');

              if ($($event.target).is(':checked')) {

                  $scope.changeListId($(this).attr('data-id'));

              }              

          });

      };

      $scope.changeCheck = function (id) {

          $scope.changeListId(id);

      };

      $scope.resetList = function () {

          $scope.listId = [];

          $scope.loadData();

      };

      $scope.changeListId = function (id) {

          if($scope.listId.indexOf(id) == -1){

              $scope.listId.push(id);

          } else {

              $scope.listId = $.grep($scope.listId, function (n, i) {
                  return (n !== id);
              });

          }

      };

      //render string data to html
      $scope.renderHtml = function (htmlCode) {

          return $sce.trustAsHtml(htmlCode);

      };

      //format date
      $scope.formatDate = function (date) {

          return Web365Utility.formatDate(date);

      };

  }]);