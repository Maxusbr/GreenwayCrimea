; (function () {
    'use strict';

    var container, containerEmpty, blocks = [], listProperties, listProducts, blocksItem = {}, btnsRemove;

    var makeArray = function (list) {
        var arr = [];

        if (list == null) {
            return arr;
        }

        for (var i = 0, il = list.length; i < il; i++) {
            arr.push(list[i]);
        }

        return arr;
    }

    var getParent = function (elem, selector) {
        var matchesSelector = elem.matches || elem.webkitMatchesSelector || elem.mozMatchesSelector || elem.msMatchesSelector;

        while (elem) {
            if (matchesSelector.bind(elem)(selector)) {
                return elem;
            } else {
                elem = elem.parentElement;
            }
        }

        return null;
    };

    var findRows = function () {

        var containerProducts = container.querySelector('.js-compareproduct-block-products'),
            containerProperty = container.querySelector('.js-compareproduct-block-properties');

        listProducts = containerProducts == null ? [] : makeArray(containerProducts.querySelectorAll('.js-compareproduct-block-row'));
        listProperties = containerProperty == null ? [] : makeArray(containerProperty.querySelectorAll('.js-compareproduct-block-row'));
    };

    var itemMouseOver = function (e) {
        var target = e.target,
            row = getParent(target, '.js-compareproduct-block-row'),
            rowIndex;

        if (row != null) {
            rowIndex = row.getAttribute('data-row-index');

            if (rowIndex == null) {
                return;
            }

            for (var i = 0, il = listProperties.length; i < il; i++) {
                if (listProperties[i].getAttribute('data-row-index') === rowIndex) {
                    listProperties[i].classList.add('compareproduct-block-item-hover');
                    break;
                }
            }

            for (var j = 0, jl = listProducts.length; j < jl; j++) {
                if (listProducts[j].getAttribute('data-row-index') === rowIndex) {
                    listProducts[j].classList.add('compareproduct-block-item-hover');
                    break;
                }
            }
        }
    };

    var itemMouseOut = function (e) {
        var target = e.target,
            row = getParent(target, '.js-compareproduct-block-row'),
            rowIndex;

        if (row != null) {
            rowIndex = row.getAttribute('data-row-index');

            for (var i = 0, il = listProperties.length; i < il; i++) {
                if (listProperties[i].getAttribute('data-row-index') === rowIndex) {
                    listProperties[i].classList.remove('compareproduct-block-item-hover');
                    break;
                }
            }

            for (var j = 0, jl = listProducts.length; j < jl; j++) {
                if (listProducts[j].getAttribute('data-row-index') === rowIndex) {
                    listProducts[j].classList.remove('compareproduct-block-item-hover');
                    break;
                }
            }
        }
    };

    var itemRemove = function (selector) {
        var itemsForRemove = container.querySelectorAll(selector);

        var tempElement, tempParent;
        for (var i = 0, il = itemsForRemove.length; i < il; i++) {
            tempElement = itemsForRemove[i];
            tempParent = tempElement.parentNode;
            tempParent.removeChild(tempElement);
        }

        rowsRemove();
    };

    var rowRemove = function (arrayIndexs) {

        var itemProduct,
            itemProperty,
            parentItemProduct,
            parentItemProperty;

        for (var i = 0, il = arrayIndexs.length; i < il; i++) {
            itemProduct = listProducts[arrayIndexs[i]];
            itemProperty = listProperties[arrayIndexs[i]];
            parentItemProduct = itemProduct.parentNode;
            parentItemProperty = itemProperty.parentNode;

            parentItemProduct.removeChild(itemProduct);
            parentItemProperty.removeChild(itemProperty);
        }

        findRows();

        if (listProperties.length === 0 && listProducts.length === 0) {
            container.style.display = 'none';
            containerEmpty.style.display = 'block';
        }
    };

    var rowsRemove = function () {

        var isNeedRemove = true,
            row,
            childs,
            indexesRemove = [];

        //ищем строки которые надо удалить
        for (var j = 0, jl = listProducts.length; j < jl; j++) {

            row = listProducts[j];

            childs = makeArray(row.querySelectorAll('.js-compareproduct-product-item'));


            for (var c = 0, cl = childs.length; c < cl; c++) {
                if (childs[c].innerHTML.trim().length > 0) {
                    isNeedRemove = false;
                    break;
                }
            }

            if (isNeedRemove === true) {
                indexesRemove.push(j);
            }

            isNeedRemove = true;
        }

        //удаляем найденые строки
        rowRemove(indexesRemove);

        indexesRemove = [];
    };

    var remove = function (e) {
        var id = e.target.getAttribute('data-compare-product-id');
        itemRemove('[data-compare-product-id="' + id + '"]');
        e.target.removeEventListener('click', remove, false);
    };

    var init = function () {
        container = document.querySelector('.js-compareproduct-container');
        containerEmpty = document.querySelector('.js-compareproduct-empty');

        if (container == null) {
            return;
        }

        blocks = makeArray(container.querySelectorAll('.js-compareproduct-block'));

        btnsRemove = container.querySelectorAll('[data-compare-remove]');

        //разделяем по строчкам

        findRows();

        var maxHeight = 0;
        for (var k = 0, kl = listProperties.length; k < kl; k++) {
            maxHeight = Math.max(listProperties[k].clientHeight, listProducts[k].clientHeight);

            listProperties[k].style.height = maxHeight + 'px';
            listProducts[k].style.height = maxHeight + 'px';
        }

        container.addEventListener('mouseover', itemMouseOver);
        container.addEventListener('mouseout', itemMouseOut);

        for (var c = 0, cl = btnsRemove.length; c < cl; c++) {
            btnsRemove[c].addEventListener('click', remove);
        }

        container.classList.remove('compareproduct-container-processing');

    };

    var load = function () {

        init();

        window.removeEventListener('load', load, false);
    };

    window.addEventListener('load', load);

})();