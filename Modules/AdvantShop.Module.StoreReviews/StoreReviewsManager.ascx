<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.StoreReviews.Admin_StoreReviewsManager" CodeBehind="StoreReviewsManager.ascx.cs" %>
<%@ Register TagPrefix="adv" Namespace="AdvantShop.Core.Controls" Assembly="AdvantShop.Core" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="AdvantShop.Module.StoreReviews" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<style>
    .reviewsTable {
        width: 100%;
        border-collapse: collapse;
    }

        .reviewsTable td, .reviewsTable th {
            text-align: left;
            border-bottom: 1px solid #000000;
            height: 30px;
        }

            .reviewsTable td.img {
                padding: 5px 5px 0 0;
            }
</style>
    <script type="text/javascript">
        $(document).ready(function () {
            initgrid();
        });
    </script>
<div>
    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: StoreReviews_ManagerHeader%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="pnlProducts">
        <ul class="justify panel-do-grid">
            <li class="justify-item panel-do-grid-item">
                <select id="commandSelect" onchange="ChangeSelect()" style="width: 200px">
                    <option value="selectAll">
                        <asp:Localize ID="Localize5" runat="server" Text="Выделить все"></asp:Localize>
                    </option>
                    <option value="unselectAll">
                        <asp:Localize ID="Localize6" runat="server" Text="Снять выделение"></asp:Localize>
                    </option>
                    <option value="deleteSelected">
                        <asp:Localize ID="Localize7" runat="server" Text="Удалить выделенные"></asp:Localize>
                    </option>
                    <option value="confirmSelected">
                        <asp:Localize ID="Localize9" runat="server" Text="Проверить выделенные"></asp:Localize>
                    </option>
                </select>
                <a href="javascript:void(0)" class="btn btn-middle btn-action btn-do-grid" id="commandButton">
                    GO</a>
                <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" OnClick="lbDeleteSelected_Click" />
                <asp:LinkButton ID="lbConfirmSelected" Style="display: none" runat="server" OnClick="lbConfirmSelected_Click" />
                <span class="panel-do-grid-selected-rows"><span id="selectedIdsCount" class="panel-do-grid-count"></span>
                    позиций выделено 
                </span></li>
            <li class="justify-item panel-do-grid-item">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <span class="subcategories-count-wrap">
                            Отзывов:
                            <asp:Label ID="lblProducts" CssClass="foundrecords panel-do-grid-count" runat="server" Text="" />
                        </span>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </li>
        </ul>
        <div style="width: 100%; clear: both;">
            <div style="width: 100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="grid" EventName="DataBinding" />
                    </triggers>
        <contenttemplate>
            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="0"
                                    CellSpacing="0" Confirmation="Вы действительно хотите удалить товар?" CssClass="tableview"
                                    GridLines="None" TooltipImgCellIndex="2" TooltipTextCellIndex="5"
                                    ReadOnlyGrid="True" OnRowCommand="grid_RowCommand" >
                <Columns>
                    <asp:TemplateField AccessibleHeaderText="Id" Visible="false" HeaderStyle-Width="50px">
                        <EditItemTemplate>
                            <asp:Label ID="Label0" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="checkboxcolumnheader" ItemStyle-CssClass="checkboxcolumn">
                        <ItemTemplate>
                            <%# Eval("IsSelected") != DBNull.Value && (bool)Eval("IsSelected") ? "<input type='checkbox' class='sel' checked='checked' />" : "<input type='checkbox' class='sel' />"%>
                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="Image" ItemStyle-CssClass="colid" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                                Изображение
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Image CssClass="arrow" ID="ImageReview" ImageUrl='<%# Eval("ReviewerImage") != DBNull.Value ? UrlService.GetUrl("pictures/modules/storereviews/") + Eval("ReviewerImage") : string.Empty %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="Review" HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                                Отзыв
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lReview" runat="server" Text='<%# Eval("Review") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="DateAdded" ItemStyle-CssClass="colid" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                                Дата добавления
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("DateAdded") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="ReviewerEmail" ItemStyle-CssClass="colid" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                                Email
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("ReviewerEmail") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="Moderated" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                        <HeaderTemplate>
                                Проверен
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Moderated") %>' Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="Rate" ItemStyle-CssClass="colid" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                                Оценка
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="Buttons" ItemStyle-CssClass="colid" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <EditItemTemplate>
                            <a href='<%# "javascript:open_window(\"" + UrlService.GetUrl("modules/StoreReviews/editreview.aspx?id=" + Eval("ID")) + "\", 830, 600)" %>' class="editbtn showtooltip" title='Редактировать' style="text-decoration: none">
                                <img src="<%= UrlService.GetUrl("admin/images/editbtn.gif") %>" style="border: none;" alt="" />
                            </a>
                            <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip" OnClientClick="<%$ Resources: StoreReviews_Confirm %>"
                                CommandName="deleteReview" CommandArgument='<%#Eval("ID") %>'>
                                <img src="<%= UrlService.GetUrl("admin/images/deletebtn.png") %>" style="border: none;" alt="" />
                            </asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </adv:AdvGridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <input type="hidden" id="SelectedIds" name="SelectedIds" />
        </div>
    </asp:Panel>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#commandButton").click(function () {
                var command = $("#commandSelect").val();

                switch (command) {
                    case "selectAll":
                        SelectAll(true);
                        UpdateSelectedRow(true);
                        break;
                    case "unselectAll":
                        SelectAll(false);
                        UpdateSelectedRow(false);
                        break;
                    case "deleteSelected":
                        window.__doPostBack('<%=lbDeleteSelected.UniqueID%>', ''); 
                        break;
                    case "confirmSelected":
                        window.__doPostBack('<%=lbConfirmSelected.UniqueID%>', '');
                        break;
                }
            });
        });

        function ATreeView_Select(sender, arg) {
            $("a.selectedtreenode").removeClass("selectedtreenode");
            $(sender).addClass("selectedtreenode");
            document.getElementById("TreeView_SelectedValue").value = arg;
            document.getElementById("TreeView_SelectedNodeText").value = sender.innerHTML;
            return false;
        }

        function UpdateSelectedRow(change) {
            var items = $("input.sel");
            if (items.length > 0) {
                for (var i = 0; i < items.length; i++) {
                    items[i].checked = change;
                }
            }
        }

        function ChangeSelect() {

        }

    </script>
</div>