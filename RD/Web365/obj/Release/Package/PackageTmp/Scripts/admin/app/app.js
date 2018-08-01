var web365app = angular.module('web365app', ['ngRoute',
    'ui.bootstrap',
    'angular-loading-bar',
    'ngAnimate']);

web365app.config(['$routeProvider', '$locationProvider',
  function ($routeProvider, $locationProvider) {
      $routeProvider.
        when('/dashboard', {
            title: 'Quản trị website',
            templateUrl: '/Angular/Views/Admin/Home/index.html',
            controller: 'dashboard'
        }).
        when('/login', {
            title: 'Đăng nhập hệ thống',
            templateUrl: '/Angular/Views/Admin/Home/login.html',
            controller: 'userLogin'
        }).
        when('/product', {
            title: 'Quản lý sản phẩm',
            templateUrl: '/Angular/Views/Admin/Product/index.html',
            controller: 'product'
        }).
        when('/product-variant', {
            title: 'Quản lý kiểu sản phẩm',
            templateUrl: '/Angular/Views/Admin/ProductVariant/index.html',
            controller: 'productVariant'
        }).
        when('/product-type', {
            title: 'Quản lý loại sản phẩm',
            templateUrl: '/Angular/Views/Admin/ProductType/index.html',
            controller: 'productType'
        }).
        when('/product-type-group', {
            title: 'Quản lý nhóm loại sản phẩm',
            templateUrl: '/Angular/Views/Admin/ProductTypeGroup/index.html',
            controller: 'productTypeGroup'
        }).
        when('/product-distributor', {
            title: 'Quản lý nhà phân phối',
            templateUrl: '/Angular/Views/Admin/ProductDistributor/index.html',
            controller: 'productDistributor'
        }).
        when('/product-manufacturer', {
            title: 'Quản lý nhà sản xuất',
            templateUrl: '/Angular/Views/Admin/ProductManufacturer/index.html',
            controller: 'productManufacturer'
        }).
        when('/product-status', {
            title: 'Quản lý trạng thái sản phẩm',
            templateUrl: '/Angular/Views/Admin/ProductStatus/index.html',
            controller: 'productStatus'
        }).
        when('/product-group-attribute', {
            title: 'Quản lý nhóm thuộc tính sản phẩm',
            templateUrl: '/Angular/Views/Admin/ProductGroupAttribute/index.html',
            controller: 'productGroupAttribute'
        }).
        when('/product-attribute', {
            title: 'Quản lý thuộc tính sản phẩm',
            templateUrl: '/Angular/Views/Admin/ProductAttribute/index.html',
            controller: 'productAttribute'
        }).
        when('/product-filter', {
            title: 'Quản lý thuộc tính sản phẩm',
            templateUrl: '/Angular/Views/Admin/ProductFilter/index.html',
            controller: 'productFilter'
        }).
        when('/product-label', {
            title: 'Quản lý nhãn sản phẩm',
            templateUrl: '/Angular/Views/Admin/ProductLabel/index.html',
            controller: 'productLabel'
        }).
        when('/picture', {
            title: 'Quản lý hình ảnh',
            templateUrl: '/Angular/Views/Admin/Picture/index.html',
            controller: 'picture'
        }).
        when('/article-type', {
            title: 'Quản lý loại bài viết',
            templateUrl: '/Angular/Views/Admin/ArticleType/index.html',
            controller: 'articleType'
        }).
        when('/group-type-article', {
            title: 'Quản lý nhóm loại bài viết',
            templateUrl: '/Angular/Views/Admin/ArticleTypeGroup/index.html',
            controller: 'articleTypeGroup'
        }).
        when('/article-group', {
            title: 'Quản lý nhóm bài viết',
            templateUrl: '/Angular/Views/Admin/ArticleGroup/index.html',
            controller: 'articleGroup'
        }).
        when('/article', {
            title: 'Quản lý bài viết',
            templateUrl: '/Angular/Views/Admin/Article/index.html',
            controller: 'article'
        }).
        when('/picture-type', {
            title: 'Quản lý loại hình ảnh',
            templateUrl: '/Angular/Views/Admin/PictureType/index.html',
            controller: 'pictureType'
        }).
        when('/user', {
            title: 'Quản lý người dùng',
            templateUrl: '/Angular/Views/Admin/User/index.html',
            controller: 'user'
        }).
        when('/role', {
            title: 'Quản lý quyền truy cập',
            templateUrl: '/Angular/Views/Admin/UserRole/index.html',
            controller: 'userRole'
        }).
        when('/page', {
            title: 'Quản lý trang quản trị',
            templateUrl: '/Angular/Views/Admin/UserPage/index.html',
            controller: 'userPage'
        }).
        when('/profile', {
            title: 'Thông tin người dùng',
            templateUrl: '/Angular/Views/Admin/User/profile.html',
            controller: 'profile'
        }).
        when('/advertise', {
            title: 'Quản lý quảng cáo',
            templateUrl: '/Angular/Views/Admin/Advertise/index.html',
            controller: 'advertise'
        }).
        when('/support-type', {
            title: 'Quản lý nhóm hỗ trợ',
            templateUrl: '/Angular/Views/Admin/SupportType/index.html',
            controller: 'supportType'
        }).
        when('/support', {
            title: 'Quản lý hỗ trợ',
            templateUrl: '/Angular/Views/Admin/Support/index.html',
            controller: 'support'
        }).
        when('/file-type', {
            title: 'Quản lý nhóm file',
            templateUrl: '/Angular/Views/Admin/FileType/index.html',
            controller: 'fileType'
        }).
        when('/file', {
            title: 'Quản lý file',
            templateUrl: '/Angular/Views/Admin/File/index.html',
            controller: 'file'
        }).
        when('/customer', {
            title: 'Quản lý khách hàng',
            templateUrl: '/Angular/Views/Admin/Customer/index.html',
            controller: 'customer'
        }).
        when('/order', {
            title: 'Quản lý đơn hàng',
            templateUrl: '/Angular/Views/Admin/Order/index.html',
            controller: 'order'
        }).
        when('/layout-content', {
            title: 'Quản lý nội dung web',
            templateUrl: '/Angular/Views/Admin/LayoutContent/index.html',
            controller: 'layoutContent'
        }).
        when('/contact', {
            title: 'Quản lý liên hệ',
            templateUrl: '/Angular/Views/Admin/Contact/index.html',
            controller: 'contact'
        }).
        when('/receive-info-group', {
            title: 'Quản lý nhận thông tin',
            templateUrl: '/Angular/Views/Admin/ReceiveInfoGroup/index.html',
            controller: 'receiveInfoGroup'
        }).
        when('/receive-info', {
            title: 'Quản lý nhóm nhận thông tin',
            templateUrl: '/Angular/Views/Admin/ReceiveInfo/index.html',
            controller: 'receiveInfo'
        }).
        when('/test-type', {
            title: 'Quản lý bài kiểm tra',
            templateUrl: '/Angular/Views/Admin/TestType/index.html',
            controller: 'testType'
        }).
        when('/test-question', {
            title: 'Quản lý bài câu hỏi',
            templateUrl: '/Angular/Views/Admin/TestQuestion/index.html',
            controller: 'testQuestion'
        }).
        when('/test-result', {
            title: 'Quản lý bài kết quả kiểm tra',
            templateUrl: '/Angular/Views/Admin/TestResult/index.html',
            controller: 'testResult'
        }).
        when('/menu', {
            title: 'Quản lý menu website',
            templateUrl: '/Angular/Views/Admin/Menu/index.html',
            controller: 'menu'
        }).
        otherwise({
            redirectTo: '/dashboard'
        });

      // Specify HTML5 mode (using the History APIs) or HashBang syntax.
      //$locationProvider.html5Mode(true).hashPrefix('!');

  }]);

web365app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

web365app.config(function ($compileProvider) {
    // Set the whitelist for certain URLs just to be safe
    $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|file|tel):/);
});

//cross domain for application file system
web365app.config(['$httpProvider',
    function ($httpProvider) {
        //$http.defaults.headers.get.Authorization = "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==";
        //$http.defaults.headers.post.Authorization = "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==";
        //$httpProvider.defaults.headers.common.Authorization = 'Basic YmVlcDpib29w';
        //$httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
        $httpProvider.defaults.useXDomain = true;
        delete $httpProvider.defaults.headers.common['X-Requested-With'];
    }
]);

web365app.run(function ($http, $rootScope, $timeout) {

    $rootScope.$on("$routeChangeSuccess", function (event, currentRoute, previousRoute) {

        $timeout(function () {

            $rootScope.title = currentRoute.$$route.title;

            $('.nav-list li').removeClass('active');

            $('.nav-list a[href="#' + currentRoute.$$route.originalPath + '"]').parents('li').addClass('active');

        }, 0);

    });

    $rootScope
        .$on('$viewContentLoading',
            function (event, viewConfig) {

            });

    $rootScope
        .$on('$viewContentLoaded',
            function (event, viewConfig) {

            });


});