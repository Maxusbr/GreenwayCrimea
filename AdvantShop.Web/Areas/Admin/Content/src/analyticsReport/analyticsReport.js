; (function (ng) {
    'use strict';

    var AnalyticsReportCtrl = function (toaster) {

        var ctrl = this;

        ctrl.$onInit = function() {
            ctrl.selectedTab = 'vortex';
        }

        ctrl.showTab = function (tab) {

            ctrl.showPaid = false;
            ctrl.showOrderStatus = false;
            
            switch (tab) {
                case 'vortex':
                    ctrl.vortex.recalc(ctrl.dateFrom, ctrl.dateTo);
                    break;

                case 'profit':
                    ctrl.showPaid = true;
                    ctrl.showOrderStatus = true;

                    ctrl.profit.recalc(ctrl.dateFrom, ctrl.dateTo, ctrl.paid, ctrl.orderStatus);
                    break;

                case 'avgcheck':
                    ctrl.showPaid = true;
                    ctrl.showOrderStatus = true;

                    ctrl.avgcheck.recalc(ctrl.dateFrom, ctrl.dateTo, ctrl.paid, ctrl.orderStatus);
                    break;

                case 'orders':
                    ctrl.showPaid = true;
                    ctrl.showOrderStatus = true;

                    ctrl.orders.recalc(ctrl.dateFrom, ctrl.dateTo, ctrl.paid, ctrl.orderStatus);
                    break;

                case 'abcxyz':
                    ctrl.abcxyz.recalc(ctrl.dateFrom, ctrl.dateTo);
                    break;

                case 'rfm':
                    ctrl.rfm.recalc(ctrl.dateFrom, ctrl.dateTo);
                    break;

                case 'telephony':
                    ctrl.telephony.recalc(ctrl.dateFrom, ctrl.dateTo);
                    break;

                case 'managers':
                    ctrl.managers.recalc(ctrl.dateFrom, ctrl.dateTo);
                    break;
            }

            if (ctrl.selectedTab !== tab) {
                ctrl.selectedTab = tab;
            }
        }

        ctrl.updateData = function () {
            ctrl.showTab(ctrl.selectedTab);
        }



        ctrl.onInitVortex = function (vortex) {
            ctrl.vortex = vortex;
            ctrl.showTab('vortex');
        };

        ctrl.onInitProfit = function (profit) {
            ctrl.profit = profit;
        };

        ctrl.onInitAvgcheck = function (avgcheck) {
            ctrl.avgcheck = avgcheck;
        };

        ctrl.onInitOrders = function (orders) {
            ctrl.orders = orders;
        };

        ctrl.onInitAbcxyz = function (abcxyz) {
            ctrl.abcxyz = abcxyz;
        };

        ctrl.onInitRfm = function (rfm) {
            ctrl.rfm = rfm;
        };

        ctrl.onInitTelephony = function (telephony) {
            ctrl.telephony = telephony;
        };

        ctrl.onInitManagers = function (managers) {
            ctrl.managers = managers;
        };

    };

    AnalyticsReportCtrl.$inject = ['toaster'];

    ng.module('analyticsReport', [])
      .controller('AnalyticsReportCtrl', AnalyticsReportCtrl);

})(window.angular);