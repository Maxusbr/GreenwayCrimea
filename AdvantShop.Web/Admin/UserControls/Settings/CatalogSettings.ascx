<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Settings.CatalogSettings" CodeBehind="CatalogSettings.ascx.cs" %>
<table class="info-tb">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_HeadCatalog%>
            </span>
            <br />
            <span class="subTitleNotify">Настройки категорий
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">Вывод товаров
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 295px;">
            <label class="form-lbl" for="<%= chkShowQuickView.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowQuickView%></label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkShowQuickView" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 295px;">
            <label class="form-lbl" for="<%= txtProdPerPage.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ProductPerPage%></label>
        </td>
        <td>
            <asp:TextBox ID="txtProdPerPage" runat="server" class="niceTextBox shortTextBoxClass3"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Количество товаров на странице
                    </header>
                    <div class="help-content">
                        Опция определяет, какое количество товаров выводить в категории.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbShowProductsCount.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowProductsCount%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbShowProductsCount" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Отображать количество товаров в категории
                    </header>
                    <div class="help-content">
                        Если опция <b>включена</b>, в клиентской части, рядом с названием категории в скобочках, выводится количество товаров в ней.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbShowProductArtNo.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowProductArtNo%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbShowProductArtNo" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Показывать артикул товара
                    </header>
                    <div class="help-content">
                        Показывать артикул товара в каталоге для представления "плитка"
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbEnableProductRating.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EnableProductRating%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbEnableProductRating" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Выводить рейтинг для товаров
                    </header>
                    <div class="help-content">
                        Выводить или нет рейтинг (звездочки) для товаров в категории.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbEnableCompareProducts.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EnableCompareProducts%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbEnableCompareProducts" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Разрешить сравнение товаров
                    </header>
                    <div class="help-content">
                        Определяет выводить или нет блок сравнения товаров.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbEnablePhotoPreviews.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EnablePhotoPreviews%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbEnablePhotoPreviews" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Показывать превью фотографий в каталоге
                    </header>
                    <div class="help-content">
                        В категории при наведении мыши на товар, рядом с основной картинкой товара будут показаны другие фотографии товара.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbShowCountPhoto.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowCountPhoto%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbShowCountPhoto" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Показывать количество фотографий
                    </header>
                    <div class="help-content">
                        Опция определяет отображать или нет в каталоге у товара небольшую иконку с количеством фотографий у товара.
                    </div>
                </article>
            </div>
        </td>
    </tr>

    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbShowOnlyAvalible.ClientID %>">Показывать только товары в наличии</label>
        </td>
        <td>
            <asp:CheckBox ID="cbShowOnlyAvalible" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Показывать только товары в наличии
                    </header>
                    <div class="help-content">
                        Опция определяет отображать или нет в каталоге товары, которых нет в наличии.
                    </div>
                </article>
            </div>
        </td>
    </tr>

    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbShowOnlyAvalible.ClientID %>">Товары не в наличии перемещать в конец списка</label>
        </td>
        <td>
            <asp:CheckBox ID="cbMoveNotAvaliableToEnd" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Товары не в наличии перемещать в конец списка
                    </header>
                    <div class="help-content">
                        При активации этой опции, товары которых нет в наличии будут перемещаться в конец списка, не смотря на выбранную сортировку.
                    </div>
                </article>
            </div>
        </td>
    </tr>

    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_HeadFilters%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkShowPriceFilter.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowPriceFilter%></label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowPriceFilter" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Фильтр по цене
                    </header>
                    <div class="help-content">
                        Выводить или нет фильтр по цене.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkShowProducerFilter.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowProducerFilter%></label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowProducerFilter" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Фильтр производителей
                    </header>
                    <div class="help-content">
                        Выводить или нет фильтр по производителям.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkShowSizeFilter.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowSizeFilter%></label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowSizeFilter" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Фильтр для характеристики "Размер"
                    </header>
                    <div class="help-content">
                        Выводить или нет фильтр по параметру "Размер".
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkShowColorFilter.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowColorFilter%></label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowColorFilter" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Фильтр для характеристики "Цвет"
                    </header>
                    <div class="help-content">
                        Выводить или нет фильтр по параметру "Цвет".<br />
                        Цвета в фильтре отображаются в виде кубиков.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbExluderingFilters.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ExluderingFilters%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbExluderingFilters" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Использовать исключающие фильтры
                    </header>
                    <div class="help-content">
                        Если опция включена, то при поиске по фильтру значения, которые заведомо не дадут результата, будут отмечены серым.<br />
                        <br />
                        Это позволяет пользователю заранее понять, что серые значения в фильтре не улучшат результат фильтрации, и можно не использовать их.<br />
                        <br />
                        Однако, для работы данной опции необходимо больше ресурсов хостинга.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_SizesAndColors%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtSizesHeader.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_SizesHeader%></label>
        </td>
        <td>
            <asp:TextBox ID="txtSizesHeader" runat="server" CssClass="niceTextBox shortTextBoxClass" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Заголовок для характеристики "Размер"
                    </header>
                    <div class="help-content">
                        Вы можете задать своё название для характеристики "Размер" товара.
                        <br />
                        <br />
                        Например: Объем.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtColorsHeader.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ColorsHeader%></label>
        </td>
        <td>
            <asp:TextBox ID="txtColorsHeader" runat="server" CssClass="niceTextBox shortTextBoxClass" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Заголовок для характеристики "Цвет"
                    </header>
                    <div class="help-content">
                        Вы можете задать своё название для характеристики "Цвет" товара.
                        <br />
                        <br />
                        Например: Текстура.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtColorPictureWidthCatalog.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ColorsPictureWidth%></label>
        </td>
        <td>
            <%= Resources.Resource.Admin_CommonSettings_ColorsPictureInCatalog%>:
            <asp:TextBox ID="txtColorPictureWidthCatalog" runat="server" class="niceTextBox shortTextBoxClass3" /><span class="paramUnit">px,</span>&nbsp;
            <%= Resources.Resource.Admin_CommonSettings_ColorsPictureInDetails%>:
            <asp:TextBox ID="txtColorPictureWidthDetails" runat="server" class="niceTextBox shortTextBoxClass3" /><span class="paramUnit">px</span>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Ширина иконки цвета
                    </header>
                    <div class="help-content">
                        Размеры ширины (в пикселях) иконки цвета.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtColorPictureHeightCatalog.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ColorsPictureHeight%></label>
        </td>
        <td>
            <%= Resources.Resource.Admin_CommonSettings_ColorsPictureInCatalog%>:
            <asp:TextBox ID="txtColorPictureHeightCatalog" runat="server" class="niceTextBox shortTextBoxClass3" /><span class="paramUnit">px,</span>&nbsp;
            <%= Resources.Resource.Admin_CommonSettings_ColorsPictureInDetails%>:
            <asp:TextBox ID="txtColorPictureHeightDetails" runat="server" class="niceTextBox shortTextBoxClass3" /><span class="paramUnit">px</span>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Высота иконки цвета
                    </header>
                    <div class="help-content">
                        Размеры высоты (в пикселях) иконки цвета.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbComplexFilter.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ComplexFilter%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbComplexFilter" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Учитывать связь цена-цвет-фотография
                    </header>
                    <div class="help-content">
                        Если опция включена, то у товара в каталоге будут показаны кубики цвета, при нажатии на которые отобразится фотография, соответствующая выбранному цвету.
                        <br />
                        <br />
                        Данная опция требует больше ресурсов хостинга.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_ButtonText%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtBuyButtonText.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_AddButtonText%></label>
        </td>
        <td>
            <asp:TextBox ID="txtBuyButtonText" runat="server" CssClass="niceTextBox shortTextBoxClass" />
            <asp:CheckBox runat="server" ID="cbDisplayBuyButton" CssClass="checkly-align" Style="margin-left: 5px;" />
            <label class="form-lbl" style="display: inline;" for="<%= cbDisplayBuyButton.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_DisplayButton%></label>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Кнопка добавить в корзину
                    </header>
                    <div class="help-content">
                        Отображать или нет кнопку "Добавить" у товара в каталоге.<br />
                        <br />
                        Вы также можете задать свой текст для данной кнопки.<br />
                        <br />
                        Например: Купить.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtPreOrderButtonText.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_PreOrderButtonText%></label>
        </td>
        <td>
            <asp:TextBox ID="txtPreOrderButtonText" runat="server" CssClass="niceTextBox shortTextBoxClass" />
            <asp:CheckBox ID="cbDisplayPreOrderButton" runat="server" CssClass="checkly-align" Style="margin-left: 5px;" />
            <label class="form-lbl" style="display: inline;" for="<%= cbDisplayPreOrderButton.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_DisplayButton%></label>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Кнопка под заказ
                    </header>
                    <div class="help-content">
                        Отображать или нет кнопку "Под заказ" у товара в каталоге.<br />
                        <br />
                        Вы также можете задать свой текст для данной кнопки.<br />
                        <br />
                        Например: Заказать.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_Currencies%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ddlDefaultCurrency.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_DefaultCurrency%></label>
        </td>
        <td>
            <asp:DropDownList ID="ddlDefaultCurrency" runat="server"></asp:DropDownList>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Валюта по умолчанию
                    </header>
                    <div class="help-content">
                        Валюта магазина.
                        <br />
                        <br />
                        Также если блок отображения валют включен, эта валюта будет в нем показана по умолчанию.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbAllowToChangeCurrency.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_AllowToChangeCurrency%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbAllowToChangeCurrency" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Разрешать переключение валюты
                    </header>
                    <div class="help-content">
                        Показывать или нет блок смены валют в клиентской части магазина.<br />
                        <br />
                        Если вы работаете только с одной валютой в магазине, рекомендуем отключить этот блок.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <div class="dvSubHelp">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                <a href="http://www.advantshop.net/help/pages/currency" target="_blank">Инструкция. Настройка параметров валюты в магазине</a>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_AllowChangeCatalogView%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbEnableCatalogViewChange.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EnableCatalogViewChange%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbEnableCatalogViewChange" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Переключение вида каталога
                    </header>
                    <div class="help-content">
                        Разрешить или нет в клиентской части пользователям переключать вид отображения товаров в категории.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbEnableSearchViewChange.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EnableSearchViewChange%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbEnableSearchViewChange" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Переключение вида каталога
                    </header>
                    <div class="help-content">
                        Разрешить или нет в клиентской части пользователям переключать вид отображения товаров в результатах поиска.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_Search%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_CommonSettings_SearchIndex%>
        </td>
        <td>
            <asp:LinkButton runat="server" ID="btnDoindex" Text="<%$ Resources:Resource, Admin_CommonSettings_Generate %>" OnClick="btnDoindex_Click" />
            <asp:Label runat="server" ID="lbDone" Text="<%$ Resources:Resource, Admin_CommonSettings_RunningInBackGroung%>" Visible="False"></asp:Label>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Индекс для поиска 
                    </header>
                    <div class="help-content">
                        Как правило, индекс для поиска перестраивается автоматически при добавлении товара или категории.<br />
                        <br />
                        Однако бывают ситуации, когда перестройку индексов поиска необходимо вызвать вручную.<br />
                        <br />
                        Процедура запускается в фоне, время выполнения в пределах 30 секунд.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_CommonSettings_Search_Deep%>
        </td>
        <td>
            <asp:DropDownList ID="ddlDeep" runat="server"></asp:DropDownList>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Глубина поиска
                    </header>
                    <div class="help-content">
                        Параметр указывает какими способами системе искать результат<br />
                        <br />
                        1 уровень - жеское соотвествие фразы<br />
                        <br />
                        2 уровень - 1 + поиск по отдельным словам<br />
                        <br />
                        3 уровень - 1 + 2 + поиск значения по принципу "слово*"<br />
                        <br />
                        4 уровень - выше указанные + поиск по принципу "*слово*" <br />
                        <br />
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_CommonSettings_Search_MaxItems%>
        </td>
        <td>
           <asp:TextBox ID="txtSearchMaxItems" runat="server" Width="200px"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Максимальное кол-во возвращеных результатов
                    </header>
                    <div class="help-content">
                        Число указывает на значение возвращеных результатов<br />
                        <br />
                        Чем больше число тем больше может быть разбросаны результаты<br />
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <%= Resources.Resource.Admin_CommonSettings_SearchExample%>
        </td>
        <td>
            <asp:TextBox ID="txtSearchExample" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Пример поискового запроса
                    </header>
                    <div class="help-content">
                        Пример поискового запроса в шапке сайта<br />
                        При клике по нему будет производится поиск<br />
                        по данномоу тексту.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_DefaultView%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ddlCatalogView.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_DefaultCatalogView%></label>
        </td>
        <td>
            <asp:DropDownList ID="ddlCatalogView" runat="server">
                <asp:ListItem Text="<%$ Resources:Resource, Client_Catalog_Tiles %>" Value="0"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Resource, Client_Catalog_List %>" Value="1"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Resource, Client_Catalog_Table %>" Value="2"></asp:ListItem>
            </asp:DropDownList>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Вид отображения
                    </header>
                    <div class="help-content">
                        Какой вид отображения использовать для товаров в категориях.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ddlSearchView.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_DefaultSearchView%></label>
        </td>
        <td>
            <asp:DropDownList ID="ddlSearchView" runat="server">
                <asp:ListItem Text="<%$ Resources:Resource, Client_Catalog_Tiles %>" Value="0"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Resource, Client_Catalog_List %>" Value="1"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Resource, Client_Catalog_Table %>" Value="2"></asp:ListItem>
            </asp:DropDownList>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Вид отображения
                    </header>
                    <div class="help-content">
                        Какой вид отображения использовать для товаров в результатах поиска.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbShowCategoriesInBottomMenu.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowCategoriesInBottom%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbShowCategoriesInBottomMenu" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Категории в нижнем меню
                    </header>
                    <div class="help-content">
                        Показывать или нет список категорий в нижнем меню сайта. Если категории на ряду с обычными пунктами нижнего меню не нужны, отключите опцию.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_Brands%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ddlCatalogView.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowProductsInBrand%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbShowProductsInBrand" runat="server" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Показывать список товаров
                    </header>
                    <div class="help-content">
                        Опция определяет, отображать или нет на странице бренда список товаров данного бренда.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ddlCatalogView.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowCategoryTreeInBrand%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbShowCategoryTree" runat="server" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Показывать дерево категорий
                    </header>
                    <div class="help-content">
                        Опция определяет, отображать или нет на странице бренда категории, в которых имеются товары данного бренда.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ddlCatalogView.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_BrandsPerPage%></label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtBrandsPerPage" Text="12" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Количество производителей на странице /manufacturers
                    </header>
                    <div class="help-content">
                        Количество производителей на странице /manufacturers
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_ProductList%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ddlCatalogView.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ProductList_BestDescription%></label>
        </td>
        <td>
            <asp:TextBox ID="txBBestDescription" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="300px" Width="300px" EnableViewState="False" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ddlCatalogView.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ProductList_NewDescription%></label>
        </td>
        <td>
            <asp:TextBox ID="txBNewDescription" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="300px" Width="300px" EnableViewState="False" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ddlCatalogView.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ProductList_DiscountDescription%></label>
        </td>
        <td>
            <asp:TextBox ID="txBDiscountDescription" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="300px" Width="300px" EnableViewState="False" />
        </td>
    </tr>

</table>
<asp:SqlDataSource ID="SqlDataSource2" runat="server" SelectCommand="SELECT [Name], [Code], [CurrencyIso3] FROM [Catalog].[Currency]"
    OnInit="SqlDataSource2_Init"></asp:SqlDataSource>
