<%@ Control Language="C#" AutoEventWireup="true" Inherits="Advantshop.Modules.UserControls.BuyMore.Admin_BuyMoreModule" CodeBehind="BuyMoreModule.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: BuyMore_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="padding: 20px 0 0 0; text-align: left; vertical-align: top; width: 170px;">
                <asp:Localize ID="Localize22" runat="server" Text="<%$ Resources: BuyMore_Offres%>" />
            </td>
            <td style="padding: 20px 0 0 0">

                <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                    <contenttemplate>
                        <asp:ListView ID="rprProducts" runat="server" OnItemCommand="rprProducts_ItemCommand" ItemPlaceholderID="trPlaceholderID">
                            <LayoutTemplate>
                                <table class="table-ui">
                                    <tr>
                                        <td><asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: BuyMore_OrderPriceFrom %>" /></td>
                                        <td><asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: FreeShipping %>" /></td>
                                        <td><asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: GiftProduct %>" /></td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr id="trPlaceholderID" runat="server"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# Eval("OrderPriceFrom") %>
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" Checked='<%# Eval("FreeShipping") %>' Enabled="false"/> 
                                    </td>
                                    <td>                                        
                                        <%# RenderProduct(Convert.ToString(Eval("GiftOffersIds"))) %>
                                    </td>
                                    <td>
                                         <a href='<%# "javascript:open_window(\"../Modules/BuyMore/BuyMoreModuleAddEdit.aspx?Id=" + Eval("Id") +"\",500,400)"%>'><asp:Image runat="server" ImageUrl="~/Modules/BuyMore/images/editbtn.gif" EnableViewState="false" /></a>
                                         <asp:LinkButton ID="lb" runat="server" CommandArgument='<%#Eval("Id") %>' CommandName="DeleteItem" OnClientClick="return confirmDelete();"><asp:Image runat="server" ImageUrl="~/Modules/BuyMore/images/remove.jpg" EnableViewState="false"  /></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <asp:Localize ID="Localize66" runat="server" Text="<%$ Resources: NoOffers%>" />
                            </EmptyDataTemplate>
                        </asp:ListView>
                   </contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>

        <tr>
            <td></td>
            <td>
                <div style="margin: 15px 0">
                    <a href='javascript:open_window("../Modules/BuyMore/BuyMoreModuleAddEdit.aspx",400,300)'>
                        <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: AddOffer%>" /></a>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">Кроме данных предложений, учитываются еще скидки указанные в разделе Маркетинг - Скидки.</td>
        </tr>
        <tr>
            <td colspan="2">
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Localize ID="Localize7" runat="server" Text="Максимальная стоимость доставки участвующая в акции:" /></td>
            <td>
                <asp:TextBox runat="server" ID="txtShippingPriceTo"></asp:TextBox>
                <span style='color: gray'>(если расчитанная стоимость доставки превышает данное значение, доставка не будет бесплатной)</span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Localize ID="Localize3" runat="server" Text="Выводить сообщение о предложениях:" /></td>
            <td>
                <label>
                    <input type="radio" runat="server" id="rbDisplayAlways" name="DisplayDiscounts" />Всегда</label><br />
                <label>
                    <input type="radio" runat="server" id="rbDisplayMissing" name="DisplayDiscounts" />Если не хватает
                    <asp:TextBox runat="server" ID="txtMissingDiscount" Width="50px" />% до предложения. <span style='color: gray'>(Если предложение от 1 000 руб, то сообщение будет показываться при сумме корзины от
                    <asp:Literal runat="server" ID="lSample" />
                        руб)</span></label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Localize ID="Localize8" runat="server" Text="Исключенные методы доставки" /></td>
            <td>
                <p style='color: gray'>Доставка выбранными методами не будет бесплатной</p>
                <asp:CheckBoxList runat="server" ID="cblExcludedShippings" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Save%>" />
            </td>
        </tr>
    </table>
</div>
<script type="text/javascript">
    function confirmDelete() {
        return confirm("Вы уверены, что хотите удалить предложение?");
    }
</script>
