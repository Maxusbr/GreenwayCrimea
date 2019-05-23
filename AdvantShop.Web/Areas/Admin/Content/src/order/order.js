; (function (ng) {
    'use strict';

    var OrderCtrl = function (uiGridCustomConfig, $http, toaster, $timeout, $uibModal, $q, SweetAlert, lastStatisticsService) {

        var ctrl = this;

        ctrl.initOrder = function (orderId, isEditMode, isDraft, customerId, standardPhone) {
            ctrl.orderId = orderId;
            ctrl.isEditMode = isEditMode;
            ctrl.isDraft = isDraft;
            ctrl.customerId = customerId;
            ctrl.standardPhone = standardPhone;
        }

        ctrl.startGridOrderItems = function (isPaied) {
            ctrl.isPaied = isPaied;
            
            ctrl.gridOrderItemsOptions = ng.extend({}, uiGridCustomConfig, {
                rowHeight: 95,
                columnDefs: [
                    {
                        name: 'ImageSrc',
                        displayName: '',
                        cellTemplate: '<div class="ui-grid-cell-contents"><img class="ui-grid-custom-col-img" ng-src="{{row.entity.ImageSrc}}"></div>',
                        width: 100,
                        enableSorting: false,
                    },
                    {
                        name: 'Name',
                        displayName: 'Название',
                        cellTemplate:
                            '<div class="ui-grid-cell-contents ui-grid-cell-contents-order-items">' +
                                '<div ng-if="row.entity.ProductLink != null"><a href="{{row.entity.ProductLink}}" target="_blank">{{row.entity.Name}}</a></div> ' +
                                '<div ng-if="row.entity.ProductLink == null">{{row.entity.Name}}</div> ' +
                            
                                '<div class="order-item-artno">Артикул: {{row.entity.ArtNo}}</div> ' +
                                '<div ng-if="row.entity.Color != null && row.entity.Color.length > 0">{{row.entity.Color}}</div>' +
                                '<div ng-if="row.entity.Size != null && row.entity.Size.length > 0">{{row.entity.Size}}</div>' +
                                '<div ng-if="row.entity.CustomOptions != null && row.entity.CustomOptions.length > 0"> <div ng-bind-html="row.entity.CustomOptions"></div> </div>' +
                            '</div>',
                    },
                    {
                        name: 'Price',
                        displayName: 'Цена',
                        enableCellEdit: true,
                        width: 100,
                    },
                    {
                        name: 'Amount',
                        displayName: 'Кол-во',
                        enableCellEdit: true,
                        width: 75,
                    },
                    //{
                    //    name: 'Available',
                    //    displayName: 'Доступно на складе',
                    //    cellTemplate: '<div class="ui-grid-cell-contents"><span ng-class="{\'order-avalable\' : row.entity.Available , \'order-notavalable\': !row.entity.Available}">{{row.entity.AvailableText}}</span></div>',
                    //    width: 105,
                    //},
                    {
						name: 'Weight',
                        displayName: 'Объем',
                        width: 100,
                    },
                    {
                        name: 'Cost',
                        displayName: 'Стоимость',
                        width: 100,
                    },
                    {
                        name: '_serviceColumn',
                        displayName: '',
                        width: 35,
                        cellTemplate:
                            '<div class="ui-grid-cell-contents"><div>' +
                                (isPaied === false ? '<ui-grid-custom-delete url="orders/deleteOrderItem" params="{\'orderId\': row.entity.OrderId, \'orderItemId\': row.entity.OrderItemId }"></ui-grid-custom-delete>' : '<a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.checkStopEdit()" class="ui-grid-custom-service-icon fa fa-times link-invert"></a>') +
                            '</div></div>'
                    }
                ],
            });

            ctrl.gridOrderCertificatesOptions = ng.extend({}, uiGridCustomConfig, {
                columnDefs: [
                    {
                        name: 'CustomName',
                        displayName: '',
                        cellTemplate: '<div class="ui-grid-cell-contents">Сертификат</div>'
                    },
                    {
                        name: 'CertificateCode',
                        displayName: 'Код сертификата'
                    },
                    {
                        name: 'Sum',
                        displayName: 'Сумма'
                    },
                    {
                        name: 'Price',
                        displayName: 'Применен в заказе №'
                    }
                ]
            });

            ctrl.isShowGridOrderItem = true;
        };


        ctrl.gridOrderItemsOnInit = function (grid) {
            ctrl.gridOrderItems = grid;
        };

        ctrl.gridOrderItemsSelectionOnInit = function(selectionCustom) {
            ctrl.selectionCustom = selectionCustom;
        }

        ctrl.addOrderItems = function(result) {

            if (result == null || result.ids == null || result.ids.length == 0)
                return;

            ctrl.saveDraft().then(function () {

                $http.post("orders/addOrderItems", { orderId: ctrl.orderId, offerIds: result.ids })
                    .then(function(response) {
                        if (response.data.result === true) {
                            toaster.pop('success', '', 'Товар успешно добавлен');
                        }
                    })
                    .then(ctrl.gridOrderItemUpdate)
                    .catch(function () {
                        toaster.pop('error', 'Ошибка при добавлении заказа');
                    });
            });
        }

        ctrl.gridOrderItemUpdate = function () {
            ctrl.gridOrderItems.fetchData();
            ctrl.orderItemsSummaryUpdate();
        }

        ctrl.gridOrderItemDelete = function() {
            ctrl.orderItemsSummaryUpdate();
        }

        ctrl.initOrderItemsSummary = function (orderItemsSummary) {
            ctrl.orderItemsSummary = orderItemsSummary;
        }

        ctrl.orderItemsSummaryUpdate = function () {
            if (ctrl.orderItemsSummary != null) {
                ctrl.orderItemsSummary.getOrderItemsSummary();
            }
        }
        
        ctrl.changeStatus = function () {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalChangeOrderStatusCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/order/modal/changeOrderStatus/changeOrderStatus.html',
                resolve: {
                    params: {
                        orderId: ctrl.orderId,
                        statusId: ctrl.orderStatus,
                        statusName: $(".orderstatus option:selected").text()
                    }
                }
            }).result.then(function (result) {
                if (result == null)
                    ctrl.orderStatus = ctrl.orderStatusOld;
                ctrl.modalClose(); //тут при успешном закрытии
                ctrl.orderStatusOld = ctrl.orderStatus;
                return result;
            }, function (result) {
                if (result === "cancelChangeOrderStatus")
                    ctrl.orderStatus = ctrl.orderStatusOld;
                ctrl.modalDismiss();  //тут при неудачном закрытии, отмене
                return result;
            });
        }

        ctrl.setPaied = function (checked) {

            $http.post("orders/setPaied", { orderId: ctrl.orderId, paid: checked }).then(function (response) {
                if (response.data.result === true) {
                    toaster.pop('success', '', 'Изменения сохранены');
                }
            });
        }

        ctrl.selectCustomer = function(result) {
            ctrl.getCustomer(result)
                .then(function(result) {
                    return result || $q.reject('error');
                })
                .then(ctrl.saveDraft);
        }

        ctrl.changeCustomer = function (orderCustomerForm) {
            ctrl.saveDraft().then(function () {
                orderCustomerForm.$setPristine();
            });
        }

        ctrl.saveChanges = function () {
            if (ctrl.isEditMode === true) {
                ctrl.saveOrderInfo();
            } else {
                ctrl.saveDraft();
            }
        }

        ctrl.getCustomer = function(result) {
            if (result == null || result.customerId == null) {
                return false;
            }

            return $http.get("customers/getCustomerWithContact", { params: { customerId: result.customerId}}).then(function (response) {

                var customer = response.data;

                if (customer == null) return false;

                ctrl.customerId = customer.Id;
                ctrl.firstName = ctrl.selectedFirstName = customer.FirstName;
                ctrl.lastName = ctrl.selectedLastName = customer.LastName;
                ctrl.patronymic = customer.Patronymic;
                ctrl.email = customer.Email;
                ctrl.phone = customer.Phone;
                ctrl.standardPhone = customer.StandardPhone;

                ctrl.bonusCardNumber = customer.BonusCardNumber;
                ctrl.customerGroup = customer.CustomerGroup;
                var contacts = customer.Contacts;

                if (contacts != null && contacts.length > 0) {
                    var contact = contacts[0];

                    ctrl.country = contact.Country;
                    ctrl.region = contact.Region;
                    ctrl.city = contact.City;
                    ctrl.zip = contact.Zip;
                    ctrl.street = contact.Street;
                    ctrl.entrance = contact.Entrance;
                    ctrl.floor = contact.Floor;
                    ctrl.house = contact.House;
                    ctrl.structure = contact.Structure;
                    ctrl.apartment = contact.Apartment;
                    
                    ctrl.customField1 = contact.CustomField1;
                    ctrl.customField2 = contact.CustomField2;
                    ctrl.customField3 = contact.CustomField3;
                }
                return true;
            });
        }

        // save draft
        ctrl.saveDraft = function () {

            if (!ctrl.isDraft) {
                return $q.resolve();
            }

            var orderId = ctrl.orderId;

            var params = {

                orderId: ctrl.orderId,

                orderCustomer: {
                    customerId: ctrl.customerId,
                    firstName: ctrl.firstName,
                    lastName: ctrl.lastName,
                    patronymic: ctrl.patronymic,
                    email: ctrl.email,
                    phone: ctrl.phone,
                    standardPhone: ctrl.standardPhone,
                    country: ctrl.country,
                    region: ctrl.region,
                    city: ctrl.city,
                    zip: ctrl.zip,
                    address: ctrl.address,
                    customField1: ctrl.customField1,
                    customField2: ctrl.customField2,
                    customField3: ctrl.customField3,
                    street: ctrl.street,
                    house: ctrl.house,
                    apartment: ctrl.apartment,
                    structure: ctrl.structure,
                    entrance: ctrl.entrance,
                    floor: ctrl.floor
                },

                statusComment: ctrl.statusComment,
                adminOrderComment: ctrl.adminOrderComment,
                orderSourceId: ctrl.orderSourceId,
                managerId: ctrl.managerId,
                trackNumber: ctrl.trackNumber
            }

            return $http.post("orders/saveDraft", params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    if (orderId === 0 && data.orderId !== 0) {
                        toaster.pop('success', '', 'Создан черновик заказа № ' + data.orderId);
                        ctrl.orderId = data.orderId;
                        ctrl.customerId = data.customerId;

                        ctrl.gridOrderItems.setParams({ OrderId: data.orderId });
                    } else {
                        toaster.pop('success', '', 'Изменения сохранены ');
                    }

                    ctrl.isEditMode = true;
                    ctrl.isDraft = true;
                }

                return data;
            });
        }

        // save order information in edit mode
        ctrl.saveOrderInfo = function () {

            if (ctrl.isDraft) {
                return $q.resolve();
            }
            
            var params = {
                orderId: ctrl.orderId,

                statusComment: ctrl.statusComment,
                adminOrderComment: ctrl.adminOrderComment,
                trackNumber: ctrl.trackNumber
            }

            return $http.post("orders/SaveOrderInfo", params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', 'Изменения сохранены ');
                }
                return data;
            });
        }

        ctrl.updateOrderBonusCard = function() {
            $http.post('orders/updateOrderBonusCard', { orderId: ctrl.orderId }).then(function (response) {
                window.location.reload();
            });
        }

        ctrl.deleteOrder =  function() {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('orders/deleteOrder', { orderId: ctrl.orderId }).then(function (response) {
                        lastStatisticsService.getLastStatistics();
                        window.location.assign('orders');
                    });
                }
            });
        }

        ctrl.getMapAddress = function() {
            var address = ctrl.country != null ? ctrl.country : "";
            address += (address.length > 0 ? ", " : "") + (ctrl.region != null ? ctrl.region : "");
            address += (address.length > 0 ? ", " : "") + (ctrl.city != null ? ctrl.city : "");
            if (ctrl.address != null && ctrl.address !== '') {
                address += (address.length > 0 ? ", " : "") + (ctrl.address != null ? ctrl.address : "");
            } else {
                address += (address.length > 0 ? ", " : "") + (ctrl.street != null ? ctrl.street : "");
                address += (address.length > 0 ? ", " : "") + (ctrl.house != null ? ctrl.house : "");
            }

            return encodeURIComponent(address);
        }

        ctrl.findCustomers = function(val) {
            if (ctrl.isDraft && val != null && val.length > 1) {
                return $http.get("customers/getCustomersAutocomplete?q=" + val).then(function (response) {
                    return response.data;
                });
            }
        }

        ctrl.selectCustomerByAutocomplete = function ($item, $model, $label, $event) {
            var customerId = $item.value;
            return ctrl.getCustomer({ customerId: customerId });
        }

        ctrl.dateChange = function (date) {
            alert('Изменение даты заказа: ' + date.toString());
        }
        
        ctrl.checkStopEdit = function () {
            var result = true;

            if (ctrl.isPaied === true) {
                SweetAlert.alert("Оплаченный заказ не может быть изменен", { title: "Изменение заказа" });
                result = false;
            }
      
            return result;
        }

        ctrl.gridOnInplaceBeforeApply = function () {
            return ctrl.checkStopEdit();
        }

        ctrl.resetOrderCustomer = function() {
            ctrl.customerId = null;
            ctrl.firstName = ctrl.selectedFirstName = null;
            ctrl.lastName = ctrl.selectedLastName = null;
            ctrl.patronymic = null;
            ctrl.email = null;
            ctrl.phone = null;
            ctrl.standardPhone = null;
            ctrl.country = null;
            ctrl.region = null;
            ctrl.city = null;
            ctrl.zip = null;
            ctrl.street = null;
            ctrl.entrance = null;
            ctrl.floor = null;
            ctrl.house = null;
            ctrl.structure = null;
            ctrl.apartment = null;
        }
    };

    OrderCtrl.$inject = ['uiGridCustomConfig', '$http', 'toaster', '$timeout', '$uibModal', '$q', 'SweetAlert', 'lastStatisticsService'];


    ng.module('order', ['uiGridCustom', 'urlHelper', 'ui.bootstrap', 'orderItemsSummary', 'spinbox', 'shipping', 'payment', 'orderStatusHistory', 'orderHistory'])
      .controller('OrderCtrl', OrderCtrl);

})(window.angular);