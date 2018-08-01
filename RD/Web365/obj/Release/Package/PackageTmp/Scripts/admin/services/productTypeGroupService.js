//create by BienLV2
//15-07-2014
//services use to product type service
'use strict';
web365app.factory('productTypeGroupService', ['$q', '$http', function ($q, $http) {

    var productTypeGroupService = {};

    //function get list product type
    var _getList = function (obj) {

        var deferred = $q.defer();

        //get data from services
        $http.get('/admin/ProductTypeGroup/GetList', {
            params: obj
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    //function get list for tree product type
    var _getListForTree = function (obj) {

        var deferred = $q.defer();

        //get data from services
        $http.get('/admin/ProductTypeGroup/GetListForTree').success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    //function get detail form picture type
    var _getDetailForm = function (id) {

        var deferred = $q.defer();

        //get data from services
        $http.get('/admin/ProductTypeGroup/Detail', {
            params: {
                id: id
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    //function get edit form product type
    var _getEditForm = function (id) {

        var deferred = $q.defer();

        //get data from services
        $http.get('/admin/ProductTypeGroup/EditForm', {
            params: {
                id: id
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    //function get edit form product type
    var _modified = function (obj) {

        var deferred = $q.defer();

        //get data from services
        $http.post('/admin/ProductTypeGroup/Action', obj, {
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _showAll = function (listId) {

        var deferred = $q.defer();

        $http.post('/admin/ProductTypeGroup/ShowAll', {
            listId: listId
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _hideAll = function (listId) {

        var deferred = $q.defer();

        $http.post('/admin/ProductTypeGroup/HideAll', {
            listId: listId
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _deleteAll = function (listId) {

        var deferred = $q.defer();

        $http.post('/admin/ProductTypeGroup/Delete', {
            listId: listId
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _addTypeForGroup = function (obj) {

        var deferred = $q.defer();

        $http.post('/admin/ProductTypeGroup/AddTypeForGroup', obj, {
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    productTypeGroupService.getList = _getList;
    productTypeGroupService.getListForTree = _getListForTree;
    productTypeGroupService.getDetailForm = _getDetailForm;
    productTypeGroupService.getEditForm = _getEditForm;
    productTypeGroupService.modified = _modified;
    productTypeGroupService.showAll = _showAll;
    productTypeGroupService.hideAll = _hideAll;
    productTypeGroupService.deleteAll = _deleteAll;
    productTypeGroupService.addTypeForGroup = _addTypeForGroup;

    return productTypeGroupService;
}]);