//create by BienLV2
//15-07-2014
//services use to other services
'use strict';
web365app.factory('otherServices', ['$q', '$templateCache', '$http', function ($q, $templateCache, $http) {

    var otherServices = {};

    var _nameExist = function (ctrl, id, name) {

        var deferred = $q.defer();

        $http.post('/admin/' + ctrl + '/NameExist', {
            id: id,
            name: name
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };

    otherServices.nameExist = _nameExist;

    return otherServices;
}]);