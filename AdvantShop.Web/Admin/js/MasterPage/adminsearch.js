; (function ($) {
    $(function () {

        var baseurl = document.getElementsByTagName('base')[0].href,
            searchHeader = $('#searchHeader'),
            anchorsSearch = $('#liSearchItems a'),
            searchSubmenuContainer = $('#searchSubmenuContainer'),
            searchSubmenu = $('#searchSubmenu'),
            adminInput = $('#txtAdminSearch'),
            adminSearchOptions = JSON.parse(localStorage.getItem("adminSearchOptions")) || {},
            isExistStorage = false;

        if (adminInput.length === 0) {
            return;
        }

        for (var i = 0, l = anchorsSearch.length; i < l; i += 1) {
            if (anchorsSearch.eq(i).text() === adminSearchOptions.area) {
                isExistStorage = true;
                break;
            }
        }

        if (isExistStorage === true) {
            searchHeader.text(adminSearchOptions.area);
            searchHeader.attr("data-href", adminSearchOptions.url);
            adminInput.attr('placeholder', adminSearchOptions.placeholder);
        } else {

            var firstItem = anchorsSearch.eq(0),
                firstItemPlaceholder = firstItem.attr('data-placeholder'),
                firstItemText = firstItem.text(),
                firstItemHref = firstItem.attr('href'),
                firstItemType = firstItem.attr('data-type');

            adminSearchOptions.type = firstItemType;
            adminSearchOptions.placeholder = firstItemPlaceholder;

            searchHeader.text(firstItemText);
            adminSearchOptions.area = firstItemText;

            searchHeader.attr("data-href", firstItemHref);
            adminSearchOptions.url = firstItemHref;

            localStorage.setItem("adminSearchOptions", JSON.stringify(adminSearchOptions));
        }

        searchSubmenuContainer.on('click', function () {
            searchSubmenu.show();
        });

        searchSubmenuContainer.on('mouseleave', function () {
            searchSubmenu.hide();
        })

        anchorsSearch.on('click', function (event) {

            event.preventDefault();
            event.stopPropagation();

            var aSearch = $(this),
                aSearchPlaceholder = aSearch.attr('data-placeholder'),
                aSearchType = aSearch.attr('data-type'),
                aSearchHref = aSearch.attr('href'),
                aSearchText = aSearch.text();

            searchHeader.html(aSearchText);
            adminSearchOptions.area = aSearchText;

            searchHeader.attr('data-href', aSearchHref);
            adminSearchOptions.url = aSearchHref;

            adminSearchOptions.type = aSearchType;

            adminSearchOptions.placeholder = aSearchPlaceholder;
            adminInput.attr('placeholder', aSearchPlaceholder);

            searchSubmenu.hide();

            adminInput[0].autocompleter.setExtraParams({ type: adminSearchOptions.type });

            localStorage.setItem("adminSearchOptions", JSON.stringify(adminSearchOptions));
        });

        adminInput.autocomplete('httpHandlers/adminsearch.ashx', {
            delay: 10,
            minChars: 1,
            matchSubset: 1,
            autoFill: false,
            matchContains: 1,
            cacheLength: null,
            selectFirst: false,
            //formatItem: liFormat,
            maxItemsToShow: 10,
            resultsClass: 'search-results ac_results',
            width:313,
            onItemSelect: function (li, $lnk, $input) {
                setTimeout(function () { window.location.assign($('base').attr('href') + $lnk.attr('href')); }, 1);
            }
        });

        adminInput[0].autocompleter.setExtraParams({ type: adminSearchOptions.type });

        $('#btnAdminSearch').on('click', function (event) {

            if (adminInput.val().length === 0) {
                return;
            }

            event.preventDefault();

            var urlSearch = new Advantshop.Utilities.Uri(baseurl + searchHeader.attr('data-href'));

            urlSearch.replaceQueryParam('search', adminInput.val());

            window.location = urlSearch.toString();
        });
    });
})(jQuery);
