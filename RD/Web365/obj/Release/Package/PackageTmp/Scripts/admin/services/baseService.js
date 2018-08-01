//create by BienLV2
//15-07-2014
//services use to product type service
'use strict';
web365app.factory('baseService', ['$q', '$http', '$timeout', function ($q, $http, $timeout) {

    var baseService = {};

    //function set controller link
    var _setCtrl = function (ctrl) {

        baseService.url = '/admin/' + ctrl + '/';
        
    };

    //function get list product type
    var _getList = function (obj) {

        $timeout(function () {

            var deferred = $q.defer();

            //get data from services
            $http.get(baseService.url + 'GetList', {
                params: obj
            }).success(function (data) {
                deferred.resolve(data);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;

        }, 0);        
    };

    //function get list for tree
    var _getListForTree = function () {

        var deferred = $q.defer();

        //get data from services
        $http.get(baseService.url + 'GetListForTree').success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _getPropertyFilter = function () {

        var deferred = $q.defer();

        //get data from services
        $http.get(baseService.url + 'GetPropertyFilter').success(function (data) {
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
        $http.get(baseService.url + 'Detail', {
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
        $http.get(baseService.url + 'EditForm', {
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
        $http.post(baseService.url + 'Action', obj, {
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

        $http.post(baseService.url + 'ShowAll', {
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

        $http.post(baseService.url + 'HideAll', {
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

        $http.post(baseService.url + 'Delete', {
            listId: listId
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _nameExist = function (id, name) {

        var deferred = $q.defer();

        $http.post(baseService.url + 'NameExist', {
            id: id,
            name: name
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    baseService.setCtrl = _setCtrl;
    baseService.getList = _getList;
    baseService.getListForTree = _getListForTree;
    baseService.getPropertyFilter = _getPropertyFilter;
    baseService.getDetailForm = _getDetailForm;
    baseService.getEditForm = _getEditForm;
    baseService.modified = _modified;
    baseService.showAll = _showAll;
    baseService.hideAll = _hideAll;
    baseService.deleteAll = _deleteAll;
    baseService.nameExist = _nameExist;

    return baseService;
}]);