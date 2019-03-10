<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Module.CategoriesOnMainPage.CategoriesList" CodeBehind="CategoriesList.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Module.CategoriesOnMainPage.Service" %>

<%@ Import Namespace="AdvantShop.FilePath" %>

<script type="text/javascript">
    var timeOut;
    function Darken() {
        timeOut = setTimeout('document.getElementById("inprogress").style.display = "block";', 1000);
    }

    function Clear() {
        clearTimeout(timeOut);
        document.getElementById("inprogress").style.display = "none";

        $("input.sel").each(function (i) {
            if (this.checked) $(this).parent().parent().addClass("selectedrow");
        });

        initgrid();
    }

    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(Darken);
        prm.add_endRequest(Clear);
        initgrid();
    });

    $(document).ready(function () {
        $("#commandButton").click(function () {
            var command = $("#commandSelect").val();

            switch (command) {
                case "selectAll":
                    SelectAll(true);
                    break;
                case "unselectAll":
                    SelectAll(false);
                    break;
                case "selectVisible":
                    SelectVisible(true);
                    break;
                case "unselectVisible":
                    SelectVisible(false);
                    break;
                case "deleteSelected":
                    var r = confirm("Подтвердите действие");
                    if (r) {
                        setTimeout(updateSortOrder, 500);
                        __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                    }
                    break;
            }
        });

        $("#btnUpdate").live("click", function () {
            setTimeout(updateSortOrder, 500);
        });

        $(".tabContainer").on("click", 'td .deletebtn', function () {
            setTimeout(updateSortOrder, 2000);
        });

        function updateSortOrder() {
            var arr = [];
            var maxSortOrder = 0;

            $('.tableview tr td:nth-child(5)').each(function (i, el) {
                arr.push(Number($(el).find('input').val()));
            })

            maxSortOrder = Math.max.apply(null, arr);
            if (maxSortOrder < 0) maxSortOrder = 0;

            $("#<%=txtSortedCategory.ClientID %>").val(maxSortOrder + 10);
        }
    });

    function Validation() {
        var valid = true;
        if ($("#<%=CategoryPictureLoad.ClientID %>").val() == "") {
            valid = false;
        }

        if ($("#<%=txtSortedCategory.ClientID %>").val() == "" || !parseInt($("#<%=txtSortedCategory.ClientID %>").val())) {
            //$("#<%=txtSortedCategory.ClientID %>").css("border-color", "#FF0033");
            valid = false;
        }
        else {
            //$("#<%=txtSortedCategory.ClientID %>").css("border", "inherit");
        }
        return valid;
    }
</script>

<div id="inprogress" style="display: none;">
    <div id="curtain" class="opacitybackground">
        &nbsp;
   
    </div>
    <div class="loader">
        <table width="100%" style="font-weight: bold; text-align: center;">
            <tr>
                <td align="center">
                    <img src="images/ajax-loader.gif" />
                </td>
            </tr>
            <tr>
                <td align="center" style="color: #0D76B8;">
                    <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="Пожалуйста, подождите..."></asp:Localize>
                </td>
            </tr>
        </table>
    </div>
</div>
<div class="content-own">
    <div>
        <table cellpadding="0" width="55%" cellspacing="0" style="padding-left: 10px; padding-right: 10px;">
            <tr class="rowsPost">
                <td colspan="2" style="height: 34px;">
                    <span class="spanSettCategory">
                        <asp:Localize ID="lczHeader" runat="server" Text="Категории на главной" /></span>
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <div style="float: left;">
                        <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                    </div>
                </td>
            </tr>

            <tr><td colspan="2">&nbsp;</td></tr>

            <tr class="rowsPost">
                <td style="width: 290px;">
                    <asp:Label ID="lbPicturesQuantityInLine" runat="server" Text="Количество изображений в строке"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPicturesQuantityInLine" runat="server" Width="174px"></asp:TextBox>
                </td>
            </tr>

            <tr class="rowsPost">
                <td style="width: 290px;">
                    <asp:Label ID="lbImageWidth" runat="server" Text="Ширина изображения (px)"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtImageWidth" runat="server" Width="174px"></asp:TextBox>
                </td>
            </tr>

            <tr class="rowsPost">
                <td style="width: 290px;">
                    <asp:Label ID="lbImageHeight" runat="server" Text="Высота изображения (px)"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtImageHeight" runat="server" Width="174px"></asp:TextBox>
                </td>
            </tr>

            <tr class="rowsPost">
                <td style="width: 290px;">
                    <asp:Label ID="lbNewWindow" runat="server" Text="Открывать в новом окне"></asp:Label>
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                Открывать в новом окне
                            </header>
                            <div class="help-content">
                                Если настройка активна, при клике на элемент переход будет осуществляться в новом окне браузера.
                            </div>
                        </article>
                    </div>
                </td>
                <td>
                    <asp:CheckBox ID="chbNewWindow" runat="server"></asp:CheckBox>
                </td>
            </tr>

            <tr class="rowsPost">
                <td style="width: 290px;">
                    <asp:Label ID="lbNoShowCategoryName" runat="server" Text="Не выводить название категорий"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="chbNoShowCategoryName" runat="server"></asp:CheckBox>
                </td>
            </tr>

            <tr class="rowsPost">
                <td style="width: 290px;">
                    <asp:Label ID="lbNoShowBorder" runat="server" Text="Не показывать обводку"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="chbNoShowBorder" runat="server"></asp:CheckBox>
                </td>
            </tr>

            <tr class="rowsPost">
                <td style="width: 290px;">
                    <asp:Label ID="lbLocation" runat="server" Text="Расположение"></asp:Label>
                    <div data-plugin="help" class="help-block">
                    <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                Расположение
                            </header>
                            <div class="help-content">
                                Место, где будет отображаться список категорий
                            </div>
                        </article>
                    </div>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLocation" DataTextField="Text" DataValueField="Value" Width="200px" />
                </td>
            </tr>
            
            <tr class="rowsPost">
                <td style="width: 290px;">
                    <asp:Label ID="Label3" runat="server" Text="Эффект при наведении на изображение"></asp:Label>
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                Эффект при наведении на изображение
                            </header>
                            <div class="help-content">
                                Настройка активна только при активности настройки "Не показывать обводку"
                            </div>
                        </article>
                    </div>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlEffects" DataTextField="Text" DataValueField="Value" Width="200px" />
                </td>
            </tr>

            <tr class="rowsPost">
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="Сохранить" OnClick="btnSave_Click" />
                    <asp:Button ID="btnResizeImages" runat="server" Text="Пережать изображения" OnClick="btnResizeImages_Click" />
                </td>
        </table>

        <hr style="color: #C2C2C4" />

        <table cellpadding="0" class="tableCarousel" width="100%" cellspacing="0" style="padding-left: 10px; padding-right: 10px">
            <tr>
                <td style="width: 200px;">
                    <asp:Label ID="lbSelectCategory" runat="server" Text="Категория">
                    </asp:Label>
                </td>
                <td style="color: Red;">
                    <asp:DropDownList runat="server" ID="ddlAllCategories" DataTextField="Text" DataValueField="Value" Width="200px" />
                    <div>
                            <asp:Label runat="server" ID="lblErrorImage" ForeColor="Red"></asp:Label>
                        </div>
                </td>
            </tr>
            <tr style="height: 0px;"><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td style="width: 200px;">
                    <asp:Label ID="lbCheckFile" runat="server" Text="Выбрать файл"></asp:Label>
                </td>
                <td>
                    <div style="color: Red; float: left;">
                        <asp:FileUpload ID="CategoryPictureLoad" onchange="$('input:file').css('color','black')" runat="server" />*
                    </div>
                </td>
            </tr>
            <tr style="height: 18px;"><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td style="width: 200px;">
                    <asp:Label ID="lbURL" runat="server" Text="Ссылка"></asp:Label>
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                Ссылка
                            </header>
                            <div class="help-content">
                                В данном поле вы можете указать URL, отличный от URL выбранной категории, чтобы перенаправить пользователя на отдельный товар, статическую страницу или подборку товаров по фильтру.
                            </div>
                        </article>
                    </div>
                </td>
                <td style="color: Red;">
                    <asp:TextBox ID="txtURL" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr style="height: 18px;"><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td style="width: 100px;">
                    <asp:Label ID="lbSortedCategory" runat="server" Text="Порядок сортировки"></asp:Label>
                </td>
                <td style="color: Red;">
                    <asp:TextBox ID="txtSortedCategory" runat="server" Width="200px"></asp:TextBox>*
                </td>
            </tr>
            <tr style="height: 18px;"><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="bthAddCategory" runat="server" Text="Добавить"
                        OnClientClick="javascript:return Validation();" OnClick="bthAddCategory_Click" />
                </td>
            </tr>
        </table>
        <br />
        <ul id="ulValidationFailed" runat="server" visible="false" class="ulValidFaild">
        </ul>
        <div style="width: 100%">
            <table style="width: 99%;" class="massaction">
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Label ID="lMessage" runat="server" ForeColor="Red" Visible="false" EnableViewState="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                            <asp:Localize ID="Localize1" runat="server" Text="Команда"></asp:Localize>
                        </span><span style="display: inline-block;">
                            <select id="commandSelect">
                                <option value="selectAll">
                                    <asp:Localize ID="Localize2" runat="server" Text="Выбрать все"></asp:Localize>
                                </option>
                                <option value="unselectAll">
                                    <asp:Localize ID="Localize3" runat="server" Text="Снять все"></asp:Localize>
                                </option>
                                <option value="deleteSelected">
                                    <asp:Localize ID="Localize6" runat="server" Text="Удалить выбранные"></asp:Localize>
                                </option>
                            </select>
                            <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="Удалить выбранные"
                                OnClick="lbDeleteSelected_Click" />
                        </span>
                            <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                            <span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span>Выбранные</span></span></span>
                    </td>
                    <td align="right" class="selecteditems">
                        <asp:UpdatePanel ID="upCounts" runat="server">
                            <%--                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                            </Triggers>--%>
                            <ContentTemplate>
                                Всего
                               
                                <span class="bold">
                                    <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;Найдено
                           
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 8px;"></td>
                </tr>
                <tr><td colspan="2" style="height: 18px;">&nbsp;</td></tr>
            </table>
            <div>
                <%--                <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                    <table class="filter" cellpadding="2" cellspacing="0" style="display: none;">
                        <tr style="height: 5px;">
                            <td colspan="5"></td>
                        </tr>
                        <tr>
                            <td style="width: 70px; text-align: center;">
                                <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                    Width="65">
                                    <asp:ListItem Selected="True" Text="Любой" />
                                    <asp:ListItem Text="Да" />
                                    <asp:ListItem Text="Нет" />
                                </asp:DropDownList>
                            </td>
                            <td style="width: 320px;">
                                <div style="width: 320px; font-size: 0px; height: 0px;">
                                </div>
                            </td>
                            <td>
                                <div style="width: 130px; font-size: 0px; height: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtUrlFilter" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td style="width: 130px;">
                                <div style="width: 130px; font-size: 0px; height: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtSortOrder" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td style="width: 130px; text-align: center;">

                            </td>
                            <td style="width: 90px; text-align: center;">
                                <div style="width: 90px; font-size: 0px; height: 0px;">
                                </div>
                                <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                    TabIndex="23" Text="Фильтр" OnClick="btnFilter_Click" />
                                <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                    TabIndex="24" Text="Сброс" OnClick="btnReset_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="5"></td>
                        </tr>
                    </table>
                </asp:Panel>--%>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lbDeleteSelected" EventName="Click" />
                        <%--                        <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />--%>
                        <asp:AsyncPostBackTrigger ControlID="grid" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="grid" EventName="Sorting" />
                    </Triggers>
                    <ContentTemplate>
                        <adv:advgridview id="grid" runat="server" allowsorting="true" autogeneratecolumns="False"
                            cellpadding="2" cellspacing="0" confirmation="Вы действительно хотите удалить?"
                            cssclass="tableview" style="cursor: pointer" datafieldforediturlparam="" datafieldforimagedescription=""
                            datafieldforimagepath="" editurl="" gridlines="None" onrowcommand="grid_RowCommand"
                            onsorting="grid_Sorting" showfooter="false">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                    <EditItemTemplate>
                                        <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="Label02" runat="server" Text='0'></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <div style="width: 70px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div style="width: 130px; font-size: 0px; height: 0px;">
                                        </div>
                                        <%--<asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="Name">--%>
                                            Категория
                                            <%--<asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />--%>
                                        <%--</asp:LinkButton>--%>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNameBind" runat="server" Text='<%# Eval("Name") %>' Width="130px"></asp:TextBox>                           
                                        <%--<asp:Label ID="txtNameBind" runat="server" Text='<%# Bind("Name") %>' Width="130px"></asp:Label>--%>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="ImageUrl" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                    <HeaderTemplate>
                                        <div style="width: 150px; font-size: 0px; height: 0px;">
                                        </div>
                                        Изображение
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <div style=" text-align: center;">
                                            <a href="../modules/CategoriesOnMainPage/Pictures/<%# Eval("ImageUrl") %>" target="_blank">
                                                <img src='../modules/CategoriesOnMainPage/Pictures/<%# Eval("ImageUrl") %>'
                                                style="border: 1px solid #000000; vertical-align: middle; max-height: 100px; max-width: 200px;" />
                                            </a>
                                        </div>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <div style=" text-align: center;">
                                            <a href="../modules/CategoriesOnMainPage/Pictures/<%# Eval("ImageUrl") %>" target="_blank">
                                                <img src='../modules/CategoriesOnMainPage/Pictures/<%# Eval("ImageUrl") %>'
                                                style="border: 1px solid #000000; vertical-align: middle; max-height: 100px; max-width: 200px;" />
                                            </a>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="URL" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div style="width: 250px; font-size: 0px; height: 0px;">
                                        </div>
                                        <%--<asp:LinkButton ID="lbSortURL" runat="server" CommandName="Sort" CommandArgument="URL">--%>
                                            URL
                                            <%--<asp:Image ID="arrowURL" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />--%>
                                        <%--</asp:LinkButton>--%>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtURLBind" runat="server" Text='<%# Eval("URL") %>' Width="250px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lURL" runat="server" Text='<%# Bind("URL") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="SortOrder" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="50px">
                                    <HeaderTemplate>
                                        <div style="width: 50px; font-size: 0px; height: 0px;">
                                        </div>
                                        <%--<asp:LinkButton ID="lbSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder">--%>
                                            Порядок сортировки
                                            <%--<asp:Image ID="arrowSortOrder" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />--%>
                                        <%--</asp:LinkButton>--%>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSortOrderBind" runat="server" Text='<%# Eval("SortOrder") %>' Width="50px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lSortOrder" runat="server" Text='<%# Bind("SortOrder") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="90px">
                                    <EditItemTemplate>
                                        <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                            src="images/updatebtn.png" onclick="<%#Page.ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                            style="display: none" title='Обновить' />
                                        <asp:LinkButton ID="buttonDelete" runat="server"
                                            CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteCategory" CommandArgument='<%# Eval("ID")%>'
                                            data-confirm="Вы действительно хотите удалить элемент?"
                                            ToolTip='Удалить'/>
                                        <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                            src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                            style="display: none" title="Отмена" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#ccffcc" />
                            <HeaderStyle CssClass="header" />
                            <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                            <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                            <EmptyDataTemplate>
                                <center style="margin-top: 20px; margin-bottom: 20px;">
                                    Нет записей
                            </center>
                            </EmptyDataTemplate>
                        </adv:advgridview>
                        <div style="border-top: 1px #c9c9c7 solid;">
                        </div>
                        <table class="results2">
                            <tr>
                                <td style="width: 157px; padding-left: 6px;">
                                    <%--                                    Записей на странице:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                        runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>100</asp:ListItem>
                                    </asp:DropDownList>--%>
                                </td>
                                <td align="center">
                                    <adv:pagenumberer cssclass="PageNumberer" id="pageNumberer" runat="server" displayedpages="7"
                                        usehref="false" usehistory="false" onselectedpagechanged="pn_SelectedPageChanged" />
                                </td>
                                <td style="width: 157px; text-align: right; padding-right: 12px">
                                    <%--                                    <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo">
                                        <span style="color: #494949">
                                            Номер страницы&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
                                                Width="30" /></span>
                                        <asp:Button ID="btnGo" runat="server" CssClass="btn" Text="GO"
                                            OnClick="linkGO_Click" />
                                    </asp:Panel>--%>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <input type="hidden" id="SelectedIds" name="SelectedIds" />
        </div>
    </div>
</div>
