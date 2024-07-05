$(document).ready(function () {
    //Search Product
    $(document).on("keyup", "#input-search", function () {
        $(".searchList li").remove();
        let value = $("#input-search").val();
        if (value) {
            $.ajax({
                url: "/home/search?value=" + value,
                method: "get",
                success: function (datas) {
                    $(".searchList").append(datas);
                },
                error: function (error) {
                    console.log(error)
                },
            });
        }
    });

    //Search Blog
    $(document).on("keyup", "#input-search", function () {
        $(".searchList li").remove();
        let value = $("#input-search").val();
        if (value) {
            $.ajax({
                url: "/blog/search?value=" + value,
                method: "get",
                success: function (datas) {
                    $(".searchList").append(datas);
                },
                error: function (error) {
                    console.log(error)
                },
            });
        }
    });


    //Load More btn
    let skip = 3;
    $(document).on("click", ".loadMoreBtn", function () {
        $.ajax({
            url: "/blog/loadmore?offset=" + skip,
            method: "get",
            success: function (datas) {
                const count = $(".count").html();
                $(".blogList").append(datas);
                skip += 3;
                if (skip >= count) {
                    $(".loadMoreBtn").remove();
                }
            },
            error: function (error) {
                console.log(error)
            },
        });
    });

    // HEADER
    $(document).on('click', '#search', function () {
        $(this).next().toggle();
    });

    $(document).on('click', '#mobile-navbar-close', function () {
        $(this).parent().removeClass("active");
    });

    $(document).on('click', '#mobile-navbar-show', function () {
        $('.mobile-navbar').addClass("active");
    });

    $(document).on('click', '.mobile-navbar ul li a', function () {
        if ($(this).children('i').hasClass('fa-caret-right')) {
            $(this).children('i').removeClass('fa-caret-right').addClass('fa-sort-down');
        } else {
            $(this).children('i').removeClass('fa-sort-down').addClass('fa-caret-right');
        }
        $(this).parent().next().slideToggle();
    });

    // SLIDER
    $(".slider").owlCarousel({
        items: 1,
        loop: true,
        autoplay: true
    });

    // PRODUCT
    $(document).on('click', '.categories', function (e) {
        e.preventDefault();
        $(this).next().next().slideToggle();
    });

    $(document).on('click', '.category li a', function (e) {
        e.preventDefault();
        let category = $(this).attr('data-id');
        let products = $('.product-item');

        products.each(function () {
            if (category == $(this).attr('data-id')) {
                $(this).parent().fadeIn();
            } else {
                $(this).parent().hide();
            }
        });
        if (category == 'all') {
            products.parent().fadeIn();
        }
    });

    // ACCORDION
    $(document).on('click', '.question', function () {
        $(this).siblings('.question').children('i').removeClass('fa-minus').addClass('fa-plus');
        $(this).siblings('.answer').not($(this).next()).slideUp();
        $(this).children('i').toggleClass('fa-plus').toggleClass('fa-minus');
        $(this).next().slideToggle();
        $(this).siblings('.active').removeClass('active');
        $(this).toggleClass('active');
    });

    // TAB
    $(document).on('click', 'ul li', function () {
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().next().children('p.active').removeClass('active');

        $(this).parent().next().children('p').each(function () {
            if (dataId == $(this).attr('data-id')) {
                $(this).addClass('active');
            }
        });
    });

    $(document).on('click', '.tab4 ul li', function () {
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().parent().next().children().children('p.active').removeClass('active');

        $(this).parent().parent().next().children().children('p').each(function () {
            if (dataId == $(this).attr('data-id')) {
                $(this).addClass('active');
            }
        });
    });

    // INSTAGRAM
    $(".instagram").owlCarousel({
        items: 4,
        loop: true,
        autoplay: true,
        responsive: {
            0: {
                items: 1
            },
            576: {
                items: 2
            },
            768: {
                items: 3
            },
            992: {
                items: 4
            }
        }
    });

    $(".say").owlCarousel({
        items: 1,
        loop: true,
        autoplay: true
    });
});
