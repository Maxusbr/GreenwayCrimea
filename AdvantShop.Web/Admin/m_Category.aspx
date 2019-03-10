<%@ Page Language="C#" AutoEventWireup="true" Inherits="Admin.m_Category"
    MasterPageFile="~/Admin/CatalogLayout.master" ValidateRequest="false" Codebehind="m_Category.aspx.cs" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="Resources" %>


<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <link rel="stylesheet" href="js/plugins/chosen/css/chosen.css">
    <script type="text/javascript" src="js/plugins/chosen/chosen.jquery.js"></script>
    <script src="js/plugins/chosen/chosen.ajaxaddition.jquery.js" type="text/javascript"></script>
    
    <h2 class="header-edit cat-name">
        <asp:Literal ID="lblSubHead" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_EditCategory %>" />
    </h2>
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
        <ul class="category-content">
            <li>
                <div class="category-h-item">
                    <div class="category-item-title"><%= Resource.Admin_m_Category_Category %></div>
                    <div class="category-item-subtitle"><%= Resource.Admin_m_Category_SynonymInfo %></div>
                </div>
                <div class="category-c-item"> 
                    <ul>
                        <li>
                            <div class="category-c-item-title">
                                <%= Resource.Admin_m_Category_Name %> <span class="valid-star">*</span>
                            </div>
                            <asp:TextBox ID="txtName" runat="server" Text="" ValidationGroup="vGroup" Width="710px" CssClass="category-inp niceTextBox textBoxLong" data-picture-name />
                            <div data-plugin="help" class="help-block">
                                <div class="help-icon js-help-icon"></div>
                                <article class="bubble help js-help">
                                    <header class="help-header">
                                        Название категории
                                    </header>
                                    <div class="help-content">
                                        Название, наименование категории. <br />
                                        <br />
                                        Например: Детские подарки
                                    </div>
                                </article>
                            </div>
                        </li>
                        <li>
                            <div class="category-c-item-title">
                                <%= Resource.Admin_m_Product_UrlSynonym %> <span class="valid-star">*</span>
                            </div>
                            <asp:TextBox ID="txtSynonym" runat="server" CssClass="niceTextBox textBoxLong" Width="710px" />
                            <div data-plugin="help" class="help-block">
                                <div class="help-icon js-help-icon"></div>
                                <article class="bubble help js-help">
                                    <header class="help-header">
                                        Синоним для URL запроса
                                    </header>
                                    <div class="help-content">
                                        Обязательное поле.<br />
                                        <br />
                                        Синоним не должен содержать пробелов и знаков препинания, кроме '_' и '-'<br />
                                        <br />
                                        Например: padarki-detyam
                                    </div>
                                </article>
                            </div>
                        </li>
                        <li>
                            <div class="category-c-item-col1" style="width:363px;">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnUpdateParent" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="category-c-item-title">
                                            <%= Resource.Admin_m_Category_Parent %> <span class="valid-star">*</span>
                                        </div>
                                        <div class="category-c-parent">
                                            <asp:Literal ID="lParent" runat="server" Text="" />
                                            <asp:LinkButton ID="lbParentChange" CssClass="cat-lnk" runat="server" 
                                                Text="<%$ Resources:Resource, Admin_m_Category_ChangeParent %>" OnClick="lbParentChange_Click" />
                                            <div data-plugin="help" class="help-block">
                                                <div class="help-icon js-help-icon"></div>
                                                <article class="bubble help js-help">
                                                    <header class="help-header">
                                                        Родительская категория
                                                    </header>
                                                    <div class="help-content">
                                                        Категория внутри которой будет находиться данная категория.
                                                    </div>
                                                </article>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="category-c-item-col2" style="width:380px;">
                                <div class="category-c-item-title">
                                    <%= Resource.Admin_m_Category_SortOrder %>
                                </div>
                                <asp:TextBox ID="txtSortIndex" runat="server" Width="341px" CssClass="niceTextBox textBoxClass" />
                                <div data-plugin="help" class="help-block">
                                    <div class="help-icon js-help-icon"></div>
                                    <article class="bubble help js-help">
                                        <header class="help-header">
                                            Порядок сортировки
                                        </header>
                                        <div class="help-content">
                                            Порядковый номер категории. <br />
                                            <br />
                                            Используется сортировка по возрастанию (ноль вверху).
                                        </div>
                                    </article>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="category-c-item-title" style="margin-top:10px;">
                                <%= Resource.Admin_m_Category_Description %>
                                <div data-plugin="help" class="help-block">
                                    <div class="help-icon js-help-icon"></div>
                                    <article class="bubble help js-help">
                                        <header class="help-header">
                                            <%= Resource.Admin_m_Category_Description %>
                                        </header>
                                        <div class="help-content">
                                            Выводится на странице просмотра категории внизу страницы.
                                        </div>
                                    </article>
                                </div>
                            </div>
                             <asp:TextBox ID="fckDescription" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="250px" Width="100%" />
                        </li>
                        <li>
                            <div class="category-c-item-title">
                                <%= Resource.Admin_m_Category_BriefDescription %>
                                <div data-plugin="help" class="help-block">
                                    <div class="help-icon js-help-icon"></div>
                                    <article class="bubble help js-help">
                                        <header class="help-header">
                                            <%= Resource.Admin_m_Category_BriefDescription %>
                                        </header>
                                        <div class="help-content">
                                            Выводится на странице просмотра категории вверху страницы.
                                        </div>
                                    </article>
                                </div>
                            </div>
                            <asp:TextBox ID="fckBriefDescription" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="250px" Width="100%" />
                        </li>
                    </ul>
                </div>
            </li>
            <li>
                <div class="category-h-item">
                    <div class="category-item-title"><%= Resource.Admin_m_Category_Pictures %></div>
                    <div class="category-item-subtitle"></div>
                </div>
                <div class="category-c-item">
                     <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnDeleteImage" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnDeleteMiniImage" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lbParentChange" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="category-c-item-col1" data-picture-type="picture">
                                <%= Resource.Admin_m_Category_Picture %>
                                <asp:Label ID="lblImageInfo" runat="server" Font-Bold="False" Font-Size="Smaller" ForeColor="Gray" />
                                <div data-plugin="help" class="help-block">
                                    <div class="help-icon js-help-icon"></div>
                                    <article class="bubble help js-help">
                                        <header class="help-header">
                                            Изображение
                                        </header>
                                        <div class="help-content">
                                            Главное изображение (баннер) категории.<br />
                                            <br />
                                            Изображение отображается при просмотре категории в клиентской части магазина.<br />
                                            <br />
                                            Размер по умолчанию 740px на 200px.
                                        </div>
                                    </article>
                                </div>
                                <div class="cat-picture">
                                    <asp:Image ID="imgCategoryPicture" runat="server" ImageUrl="../images/nophoto_small.jpg" Width="200px" data-picture-img />
                                </div>
                                <asp:Button ID="btnDeleteImage" runat="server" Text="<%$  Resources:Resource, Admin_Delete%>" 
                                    OnClick="btnDeleteImage_Click" Visible="False" />
                                <asp:FileUpload ID="PictureFileUpload" runat="server" Height="20px" Width="308px" />
                                <div id="pnlSearchPicture" runat="server" style="margin-top:3px;">
                                    <a href="javascript:void(0)" class="btn btn-action btn-small" data-picture-modal-caller>
                                        <%= Resource.Admin_m_Product_SearchPhotos %>
                                    </a>
                                    <span data-picture-chosen></span><span data-picture-remove>X</span>
                                    <asp:HiddenField ID="hfGooglePicture" runat="server" />
                                </div>
                            </div>
                            <div class="category-c-item-col2" data-picture-type="miniPicture">
                                <%= Resource.Admin_m_Category_MiniPicture %>
                                <asp:Label ID="lblMiniPhotoInfo" runat="server" Font-Bold="False" Font-Size="Smaller" ForeColor="Gray" />
                                <div data-plugin="help" class="help-block">
                                    <div class="help-icon js-help-icon"></div>
                                    <article class="bubble help js-help">
                                        <header class="help-header">
                                            Мини-картинка
                                        </header>
                                        <div class="help-content">
                                            Мини-картинка которая отобразиться у категории, при просмотре списка категорий в клиентской части магазина.<br />
                                            <br />
                                            Настоятельно рекомендуем указывать картинку для категорий.<br />
                                            <br />
                                            Размер по умолчанию 80px на 80px.
                                        </div>
                                    </article>
                                </div>
                                <div class="cat-picture">
                                    <asp:Image ID="imgMiniPicture" runat="server" ImageUrl="../images/nophoto_small.jpg" Width="80px" data-picture-img />
                                </div>
                                <asp:Button ID="btnDeleteMiniImage" runat="server" Text="<%$  Resources:Resource, Admin_Delete%>" 
                                    OnClick="btnDeleteMiniImage_Click" Visible="False" />
                                <asp:FileUpload ID="MiniPictureFileUpload" runat="server" Height="20px" Width="308px" />
                                <div id="pnlSearchMiniPicture" runat="server" style="margin-top:3px;">
                                    <a href="javascript:void(0)" class="btn btn-action btn-small" data-picture-modal-caller>
                                        <%= Resource.Admin_m_Product_SearchPhotos %>
                                    </a>
                                    <span data-picture-chosen></span><span data-picture-remove>X</span>
                                    <asp:HiddenField ID="hfGoogleMiniPicture" runat="server" />
                                </div>
                            </div>
                            <div class="category-c-item-col1" data-picture-type="icon">
                                <div style="padding: 15px 0 0 0;">
                                    <%= Resource.Admin_m_Category_Icon %>
                                    <asp:Label ID="lblIconPhotoInfo" runat="server" Font-Bold="False" Font-Size="Smaller" ForeColor="Gray" />
                                    <div data-plugin="help" class="help-block">
                                        <div class="help-icon js-help-icon"></div>
                                        <article class="bubble help js-help">
                                            <header class="help-header">
                                                Иконка для меню
                                            </header>
                                            <div class="help-content">
                                                Иконка категории для отображения в пункте меню.<br />
                                                <br />
                                                Рекомендуемый формат файла: *.png с прозрачным фоном.<br />
                                                <br /> 
                                                Размер 30px на 30px.<br />
                                            </div>
                                        </article>
                                    </div>
                                </div>
                                <div class="cat-picture">
                                    <asp:Image ID="imgIcon" runat="server" Width="30px" ImageUrl="../images/nophoto_small.jpg" data-picture-img />
                                </div>
                                <asp:Button ID="btnDeleteIcon" runat="server" Text="<%$  Resources:Resource, Admin_Delete%>" 
                                    OnClick="btnDeleteIcon_Click" Visible="False" />
                                <asp:FileUpload ID="IconFileUpload" runat="server" Height="20px" Width="308px" />
                                <div id="pnlSearchIcon" runat="server" style="margin-top:3px;">
                                    <a href="javascript:void(0)" class="btn btn-action btn-small" data-picture-modal-caller>
                                        <%= Resource.Admin_m_Product_SearchPhotos %>
                                    </a>
                                    <span data-picture-chosen></span><span data-picture-remove>X</span>
                                    <asp:HiddenField ID="hfGoogleIcon" runat="server" />
                                </div>
                                <div class="dvSubHelp">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                                    <a href="http://www.advantshop.net/help/pages/all-picture-size#6" target="_blank">Инструкция. Большая и маленькая фотографии категории.</a>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </li>
            <li>
                <div class="category-h-item">
                    <div class="category-item-title"><%= Resource.Admin_m_Category_Display %></div>
                    <div class="category-item-subtitle"></div>
                </div>
                <div class="category-c-item">
                    <ul>
                        <li>
                            <div class="category-c-item-col1">
                                <div class="category-c-item-title">
                                    <%= Resource.Admin_m_Category_SubCategoryDisplayStyle %>
                                    <div data-plugin="help" class="help-block">
                                        <div class="help-icon js-help-icon"></div>
                                        <article class="bubble help js-help">
                                            <header class="help-header">
                                                <%= Resource.Admin_m_Category_SubCategoryDisplayStyle %>
                                            </header>
                                            <div class="help-content">
                                                Подкатегории в данной категории будут отображены одним из выбранных вариантов.
                                            </div>
                                        </article>
                                    </div>
                                </div>
                                <asp:RadioButtonList ID="SubCategoryDisplayStyle" runat="server">
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_m_Category_Tile %>" Value="1" Selected="True" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_m_Category_List %>" Value="2" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_m_Category_None %>" Value="0" />
                                </asp:RadioButtonList>
                            </div>
                            <div class="category-c-item-col2">
                                <div class="category-c-item-title">
                                    <%= Resource.Admin_m_Category_Sorting %>
                                    <div data-plugin="help" class="help-block">
                                    <div class="help-icon js-help-icon"></div>
                                    <article class="bubble help js-help">
                                        <header class="help-header">
                                            <%= Resource.Admin_m_Category_Sorting %>
                                        </header>
                                        <div class="help-content">
                                            Данная опция будет выбрана для товаров данной категории по умолчанию, 
                                            пользователь имеет возможность изменить сортировку на удобную.
                                        </div>
                                    </article>
                                </div>
                                </div>
                                <asp:RadioButtonList ID="ddlSorting" runat="server">
                                </asp:RadioButtonList>
                            </div>
                        </li>
                        <li>
                            <div class="category-c-item-col1">
                                <div class="category-c-item-title">
                                    <%= Resource.Admin_m_Category_DisplayBrands %>
                                    <div data-plugin="help" class="help-block">
                                        <div class="help-icon js-help-icon"></div>
                                        <article class="bubble help js-help">
                                            <header class="help-header">
                                                <%= Resource.Admin_m_Category_DisplayBrands %>
                                            </header>
                                            <div class="help-content">
                                                Опция включает или выключает отображение колонки "производители" в меню каталога для данной категории.
                                            </div>
                                        </article>
                                    </div>
                                </div>
                                <asp:RadioButtonList ID="rblDisplayBrands" runat="server">
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Yes %>" Value="True" Selected="True" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_No %>" Value="False" />
                                </asp:RadioButtonList>
                            </div>
                            <div class="category-c-item-col2">
                                <div class="category-c-item-title">
                                    <%= Resource.Admin_m_Category_DisplaySubCategories %>
                                    <div data-plugin="help" class="help-block">
                                        <div class="help-icon js-help-icon"></div>
                                        <article class="bubble help js-help">
                                            <header class="help-header">
                                                <%= Resource.Admin_m_Category_DisplaySubCategories %>
                                            </header>
                                            <div class="help-content">
                                                Выводить один или два уровня дерева категорий в пункте меню.<br />
                                                <br />
                                                Два уровня - более предпочтительный вариант.
                                            </div>
                                        </article>
                                    </div>
                                </div>
                                <asp:RadioButtonList ID="rblDisplaySubCategories" runat="server">
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Yes %>" Value="True" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_No %>" Value="False" Selected="True" />
                                </asp:RadioButtonList>
                            </div>
                        </li>
                    </ul>
                </div>
            </li>
            <li>
                <div class="category-h-item">
                    <div class="category-item-title">
                        <%= Resource.Admin_m_Category_Enabled %>
                    </div>
                </div>
                <div class="category-c-item" style="width:50px;">
                    <asp:RadioButtonList ID="rblEnableCategory" runat="server">
                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Yes %>" Value="True" Selected="True" />
                        <asp:ListItem Text="<%$ Resources:Resource, Admin_No %>" Value="False" />
                    </asp:RadioButtonList>
                </div>
                <div style="width: 50px; height: 30px; display: inline-flex; align-items: center;">
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                <%= Resource.Admin_m_Category_Enabled %>
                            </header>
                            <div class="help-content">
                                Если категория не активна, то действуют правила:<br />
                                - Категория скрывается из меню каталога<br />
                                - Переход по URL категории будет выдавать 404<br />
                                - Все товары категории (если она отмечена как основная) будут так же скрыты, т.е. выдавать 404
                            </div>
                        </article>
                    </div>
                </div>
            </li>
               <li>
                <div class="category-h-item">
                    <div class="category-item-title">
                        <%= Resource.Admin_m_Category_Hidden %>
                    </div>
                </div>
                <div class="category-c-item" style="width:50px;">
                    <asp:RadioButtonList ID="rblHiddenCategory" runat="server">
                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Yes %>" Value="True" />
                        <asp:ListItem Text="<%$ Resources:Resource, Admin_No %>" Value="False" Selected="True" />
                    </asp:RadioButtonList>
                </div>
                <div style="width: 50px; height: 30px; display: inline-flex; align-items: center;">
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                <%= Resource.Admin_m_Category_Hidden %>
                            </header>
                            <div class="help-content">
                                Если категория скрыта, то действуют правила:<br />
                                - Категория скрывается из меню каталога<br />
                                - Переход по URL категории работает<br />
                                - Все товары категории (если она отмечена как основная) будут так же доступны.<br />
                                <br />
                                Ссылки на товары и категорию продолжают работать, категория скрывается только из меню.
                            </div>
                        </article>
                    </div>
                </div>
            </li>
            <li runat="server" id="liTags">
                <div class="category-h-item">
                    <div class="category-item-title"><%= Resource.Admin_m_Category_Tags %></div>
                </div>
                <div class="category-c-item chosendiv">
                    <asp:ListBox Width="339px"   runat="server" ID="lbTag" Name="multiTag" CssClass="chosen" SelectionMode="Multiple"/>
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                <%= Resource.Admin_m_Category_Tags %>
                            </header>
                            <div class="help-content">
                                С помощью тегов существует возможность создавать виртуальные категории (списки товаров) с набором товаров которым присвоены определенные метки (Теги).<br />
                                <br />
                                Инструкция: <a href="http://www.advantshop.net/help/pages/tags" target="_blank">Теги, механизм тегов</a>
                            </div>
                        </article>
                    </div>
                </div>
            </li>
            <li>
                <div class="category-h-item">
                    <div class="category-item-title">SEO</div>
                </div>
                <div class="category-c-item" style="width:420px;">
                    <ul>
                        <li>
                            <div>
                                <label>
                                    <%= Resource.Admin_Catalog_UseDefaultMeta %>
                                    <asp:CheckBox runat="server" ID="chbDefaultMeta" Checked="true" CssClass="checkly-align"/>
                                </label>
                                <div data-plugin="help" class="help-block">
                                    <div class="help-icon js-help-icon"></div>
                                    <article class="bubble help js-help">
                                        <header class="help-header">
                                            Использовать Meta по умолчанию
                                        </header>
                                        <div class="help-content">
                                            Если опция <b>включена</b>, SEO настройки будут взяты из общих настроек магазина.<br />
                                            <br />
                                            Если опция <b>выключена</b>, SEO настройки для товара будут взяты с формы ниже.<br />
                                            <br />
                                            Подробнее:<br />
                                            <a href="http://www.advantshop.net/help/pages/settings-of-seo-module" target="_blank">Настройка мета по умолчанию для магазина.</a>
                                        </div>
                                    </article>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="category-c-item-title">
                                <%= Resource.Admin_m_Product_HeadTitle %>
                            </div>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="niceTextBox textBoxClass" />
                        </li>
                        <li>
                            <div class="category-c-item-title">
                                H1
                            </div>
                            <asp:TextBox ID="txtH1" runat="server" CssClass="niceTextBox textBoxClass" />
                        </li>
                        <li>
                            <div class="category-c-item-title">
                                <%= Resource.Admin_m_Product_MetaKeywords %>
                            </div>
                            <asp:TextBox ID="txtMetaKeywords" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" />
                        </li>
                        <li>
                            <div class="category-c-item-title">
                                <%= Resource.Admin_m_Product_MetaDescription %>
                            </div>
                            <asp:TextBox ID="txtMetaDescription" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines" />
                        </li>
                    </ul>
                    <asp:HiddenField ID="hfMetaId" runat="server" />
                </div>
                <div style="width: 300px; height: 215px; display: inline-flex; align-items: center;" class="subSaveNotify">
                    <%= Resource.Admin_m_Category_UseGlobalVariables %>
                </div>
            </li>
            <li runat="server" id="pnlPropertyGroups">
                <div class="category-h-item">
                    <div class="category-item-title"><%= Resource.Admin_m_Category_PropertyGroups %></div>
                </div>
                <div class="category-c-item">
                    <div style="margin: 0 0 15px 0;">
                        <%= Resource.Admin_m_Category_Groups%>
                        <asp:Button runat="server" ID="btnAddGroup" Text="<%$ Resources: Resource, Admin_m_Category_AddGroup %>" 
                                    OnClick="btnAddGroup_OnClick" style="margin-left: 15px;" />
                        
                        <div data-plugin="help" class="help-block">
                                    <div class="help-icon js-help-icon"></div>
                                    <article class="bubble help js-help">
                                        <header class="help-header">
                                            Группы свойств для данной категории
                                        </header>
                                        <div class="help-content">
                                            Для данной категории можно назначить одну или несколько групп свойств.<br />
                                            <br />
                                            Подробнее:<br />
                                            <a href="http://www.advantshop.net/help/pages/property-sets" target="_blank">Инструкция. Группы свойств.</a>
                                        </div>
                                    </article>
                                </div>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAddGroup" EventName="Click" />                
                    </Triggers>
                    <ContentTemplate>
                        <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                            CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_m_Category_GroupConfirmation %>"
                            CssClass="tableview" Style="cursor: pointer" GridLines="None" OnRowCommand="grid_RowCommand"
                            OnRowDeleting="grid_RowDeleting" OnRowDataBound="grid_DataBound" ShowFooter="false">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="PropertyGroupId" Visible="false">
                                    <EditItemTemplate>
                                        <asp:Label ID="Label0" runat="server" Text='<%# Eval("PropertyGroupId") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=Resource.Admin_Catalog_Name%>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList runat="server" ID="ddlNewGroupName" CssClass="add" Width="200px" DataTextField="Name" DataValueField="PropertyGroupId" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="50px" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center"
                                    FooterStyle-HorizontalAlign="Center">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="buttonDelete" runat="server"
                                            CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteGroup" CommandArgument='<%# Eval("PropertyGroupId")%>'
                                            data-confirm="<%$ Resources:Resource, Admin_m_Category_GroupConfirmation %>"
                                            ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="ibAddProperty" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddGroup"
                                            ToolTip="<%$ Resources:Resource, Admin_Property_Add  %>" />
                                        <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                            CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Property_CancelAdd  %>" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#ccffcc" />
                            <HeaderStyle CssClass="header" />
                            <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                            <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                            <EmptyDataTemplate>
                                <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                    <%=Resource.Admin_Catalog_NoRecords%>
                                </div>
                            </EmptyDataTemplate>
                        </adv:AdvGridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </li>
            <li runat="server" id="pnlRecomendations">
                <div class="category-h-item">
                    <div class="category-item-title"><%=Resource.Admin_Catalog_AutoRecommendations%></div>
                    <div class="category-item-subtitle">
                        <%=Resource.Admin_Catalog_AutoRecommendations_Hint%>
                    </div>
                </div>
                <div class="category-c-item">
                    <ul>
                        <li>
                            <div class="category-c-item-title">
                                <b><%= SettingsCatalog.RelatedProductName %></b>
                                <div data-plugin="help" class="help-block">
                                    <div class="help-icon js-help-icon"></div>
                                    <article class="bubble help js-help">
                                        <header class="help-header">
                                            <%= SettingsCatalog.RelatedProductName %>
                                        </header>
                                        <div class="help-content">
                                            <%= Resource.Admin_Catalog_RelatetedProductsByCategoryHint %>
                                        </div>
                                    </article>
                                </div>
                            </div>
                            <asp:UpdatePanel runat="server" ID="upRelatedCats">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddRelatedCategory" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <div style="padding: 5px 0 0 0">
                                    <%= Resource.Admin_m_Category_Categories %>:
                                    <asp:ListView runat="server" ID="lvRelatedCategories" OnItemCommand="lvRelatedCategories_RowCommand">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex != 0 ? ", " : "" %>
                                            <span>
                                                <%# Eval("Name") %> 
                                                (<asp:LinkButton runat="server" ID="lbRemove" CommandName="DeleteRelCat" CommandArgument='<%# Eval("CategoryId")%>' 
                                                                    Text="<%$ Resources:Resource, Admin_Delete%>" CssClass="cat-lnk" />)
                                            </span>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <span><%=Resource.Admin_Category_NoSelectedCategories%></span>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    
                                
                                    <div style="padding: 10px 0 15px 0">
                                        <asp:DropDownList runat="server" ID="ddlRelatedCategory" DataTextField="Text" DataValueField="Value" Width="300px" /> 
                                        <asp:Button runat="server" ID="btnAddRelatedCategory" Text="<%$ Resources:Resource, Admin_Add%>" OnClick="btnAddRelatedCategory_Click" />
                                    </div>
                                    
                                    <div style="padding: 5px 0 0 0">
                                        <%=Resource.Admin_Category_RelatedProperty%>: 
                                        <asp:ListView runat="server" ID="lvRelatedProperties" OnItemCommand="lvRelatedProperties_RowCommand">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex != 0 ? ", " : "" %>
                                                <span>
                                                    <%# Eval("Name") %> - <%= Resource.Admin_Catalog_PropertyLikeProduct %>
                                                    (<asp:LinkButton runat="server" ID="lbRemove" CommandName="DeleteRelatedProperty" 
                                                        CommandArgument='<%# Eval("PropertyId")%>' 
                                                        Text="<%$ Resources:Resource, Admin_Delete%>" CssClass="cat-lnk" />)
                                                </span>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        
                                        <asp:ListView runat="server" ID="lvRelatedPropertyValues" OnItemCommand="lvRelatedProperties_RowCommand">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex != 0 ? ", " : "" %>
                                                <span>
                                                    <%# Eval("Property.Name") %> - <%# Eval("Value") %>
                                                    (<asp:LinkButton runat="server" ID="lbRemove" CommandName="DeleteRelatedPropertyValue" 
                                                        CommandArgument='<%# Eval("PropertyValueId")%>' 
                                                        Text="<%$ Resources:Resource, Admin_Delete%>" CssClass="cat-lnk" />)
                                                </span>
                                            </ItemTemplate>
                                        </asp:ListView>

                                        <div style="padding: 10px 0 15px 0">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlRelatedProperties" DataTextField="Name" DataValueField="PropertyId" Width="200px"
                                                            OnSelectedIndexChanged="ddlRelatedProperties_SelectedIndexChanged" AutoPostBack="True" />
                                                    </td>
                                                    <td style="padding: 0 10px;">
                                                        <label><input type="radio" id="inpRelTypeValue" runat="server" name="relType" checked="True" /> <asp:DropDownList runat="server" ID="ddlPropertValue" Width="200px" /></label>
                                                    </td>
                                                    <td>
                                                        <asp:Button runat="server" ID="btnAddRelatedProperty" Text="<%$ Resources:Resource, Admin_Add%>" OnClick="btnAddRelatedProperty_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td style="padding: 0 10px;">
                                                        <label><input type="radio" id="inpRelTypeProperty" runat="server" name="relType" /> <%= Resource.Admin_Catalog_PropertyLikeProductProperty %></label>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>
                        <li>
                            <div class="category-c-item-title">
                                <b><%= SettingsCatalog.AlternativeProductName %></b>
                                <div data-plugin="help" class="help-block">
                                    <div class="help-icon js-help-icon"></div>
                                    <article class="bubble help js-help">
                                        <header class="help-header">
                                            <%= SettingsCatalog.AlternativeProductName %>
                                        </header>
                                        <div class="help-content">
                                            <%= Resource.Admin_Catalog_RelatetedProductsByCategoryHint %>
                                        </div>
                                    </article>
                                </div>
                            </div>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddRelatedCategory" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddlAlternativeProperties" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <div style="padding: 5px 0 0 0">
                                    <div>
                                        <%= Resource.Admin_m_Category_Categories %>:
                                        <asp:ListView runat="server" ID="lvAlternativeCategories" OnItemCommand="lvAlternativeCategories_RowCommand">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex != 0 ? ", " : "" %>
                                                <span>
                                                    <%# Eval("Name") %> 
                                                    (<asp:LinkButton runat="server" ID="lbRemove" CommandName="DeleteAltCat" CommandArgument='<%# Eval("CategoryId")%>' 
                                                                        Text="<%$ Resources:Resource, Admin_Delete%>" CssClass="cat-lnk" />)
                                                </span>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <span><%=Resource.Admin_Category_NoSelectedCategories%></span>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </div>
                                    <div style="padding: 10px 0 15px 0">
                                        <asp:DropDownList runat="server" ID="ddlAlternativeCategory" DataTextField="Text" DataValueField="Value" Width="300px" /> 
                                        <asp:Button runat="server" ID="btnAddAlternativeCategory" Text="<%$ Resources:Resource, Admin_Add%>" OnClick="btnAddAlternativeCategory_Click" />
                                    </div>

                                    <div style="padding: 5px 0 0 0">
                                        <%=Resource.Admin_Category_RelatedProperty%>: 
                                        <asp:ListView runat="server" ID="lvAltProperties" OnItemCommand="lvAltProperties_RowCommand">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex != 0 ? ", " : "" %>
                                                <span>
                                                    <%# Eval("Name") %> - <%= Resource.Admin_Catalog_PropertyLikeProduct %>
                                                    (<asp:LinkButton runat="server" ID="lbRemove" CommandName="DeleteAltProperty" CommandArgument='<%# Eval("PropertyId")%>' 
                                                                        Text="<%$ Resources:Resource, Admin_Delete%>" CssClass="cat-lnk" />)
                                                </span>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <asp:ListView runat="server" ID="lvAltPropertyValues" OnItemCommand="lvAltProperties_RowCommand">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex != 0 ? ", " : "" %>
                                                <span>
                                                    <%# Eval("Property.Name") %> - <%# Eval("Value") %>
                                                    (<asp:LinkButton runat="server" ID="lbRemove" CommandName="DeleteAltPropertyValue" 
                                                        CommandArgument='<%# Eval("PropertyValueId")%>' 
                                                        Text="<%$ Resources:Resource, Admin_Delete%>" CssClass="cat-lnk" />)
                                                </span>
                                            </ItemTemplate>
                                        </asp:ListView>

                                        <div style="padding: 10px 0 15px 0">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlAlternativeProperties" DataTextField="Name" DataValueField="PropertyId" Width="200px"
                                                            OnSelectedIndexChanged="ddlAlternativeProperties_SelectedIndexChanged" AutoPostBack="True" />
                                                    </td>
                                                    <td style="padding: 0 10px;">
                                                        <label><input type="radio" id="inpAltTypeValue" runat="server" name="altType" checked="True" /> <asp:DropDownList runat="server" ID="ddlAlternativePropertyValues" Width="200px" /></label>
                                                    </td>
                                                    <td>
                                                        <asp:Button runat="server" ID="btnAddAltProperty" Text="<%$ Resources:Resource, Admin_Add%>" OnClick="btnAddAltProperty_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td style="padding: 0 10px;">
                                                        <label><input type="radio" id="inpAltTypeProperty" runat="server" name="altType" /> <%= Resource.Admin_Catalog_PropertyLikeProductProperty %></label>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>
                    </ul>
                </div>
            </li>
        </ul>
    <div>
        <asp:Button runat="server" ID="btnAdd" ValidationGroup="vGroup" OnClick="btnAdd_Click" CssClass="btn btn-add btn-middle" />
    </div>
    <%--<tr style="background-color: #eff0f1;">
        <td style="width: 49%; height: 26px; text-align: right; vertical-align: top;">
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_DisplayChildProducts %>" />
            <br />
            <span class="PaymentMethod_description">
                <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_m_Category_DisplayChildProductsWarning %>"></asp:Localize></span>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:CheckBox ID="ChkDisplayChildProducts" runat="server" />
        </td>
    </tr>--%>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="tree" EventName="TreeNodePopulate" />
            <asp:AsyncPostBackTrigger ControlID="tree" EventName="SelectedNodeChanged" />
        </Triggers>
        <ContentTemplate>
            <ajaxToolkit:ModalPopupExtender ID="mpeTree" runat="server" PopupControlID="pTree"
                TargetControlID="hhl" BackgroundCssClass="blackopacitybackground" CancelControlID="btnCancelParent"
                BehaviorID="ModalBehaviour">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HyperLink ID="hhl" runat="server" Style="display: none;" />
            <asp:Panel runat="server" ID="pTree" CssClass="modal-admin" Style="display: none">
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <span style="font-size: 11pt;">
                                    <asp:Localize ID="Localize_Admin_CatalogLinks_ParentCategory" runat="server" Text="<%$ Resources:Resource, Admin_CatalogLinks_ParentCategory %>"></asp:Localize>:</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="height: 360px; width: 450px; overflow: scroll; background-color: White;
                                    text-align: left">
                                    <asp:TreeView ID="tree" ForeColor="Black" PopulateNodesFromClient="true" runat="server"
                                        ShowLines="True" ExpandImageUrl="images/loading.gif" BackColor="White" OnTreeNodePopulate="PopulateNode"
                                        AutoPostBack="false" OnSelectedNodeChanged="Select_change" SelectedNodeStyle-BackColor="Yellow" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom" style="height: 36px;">
                                <asp:Button ID="btnUpdateParent" runat="server" Text="<%$ Resources:Resource,Admin_CatalogLinks_UpdateCategory %>"
                                    OnClick="btnUpdateParent_Click" />
                                <asp:Button ID="btnCancelParent" runat="server" Text="<%$ Resources: Resource, Admin_Cancel %>"
                                    Width="67" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { initHidden(); });

        $(document).ready(function () {
            var body = $('.chosendiv');
            ChosenInit(body);
        });

        $('body').on('focus', '#<%=txtSynonym.ClientID %>', fillUrl);
        $('body').on('focusout', '.category-inp', fillUrl);

        function fillUrl() {
            var text = $('#<%=txtName.ClientID %>').val();
            var url = $('#<%=txtSynonym.ClientID %>').val();
            if ((text != "") & (url == "")) {
                $('#<%=txtSynonym.ClientID %>').val(translite(text));
            }
        }

        $(function () {
            if ($('#<%= chbDefaultMeta.ClientID %>').is(":checked")) {
                $('#<%=txtTitle.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtH1.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaDescription.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaKeywords.ClientID %>').attr("disabled", "disabled");
            }
        });

        $('#<%= chbDefaultMeta.ClientID %>').click(function () {
            if ($('#<%= chbDefaultMeta.ClientID %>').is(":checked")) {
                $('#<%=txtTitle.ClientID %>').val("");
                $('#<%=txtH1.ClientID %>').val("");
                $('#<%=txtMetaDescription.ClientID %>').val("");
                $('#<%=txtMetaKeywords.ClientID %>').val("");

                $('#<%=txtTitle.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtH1.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaDescription.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaKeywords.ClientID %>').attr("disabled", "disabled");

            } else {
                $('#<%=txtTitle.ClientID %>').removeAttr("disabled");
                $('#<%=txtH1.ClientID %>').removeAttr("disabled");
                $('#<%=txtMetaDescription.ClientID %>').removeAttr("disabled");
                $('#<%=txtMetaKeywords.ClientID %>').removeAttr("disabled");
            }
        });

        function initHidden() {
            $("#<%= hfGooglePicture.ClientID %>").attr("data-picture-new", "true");
            $("#<%= hfGoogleMiniPicture.ClientID %>").attr("data-picture-new", "true");
            $("#<%= hfGoogleIcon.ClientID %>").attr("data-picture-new", "true");

            $("#<%= imgCategoryPicture.ClientID %>").attr("data-picture-prev", $("#<%= imgCategoryPicture.ClientID %>").attr("src"));
            $("#<%= imgMiniPicture.ClientID %>").attr("data-picture-prev", $("#<%= imgMiniPicture.ClientID %>").attr("src"));
            $("#<%= imgIcon.ClientID %>").attr("data-picture-prev", $("#<%= imgIcon.ClientID %>").attr("src"));
        }

        $(document).ready(function () {
            initHidden();

            $("[data-picture-remove]").on("click", function () {
                var type = $(this).closest("[data-picture-type]").data("picture-type");
                var img = getPicObj(type, "img");
                img.attr("src", img.attr("data-picture-prev"));
                getPicObj(type, "new").val("");
                getPicObj(type, "chosen").text("");
                getPicObj(type, "remove").hide();
            });

            var modalPhotos = $.advModal({
                title: "Поиск фото в интернете",
                clickOut: false,
                htmlContent: "<div class='photosSearch'><img src='images/ajax-loader.gif'/></div><div class='photo-message'></div><input type='hidden' id='photosPage' value='0'/>",
                control: "[data-picture-modal-caller]",
                controlBeforeOpen: function (modal, event) {
                    modal.pictureType = $(event.target.closest("[data-picture-type]")).data("picture-type");
                },
                beforeOpen: function () {
                    searchPhotos(modalPhotos, 0);
                },
                buttons: [{
                        textBtn: 'Найти еще',
                        classBtn: 'btn-action',
                        func: function () {
                            var page = parseInt($("#photosPage").val()) + 1;
                            searchPhotos(modalPhotos, page);
                            $("#photosPage").val(page);
                        }
                    }, {
                        textBtn: 'Добавить выбранное',
                        classBtn: 'btn-submit',
                        func: function () {
                            var imgSelected = $(".photosSearch input[type=radio]:checked:first"),
                                link = imgSelected.val();
                            if (link.length) {
                                var img = getPicObj(modalPhotos.pictureType, "img");
                                img.attr("src", imgSelected.data("thumb"));
                                getPicObj(modalPhotos.pictureType, "new").val(link);
                                getPicObj(modalPhotos.pictureType, "chosen").text("Выбрано: 1");
                                getPicObj(modalPhotos.pictureType, "remove").show();
                            }
                            modalPhotos.modalClose();
                        }
                    }, {
                        textBtn: 'Закрыть',
                        classBtn: 'btn-confirm',
                        func: function () {
                            $(".photo-message").text('');
                            modalPhotos.modalClose();
                        }
                    }]
            });
        });

        function getPicObj(pictureType, objType) {
            return $("[data-picture-type=" + pictureType + "] [data-picture-" + objType + "]");
        }

        function searchPhotos(modalPhotos, page) {
            $(".photosSearch").empty();

            var term = $('[data-picture-name]').val();
            if (term.length == 0) {
                $(".photosSearch").html("Фотографии не найдены. Укажите название категории.");
                return;
            }
            //if (modalPhotos.pictureType == "icon")
            //    term += " icon";

            $.ajax({
                dataType: "json",
                data: { term: term, page: page },
                url: '../googleimagessearch/searchimages',
                cache: false,
                success: function (data) {
                    if (data != null && data.items != null) {
                        for (var i = 0; i < data.items.length; i++) {
                            renderPhoto(data.items[i]);
                        }
                    } else if (data != null && data.error != null && data.error.message != null) {
                        $(".photosSearch").html(data.error.message);
                    } else {
                        $(".photosSearch").html("Фотографии не найдены");
                    }
                    modalPhotos.modalPosition();
                }
            });
        }

        function renderPhoto(item) {

            var label = $('<label class="search-photos progress"></label>');
            $(".photosSearch").append(label);
            checkImage(item.image.thumbnailLink, function () {
                var str = '<input type="radio" value="' + item.link + '" name="searchCategoryPicture" data-thumb="' + item.image.thumbnailLink + '" />' +
                    '<img src="' + item.image.thumbnailLink + '" title="' + item.title + '" /> <div class="photo-size">' + item.image.width + ' x ' + item.image.height + '</div>';
                label.removeClass('progress').html(str);
            }, function () {
                label.remove();
            });
        }

        function checkImage(src, good, bad) {
            var img = new Image();
            img.onload = good;
            img.onerror = bad;
            img.src = src;
        }
    </script>
</asp:Content>