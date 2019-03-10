<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Products.ProductPhotos" CodeBehind="ProductPhotos.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Core.Modules.Interfaces" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<script type="text/javascript">
    function CreateHistory(hist) {
        $.historyLoad(hist);
    }

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
                    var r = confirm("<%= Resources.Resource.Admin_Catalog_Confirm%>");
                    if (r) __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                    break;
            }
        });
    });
</script>
<table class="table-p">
    <tr>
        <td class="formheader" colspan="2">
            <h2><%=Resources.Resource.Admin_m_Product_Photos%></h2>
            <span id="fuPhotoError" style="color: Red; font-weight: bold; display: none;">
                <%=Resources.Resource.Admin_m_Product_SelectPhoto%></span>
        </td>
    </tr>
    <tr class="formheaderfooter">
        <td colspan="2"></td>
    </tr>
    <tr>
        <td colspan="2" style="height: 10px;">
            <input type='file' name='filesUploadNew' id="filesUploadNew" multiple style="width: 300px; display: none;" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: right">
            <a href="javascript:void(0)" class="btn btn-add btn-middle" onclick="ajaxFileUploadNew('<%=ProductID %>')"><%=Resources.Resource.Admin_m_Product_Upload%></a>
            <% if (AdvantShop.Core.Modules.AttachedModules.GetModules<IPhotoSearcher>().Any())
                { %>
            <a href="javascript:void(0)" class="btn btn-action btn-middle" id="aSearchPhotos" style="margin-left:5px;"><%= Resources.Resource.Admin_m_Product_SearchPhotos %></a>
            <% } %>
        </td>
    </tr>
    <tr style="height: 35px;">
        <td colspan="2">
            <%=Resources.Resource.Admin_m_Product_CurrentImages%>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="lPhotoMessage" runat="server" ForeColor="Red" Visible="false" EnableViewState="false"></asp:Label>
            <asp:LinkButton ID="lnkUpdatePhoto" runat="server" OnClick="lnkUpdatePhoto_Click"
                EnableViewState="false" />
            <div>
                <table style="width: 100%;" class="massaction">
                    <tr>
                        <td>
                            <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                            </span><span style="display: inline-block;">
                                <select id="commandSelect">
                                    <option value="selectAll">
                                        <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectAll %>"
                                            EnableViewState="false"></asp:Localize>
                                    </option>
                                    <option value="unselectAll">
                                        <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectAll %>"
                                            EnableViewState="false"></asp:Localize>
                                    </option>
                                    <option value="selectVisible">
                                        <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectVisible %>"
                                            EnableViewState="false"></asp:Localize>
                                    </option>
                                    <option value="unselectVisible">
                                        <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectVisible %>"
                                            EnableViewState="false"></asp:Localize>
                                    </option>
                                    <option value="deleteSelected">
                                        <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                            EnableViewState="false"></asp:Localize>
                                    </option>
                                </select>
                                <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                    OnClick="lbDeleteSelected_Click" />
                            </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span></span></span>
                        </td>
                        <td align="right" class="selecteditems">
                            <asp:UpdatePanel ID="upCounts" runat="server">
                                <Triggers>
                                </Triggers>
                                <ContentTemplate>
                                    <%=Resources.Resource.Admin_Catalog_Total%>
                                    <span class="bold">
                                        <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<%=Resources.Resource.Admin_Catalog_RecordsFound%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 8px;"></td>
                    </tr>
                </table>
                <br />
                <div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lbDeleteSelected" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                CellPadding="2" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Currencies_QDelete %>"
                                CssClass="tableview" Style="cursor: pointer" DataFieldForEditURLParam="" DataFieldForImageDescription="Description"
                                EditURL="" GridLines="None" OnRowCommand="grid_RowCommand" DataFieldForImagePath="PhotoName"
                                OnSorting="grid_Sorting" OnRowDeleting="grid_RowDeleting" OnRowDataBound="grid_RowDataBound"
                                ShowFooter="false" TooltipImgCellIndex="2">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="PhotoName" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <%= Resources.Resource.Admin_m_Product_Image %>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <img src='<%# FoldersHelper.GetImageProductPath(ProductImageType.XSmall, Eval("PhotoName").ToString(), true) %>' alt='<%# Eval("Description") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Description" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbDescription" runat="server" CommandName="Sort" CommandArgument="Description">
                                                <%= Resources.Resource.Admin_Product_Description %>
                                                <asp:Image ID="arrowDescription" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" Text='<%# Eval("Description") %>'
                                                Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lCode" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ColorID">
                                        <HeaderTemplate>

                                            <asp:LinkButton ID="lbColorID" runat="server" CommandName="Sort" CommandArgument="ColorID">
                                                <%= Resources.Resource.Admin_Product_Color %>
                                                <asp:Image ID="arrowColorID" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlColorID" runat="server" Width="100px" DataTextField="ColorName"
                                                DataValueField="ColorID" DataSourceID="sdsColors" SelectedValue='<%# Eval("ColorID") != DBNull.Value ? Eval("ColorID") : "null" %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="PhotoSortOrder">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbPhotoSortOrder" runat="server" CommandName="Sort" CommandArgument="PhotoSortOrder">
                                                <%= Resources.Resource.Admin_m_Product_Order %>
                                                <asp:Image ID="arrowPhotoSortOrder" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPhotoSortOrder" runat="server" Text='<%# Eval("PhotoSortOrder") %>'
                                                Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lPhotoSortOrder" runat="server" Text='<%# Bind("PhotoSortOrder") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Main" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbMain" runat="server" CommandName="Sort" CommandArgument="Main">
                                                <%= Resources.Resource.Admin_m_Product_Default %>
                                                <asp:Image ID="arrowMain" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbMain" runat="server" Checked='<%# Eval("Main")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center">
                                        <EditItemTemplate>
                                            <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                src="images/updatebtn.png" onclick="<%# this.Page.ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                            <asp:LinkButton ID="buttonDelete" runat="server"
                                                CssClass="deletebtn showtooltip valid-confirm" CommandName="Delete" CommandArgument='<%# Eval("ID")%>'
                                                data-confirm="<%$ Resources:Resource, Admin_Product_ConfirmDeletingPhoto %>"
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                            <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                                style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
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
                                        <%=Resources.Resource.Admin_m_Product_NoFoto %>
                                    </center>
                                </EmptyDataTemplate>
                            </adv:AdvGridView>
                            <input type="hidden" id="SelectedIds" name="SelectedIds" />
                            <br />
                            <table class="results2">
                                <tr>
                                    <td style="width: 157px; padding-left: 6px;">
                                        <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                            runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center">
                                        <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                            UseHref="false" UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged" />
                                    </td>
                                    <td style="width: 157px; text-align: right; padding-right: 12px">
                                        <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo" EnableViewState="false">
                                            <span style="color: #494949">
                                                <%=Resources.Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
                                                    Width="30" /></span>
                                            <asp:Button ID="btnGo" runat="server" CssClass="btn" Text="<%$ Resources:Resource, Admin_Catalog_GO %>"
                                                OnClick="linkGO_Click" EnableViewState="false" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </td>
    </tr>
    <tr style="height: 40px;">
        <td colspan="2">
            <div class="dvSubHelp" style="margin-bottom: 5px; margin-top:0;">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                <a href="http://www.advantshop.net/help/pages/all-picture-size#7" target="_blank">Инструкция. Как изменить размер фотографии</a>
            </div>
            <div class="dvSubHelp" style="margin:0;">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                <a href="http://www.advantshop.net/help/pages/color-photo" target="_blank">Инструкция. Как задать цвет для фотографии товара</a>
            </div>
        </td>
    </tr>
</table>

<asp:SqlDataSource runat="server" ID="sdsColors" SelectCommand="Select '0' as ColorID, '––––' as ColorName, -1000 as SortOrder 
        Union
        Select cast(Color.ColorID as nvarchar(10)), ColorName, SortOrder from Catalog.Photo inner join Catalog.Color on Color.ColorID=Photo.ColorID where objId=@productid and type='Product' 
        union 
        Select cast(Color.ColorID as nvarchar(10)), ColorName, SortOrder  From Catalog.Color inner join catalog.Offer on offer.ColorID=Color.Colorid
        where productid=@productid order by ColorName"
    OnInit="sds_Init">
    <SelectParameters>
        <asp:QueryStringParameter Name="productid" QueryStringField="ProductID" Type="Int32" DefaultValue="" />
    </SelectParameters>
</asp:SqlDataSource>

<script type="text/javascript">
    $(document).ready(function () {

        var modalPhotos = $.advModal({
            title: "Поиск фото в интернете",
            clickOut: false,
            htmlContent: "<div class='photosSearch'><img src='images/ajax-loader.gif'/></div><div class='photo-message'></div><input type='hidden' id='photosPage' value='0'/>",
            control: "#aSearchPhotos",
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
                        $(".photo-message").hide();
                    }
                }, {
                    textBtn: 'Добавить выбранные',
                    classBtn: 'btn-submit',
                    func: function () {
                        var links = '';
                        $(".photosSearch input[type=checkbox]:checked").each(function () {
                            links += $(this).val() + ",";
                        });
                        var progress = Advantshop.ScriptsManager.Progress.prototype.Init($('.photosSearch'));
                        progress.Show();
                        $.ajax({
                            data: { links: links, productid: '<%= Request["Productid"]%>' },
                            url: 'httphandlers/UploadPhotosByLink.ashx',
                            cache: false,
                            success: function (data) {
                                if (data != "0") {
                                    $(".photo-message").text('Добавлено ' + data + ' фото').show();
                                } else
                                    $(".photo-message").text('Фотографии не добавлены').show();
                                progress.Hide();
                            }
                        });
                    }
                }, {
                    textBtn: 'Закрыть',
                    classBtn: 'btn-confirm',
                    func: function () {
                        window.location.reload();
                    }
                }]
        });
    });

    $(document.body).on("change", "#filesUploadNew", function () {

        var fd = new FormData();
        for (var i = 0; i < $("#filesUploadNew")[0].files.length; i++) {
            fd.append('file_' + i, $("#filesUploadNew")[0].files[i]);
        }
        //alert("перед вызовом хендлера");
        $.ajax({
            url: 'httphandlers/uploadphoto.ashx?ProductID=' + <%= Request["Productid"] %> + "&description=",
            data: fd,
            processData: false,
            contentType: false,
            type: 'POST',
            traditional: true,
            success: function (data) {
                <%= Page.ClientScript.GetPostBackEventReference(lnkUpdatePhoto, "") %>;
            },
            error: function (data, status, e) {
                alert("error " + e + status);
            }
        });

    });

    function searchPhotos(modalPhotos, page) {
        $(".photosSearch").empty();

        $.ajax({
            dataType: "json",
            data: { term: '<%= HttpUtility.HtmlEncode( ProductName) %>', page: page },
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

        if (item.image == null)
            return;

        var label = $('<label class="search-photos progress"></label>');
        $(".photosSearch").append(label);
        checkImage(item.image.thumbnailLink,
            function () {
                var str = '<input type="checkbox" value="' + item.link + '"/>' +
                    '<img src="' + item.image.thumbnailLink + '" title="' + item.title + '" /> <div class="photo-size">' + item.image.width + ' x ' + item.image.height + '</div>';
                label.removeClass('progress').html(str);
            },
            function () {
                label.remove();
            });
    }

    function checkImage(src, good, bad) {
        var img = new Image();
        img.onload = good;
        img.onerror = bad;
        img.src = src;
    }


    function ajaxFileUploadNew(productId) {

        $("#filesUploadNew").click();

        return false;
    }
</script>
