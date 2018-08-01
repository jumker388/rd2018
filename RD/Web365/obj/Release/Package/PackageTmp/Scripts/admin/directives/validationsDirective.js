'use strict';
web365app.directive('nameOnly', ['$http', 'otherServices',
    function ($http, otherServices) {
        return {
            require: 'ngModel',
            link: function (scope, ele, attrs, c) {

                scope.$watch(attrs.ngModel, function (n, o) {

                    otherServices.nameExist(attrs.datactrl, attrs.dataid, n).then(function (res) {
                        c.$setValidity('unique', res.exist);
                    });

                });
            }
        }
    }]);