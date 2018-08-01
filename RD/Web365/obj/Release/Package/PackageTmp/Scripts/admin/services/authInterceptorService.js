'use strict';
web365app.factory('authInterceptorService', ['$q', '$location', '$rootScope', '$timeout',
    function ($q, $location, $rootScope, $timeout) {

        var authInterceptorServiceFactory = {};

        var _request = function (config) {

            config.headers = config.headers || {};

            config.headers.Authorization = 'Basic dGVzdHVzZXJuYW1lOnRlc3RwYXNzd29yZA==';

            return config;
        }

        var _responseError = function (rejection) {

            if (rejection.status === 401) {
                window.location.href = '/Admin/Login';
            }
            return $q.reject(rejection);
        }

        var _response = function (response) {

            return response;
        }

        authInterceptorServiceFactory.request = _request;
        authInterceptorServiceFactory.response = _response;
        authInterceptorServiceFactory.responseError = _responseError;

        return authInterceptorServiceFactory;
    }]);