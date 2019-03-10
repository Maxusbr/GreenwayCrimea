; (function (ng) {
    'use strict';

    ng.module('subblockInplace')
        .constant('subblockInplaceTextColors', [
            {
                "Color": "lightblack",
                "ColorId": "lightblack",
                "ColorCode": "#434142",
                "ColorName": "Светло-черная"
            },
            {
                "Color": "darkwhite",
                "ColorId": "darkwhite",
                "ColorCode": "#efefef",
                "ColorName": "Темно-белая"
            },
            {
                "Color": "black",
                "ColorId": "black",
                "ColorCode": "#000000",
                "ColorName": "Черная"
            },
            {
                "Color": "white",
                "ColorId": "white",
                "ColorCode": "#ffffff",
                "ColorName": "Белая"
            },
        ])
        .constant('subblockInplaceBackgroundColors', [{
            "Color": "_none",
            "ColorId": "_none",
            "ColorCode": "transparent",
            "ColorName": "Прозрачная"
        }, {
            "Color": "blue",
            "ColorId": "blue",
            "ColorCode": "#0662c1",
            "ColorName": "Синяя"
        }, {
            "Color": "white",
            "ColorId": "white",
            "ColorCode": "#ffffff",
            "ColorName": "Белая"
        },  {
            "Color": "darkwhite",
            "ColorId": "darkwhite",
            "ColorCode": "#efefef",
            "ColorName": "Темно-белая"
        }, {
            "Color": "turquoise",
            "ColorId": "turquoise",
            "ColorCode": "#7bc8a4",
            "ColorName": "Бирюзовая"
        }, {
            "Color": "turquoise2",
            "ColorId": "turquoise2",
            "ColorCode": "#72a6aa",
            "ColorName": "Бирюзовая 2",
        }, {
            "Color": "pale-red",
            "ColorId": "pale-red",
            "ColorCode": "#F3728A",
            "ColorName": "Бледно-красный"
        }, {
            "Color": "blue2",
            "ColorId": "blue2",
            "ColorCode": "#3792C5",
            "ColorName": "Голубая 2"
        }, {
            "Color": "yellow",
            "ColorId": "yellow",
            "ColorCode": "#f8c312",
            "ColorName": "Желтая"
        }, {
            "Color": "green",
            "ColorId": "green",
            "ColorCode": "#67b353",
            "ColorName": "Зеленая"
        }, {
            "Color": "green2",
            "ColorId": "green2",
            "ColorCode": "#3fc001",
            "ColorName": "Зеленая 2"
        }, {
            "Color": "brown",
            "ColorId": "brown",
            "ColorCode": "#864715",
            "ColorName": "Коричневая"
        }, {
            "Color": "red",
            "ColorId": "red",
            "ColorCode": "#e44937",
            "ColorName": "Красная"
        }, {
            "Color": "red2",
            "ColorId": "red2",
            "ColorCode": "#ff0129",
            "ColorName": "Красная 2"
        }, {
            "Color": "azure",
            "ColorId": "azure",
            "ColorCode": "#0095c0",
            "ColorName": "Лазурная"
        }, {
            "Color": "seablue",
            "ColorId": "seablue",
            "ColorCode": "#42bde6",
            "ColorName": "Морская"
        }, {
            "Color": "orange",
            "ColorId": "orange",
            "ColorCode": "#fe7e00",
            "ColorName": "Оранжевая",
        }, {
            "Color": "orange2",
            "ColorId": "orange2",
            "ColorCode": "#f39500",
            "ColorName": "Оранжевая 2"
        }, {
            "Color": "pink",
            "ColorId": "pink",
            "ColorCode": "#db44bc",
            "ColorName": "Розовая"
        }, {
            "Color": "pink2",
            "ColorId": "pink2",
            "ColorCode": "#ff4b9a",
            "ColorName": "Розовая 2"
        }, {
            "Color": "lightgreen",
            "ColorId": "lightgreen",
            "ColorCode": "#9ccf31",
            "ColorName": "Салатовый"
        }, {
            "Color": "lightgreen2",
            "ColorId": "lightgreen2",
            "ColorCode": "#90bf33",
            "ColorName": "Светло-зеленая 2"
        }, {
            "Color": "silver",
            "ColorId": "silver",
            "ColorCode": "#dedede",
            "ColorName": "Серебрянная"
        }, {
            "Color": "silver2",
            "ColorId": "silver2",
            "ColorCode": "#d6d6d6",
            "ColorName": "Серебрянная 2"
        }, {
            "Color": "blue3",
            "ColorId": "blue3",
            "ColorCode": "#0476d6",
            "ColorName": "Синяя 3"
        }, {
            "Color": "lilac",
            "ColorId": "lilac",
            "ColorCode": "#e594a3",
            "ColorName": "Сиреневая"
        }, {
            "Color": "dark",
            "ColorId": "dark",
            "ColorCode": "#4b4f58",
            "ColorName": "Темная"
        }, {
            "Color": "dark-green",
            "ColorId": "dark-green",
            "ColorCode": "#327f0f",
            "ColorName": "Темно-зеленая"
        }, {
            "Color": "darkgray",
            "ColorId": "darkgray",
            "ColorCode": "#868686",
            "ColorName": "Темно-серая",
        }, {
            "Color": "blue-black",
            "ColorId": "blue-black",
            "ColorCode": "#063b6f",
            "ColorName": "Темно-синяя",
        }, {
            "Color": "darkblue",
            "ColorId": "darkblue",
            "ColorCode": "#3c668e",
            "ColorName": "Темно-синяя",
        }, {
            "Color": "tomato",
            "ColorId": "tomato",
            "ColorCode": "#ad3c10",
            "ColorName": "Томатная"
        }, {
            "Color": "purple",
            "ColorId": "purple",
            "ColorCode": "#93648d",
            "ColorName": "Фиолетовая"
        }, {
            "Color": "purple2",
            "ColorId": "purple2",
            "ColorCode": "#9e2a8b",
            "ColorName": "Фиолетовая 2"
        }, {
            "Color": "black",
            "ColorId": "black",
            "ColorCode": "#434142",
            "ColorName": "Черная"
        }, {
            "Color": "brightYellow",
            "ColorId": "brightYellow",
            "ColorCode": "#ffdd00",
            "ColorName": "Ярко желтая"
        }]);

})(window.angular);