<%@ Control Language="C#" AutoEventWireup="true" Inherits="Advantshop.Modules.UserControls.FindCheaper.Admin_FindCheaperManager" CodeBehind="FindCheaperManager.ascx.cs" %>
<style>
    .reviewsTable {
        border-collapse: collapse;
        width: 100%;
    }

        .reviewsTable td, .reviewsTable th {
            border-bottom: 1px solid #000000;
            height: 30px;
            text-align: left;
        }
</style>
<div>
    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Label ID="lblHeader" runat="server" Text="Управление запросами пользователей" /></span>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
    </table>

    <asp:ListView ID="lvItems" runat="server" ItemPlaceholderID="itemPlaceHolder" OnItemCommand="lvItemsItemCommand" OnItemEditing="lvTabs_ItemEditing" OnItemCanceling="lvTabs_ItemCanceling">
        <layouttemplate>
            <table class="tableview">
                <tr>
                    <th>
                        <asp:Label ID="lblHdrRequestDate" runat="server" Text='Дата запроса'></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lblHdrName" runat="server" Text='Имя'></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lblHdrPhone" runat="server" Text='Телефон'></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lblHdrProduct" runat="server" Text='Артикул варианта - Имя продукта'></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lblHdrPrice" runat="server" Text='Цена'></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lblHdrWishPrice" runat="server" Text='Желаемая цена'></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lblHdrWhereCheaper" runat="server" Text='Где дешевле'></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lblHdrManagerComment" runat="server" Text='Комментарий'></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lblHdrIsProcessed" runat="server" Text='Обработано'></asp:Label>
                    </th>
                    <th>
                    </th>
                </tr>
                <tr runat="server" id="itemPlaceHolder">
                </tr>
            </table>
        </layouttemplate>
        <edititemtemplate>
            <tr>
                <td>
                    <asp:Label ID="lblRequestDate" runat="server" Text='<%# Eval("RequestDate") %>'></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtClientName" runat="server" Text='<%# Eval("ClientName") %>'></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtClientPhone" runat="server" Text='<%# Eval("ClientPhone") %>'></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblProduct" runat="server" Text='<%# Eval("OfferArtNo") + " - " + Eval("ProductName") %>'></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtWishPrice" runat="server" Text='<%# Eval("WishPrice") %>'></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtWhereCheaper" runat="server" Text='<%# Eval("WhereCheaper") %>'></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtManagerComment" runat="server" Text='<%# Eval("ManagerComment") %>' TextMode="MultiLine"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="ckbIsProcessed" runat="server" Checked='<%#Eval("IsProcessed") %>'/>
                </td>
                <td>
                    <asp:LinkButton ID="UpdateButton" runat="server" CommandName="updateItem" CommandArgument='<%#Eval("Id") %>' Text="Сохранить"></asp:LinkButton>
                    <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" Text="Отмена"></asp:LinkButton>
                </td>
            </tr>
        </edititemtemplate>
        <itemtemplate>
            <tr>
                <td>
                    <label><%# Eval("RequestDate") %></label>
                </td>
                <td>
                    <label><%# Eval("ClientName") %></label>
                </td>
                <td>
                    <label><%# Eval("ClientPhone") %></Label>
                </td>
                <td>
                    <label><%# Eval("OfferArtNo") + " - " + Eval("ProductName") %></label>
                </td>
                <td>
                    <label><%# Eval("Price") %></label>
                </td>
                <td>
                    <label><%# Eval("WishPrice") %></label>
                </td>
                <td>
                    <label><%# Eval("WhereCheaper") %></label>
                </td>
                <td>
                    <label><%# Eval("ManagerComment") %></label>
                </td>
                <td>
                    <input type="checkbox" <%#Convert.ToBoolean(Eval("IsProcessed")) ? "checked=\"checked\"" :string.Empty %> disabled/>
                </td>
                <td>
                      <asp:LinkButton ID="lnkbEdit" runat="server" Text='Редактировать' CommandName="Edit"></asp:LinkButton>
                      <asp:LinkButton ID="lnkbDelete" runat="server" Text='Удалить' CssClass="valid-confirm" data-confirm="Вы уверены что хотите удалить?" CommandName="deleteItem" CommandArgument='<%#Eval("Id") %>' />
                </td>
            </tr>
        </itemtemplate>
        <EmptyDataTemplate>
            <div style="padding: 10px 0">
                Нет запросов
            </div>
        </EmptyDataTemplate>
    </asp:ListView>
</div>
