
$(document).ready(function () {
    
    $('.add-order').click(function (e) {
        e.preventDefault();

        var _product = $('input[name=' + $(this).attr('data-name') + ']:checked');

        if (_product.length > 0) {

            var list = [];

            var isCheck = -1;

            if (typeof $.cookie('_coc') !== 'undefined') {
                list = JSON.parse(Base64.decode($.cookie('_coc')));
            }

            $.each(list, function (i, v) {

                if (v.ProductID == parseInt(_product.attr('data-productid')) && v.ProductVariantID == parseInt(_product.attr('data-variantid'))) {
                    isCheck = i;

                }

            });

            if (isCheck == -1) {
                list.push({
                    ProductID: parseInt(_product.attr('data-productid')),
                    ProductName: _product.attr('data-productname'),
                    Picture: _product.attr('data-picture'),
                    ProductVariantID: parseInt(_product.attr('data-variantid')),
                    ProductVariantName: _product.attr('data-variantname'),
                    Price: parseFloat(_product.attr('data-price')),
                    Quantity: 1
                });
            }            

            $.cookie('_coc', Base64.encode(JSON.stringify(list)), { expires: 1, path: '/' });

            $('.cart').text('Giỏ hàng (' + list.length + ')');
            
            alert('Đã thêm "' + _product.attr('data-productname') + '" vào giỏ hàng.');

        }

    });

    $('.logout').click(function (e) {
        e.preventDefault();
        
        $.post('/Ajax/Customer/Logout', {}, function (res) {
            document.location.reload();
        }).complete(function () {

        });        

    });

    $('#search').submit(function (e) {
        e.preventDefault();

        var keyword = $(this).find('input[type="text"]').val();

        document.location.href = '/vn/tim-kiem?q=' + keyword;

    });

    $('#select-filter').submit(function (e) {
        e.preventDefault();

        var type = $(this).find('#product-type').val();

        var filter = $(this).find('#product-filter').val();

        document.location.href = '/vn/tim-kiem/' + type + '/' + filter;

    });

});