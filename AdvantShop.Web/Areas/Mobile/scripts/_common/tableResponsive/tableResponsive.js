; (function () {
    'use strict';

    window.addEventListener('load', function load() {
        window.removeEventListener('load', load);

        var mediaObject = window.matchMedia('only screen and (max-width: 760px), (min-device-width: 768px) and (max-device-width: 1024px)');

        if (mediaObject.matches === true) {
            process();
        } else {
            mediaObject.addListener(function media() {
                mediaObject.removeListener(media);

                process();
            });
        }

        function process() {
            var headertext = [],
                ignore = [],
                tables = document.querySelectorAll('table'),
                scrollWrapTpl = document.createElement('div'),
                scrollWrapClone,
                scrollWrapParent,
                headers,
                bodyes,
                rows,
                cells,
                colspan;

            removeCellHeader();

            scrollWrapTpl.classList.add('table-responsive-scroll-wrap');

            for (var i = 0, len = tables.length; i < len; i++) {

                if (tables[i].classList.contains('js-table-responsive-ignore')) {
                    continue;
                }

                headers = tables[i].querySelectorAll('th');
                bodyes = tables[i].querySelectorAll('tbody');

                for (var b = 0, lenB = bodyes.length; b < lenB; b++ ){

                    rows = bodyes[b].rows;

                    if (headers == null || headers.length === 0) {
                        scrollWrapParent = tables[i].parentNode;
                        scrollWrapClone = scrollWrapTpl.cloneNode();
                        scrollWrapParent.insertBefore(scrollWrapClone, tables[i]);
                        scrollWrapClone.appendChild(tables[i]);
                        continue;
                    }

                    for (var h = 0, lenH = headers.length; h < lenH; h++) {

                        colspan = parseFloat(headers[h].getAttribute('colspan'));

                        if (isNaN(colspan) === false) {
                            ignore = ignore.concat(generateIndexArray(h, colspan));
                        }

                        headertext = headertext.concat(saveHeaderText(headers[h].textContent.replace(/\r?\n|\r/, ''), h, colspan));
                    };

                    if (rows != null && rows.length > 0) {
                        for (var r = 0, lenR = rows.length; r < lenR; r++) {
                            cells = Array.prototype.slice.call(rows[r].cells);

                            if (cells != null && cells.length > 0) {
                                for (var c = 0, lenC = cells.length; c < lenC; c++) {
                                    if (ignore.indexOf(c) !== -1 || (cells[c].getAttribute('colspan') != null && cells[c].getAttribute('colspan').length > 0)) {
                                        cells[c].classList.add('table-responsive-cell-ignore');
                                        cells[c].removeAttribute('data-label');

                                        if (ignore.indexOf(c - 1) === -1) {
                                            cells[c].parentNode.insertBefore(generateCellHeader(headertext[c]), cells[c]);
                                        }

                                    } else {
                                        cells[c].classList.remove('table-responsive-cell-ignore');
                                        cells[c].setAttribute('data-label', headertext[c]);
                                    }
                                }
                            }
                        }
                    }

                    tables[i].classList.add('table-responsive-transform');

                    headertext.length = 0;
                }
            }
        };

        function generateIndexArray(start, colspan) {
            var array = [];

            colspan = colspan != null && isNaN(colspan) === false ? colspan : (start + 1);

            for (var i = start; i < colspan + start; i++) {
                array.push(i);
            }

            return array;
        }

        function saveHeaderText(text, index, colspan) {
            var array = [];

            colspan = colspan != null && isNaN(colspan) === false ? colspan : (index + 1);

            for (var i = index; i < colspan; i++) {
                array.push(text);
            }

            return array;
        }

        function generateCellHeader(text) {
            var td = document.createElement('td');
            td.classList.add('table-responsive-header-colspan');
            td.classList.add('js-table-responsive-header-colspan');
            td.innerHTML = text;

            return td;
        }

        function removeCellHeader() {
            var cells = document.querySelectorAll('.js-table-responsive-header-colspan');

            Array.prototype.slice.call(cells).forEach(function (item) {
                item.parentNode.removeChild(item);
            });
        }
    });

})();