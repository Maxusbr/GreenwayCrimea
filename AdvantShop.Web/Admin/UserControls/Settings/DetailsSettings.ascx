<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Settings.DetailsSettings" Codebehind="DetailsSettings.ascx.cs" %>
<table class="info-tb">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_ProductDetails %>
            </span>
            <br />
            <span class="subTitleNotify">
                <%= Resources.Resource.Admin_CommonSettings_ProductDetailsSub %>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_DisplayFields%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width:280px;">
            <label class="form-lbl" for="<%= chkDisplayWeight.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_DisplayWeight%></label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkDisplayWeight" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Отображать вес
                    </header>
                    <div class="help-content">
                        Отображать или нет графу "вес" в карточке товара.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkDisplayDimensions.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_DisplayDimensions%></label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkDisplayDimensions" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Отображать габариты
                    </header>
                    <div class="help-content">
                        Отображать или нет графу "габариты" в карточке товара.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbShowStockAvailability.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ShowStockAvailability%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbShowStockAvailability" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div> 
                <article class="bubble help js-help">
                    <header class="help-header">
                        Отображать остаток товара
                    </header>
                    <div class="help-content">
                        Отображать или нет остаток товара (количество) в графе "наличие" у товара. <br />
                        <br />
                        Например: <br />
                        При включённой опции - В наличии (100 шт.)<br />
                        При отключенной опции - В наличии
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_ProductPhotos%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkCompressBigImage.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_CompressBigImage%></label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkCompressBigImage" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Сжимать большую фотографию
                    </header>
                    <div class="help-content">
                        Опция определяет, как поступать с большой фотографией товара. 
                        <br /><br />
                        Если опция <b>включена</b>, то фотография товара будет пережата в соответствии с настройками для (big) большой фотографии.
                        <br /><br />
                        Если опция <b>выключена</b>, то фотография будет загружена "как есть".
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkEnableZoom.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EnableZoom%>:</label>
        </td>
        <td>
            <asp:CheckBox ID="chkEnableZoom" runat="server" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        <%= Resources.Resource.Admin_CommonSettings_EnableZoom %>
                    </header>
                    <div class="help-content">
                        Включает приближение фотографии товара при наведении курсора мыши.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_Reviews%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkAllowReviews.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_AllowReviews%></label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkAllowReviews" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Отзывы к товарам
                    </header>
                    <div class="help-content">
                        Опция определяет, разрешить или нет добавление отзывов к товарам.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbModerateReviews.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ModerateReviews%></label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="ckbModerateReviews" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Модерировать отзывы
                    </header>
                    <div class="help-content">
                        Если опция <b>включена</b>, отзывы сначала попадут на модерирование, и будут отображены в карточке товара только после проверки администратором.
                    </div>
                </article>
            </div>
        </td>
    </tr>
        <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkDisplayReviewsImage.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_Reviews_DisplayImage%></label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkDisplayReviewsImage" CssClass="checkly-align" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkAllowReviewsImageUploading.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_Reviews_AllowImageUploading%></label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkAllowReviewsImageUploading" CssClass="checkly-align" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Разрешить загрузку изображений
                    </header>
                    <div class="help-content">
                        Опция определяет, разрешить или нет прикреплять изображения к отзывам из клиентской части.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtReviewImageWidth.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_Reviews_ImageDimensions%></label>
        </td>
        <td>
            <%= Resources.Resource.Admin_CommonSettings_Reviews_ImageWidth%>:
            <asp:TextBox ID="txtReviewImageWidth" runat="server" class="niceTextBox shortTextBoxClass3" /><span class="paramUnit">px,</span>&nbsp;
            <%= Resources.Resource.Admin_CommonSettings_Reviews_ImageHeight%>:
            <asp:TextBox ID="txtReviewImageHeight" runat="server" class="niceTextBox shortTextBoxClass3" /><span class="paramUnit">px</span>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Размеры изображений отзывов
                    </header>
                    <div class="help-content">
                        Максимальные размеры изображений отзывов (в пикселях).
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_ProductDetailsShipping %>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ddlShowShippingsMethodsInDetails.ClientID %>"><%= Resources.Resource.Admin_DetailsSettings_DisplayShipping%></label>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlShowShippingsMethodsInDetails">
                <asp:ListItem Text="<%$ Resources:Resource, Admin_DetailsSettings_DontDisplay%>" Value="0"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Resource, Admin_DetailsSettings_DisplayOnClick%>" Value="1"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Resource, Admin_DetailsSettings_DisplayAlways%>" Value="2"></asp:ListItem>
            </asp:DropDownList>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Отображение доставки в карточке товара
                    </header>
                    <div class="help-content">
                        Определяет тип отображения вариантов доставки в карточке товара.<br /><br />
                        Также у каждого метода доставки есть опция отключения отображения в этом списке.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtShippingsMethodsInDetailsCount.ClientID %>"><%= Resources.Resource.Admin_DetailsSettings_DisplayShippingCount%></label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtShippingsMethodsInDetailsCount" CssClass="niceTextBox shortTextBoxClass2" Text="" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Количество отображаемых методов доставки
                    </header>
                    <div class="help-content">
                        Определяет количество разрешённых методов доставки для отображения в карточки товара.<br /><br />
                        Также у каждого метода доставки есть опция отключения отображения в этом списке.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory spanSettCategorySub">
                <%= Resources.Resource.Admin_CommonSettings_Marketing%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtBlockOne.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_BlockOne%></label>
        </td>
        <td>
            <asp:TextBox ID="txtBlockOne" runat="server" CssClass="niceTextBox shortTextBoxClass" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Перекрестный маркетинг
                    </header>
                    <div class="help-content">
                        К товару можно привязать 2 списка связанных с ним товаров. Вы можете задать любое название данным спискам.
                        <br />
                        <br />
                        Например: С этим товаром покупают.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtBlockTwo.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_BlockTwo%></label>
        </td>
        <td>
            <asp:TextBox ID="txtBlockTwo" runat="server" CssClass="niceTextBox shortTextBoxClass" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Перекрестный маркетинг
                    </header>
                    <div class="help-content">
                        К товару можно привязать 2 списка связанных с ним товаров. Вы можете задать любое название данным спискам.
                        <br />
                        <br />
                        Например: Похожие товары.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
	    <td>
            <label class="form-lbl" for="<%= ddlRelatedProductSourceType.ClientID %>"><%= Resources.Resource.Admin_DetailsSettings_RelatedProdSource_Type%></label>
	    </td>
	    <td>
		    <asp:DropDownList runat="server" ID="ddlRelatedProductSourceType">
			    <asp:ListItem Text="<%$ Resources:Resource, Admin_DetailsSettings_RelatedProdSource_Default %>" Value="0"></asp:ListItem>
			    <asp:ListItem Text="<%$ Resources:Resource, Admin_DetailsSettings_RelatedProdSource_FromCategory %>" Value="1"></asp:ListItem>
			    <asp:ListItem Text="<%$ Resources:Resource, Admin_DetailsSettings_RelatedProdSource_Module %>" Value="2"></asp:ListItem>
		    </asp:DropDownList>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Источник для перекрестного маркетинга
                    </header>
                    <div class="help-content">
                        Вы можете выбрать откуда подгружать список товаров. 
                        <br />
                        <br />
                        <b>По умолчанию</b> - означает показывать товары из списка, который вы задаёте индивидуально для товара.
                    </div>
                </article>
            </div>
	    </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtBlockTwo.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_RelatedProductsMaxCount%></label>
        </td>
        <td>
            <asp:TextBox ID="txtRelatedProductsMaxCount" runat="server" CssClass="niceTextBox shortTextBoxClass" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Перекрестный маркетинг
                    </header>
                    <div class="help-content">
                        Максимальное кол-во товаров из назначенной категории
                    </div>
                </article>
            </div>
        </td>
    </tr>
</table>
