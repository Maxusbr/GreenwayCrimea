; (function (window) {
    'use strict';

    var isTouchDevice = 'ontouchstart' in document.documentElement,
        noopStyle = document.documentElement.style,
        autoStop = false,
        clonesForCreate = {},
        transformName,
        transitionDurationName,
        transitionEventName,
        Carousel;

    transformName = (function () {
        var arr = ['transform', 'webkitTransform', 'msTransform'];

        for (var i = 0, il = arr.length; i < il; i += 1) {
            if (noopStyle[arr[i]] !== undefined) {
                return arr[i];
            }
        }
    })();

    transitionDurationName = (function () {
        var arr = ['transitionDuration', 'webkitTransitionDuration', 'msTransitionDuration'];

        for (var i = 0, il = arr.length; i < il; i += 1) {
            if (noopStyle[arr[i]] !== undefined) {
                return arr[i];
            }
        }
    })();

    transitionEventName = (function () {
        var transitions = {
            'transition': 'transitionend',
            'OTransition': 'otransitionend',  // oTransitionEnd in very old Opera
            'MozTransition': 'transitionend',
            'WebkitTransition': 'webkitTransitionEnd'
        };

        for (var i in transitions) {
            if (transitions.hasOwnProperty(i) === true && noopStyle[i] !== undefined) {
                return transitions[i];
            }
        }
    })();

    Carousel = function (element, options) {
        this.list = element;
        this.items = Array.prototype.slice.call(element.children);
        this.options = options;
        this.propName = Carousel.prototype.getPropName(options.isVertical);
        this.cache = this.items.slice();
        return this;
    };

    Carousel.prototype.addToCache = function (item) {
        this.cache.push(item);
    };

    Carousel.prototype.getFromCache = function (item) {
        var index;

        if (typeof (item) === 'number') {
            index = item;
        } else {
            index = this.cache.indexOf(item);
        }

        return this.cache[index];
    };

    Carousel.prototype.removeFromCache = function (item) {

        var index, removeItem;

        if (typeof (item) === 'number') {
            index = item;
        } else {
            index = this.cache.indexOf(item);
        }

        if (index !== -1) {
            this.cache.splice(index, 1);
        }

        return this.cache[index];
    };

    Carousel.prototype.getSize = function (totalCount, maxWidth, maxHeight, isVertical) {
        var size = {};

        if (isVertical === false) {
            size['width'] = totalCount * maxWidth;
            size['height'] = maxHeight;
        } else {
            size['width'] = maxWidth;
            size['height'] = totalCount * maxHeight;
        }

        return size;
    };

    Carousel.prototype.getPropName = function (isVertical) {
        return (isVertical === false ? 'width' : 'height');
    };

    Carousel.prototype.getItemsMaxSizes = function (items) {
        var tempWidth = 0,
            tempHeight = 0,
            maxWidth = 0,
            maxHeigth = 0;

        for (var i = items.length - 1; i >= 0; i--) {

            if (items[i].carouselItemData == null) {
                continue;
            }

            tempWidth = items[i].carouselItemData.originalWidth;

            if (tempWidth > maxWidth) {
                maxWidth = tempWidth;
            }

            tempHeight = items[i].carouselItemData.originalHeight;

            if (tempHeight > maxHeigth) {
                maxHeigth = tempHeight;
            }
        }

        return {
            'width': maxWidth,
            'height': maxHeigth
        };
    };

    Carousel.prototype.setItemSize = function (item, value) {
        var self = this,
            valueStr = value + 'px';

        item.style[self.propName] = valueStr;
        //item.style['min' + self.propName.charAt(0).toUpperCase() + self.propName.slice(1)] = valueStr;
        item.style['max' + self.propName.charAt(0).toUpperCase() + self.propName.slice(1)] = valueStr;
        item.style['flexBasis'] = valueStr;
        item.style['msFlexPreferredSize'] = valueStr;
        item.style['webkitFlexBasis'] = valueStr;
    };

    Carousel.prototype.processItems = function (items) {

        var self = this;

        for (var i = 0, len = items.length - 1; i <= len; i++) {
            self.processItem(items[i], i);
        }
    };

    Carousel.prototype.processItem = function (item, index) {
        var self = this;
        var itemStylesComputed = getComputedStyle(item),
            itemBorderLeft,
            itemBorderRight,
            itemBorderTop,
            itemBorderBottom;

        itemBorderLeft = parseInt(itemStylesComputed['border-left-width'], 10);
        itemBorderRight = parseInt(itemStylesComputed['border-right-width'], 10);
        itemBorderTop = parseInt(itemStylesComputed['border-top-width'], 10);
        itemBorderBottom = parseInt(itemStylesComputed['border-bottom-width'], 10);

        itemBorderLeft = isNaN(itemBorderLeft) ? 0 : itemBorderLeft;
        itemBorderRight = isNaN(itemBorderRight) ? 0 : itemBorderRight;
        itemBorderTop = isNaN(itemBorderTop) ? 0 : itemBorderTop;
        itemBorderBottom = isNaN(itemBorderBottom) ? 0 : itemBorderBottom;

        /*,
        itemStylesComputed = getComputedStyle(item),
        itemStylesComputed = item.getBoundingClientRect(),
        itemPaddingLeft,
        itemPaddingRight,
        itemPaddingTop,
        itemPaddingBottom;*/

        /*itemPaddingLeft = parseInt(itemStylesComputed['padding-left'], 10);
        itemPaddingRight = parseInt(itemStylesComputed['padding-right'], 10);
        itemPaddingTop = parseInt(itemStylesComputed['padding-top'], 10);
        itemPaddingBottom = parseInt(itemStylesComputed['padding-bottom'], 10);

        itemPaddingLeft = isNaN(itemPaddingLeft) ? 0 : itemPaddingLeft;
        itemPaddingRight = isNaN(itemPaddingRight) ? 0 : itemPaddingRight;
        itemPaddingTop = isNaN(itemPaddingTop) ? 0 : itemPaddingTop;
        itemPaddingBottom = isNaN(itemPaddingBottom) ? 0 : itemPaddingBottom;*/

        item.carouselItemData = {
            originalWidth: item.clientWidth + itemBorderLeft + itemBorderRight,
            originalHeight: item.clientHeight + itemBorderTop + itemBorderBottom,
            index: index != null ? index : self.items.length,
            parameters: item.getAttribute('data-parameters') != null ? (new Function('return ' + item.getAttribute('data-parameters')))() : null,
            stylesRaw: item.getAttribute('style')
        };

        item.classList.add('js-carousel-item');

        return item;
    };

    Carousel.prototype.setSizes = function (wrapSize, innerSize, listSize, itemsSizes) {
        var self = this;

        //if (wrapSize != null) {
        //    self.wrap.style[self.propName] = wrapSize[self.propName] + 'px';
        //}

        if (innerSize != null) {
            self.inner.style[self.propName] = innerSize[self.propName] + 'px';
        }

        if (listSize != null) {
            self.list.style[self.propName] = listSize[self.propName] + 'px';
        }

        if (itemsSizes != null) {
            for (var i = self.items.length - 1; i >= 0; i--) {
                self.setItemSize(self.items[i], itemsSizes[self.propName]);
            }
        }
    };

    Carousel.prototype.calc = function (items, options) {
        var self = this,
            slidesMaxSize,
            result,
            countVisible,
            countVisibleDirty,
            dimension,
            carouselSizes,
            carouselStylesComputed,
            carouselPaddingLeft,
            carouselPaddingRight,
            carouselPaddingTop,
            carouselPaddingBottom;

        carouselStylesComputed = getComputedStyle(self.wrap);

        carouselPaddingLeft = parseInt(carouselStylesComputed['padding-left'], 10);
        carouselPaddingRight = parseInt(carouselStylesComputed['padding-right'], 10);
        carouselPaddingTop = parseInt(carouselStylesComputed['padding-top'], 10);
        carouselPaddingBottom = parseInt(carouselStylesComputed['padding-bottom'], 10);

        carouselPaddingLeft = isNaN(carouselPaddingLeft) ? 0 : carouselPaddingLeft;
        carouselPaddingRight = isNaN(carouselPaddingRight) ? 0 : carouselPaddingRight;
        carouselPaddingTop = isNaN(carouselPaddingTop) ? 0 : carouselPaddingTop;
        carouselPaddingBottom = isNaN(carouselPaddingBottom) ? 0 : carouselPaddingBottom;

        self.carouselSizes = {
            width: Math.floor(self.wrap.clientWidth - carouselPaddingLeft - carouselPaddingRight),
            height: Math.floor(self.wrap.clientHeight - carouselPaddingTop - carouselPaddingBottom)
        }

        carouselSizes = self.carouselSizes;

        slidesMaxSize = self.getItemsMaxSizes(items);

        countVisibleDirty = carouselSizes[self.propName] / slidesMaxSize[self.propName];

        //countVisible = Math.floor(countVisibleDirty);
        countVisible = Math.round(countVisibleDirty);

        if ((options.visibleMax != null && options.visibleMax > items.length) || (options.visibleMin != null && options.visibleMin > items.length)) {
            countVisible = items.length;
            dimension = countVisibleDirty - countVisible;
        } else if (countVisible > items.length) {
            countVisible = items.length;
            dimension = 0;
        } else if (countVisible < 1) {
            countVisible = 1;
            dimension = countVisibleDirty - countVisible;
        }
        else {
            dimension = countVisibleDirty - countVisible;
        }

        if ((options.visibleMax != null && options.visibleMax < countVisible) || (options.visibleMin != null && options.visibleMin > countVisible)) {

            if (options.visibleMax != null && options.visibleMax < countVisible) {
                countVisible = options.visibleMax;
            } else if (options.visibleMin != null && options.visibleMin > countVisible) {
                countVisible = options.visibleMin;
                slidesMaxSize[self.propName] = self.carouselSizes.width / countVisible;
            }

            if (options.stretch) {
                slidesMaxSize[self.propName] = carouselSizes[self.propName] / countVisible;
            } else {
                carouselSizes[self.propName] = carouselSizes[self.propName] - (slidesMaxSize[self.propName] * countVisible);
                self.carouselSize = (slidesMaxSize[self.propName] * countVisible) + 'px';
            }

        } else {

            if (isNaN(dimension) == false && dimension !== 0) {
                if (options.stretch) {
                    slidesMaxSize[self.propName] += (slidesMaxSize[self.propName] * dimension) / countVisible;
                } else {

                    if (dimension > 0) {
                        carouselSizes[self.propName] = carouselSizes[self.propName] - (slidesMaxSize[self.propName] * dimension);
                        self.carouselSize = carouselSizes[self.propName] + 'px';
                    } else {
                        slidesMaxSize[self.propName] += (slidesMaxSize[self.propName] * dimension) / countVisible;

                        if (slidesMaxSize[self.propName] <= 0) {
                            slidesMaxSize[self.propName] = carouselSizes[self.propName];
                        }
                    }
                }
            } else {
                self.carouselSize = (slidesMaxSize[self.propName] * countVisible) + 'px';
            }
        }

        if (countVisible <= 1) {
            countVisible = 1;
            result = slidesMaxSize[self.propName];
            //slidesMaxSize[self.propName] = carouselSizes[self.propName];
        } else {
            result = slidesMaxSize[self.propName];
        }

        if (options.isVertical === false) {
            self.slidesSize = {
                width: result,
                height: slidesMaxSize.height
            };
        } else {
            self.slidesSize = {
                width: slidesMaxSize.width,
                height: result
            };
        }

        self.countVisible = countVisible;

        self.listSize = self.getSize(self.items.length, slidesMaxSize.width, slidesMaxSize.height, options.isVertical);
        self.innerSize = self.getSize(countVisible, slidesMaxSize.width, slidesMaxSize.height, options.isVertical);

        return {
            wrapSize: carouselSizes,
            listSize: self.listSize,
            innerSize: self.innerSize,
            itemsSize: self.slidesSize
        };
    };

    Carousel.prototype.renderDots = function () {

        var self = this,
            fragment = document.createDocumentFragment(),
            dot;

        self.dotsContainer = createComponent('ul');
        self.dotsContainer.className = 'carousel-dots ' + (self.options.dotsClass || '');

        self.dots = [];

        for (var d = 0, len = self.items.length - 1; d <= len; d++) {
            dot = createComponent('li');
            dot.classList.add('carousel-dots-item');
            dot.setAttribute('data-index', d);
            dot.innerHTML = '<i class="carousel-dots-item-inner ' + (self.options.dotsItemClass || '') + '" />';
            self.dotsContainer.appendChild(dot);
            self.dots.push(dot);
        };

        self.wrap.appendChild(self.dotsContainer);
    };

    Carousel.prototype.renderNav = function () {

        var self = this,
            nav = self.wrap.querySelector('.carousel-nav'),
            navPrev, navNext, needRenderNav, needRenderPrev, needRenderNext;

        //#region nav find or create
        if (nav == null || nav.parentNode !== self.wrap) {
            nav = createComponent('div');
            needRenderNav = true;
        }

        nav.className = 'carousel-nav ' + ('carousel-nav-' + self.options.navPosition);

        self.nav = nav;
        //#endregion

        //#region prev find or create
        navPrev = nav.querySelector('.carousel-nav-prev');

        if (navPrev == null) {
            navPrev = createComponent('button');
            needRenderPrev = true;
        }

        navPrev.className = 'carousel-nav-prev ' + (self.options.isVertical ? self.options.prevIconVertical : self.options.prevIcon);

        if (self.options.prevClass) {
            self.options.prevClass.split(' ').forEach(function (item) {
                navPrev.classList.add(item);
            });
        }

        self.navPrev = navPrev;
        //#endregion

        //#region next find or create
        navNext = nav.querySelector('.carousel-nav-next');

        if (navNext == null) {
            navNext = createComponent('button');
            needRenderNext = true;
        }

        navNext.className = 'carousel-nav-next ' + (self.options.isVertical ? self.options.nextIconVertical : self.options.nextIcon);

        if (self.options.nextClass) {
            self.options.nextClass.split(' ').forEach(function (item) {
                navNext.classList.add(item);
            });
        }

        self.navNext = navNext;
        //#endregion

        if (needRenderPrev === true) {
            nav.appendChild(navPrev);
        }

        if (needRenderNext === true) {
            nav.appendChild(navNext);
        }

        if (needRenderNav === true) {
            self.wrap.appendChild(nav);
        }
    };

    Carousel.prototype.generate = function (element) {
        var self = this,
            wrap,
            inner,
            needRenderInner,
            needRenderWrap;

        element.classList.add('carousel-list');

        //#region inner find or create
        if (self.list.parentNode != null && self.list.parentNode.classList.contains('carousel-inner') === true) {
            inner = self.list.parentNode;
        } else {
            inner = createComponent('div');
            needRenderInner = true;
        }

        inner.classList.add('carousel-inner');

        self.inner = inner;
        //#endregion

        //#region wrap find or create
        if (self.inner.parentNode != null && self.inner.parentNode.classList.contains('carousel') === true) {
            wrap = self.inner.parentNode;
        } else {
            wrap = createComponent('div');
            needRenderWrap = true;
        }

        wrap.classList.add('carousel');
        wrap.classList.add('carousel-' + (self.options.isVertical ? 'vertical' : 'horizontal'));
        wrap.classList.add('carousel-wrap-nav-' + self.options.navPosition);

        if (self.options.carouselClass != null && self.options.carouselClass.length > 0) {
            wrap.classList.add(self.options.carouselClass);
        }

        self.wrap = wrap;
        //#endregion

        //TODO подумать, можно ли оптимизировать рендеринг
        if (needRenderInner) {
            wrap.appendChild(inner);
        }

        if (needRenderWrap) {
            element.parentNode.appendChild(wrap);
        }

        if (needRenderInner) {
            inner.appendChild(element);
        }
    };

    Carousel.prototype.selectDots = function (index) {
        var self = this, dotsItems;

        if (self.dots == null || self.dotActive === self.dots[index]) {
            return;
        }

        if (self.dotActive != null) {
            self.dotActive.classList.remove('carousel-dots-selected');
        }

        if (self.dots[index] != null) {
            self.dotActive = self.dots[index];
            self.dots[index].classList.add('carousel-dots-selected')
        }
    };

    Carousel.prototype.doClone = function () {
        var self = this, oldClones, itemsDuplicate, item, itemsClonePrev, itemsCloneNext, fragmentPrev, fragmentNext, clonePrev, cloneNext, marginLeftValue;

        //#region find and delete old clones


        oldClones = self.list.querySelectorAll('.js-carousel-clone');
        for (var c = oldClones.length - 1; c >= 0; c--) {
            oldClones[c].parentNode.removeChild(oldClones[c]);
        }

        for (var i = self.items.length - 1; i >= 0; i--) {
            delete self.items[i].carouselItemData.clone;
        }

        self.list.style.marginLeft = '0px';

        //#endregion

        if (self.countVisible >= self.items.length) {
            return null;
        }

        itemsDuplicate = self.items.slice();

        itemsClonePrev = Array.prototype.slice.call(itemsDuplicate.reverse(), 0, self.countVisible).reverse();
        itemsCloneNext = Array.prototype.slice.call(itemsDuplicate.reverse(), 0, self.countVisible);

        fragmentPrev = document.createDocumentFragment();
        fragmentNext = document.createDocumentFragment();

        for (var p = 0, len = itemsClonePrev.length; p < len; p++) {
            clonePrev = itemsClonePrev[p].cloneNode(true);
            clonePrev.classList.add('js-carousel-clone');

            self.setItemSize(clonePrev, self.slidesSize[self.propName]);

            fragmentPrev.appendChild(clonePrev);

            itemsClonePrev[p].carouselItemData.clone = clonePrev;
        }

        for (var n = 0, l = itemsCloneNext.length; n < l; n++) {
            cloneNext = itemsCloneNext[n].cloneNode(true);
            cloneNext.classList.add('js-carousel-clone');

            self.setItemSize(cloneNext, self.slidesSize[self.propName]);

            fragmentNext.appendChild(cloneNext);

            itemsCloneNext[n].carouselItemData.clone = cloneNext;
        }

        //insert for prev
        self.list.insertBefore(fragmentPrev, self.items[0]);

        //insert for next
        self.list.appendChild(fragmentNext);

        marginLeftValue = (-itemsClonePrev.length * self.slidesSize[self.propName]);

        self.list.style.marginLeft = marginLeftValue + 'px';

        return {
            clonesNext: fragmentNext,
            clonesPrev: fragmentPrev,
            clonesNextCount: itemsCloneNext.length,
            clonesPrevCount: itemsClonePrev.length,
            marginLeftValue: marginLeftValue
        };
    };

    Carousel.prototype.getMoveData = function (index) {

        var self = this, result;

        if (self.items.length > self.countVisible) {
            result = Math.abs(index) * (self.options.scrollCount * self.slidesSize[self.propName]) * (index < 0 ? 1 : -1);
        } else {
            result = 0;
        }

        return result;
    };

    Carousel.prototype.move = function (transformValue, useAnimate) {

        useAnimate = useAnimate != null ? useAnimate : true;

        var self = this,
            transformObj = {},
            transformStyle;

        transformObj[self.options.isVertical ? 'top' : 'left'] = transformValue;

        transformStyle = ['translate3d(', transformObj.left || 0, 'px,', ' ', transformObj.top || 0, 'px, 0px)'].join('');

        self.list.style[transformName] = transformStyle;
        self.list.style[transitionDurationName] = useAnimate === false ? '0s' : (self.options.speed / 1000) + 's';

        self.transformValue = transformValue;
    };

    Carousel.prototype.moveAuto = function () {

        var self = this;

        if (autoStop === true) {
            return;
        }

        clearTimeout(self.timerAuto);

        self.timerAuto = setTimeout(function () {

            if (autoStop === true) {
                return;
            }

            self.next();

            self.moveAuto();

        }, self.options.autoPause);

    };

    Carousel.prototype.stopAuto = function () {

        autoStop = true;

        if (self.timerAuto != null) {
            clearTimeout(self.timerAuto);
        }
    };

    Carousel.prototype.startAuto = function () {

        var self = this;

        autoStop = false;

        self.moveAuto();
    };

    Carousel.prototype.checkNav = function () {

        var self = this,
            itemsCount = self.items.length;

        self.isPrevDisabled = (self.options.auto === false && 0 === self.options.indexActive) || self.countVisible >= itemsCount;
        self.isNextDisabled = (self.options.auto === false && (self.options.indexActive + self.countVisible) === self.items.length) || self.countVisible >= itemsCount;
        self.isNavNotShow = itemsCount <= self.countVisible;

        self.isPrevDisabled ? self.navPrev.setAttribute('disabled', 'disabled') : self.navPrev.removeAttribute('disabled');
        self.isNextDisabled ? self.navNext.setAttribute('disabled', 'disabled') : self.navNext.removeAttribute('disabled');

        self.wrap.classList[self.isNavNotShow === true ? 'add' : 'remove']('carousel-nav-not-show');
    };

    Carousel.prototype.prev = function (isAnimate) {

        var self = this, transform, newIndex;

        if (self.isPrevDisabled === true || self.animationLoop === true) {
            return;
        }

        newIndex = self.options.indexActive - self.options.scrollCount;

        //go to last item
        if (self.options.auto === true && newIndex < 0) {

            self.animationLoop = true;

            var returnFn = function () {
                self.list.removeEventListener('transitionend', returnFn);

                setTimeout(function () {
                    self.animationLoop = false;
                    self.goto(self.items.length - 1, false);
                }, 0);
            };

            self.list.addEventListener('transitionend', returnFn);
        }

        self.goto(newIndex, true);
    };

    Carousel.prototype.next = function (isAnimate) {
        var self = this, transform, newIndex;

        if (self.isNextDisabled === true || self.animationLoop === true) {
            return;
        }

        newIndex = self.options.indexActive + self.options.scrollCount;

        //go to first item
        if (self.options.auto === true && newIndex === self.items.length) { // newIndex > self.items.length - self.countVisible

            self.animationLoop = true;

            var returnFn = function () {
                self.list.removeEventListener('transitionend', returnFn);

                setTimeout(function () {
                    self.animationLoop = false;
                    self.goto(0, false);
                }, 0);
            };

            self.list.addEventListener('transitionend', returnFn);
        }

        self.goto(newIndex, true);
    };

    Carousel.prototype.goto = function (index, isAnimate) {
        var self = this;

        self.options.indexActive = index;

        isAnimate = isAnimate != null ? isAnimate : true;

        var transform = self.getMoveData(self.options.indexActive);

        self.move(transform, isAnimate);

        self.checkNav();

        if (self.options.dots) {
            self.selectDots(self.options.indexActive);
        }

    };

    Carousel.prototype.removeItem = function (child, keepInCache) {
        var self = this, index, clone;

        index = self.items.indexOf(child);

        if (index < 0) {
            return;
        }

        keepInCache = keepInCache != null ? keepInCache : true;

        if (child != null && child.parentNode != null) {

            if (self.options.auto === true && child.carouselItemData.clone != null) {
                clone = child.carouselItemData.clone;
                clone.parentNode.removeChild(clone);
            }

            child.parentNode.removeChild(child);
            self.items.splice(index, 1);
        };

        if (keepInCache === false) {
            self.removeFromCache(child);
        }
        //else {
        //    self.addToCache(child);
        //}

        return child;
    };

    Carousel.prototype.addItem = function (item) {
        var self = this,
            sibling = null,
            index = self.cache.indexOf(item);

        if (self.items.length > 0) {
            for (var i = index; i >= 0; i--) {
                if (self.items[i] != null && self.items[i].carouselItemData != null && self.cache.indexOf(self.items[i]) < index) {
                    sibling = self.items[i];
                    self.items.splice(index + 1, 0, item);
                    break;
                }
            }
        }

        if (sibling == null) {
            self.items['unshift'](item);
        }

        self.list.insertBefore(item, sibling != null ? sibling.nextSibling : self.list.firstChild);

        if (item.carouselItemData == null) {
            self.processItem(item);
        }
        //var self = this,
        //    sibling = null,
        //    leftIndex = self.items.length > 0 ? self.items[0].carouselItemData.index : Number.POSITIVE_INFINITY,
        //    index = item.carouselItemData != null ? item.carouselItemData.index : self.items.length;


        //if (self.items.length > 0) {
        //    for (var i = index; i >= 0; i--) {
        //        if (self.items[i] != null && self.items[i].carouselItemData != null && self.items[i].carouselItemData.index < index) {
        //            sibling = self.items[i];
        //            self.items.splice(index + 1, 0, item);
        //            break;
        //        }
        //    }
        //}

        //if (sibling == null) {
        //    self.items[index < leftIndex ? 'unshift' : 'push'](item);
        //}

        //self.list.insertBefore(item, sibling != null ? sibling.nextSibling : (index < leftIndex && self.items.length > 0 ? self.list.firstChild : null));

        //if (item.carouselItemData == null) {
        //    self.processItem(item);
        //}

        return item;
    };

    Carousel.prototype.getItems = function () {
        return this.items;
    };

    Carousel.prototype.filterItems = function (filterFunction) {

        var self = this,
            arrayAll = self.cache,
            itemsForVisible;

        itemsForVisible = arrayAll.filter(filterFunction);

        for (var i = 0, len = arrayAll.length - 1; i <= len; i++) {
            if (itemsForVisible.indexOf(arrayAll[i]) !== -1 && self.items.indexOf(arrayAll[i]) === -1) {
                self.addItem(arrayAll[i]);
            } else if (itemsForVisible.indexOf(arrayAll[i]) === -1) {
                self.removeItem(arrayAll[i]);
            }
        };

        self.goto(0, false);

        self.checkNav();
    };

    Carousel.prototype.clearFilterItems = function () {
        var self = this;

        self.filterItems(function () { return true; });
    };

    Carousel.prototype.getActiveItem = function () {
        return this.items[this.options.indexActive];
    };

    Carousel.prototype.getSelectedItem = function () {
        return this.itemSelected;
    };

    Carousel.prototype.setItemSelect = function (item) {
        var self = this;

        if (item == null) {
            return;
        }

        self.itemSelected = null;

        if (typeof (item) === 'number') {
            item = self.items[item];
        }

        for (var j = self.items.length - 1; j >= 0; j--) {

            if (self.options.itemSelectClass != null) {
                self.items[j].classList.remove(self.options.itemSelectClass);
            }

            if (self.items[j].carouselItemData != null) {
                self.items[j].carouselItemData.isSelect = false;
            }

        }

        if (self.options.itemSelectClass != null) {
            self.options.itemSelectClass.split(' ').forEach(function (cssClass) {
                item.classList.add(cssClass);

                if (self.options.auto === true && item.carouselItemData != null && item.carouselItemData.clone != null) {
                    item.carouselItemData.clone.classList.add(cssClass);
                }
            });
        }

        if (item.carouselItemData != null) {
            item.carouselItemData.isSelect = true;
            self.itemSelected = item;
        }
    };

    Carousel.prototype.dotClick = function (event) {
        var self = this, currentDot, index;

        if (event.target.tagName.toLowerCase() === 'i') {
            currentDot = event.target.parentNode;
        } else if (event.target.tagName.toLowerCase() === 'li') {
            currentDot = event.target;
        } else {
            return;
        }

        index = parseInt(currentDot.getAttribute('data-index'));

        self.goto(index);
    };

    Carousel.prototype.itemClick = function (item) {
        var self = this;

        self.setItemSelect(item);

        if (self.options.itemSelect != null) {
            self.options.itemSelect(self, item, self.items.indexOf(item));
        }
    };

    Carousel.prototype.touch = function () {

        var self = this;

        // The maximum vertical delta for a swipe should be less than 75px.
        var MAX_VERTICAL_DISTANCE = 75;
        // Vertical distance should not be more than a fraction of the horizontal distance.
        var MAX_VERTICAL_RATIO = 0.5;
        // At least a 30px lateral motion is necessary for a swipe.
        var MIN_HORIZONTAL_DISTANCE = 30;
        // The total distance in any direction before we make the call on swipe vs. scroll.
        var MOVE_BUFFER_RADIUS = 10;

        var startCoords, moved = false;

        //function validSwipe(coords, direction) {
        //    // Check that it's within the coordinates.
        //    // Absolute vertical distance must be within tolerances.
        //    // Horizontal distance, we take the current X - the starting X.
        //    // This is negative for leftward swipes and positive for rightward swipes.
        //    // After multiplying by the direction (-1 for left, +1 for right), legal swipes
        //    // (ie. same direction as the directive wants) will have a positive delta and
        //    // illegal ones a negative delta.
        //    // Therefore this delta must be positive, and larger than the minimum.
        //    if (!startCoords) return false;
        //    var deltaY = Math.abs(coords.y - startCoords.y);
        //    var deltaX = (coords.x - startCoords.x) * direction;
        //    return valid && // Short circuit for already-invalidated swipes.
        //        deltaY < MAX_VERTICAL_DISTANCE &&
        //        deltaX > 0 &&
        //        deltaX > MIN_HORIZONTAL_DISTANCE &&
        //        deltaY / deltaX < MAX_VERTICAL_RATIO;
        //}

        function validSwipe(coords, direction) {
            var deltaAlt = Math.abs(coords.alt - startCoords.alt);
            var deltaMain = (coords.main - startCoords.main) * direction;
            return startCoords != null && (deltaAlt / deltaMain < MAX_VERTICAL_RATIO);
        }

        function getCoordinates(event) {
            var originalEvent = event.originalEvent || event;
            var touches = originalEvent.touches && originalEvent.touches.length ? originalEvent.touches : [originalEvent];
            var e = (originalEvent.changedTouches && originalEvent.changedTouches[0]) || touches[0];
            var result;

            if (self.options.isVertical) {
                result = {
                    main: e.clientY,
                    alt: e.clientX
                }
            } else {
                result = {
                    main: e.clientX,
                    alt: e.clientY
                }
            }

            return result;
        }

        function touchStart(event) {
            event.stopPropagation();

            startCoords = getCoordinates(event);

            moved = false;

            self.stopAuto();

            self.wrap.addEventListener('touchmove', touchMove);
            self.wrap.addEventListener('touchend', touchEnd);
        }

        function touchMove(event) {

            var coords = getCoordinates(event),
                dim = coords.main - startCoords.main,
                dir = dim < 0 ? 'next' : 'prev';

            if (validSwipe(coords, dim > 0 ? 1 : -1) === false) {
                self.wrap.removeEventListener('touchmove', touchMove);
                self.wrap.removeEventListener('touchend', touchEnd);
                startCoords = null;
                moved = false;
                return;
            }

            event.stopPropagation();
            event.preventDefault();

            if (Math.abs(dim) > MIN_HORIZONTAL_DISTANCE) {

                if (dir === 'prev' && self.options.indexActive > 0) {
                    self.prev();
                } else if (dir === 'next' && self.options.indexActive < self.items.length - 1) {
                    self.next();
                } else {
                    self.move(self.getMoveData(self.options.indexActive), true);
                }

                self.wrap.removeEventListener('touchmove', touchMove);
                self.wrap.removeEventListener('touchend', touchEnd);

                startCoords = null;

                if (self.options.auto === true) {
                    self.startAuto();
                }
            } else {
                self.move((self.transformValue || 0) + dim, false);
            }
        }

        function touchEnd(event) {
            var transformValue;

            if (moved === false) {
                transformValue = self.getMoveData(self.options.indexActive);
                self.move(transformValue, true);
            }

            self.wrap.removeEventListener('touchmove', touchMove);
            self.wrap.removeEventListener('touchend', touchEnd);
        }

        self.wrap.addEventListener('touchstart', touchStart);

    };

    Carousel.prototype.bindIt = function () {

        var self = this,
            startX, startY,
            options = self.options;

        if (isTouchDevice === true) {
            self.touch();
            window.addEventListener('orientationchange', self.update.bind(self));
        };


        if (options.auto === true) {
            //self.wrap.removeEventListener('mouseenter', self.stopAuto);
            self.wrap.addEventListener('mouseenter', function () {
                self.stopAuto();
            });

            //self.wrap.removeEventListener('mouseleave', self.startAuto);
            self.wrap.addEventListener('mouseleave', function () {
                self.startAuto();
            });
        }

        self.wrap.addEventListener('click', function (event) {
            var itemClicked;

            if (options.nav === true) {
                if (event.target === self.navNext) {
                    self.next();
                    return;
                } else if (event.target === self.navPrev) {
                    self.prev();
                    return;
                }
            }

            if (options.dots === true && closest(event.target, self.dotsContainer) !== null) {
                self.dotClick(event);
                return;
            }

            itemClicked = closest(event.target, '.js-carousel-item');

            if (itemClicked !== null) {
                self.itemClick(itemClicked);
            }
        });

        window.addEventListener('resize', function () {
            self.update();
        });
    };

    Carousel.prototype.init = function () {
        var self = this, sizes, clones;

        //self.cache.length = 0;

        self.processItems(self.items);

        self.generate(self.list);

        sizes = self.calc(self.items, self.options); //self.wrap, self.inner, self.list,

        self.setSizes(sizes.wrapSize, sizes.innerSize, sizes.listSize, sizes.itemsSize);

        if (self.options.dots === true && self.items.length !== 1 && self.countVisible !== self.items.length) {

            self.renderDots();
        }

        if (self.options.nav === true) {
            self.renderNav();
        };

        if (self.options.auto === true) {
            if (self.countVisible < self.items.length) {

                var cloneResult = self.doClone();

                sizes.listSize[self.propName] += Math.abs(cloneResult.marginLeftValue) * 2; //2 - с обеих сторон ширина клонированных слайдов

                self.setSizes(sizes.wrapSize, sizes.innerSize, sizes.listSize, sizes.itemsSize);

                var transform = self.getMoveData(self.options.indexActive);

                self.move(transform, false);
            }
        }

        self.checkNav();


        if (self.options.auto === true) {
            self.startAuto();
        };

        self.bindIt();

        if (self.dots != null) {
            self.selectDots(self.options.indexActive);
        }

        if (self.options.initFn != null) {
            self.options.initFn(self);
        }


        self.initilized = true;


        self.wrap.classList.add('carousel-initilized');

        return self;
    };

    Carousel.prototype.resetSizes = function () {
        var self = this;

        self.wrap.style[self.propName] = self.options.isVertical ? '100%' : 'auto';
        self.inner.style[self.propName] = self.options.isVertical ? '100%' : 'auto';
        self.list.style[self.propName] = self.options.isVertical ? '100%' : 'auto';

        for (var i = self.items.length - 1; i >= 0; i--) {
            self.items[i].style[self.propName] = 'auto';
            self.items[i].style['flex-basis'] = 'auto';
            self.items[i].style['msFlexPreferredSize'] = 'auto';
            self.items[i].style['webkitFlexBasis'] = 'auto';

            if (self.propName === 'width') {
                //self.items[i].style.minWidth = '0';
                self.items[i].style.maxWidth = 'none';
            } else {
                //self.items[i].style.minHeight = '0';
                self.items[i].style.maxHeight = 'none';
            }

            self.items[i].setAttribute('style', self.items[i].carouselItemData.stylesRaw);
        }
    };

    Carousel.prototype.update = function () {
        var self = this, sizes;

        self.wrap.classList.remove('carousel-nav-not-show');
        self.wrap.classList.add('carousel-update');

        self.resetSizes();

        //self.cache.length = 0;

        self.processItems(self.items);

        sizes = self.calc(self.items, self.options);

        self.setSizes(sizes.wrapSize, sizes.innerSize, sizes.listSize, sizes.itemsSize);

        if (self.options.auto === true) {
            
            var cloneResult = self.doClone();

            if (cloneResult != null) {
                sizes.listSize[self.propName] += Math.abs(cloneResult.marginLeftValue) * 2; //2 - с обеих сторон ширина клонированных слайдов
            }

            self.setSizes(sizes.wrapSize, sizes.innerSize, sizes.listSize, sizes.itemsSize);

            self.options.indexActive = 0;
        } else {
            self.checkNav();
        }

        var transform = self.getMoveData(self.options.indexActive);

        self.move(transform, false);


        if (self.options.dots === true) {

            if (self.dotsContainer != null && self.dotsContainer.parentNode != null) {
                self.dotsContainer.parentNode.removeChild(self.dotsContainer);
            }

            if (self.items.length !== 1 && self.countVisible !== self.items.length) {
                self.renderDots();
                self.selectDots(self.options.indexActive);
            }
        }

        self.wrap.classList.remove('carousel-update');
    }

    window.Carousel = Carousel;

    function createComponent(tagName) {

        if (clonesForCreate[tagName] == null) {
            clonesForCreate[tagName] = document.createElement(tagName);
        }

        return clonesForCreate[tagName].cloneNode();
    }

    function closest(element, selector) {
        var parent = element,
            matchesSelector;

        if (parent == null) {
            return null;
        }

        matchesSelector = parent.matches || parent.webkitMatchesSelector || parent.mozMatchesSelector || parent.msMatchesSelector;

        while (parent != document.body && parent != document && parent != null) {

            if (typeof (selector) === 'string') {
                if (matchesSelector.bind(parent)(selector) === true) {
                    return parent;
                }
            } else {
                if (parent == selector) {
                    return parent;
                }
            }

            parent = parent.parentNode;
        }

        return null;
    };

})(window);