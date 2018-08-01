//create by BienLV
//09-07-2014
//functions utilies for project
function Web365Utility() { };

//add by BienLV
//09-07-2014
//function get address services
//param url: router of services
Web365Utility.getLinkServices = function (url) {
    return Web365Config.ServerServices + url;
};

//add by BienLV
//09-07-2014
//function formart price ex: '1000000' to '1.000.000'
//use Web365Utility.formatPrice('1000000')
//param str: string need format
Web365Utility.formatPrice = function (str) {
    return str.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
};

Web365Utility.formatDate = function (date) {

    var year = date.getFullYear();

    var month = (1 + date.getMonth()).toString();

    month = month.length > 1 ? month : '0' + month;

    var day = date.getDate().toString();

    day = day.length > 1 ? day : '0' + day;

    return day + '/' + month + '/' + year;
};

//add by BienLV
//16-07-2014
//function sort list param by name when you use method GET or POST
Web365Utility.sortParam = function sortObject(o) {

    var sorted = {},
    key, a = [];

    for (key in o) {
        if (o.hasOwnProperty(key)) {
            a.push(key);
        }
    }

    a.sort();

    for (key = 0; key < a.length; key++) {
        sorted[a[key]] = o[a[key]];
    }
    return sorted;
};

//add by DuongNT
//10-07-2014
//function encode data post and return object json. it is sent to the Web API via the POST method
//var ojb={name:'1',email:'duongnt13@ftp.com.vn'}
//use Web365Utility.encodeObjectJson(ojb)
//param jobject: json need format
Web365Utility.encodeObjectJson = function (jobject) {

    var str = '';

    if (jobject == null) {
        jobject = [];
    }

    jobject = Web365Utility.sortParam(jobject);

    for (var prop in jobject) {
        str += encodeURIComponent(jobject[prop]);
    }

    //str = encodeURIComponent(str);
    var hmac = Crypto.HMAC(Crypto.SHA1, str.toLowerCase(), Web365Config.PassEncrypt);

    jobject[Web365Config.NameCheckSum] = hmac;

    return jobject;
};

//add by phenv
//11-07-2014
Web365Utility.formatFilter = function (str1, str2) {
    if (str1 != '' && str2 != '') {
        return str1 + ',' + str2;
    }
    if (str1 != '' && str2 == '') {
        return str1;
    }
    if (str1 == '' && str2 != '') {
        return str2;
    }
    return '';
};

// Nghiatc2.
// Get path image.
Web365Utility.getLinkUrlPictures = function (url) {
    return Web365Config.UrlPicture + url;
};

Web365Utility.getBase64Image = function (imgElem) {

    var canvas = document.createElement("canvas");
    canvas.width = imgElem.clientWidth;
    canvas.height = imgElem.clientHeight;
    var ctx = canvas.getContext("2d");
    ctx.drawImage(imgElem, 0, 0);

    console.log(imgElem);

    var dataURL = canvas.toDataURL("image/png");

    return dataURL.replace(/^data:image\/(png|jpg);base64,/, '');
};

Web365Utility.removeUnicode = function (str) {

    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_|–|”|“|`/g, "-");

    str = str.replace(/-+-/g, "-");
    str = str.replace(/^\-+|\-+$/g, "");

    return str;
}