//create by BienLV2
//09-07-2014 
//info config setting for application
Web365Config = (function () {

    var web365config = {};

    //list record in list
    web365config.listNumberRow = [10, 25, 50, 100];

    //password encrypt
    web365config.PassEncrypt = '123456';

    //password encrypt
    web365config.NameCheckSum = 'checksum';

    //address server services
    //web365config.ServerServices = 'http://services.cafebongda.vn/';
    web365config.ServerServices = 'http://10.13.47.76:9000/';

    //address server thumb images
    web365config.UrlPicture = 'http://fptshop.com.vn/Uploads/Thumbs/';

    //address server originals images
    web365config.UrlOriginalsPicture = 'http://fptshop.com.vn/Originals/Thumbs/';

    return web365config;

})();