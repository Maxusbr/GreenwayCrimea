$(document).ready(function () {
    $.ajax({
        url: "../pageload",
        success: function (data) {
            $(".main-container").html(data);
        }
    })

    //var parceCategory = setInterval(getProcess, 500);

    //function getProcess() {
    //    $.ajax({
    //        url: "../ComparisonCategory/GetParsingCategoryStatus",
    //        success: function (data) {
    //            if (data.status == "noprocess") {
    //                clearInterval(parceCategory);
    //            }
    //            $('#parseStatus').html(data.message);
    //        }
    //    })
    //}

    /* LOAD SETTINGS */

    $.ajax({
        url: "../comparisoncategory/loadsettings",
        success: function (data) {
            $("#checkArtno").attr('checked', data.lArtno);
            $("#priceInRange").attr('checked', data.aRange);
            $("#priceInRange").change();
            $("#fromPrice").val(data.fPrice);
            $("#toPrice").val(data.tPrice);
            $("#downloadMarkers").attr('checked', data.dMarkers);
            $("#addedcategories").attr('checked', data.addCategories);
            $("#autoupdate").attr('checked', data.au);
            $("#autoupdate").change();
            $("#autoupdatehour").val(data.auh);
            $("#autoupdateminute").val(data.aum);
            $("#btnParse").val(data.dwnlbtn);
            $('#importDiscount').attr('checked', data.impdisc);
            $('#downloadImage').attr('checked', data.dwnlImg);
            $('#addPrefix').attr('checked', data.useprefix);
            $('#txtPrefix').val(data.txtprefix);
            $('#minMax').attr('checked', data.minmax);
            $('#threePayTwo').attr('checked', data.threepaytwo);
            $('#theePayTwoColor').val(data.colorthreepaytwo);
            $('#theePayTwoHref').val(data.hrefthreepaytwo);
            $('#theePayTwoColor').css('background', data.colorthreepaytwo)
            $('#mtGift').attr('checked', data.mtgift);
            $('#mtGiftColor').val(data.colormtgift);
            $('#mtGiftHref').val(data.hrefmtgift);
            $('#mtGiftColor').css('background', data.colormtgift);
            $('#dwnlimagetype').val(data.dwnlimagetype);
            $('#downloadImage').change();
            $('#reloadImages').attr('checked', data.reloadImages);
            $('#alwaysAvailable').attr('checked', data.alwaysAvailable);
        }
    })

    /* SAVE SETTINGS */

    $('#saveSettings').click(function (e) {

        var fData = new FormData();

        var au = $('#autoupdate').is(':checked');
        var auh = "0";
        var aum = "0";

        //if (au) {
        //    var time = getUpdateTime();
        //    auh = time.h;
        //    aum = time.m;
        //    if (!auh || !aum) {
        //        return;
        //    }
        //}

        var lArtno = $('#checkArtno').is(':checked');
        var aRange = $('#priceInRange').is(':checked');
        var fPrice = $('#fromPrice').val();
        var tPrice = $('#toPrice').val();
        var dMarkers = $('#downloadMarkers').is(':checked');
        var addCategories = $('#addedcategories').is(':checked');
        var impdisc = $('#importDiscount').is(':checked');
        var dwnlImg = $('#downloadImage').is(':checked');
        var useprefix = $('#addPrefix').is(':checked');
        var txtprefix = $('#txtPrefix').val();
        var minmax = $('#minMax').is(':checked');
        var threepaytwo = $('#threePayTwo').is(':checked');
        var colorthreepaytwo = $('#theePayTwoColor').val();
        var hrefthreepaytwo = $('#theePayTwoHref').val();
        var mtgift = $('#mtGift').is(':checked');
        var colormtgift = $('#mtGiftColor').val();
        var hrefmtgift = $('#mtGiftHref').val();
        var dwnlimagetype = $('#dwnlimagetype').val();
        var reloadImages = $('#reloadImages').is(':checked');
        var alwaysAvailable = $('#alwaysAvailable').is(':checked');

        fData.append("lArtno", lArtno);
        fData.append("aRange", aRange);
        fData.append("fPrice", fPrice);
        fData.append("tPrice", tPrice);
        fData.append("dMarkers", dMarkers);
        fData.append("addCategories", addCategories);
        fData.append("impdisc", impdisc);
        fData.append("dwnlImg", dwnlImg);
        fData.append("useprefix", useprefix);
        fData.append("txtprefix", txtprefix);
        fData.append("au", au);
        fData.append("auh", auh);
        fData.append("aum", aum);
        fData.append("minmax", minmax);
        fData.append("threepaytwo", threepaytwo);
        fData.append("colorthreepaytwo", colorthreepaytwo);
        fData.append("hrefthreepaytwo", hrefthreepaytwo);
        fData.append("mtgift", mtgift);
        fData.append("colormtgift", colormtgift);
        fData.append("hrefmtgift", hrefmtgift);
        fData.append("dwnlimagetype", dwnlimagetype);
        fData.append("reloadImages", reloadImages);
        fData.append("alwaysAvailable", alwaysAvailable);

        $.ajax({
            type: 'POST',
            url: "../comparisoncategory/savedata",
            contentType: false,
            processData: false,
            data: fData,
            success: function (data) {
                statusMessage(data);
            }
        });
        $('#saveSettings').attr('disabled', 'disabled');
        $('#saveSettings').val("Сохраняю...");
    });

    //function getUpdateTime() {
    //    var h = $('#autoupdatehour').val();
    //    var m = $('#autoupdateminute').val();
    //    if ((h >= 0 && h <= 23) && (m >= 0 && m <= 59)) {
    //        return { h: h, m: m };
    //    } else {
    //        return false;
    //    }
    //}

    $('#addPrefix').on('change', function (e) {
        var c = $(e.target).is(':checked');

        if (c && !confirm('Внимание! Если товары уже загружены и имеют артикул без префикса или с отличным префиксом, то будет произведена повторная загрузка товаров!')) {
            $(e.target).removeAttr("checked");
        }
    })

    $('.main-container').on('click', '.link-category', function (e) {
        var elem = e.target,
              data = elem.dataset;
        var l = data.level;
        var i = data.categoryid;
        var b = data.prev;
        $.ajax({
            url: "../pageload",
            data: { level: l, categoryId: i, back: b },
            success: function (data) {
                $(".main-container").html(data);
            }
        })
    })

    $('.main-container').on('click', '.select-category', function (e) {
        e.preventDefault();
        var elem = e.target, data = elem.dataset;
        var tr = $(elem).parent().parent().get(0);
        var catName = $(tr).find('td:nth-child(2)').text();
        var catId = data.categoryid;
        var name = e.target.innerText;

        $('#modal_form .slcatname').html('<b>' + catName + '</b>');
        if (name !== "Выбрать") {
            $('.dropbtn').val(name);
        } else {
            $('.dropbtn').val('');
        }

        $.ajax({
            url: "../comparisoncategory/advcategory",
            success: function (data) {
                $(".dropdown-content").html(data);
            }
        })

        $('#hCatId').attr("data-categoryid", catId);
        $('#overlay').fadeIn(400,
            function () {
                $('#modal_form')
                    .css('display', 'block')
                    .animate({ opacity: 1, top: window.top.window.innerHeight / 2 }, 200);
            });
        return false;
    });

    $('.main-container').on('click', '.select-advcategory', function (e) {
        e.preventDefault();
        var elem = e.target, data = elem.dataset;
        var catId = data.categoryid;
        $.ajax({
            url: "../comparisoncategory/advcategory",
            data: { categoryId: catId },
            success: function (data) {
                $(".dropdown-content").html(data);
            }
        })
        return false;
    });

    $('.main-container').on('click', '.import-category', function (e) {
        e.preventDefault();
        $('#btnOModalOk').show();
        $('#btnOModalCancel').val('Нет');
        $('#modal_form_AddOneCategoryToAdv .header-text').text('Подтвердите действия');
        $('#modal_form_AddOneCategoryToAdv .question-text').html('Добавить категорию "<span class="slCategory-name"></span>" и все подкатегории в магазин?');
        var elem = $(e.target).parent().parent().find('.link-category');
        var data = elem[0].dataset;
        var btnOk = $('#btnOModalOk');
        var slCategoryName = $('.slCategory-name');
        btnOk[0].dataset.categoryid = data.categoryid;
        slCategoryName[0].innerText = elem[0].innerText;

        $('#overlay').fadeIn(200,
            function () {
                $('#modal_form_AddOneCategoryToAdv')
                    .css('display', 'block')
                    .animate({ opacity: 1, top: window.top.window.innerHeight / 2 }, 100);
            });

        return false;
    });

    //$('#autoupdate').on('change', function (e) {
    //    if (e.target.checked) {
    //        $('.time-update').show();
    //    } else {
    //        $('.time-update').hide();
    //    }
    //});

    $('#priceInRange').on('change', function (e) {
        if (e.target.checked) {
            $('.range-price').show();
        } else {
            $('.range-price').hide();
        }
    });

    $('#btnOModalOk').click(function (e) {
        e.preventDefault();
        var catId = e.target.dataset.categoryid;
        $.ajax({
            url: "../comparisoncategory/importcategory",
            data: { categoryId: catId },
            success: function (data) {
                $('#modal_form_AddOneCategoryToAdv .header-text').text(data.header);
                $('#modal_form_AddOneCategoryToAdv .question-text').text(data.text);
                $('#btnOModalCancel').val('OK');
                $('#btnOModalOk').hide();
            }
        })
        //$('#btnOModalCancel').click();
    })

    $('#btnParse').click(function (e) {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            url: "../comparisoncategory/parseproducts",
            success: function (data) {
                if (data.status) {
                    $('#btnParse').val('Остановить загрузку');
                } else {
                    $('#btnParse').val('Загрузить товары');
                }
                statusMessage(data.message);
            }
        })
        return false;
        //$('#btnOModalCancel').click();
    })

    $('#btnOModalCancel').click(function () {
        $('#modal_form_AddOneCategoryToAdv')
            .animate({ opacity: 0, top: '45%' }, 200,
                function () {
                    $(this).css('display', 'none');
                    $('#overlay').fadeOut(400);
                }
            );
    })

    $('#tabs-contents').on('click', '#btnAddcategoryToAdv', function (e) {
        $('#overlay').fadeIn(200,
            function () {
                $('#btnModalCancel').val("Отменить");
                $('#btnModalOk').show();
                $('#modal_form_AddcategoryToAdv')
                    .css('display', 'block')
                    .animate({ opacity: 1, top: window.top.window.innerHeight / 2 }, 100);
            });

        return false;
    });

    //$('.module-settings-controls').on('change', '#checkArtno', function (e) {
    //    var chek = e.target.checked;
    //    $.ajax({
    //        type: 'POST',
    //        url: '../comparisoncategory/activerenderartno',
    //        data: { check: chek },
    //        success: function (data) {
    //            e.target.value = data.res;
    //            console.log(data.error);
    //        }
    //    })
    //});

    $('#parseCategory').click(function (e) {
        $.ajax({
            type: "POST",
            url: "../parsecategory",
            success: function (data) {
                //window.location.reload(true);
                statusMessage(data);
            }
        });
    })



    $('#btnModalOk').click(function (e) {
        e.preventDefault();
        $.ajax({
            url: "../comparisoncategory/addcategories",
            success: function (data) {
                $('#modal_form_AddcategoryToAdv .question-text').text(data.message);
                $('#btnModalOk').hide();
                $('#btnModalCancel').val("OK");
            }
        })
        //$('#btnModalCancel').click();
    })

    //$('#btnParse').mouseenter(function (e) {
    //    $.ajax({
    //        url: "../comparisoncategory/getparsingstatus",
    //        success: function (data) {
    //            if (data.status !== "noshow") {
    //                $('#parsingStatus').show();
    //                $('#parsingStatus').text(data.message);
    //                $("#btnParse").val(data.dwnlbtn);
    //            }
    //        }
    //    });
    //});

    //$('#btnParse').mouseleave(function (e) {
    //    $('#parsingStatus').hide();
    //});

    $('#btnModalCancel').click(function () {
        $('#modal_form_AddcategoryToAdv')
            .animate({ opacity: 0, top: '45%' }, 200,
                function () {
                    $(this).css('display', 'none');
                    $('#overlay').fadeOut(400);
                    $('#myDropdown').hide();
                }
            );
    })

    $('#modal_close, #overlay').click(function () {
        $('#modal_form')
            .animate({ opacity: 0, top: '45%' }, 200,
                function () {
                    $(this).css('display', 'none');
                    $('#overlay').fadeOut(400);
                    $('#myDropdown').hide();
                }
            );
        $('#btnModalCancel').click();
        $('#btnOModalCancel').click();
        return false;
    });

    $('.dropbtn').keyup(function (e) {
        var search = $('.dropbtn').val();
        if (search.length > 3)
            alert(search);
    })

    $('#downloadImage').change(function (e) {
        if ($(e.target).is(':checked'))
        {
            $('#dwnlimagetype').removeAttr('disabled');
            $('#reloadImages').removeAttr('disabled');
        }
        else {
            $('#dwnlimagetype').attr('disabled', 'disabled');
            $('#reloadImages').attr('disabled', 'disabled');
            $('#reloadImages').removeAttr('checked');
        }
    })

    $('.dropbtn').focus(function () {
        if ($('#myDropdown').css('display') !== 'none') {
            return;
        }
        $('#myDropdown').toggle("fast");
    });

    $(document).on('click', function (e) {

        if (!$(e.target).closest(".d").length
            || $(e.target).closest(".set-advcategory").length
            || $(e.target).closest(".delete-advcategory").length) {
            $('#myDropdown').hide();
        }
        e.stopPropagation();
    });

    /*color picker*/

    $('#theePayTwoColor').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            $(el).val('#' + hex);
            $(el).css('background', '#' + hex)
            $(el).ColorPickerHide();
        },
        onBeforeShow: function () {
            $(this).ColorPickerSetColor(this.value);
        }
    })
.bind('keyup', function () {
    $(this).ColorPickerSetColor(this.value);
});

    $('#mtGiftColor').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            $(el).val('#' + hex);
            $(el).css('background', '#' + hex)
            $(el).ColorPickerHide();
        },
        onBeforeShow: function () {
            $(this).ColorPickerSetColor(this.value);
        }
    })
.bind('keyup', function () {
    $(this).ColorPickerSetColor(this.value);
});

    /*color picker*/


});

function statusMessage(obj) {
    $('#saveSettings').removeAttr('disabled');
    $('#saveSettings').val('Сохранить');
    if (obj.Message !== "") {
        $('#errorMessageLabel').show();
        $('.info-labels').css('display', 'inline-block')
                    .animate({ opacity: 1, top: '87px' }, 200);
        $('#errorMessageLabel').text(obj.Message);
        $('.info-labels').css('background', obj.BgColor);
        $('.info-labels').css('border-color', obj.BorderColor);
        setTimeout(function () {
            $('.info-labels')
            .animate({ opacity: 0, top: '40px' }, 200,
                function () {
                    $('.info-labels').css('display', 'none');
                }
            );
        }, 3000);
    }
}

document.onclick = function (e) {
    if (e.target.matches('.select-advcategory')) {
        var elem = e.target, data = elem.dataset;
        var catId = data.categoryid;
        $.ajax({
            url: "../comparisoncategory/advcategory",
            data: { parent: catId },
            success: function (data) {
                $(".dropdown-content").html(data);
            }
        })
        return false;
    }

    if (e.target.matches('.set-advcategory')) {
        var elem = e.target.parentElement.parentElement, data = elem.dataset;
        var name = data.categoryname;
        var advCatId = data.categoryid;
        var slCatId = $('#hCatId').attr("data-categoryid", catId);
        var selector = ".select-category[data-categoryid=" + slCatId + "]";
        $.ajax({
            url: "../comparisoncategory/setadvcategory",
            data: { advCatId: advCatId, slCatId: slCatId },
            success: function (data) {
                $('.dropbtn').val(name);
                $(selector).text(name);
                $(selector).addClass("link-advcategory");
                $('#modal_close, #overlay').click();
            }
        })
        return false;
    }

    if (e.target.matches('.delete-advcategory')) {
        var slCatId = $('#hCatId').attr("data-categoryid", catId);
        var selector = ".select-category[data-categoryid=" + slCatId + "]";
        $.ajax({
            url: "../comparisoncategory/deladvcategory",
            data: { slCatId: slCatId },
            success: function (data) {
                $('.dropbtn').val("");
                $(selector).text("Выбрать");
                $(selector).removeClass("link-advcategory");
                $('#modal_close, #overlay').click();
            }
        })
        return false;
    }
}