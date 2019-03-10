<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Products.ProductPhotos360" CodeBehind="ProductPhotos360.ascx.cs" %>

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


</script>

<div class="formheader">
    <h2>Фотографии для просмотра 360</h2>
    <span id="fuPhotoError360" style="color: Red; font-weight: bold; display: none;">
        <%=Resources.Resource.Admin_m_Product_SelectPhoto%>
    </span>
</div>
<div class="formheaderfooter">
</div>
<div>
    <span>Включить возможность просмотра 360</span>
    <asp:CheckBox ID="ckbActiveView360" runat="server" Text="" CssClass="ckbActiveView360 checkly-align"/>
    <div data-plugin="help" class="help-block">
        <div class="help-icon js-help-icon"></div>
        <article class="bubble help js-help">
            <header class="help-header">
                Обзор в 360 градусов
            </header>
            <div class="help-content">
                Загрузите несколько последовательных изображений, чтобы создать анимацию или эффект 360 градусного обзора.<br />
                <br />
                Подробнее - <a href="http://www.advantshop.net/help/pages/360-degree-spin" target="_blank">Обзор товара в 360 градусов</a>
            </div>
        </article>
    </div>
</div>
<asp:Panel ID="pnlPhotoView360" runat="server" CssClass="photoView360">
    <table class="table-p">
        <tr>
            <td colspan="2" style="height: 10px;">
                <input type='file' name='filesUpload360' id="filesUpload360" multiple style="width: 300px; display: none;" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: right">
                <a href="javascript:void(0)" class="btn btn-add btn-middle" onclick="ajaxFileUpload360('<%=ProductID %>')"><%=Resources.Resource.Admin_m_Product_Upload%></a>
            </td>
        </tr>
        <tr style="height: 10px;">
            <td colspan="2"></td>
        </tr>
        <tr style="height: 40px;">
            <td colspan="2">
                <%=Resources.Resource.Admin_m_Product_CurrentImages%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lPhotoMessage" runat="server" ForeColor="Red" Visible="false" EnableViewState="false"></asp:Label>

                <div>
                    <div>
                        <asp:UpdatePanel ID="UpdatePanel360" runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lnkUpdatePhoto360" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkUpdatePhoto360" runat="server" OnClick="lnkUpdatePhoto_Click"
                                    EnableViewState="false" />
                                <adv:AdvGridView ID="gridphotos360" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                    CellPadding="2" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Currencies_QDelete %>"
                                    CssClass="tableview" Style="cursor: pointer" DataFieldForEditURLParam="" DataFieldForImageDescription="Description"
                                    EditURL="" GridLines="None" OnRowCommand="grid_RowCommand" DataFieldForImagePath="PhotoName"
                                    OnSorting="grid_Sorting" OnRowDeleting="grid_RowDeleting" OnRowDataBound="grid_RowDataBound"
                                    ShowFooter="false" TooltipImgCellIndex="2">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="PhotoName" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <%= Resources.Resource.Admin_m_Product_Image %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#RenderPhotoName(Convert.ToString(Eval("ColorID"))) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="PhotoCount">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lbPhotoCount" runat="server" CommandName="Sort" CommandArgument="PhotoCount">
                                                    Количество фотографий
                                                    <asp:Image ID="arrowPhotoCount" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <%# RenderPhotoCount(Convert.ToString(Eval("ColorID"))) %>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center">
                                            <EditItemTemplate>
                                                <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                    src="images/updatebtn.png" onclick="<%# this.Page.ClientScript.GetPostBackEventReference(gridphotos360, "Update$" + Container.DataItemIndex)%>; return false;"
                                                    style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                                <asp:LinkButton ID="buttonDelete" runat="server"
                                                    CssClass="deletebtn showtooltip valid-confirm" CommandName="Delete" CommandArgument='<%# Eval("ColorID")%>'
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
        <tr style="height: 40px;">
            <td colspan="2">
                <div class="dvSubHelp">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                    <a href="http://www.advantshop.net/help/pages/360-degree-spin" target="_blank">Инструкция. Обзор товара в 360 градусов</a>
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:SqlDataSource runat="server" ID="sdsColorsIn360" SelectCommand="Select '0' as ColorID, '––––' as ColorName, -1000 as SortOrder 
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


    $(document.body).on("change", "#filesUpload360", function () {

        var fd = new FormData();
        for (var i = 0; i < $("#filesUpload360")[0].files.length; i++) {
            fd.append('file_' + i, $("#filesUpload360")[0].files[i]);
        }

        $.ajax({

            url: 'httphandlers/product/uploadphotos360.ashx?ProductID=' + '<%= Request["Productid"] %>',
            data: fd,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                <%= Page.ClientScript.GetPostBackEventReference(lnkUpdatePhoto360, "") %>;
            },
            error: function (data, status, e) {
                alert(e);
            }
        });

           });


    function ajaxFileUpload360(productId) {

        $("#filesUpload360").click();
        
        return false;
    }

</script>
