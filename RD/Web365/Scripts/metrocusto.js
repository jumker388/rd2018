$("#frmLogin").validate({
    rules: {
        Username: {
            minlength: 5,
            required: true
        },
        password: {
            required: true,
        }
    },
    submitHandler: function (form) {
        $('#m_login_signin_submit').addClass("m-loader m-loader--right m-loader--light");
        var loginModel = {
            userName: $('#lguser').val(),
            password: $('#lgpass').val(),
            psesistCookie: $('#lgremem').is(":checked")
        };

        $.post("/admin/Login/Login", loginModel, function (res) {

            if (res.status) {
                window.location.href = '/Admin/Home';
            } else {
                $('#msg').text(res.message);
                setTimeout(function () { $('#msg').text(''); }, 3000);
            }

        });
        
        $('#m_login_signin_submit').removeClass("m-loader m-loader--right m-loader--light");

    }
});