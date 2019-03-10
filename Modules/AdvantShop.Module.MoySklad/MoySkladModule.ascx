<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.MoySklad.Modules_MoySklad_MoySkladModule" Codebehind="MoySkladModule.ascx.cs" %>
<div>
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="3" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: MoySklad_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="margin-left: 10px;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td colspan="3">
                <b><asp:Localize ID="Localize30" runat="server" Text="Товарные позиции:"></asp:Localize></b>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px; width: 240px;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: MoySklad_TypeSyncProperties%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:DropDownList runat="server" ID="ddlSyncProp" Width="220">
                    <asp:ListItem Text="<%$ Resources: MoySklad_TypeSyncPropNoSync%>" Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: MoySklad_TypeSyncPropAddUpdate%>" Value="1"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: MoySklad_TypeSyncPropOneToOne%>" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="text-align: left;"><asp:Label ID="Label2" runat="server" ForeColor="Red" Text="<%$ Resources: MoySklad_TypeSyncPropertiesWarning%>"></asp:Label></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: MoySklad_TypeSyncDescription%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:DropDownList runat="server" ID="ddlSyncDesc" Width="220">
                    <asp:ListItem Text="<%$ Resources: MoySklad_TypeSyncDescNoSync%>" Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: MoySklad_TypeSyncDescLoadForNew%>" Value="1"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: MoySklad_TypeSyncDescAlways%>" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize18" runat="server" Text="<%$ Resources: MoySklad_TypeUpdateEnableProduct%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:DropDownList runat="server" ID="ddlUpdateEnableProduct" Width="220">
                    <asp:ListItem Text="<%$ Resources: MoySklad_TypeUpdateEnableProductNone%>" Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: MoySklad_TypeUpdateEnableProductOnlyNew%>" Value="1"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: MoySklad_TypeUpdateEnableProductAlways%>" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize33" runat="server" Text="<%$ Resources: MoySklad_ImportCurrencyInProduct%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:DropDownList runat="server" ID="ddlImportCurrency" Width="220"/>
            </td>
            <td style="text-align: left;"><asp:Label ID="Label11" runat="server" ForeColor="Red" Text="<%$ Resources: MoySklad_ImportCurrencyInProductWarning%>"></asp:Label></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize19" runat="server" Text="<%$ Resources: MoySklad_IsSetNoEnableProductNotSuncMoysklad%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="cbIsSetNoEnableProductNotSuncMoysklad"/>
            </td>
            <td style="text-align: left;"><asp:Label ID="Label6" runat="server" ForeColor="Red" Text="<%$ Resources: MoySklad_IsSetNoEnableProductNotSuncMoyskladWarning%>"></asp:Label></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize20" runat="server" Text="<%$ Resources: MoySklad_IsDeleteOfferNotSuncMoysklad%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="cbIsDeleteOfferNotSuncMoysklad"/>
            </td>
            <td style="text-align: left;"><asp:Label ID="Label7" runat="server" ForeColor="Red" Text="<%$ Resources: MoySklad_IsDeleteOfferNotSuncMoyskladWarning%>"></asp:Label></td>
        </tr>

         <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize24" runat="server" Text="<%$ Resources: MoySklad_DeleteOffersWithZeroAmount%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="cbDeleteOffersWithZeroAmount"/>
            </td>
            <td style="text-align: left;"><asp:Label ID="Label10" runat="server" ForeColor="Red" Text=""></asp:Label></td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize21" runat="server" Text="<%$ Resources: MoySklad_IsNewCategoryEnabled%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="cbIsNewCategoryEnabled"/>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize34" runat="server" Text="Обновлять только продукты (без остатков)"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="cbUpdateOnlyProducts" />
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize37" runat="server" Text="Установка товара под заказ если не в наличии"/>
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="cbAvailablePreOrder" />
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize38" runat="server" Text="Не менять артикул модификации на артикул товара если 1 модификация"/>
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="cbDontChangeArtnoToProductArtno" />
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                Не обновлять кол-во у товаров
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="cbNotUpdateAmount" />
            </td>
            <td style="text-align: left;"></td>
        </tr>       
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                Не обновлять цены (стоимость) у товаров
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="cbNotUpdatePrice" />
            </td>
            <td style="text-align: left;"></td>
        </tr>

        <tr class="rowsPost">
            <td colspan="3" style="padding-top: 25px;">
                <b><asp:Localize ID="Localize31" runat="server" Text="Заказы:"></asp:Localize></b>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: MoySklad_TypeSyncOrders%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:DropDownList runat="server" ID="ddlSyncOrders" Width="220">
                    <asp:ListItem Text="<%$ Resources: MoySklad_TypeSyncNewOrders%>" Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources: MoySklad_TypeSyncAllOrders%>" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                Добавлять к номеру заказа префикс
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtOrderPrefix" />
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: MoySklad_MaxSendOrders%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtSnowfallMaxSendOrders" Width="220"></asp:TextBox>
            </td>
            <td style="text-align: left;"><asp:Label ID="Label1" runat="server" ForeColor="Red" Text="<%$ Resources: MoySklad_MaxSendOrdersWarning%>"></asp:Label></td>
        </tr>
        <tr class="rowsPost">
            <td colspan="3">
                Для получения дополнительных данных по заказам в МойСклад по мере необходимости нужно в "Заказы покупателей" добавить следующие дополнительные поля:<br />
                <table>
                    <tr>
                        <td style="font-weight: bold;width: 122px;">Название</td>
                        <td style="font-weight: bold;width: 53px;">Тип</td>
                        <td style="font-weight: bold;">Обязательный</td>
                    </tr>
                    <tr>
                        <td>Адрес доставки</td>
                        <td>Строка</td>
                        <td>нет</td>
                    </tr>
                    <tr>
                        <td>Адрес плательщика</td>
                        <td>Строка</td>
                        <td>нет</td>
                    </tr>
                    <tr>
                        <td>Метод оплаты</td>
                        <td>Строка</td>
                        <td>нет</td>
                    </tr>
                    <tr>
                        <td>Заказ оплачен</td>
                        <td>Флажок</td>
                        <td>нет</td>
                    </tr>
                    <tr>
                        <td>Дата оплаты</td>
                        <td>Дата</td>
                        <td>нет</td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr class="rowsPost">
            <td colspan="3" style="padding-top: 25px;">
                <b><asp:Localize ID="Localize25" runat="server" Text="Синхронизация по API МойСклад:"></asp:Localize></b>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize26" runat="server" Text="Логин МойСклад"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtApiLogin" Width="220"></asp:TextBox>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize27" runat="server" Text="Пароль МойСклад"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtApiPassword" TextMode="Password" Width="220" CssClass="jspass"></asp:TextBox>
                <input type="hidden" id="inpApiPasswordCompare" runat="server" class="jspasscompare" />
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize28" runat="server" Text="Обновлять контрагентов из магазина"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="cbUpdateCustomersAndContacts"/>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize29" runat="server" Text="Обновлять статусы заказов из МойСклад в магазин"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="cbUpdateOrdersStatuses"/>
            </td>
            <td style="text-align: left;"></td>
        </tr>

        <tr class="rowsPost">
            <td colspan="3" style="padding-top: 25px;">
                <b><asp:Localize ID="Localize32" runat="server" Text="Прочие:"></asp:Localize></b>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize22" runat="server" Text="<%$ Resources: MoySklad_UseZip%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:CheckBox runat="server" ID="cbUseZip"/>
            </td>
            <td style="text-align: left;"><asp:Label ID="Label8" runat="server" ForeColor="Red" Text="<%$ Resources: MoySklad_UseZipWarning%>"></asp:Label></td>
        </tr>
        <tr class="rowsPost">
            <td colspan="3"><b><asp:Localize ID="Localize17" runat="server" Text="Название розничной цены в МоемСкладе:"></asp:Localize></b></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize23" runat="server" Text="Розничная цена (цена продукта)"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtRetailPriceName" Width="220"></asp:TextBox>
            </td>
            <td style="text-align: left;"><asp:Label ID="Label9" runat="server" ForeColor="Red" Text="Если название будет пустым, то цена будет искаться как 'розничная'"></asp:Label></td>
        </tr>

        <tr class="rowsPost">
            <td colspan="3"><b><asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: MoySklad_CharactToProduct%>"></asp:Localize></b></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize15" runat="server" Text="<%$ Resources: MoySklad_CharactColor%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtNameCharactColor" Width="220"></asp:TextBox>
            </td>
            <td style="text-align: left;" rowspan="2"><asp:Label ID="Label5" runat="server" ForeColor="Red" Text="<%$ Resources: MoySklad_CharactsWarning%>"></asp:Label></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize16" runat="server" Text="<%$ Resources: MoySklad_CharactSize%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtNameCharactSize" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td colspan="3"><b><asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: MoySklad_PropertyToProduct%>"></asp:Localize></b></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <b><asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: MoySklad_ProductProp%>"></asp:Localize></b>
            </td>
            <td style="text-align: left;">
                <b><asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: MoySklad_NameProperty%>"></asp:Localize></b>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources: MoySklad_PropertyWeight%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtNamePropWeight" Width="220"></asp:TextBox>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources: MoySklad_PropertySize%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtNamePropSize" Width="220"></asp:TextBox>
            </td>
            <td style="text-align: left;"><asp:Label ID="Label3" runat="server" ForeColor="Red" Text="<%$ Resources: MoySklad_PropertySizeWarning%>"></asp:Label></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize12" runat="server" Text="<%$ Resources: MoySklad_PropertyBrand%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtNamePropBrand" Width="220"></asp:TextBox>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources: MoySklad_PropertyDiscount%>"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtNamePropDiscount" Width="220"></asp:TextBox>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize35" runat="server" Text="Gtin"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtNamePropGtin" Width="220"></asp:TextBox>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize36" runat="server" Text="Штрих код"></asp:Localize>
            </td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtBarCode" Width="220"></asp:TextBox>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td colspan="2"><b><asp:Localize ID="Localize14" runat="server" Text="<%$ Resources: MoySklad_PropertyNoLoad%>"></asp:Localize></b><br/>
                <asp:Label ID="Label4" runat="server" Text="<%$ Resources: MoySklad_PropertyNoLoadInfo%>"></asp:Label>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr class="rowsPost">
            <td colspan="2">
                <asp:TextBox runat="server" ID="txtPropNoLoad" Width="100%" Height="130px" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td style="text-align: left;"></td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: MoySklad_Save%>" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label runat="server" ID="lblErr" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
</div>
<script>
    $(function () {
        var pass = $('.jspass');
        var passCompare = $('.jspasscompare');

        if (pass != null && passCompare != null) {
            pass.val(passCompare.val());
        }
    });
</script>
