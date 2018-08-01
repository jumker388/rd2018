//create by BienLV2
//15-07-2014
//services use to utility services
'use strict';
web365app.factory('utilityServices', ['$q', '$templateCache', '$http', function ($q, $templateCache, $http) {

    var utilityServices = {};

    var _notificationInfo = function (title, content) {

        $.gritter.add({
            title: title,
            text: content,
            class_name: 'gritter-info gritter-light'
        });
    };

    var _notificationSuccess = function (title, content) {

        $.gritter.add({
            title: title,
            text: content,
            class_name: 'gritter-success gritter-light'
        });
    };

    var _notificationWarning = function (title, content) {

        $.gritter.add({
            title: title,
            text: content,
            class_name: 'gritter-warning gritter-light'
        });
    };

    var _notificationError = function (title, content) {

        $.gritter.add({
            title: title,
            text: content,
            class_name: 'gritter-error gritter-light'
        });
    };

    var _loadTemplate = function (templateUrl) {

        var deferred = $q.defer();

        $http.get(templateUrl, {
            cache: $templateCache
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };

    utilityServices.notificationInfo = _notificationInfo;
    utilityServices.notificationSuccess = _notificationSuccess;
    utilityServices.notificationWarning = _notificationWarning;
    utilityServices.notificationError = _notificationError;
    utilityServices.loadTemplate = _loadTemplate;

    return utilityServices;
}]);