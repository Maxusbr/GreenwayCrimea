<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.Products.ProductOffers" CodeBehind="ProductOffers.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
    <ContentTemplate>
        <table>
            <tr>
                <td style="height: 29px; vertical-align: middle;">
                    <label class="form-lbl2" for="<%= chkMultiOffer.ClientID %>"><%= Resources.Resource.Admin_Product_VaryByColorAndSize%></label>
                    <asp:CheckBox ID="chkMultiOffer" runat="server" Checked="false" CssClass="checkly-align" style="margin-top: -2px;"
                        AutoPostBack="true" OnCheckedChanged="chkMultiOffer_Click" />
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                Цены и наличие
                            </header>
                            <div class="help-content">
                                Данная опция определяет - одна цена у товара, или меняется в зависимости от цвета и размера.<br />
                                <br />
                                Подробнее: <a href="http://www.advantshop.net/help/pages/change-price" target="_blank">Изменение цены и наличия в зависимости от размера и цвета</a>
                            </div>
                        </article>
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <asp:MultiView runat="server" ActiveViewIndex="0" ID="mvOffers">
            <asp:View runat="server" ID="viewSingleOffer">
                <table width="500px" class="table-offer">
                    <tr>
                        <th>
                            <%= Resources.Resource.Admin_Product_Price %>
                        </th>
                        <th>
                            <%= Resources.Resource.Admin_Product_SupplyPrice %>
                        </th>
                        <th>
                            <%= Resources.Resource.Admin_Product_Amount %>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPrice" runat="server" Width="160px" Text="0" CssClass="niceTextBox shortTextBoxClass3"/>
                            <asp:RangeValidator ID="rvPrice" runat="server" ControlToValidate="txtPrice" ValidationGroup="2" Display="Dynamic"
                                EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                                MaximumValue="1000000000" MinimumValue="0" Type="Double" />
                            <asp:RequiredFieldValidator ID="RangeValidator9" runat="server" ControlToValidate="txtPrice" ValidationGroup="2"
                                Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="txtSupplyPrice" runat="server" Width="160px" Text="0" CssClass="niceTextBox shortTextBoxClass3"/>
                            <asp:RangeValidator ID="rvSupplyPrice" runat="server" ControlToValidate="txtSupplyPrice" ValidationGroup="2"
                                Display="Dynamic" EnableClientScript="true" ErrorMessage='*' MaximumValue="1000000000" MinimumValue="0"
                                Type="Double" />
                            <asp:RequiredFieldValidator ID="RangeValidator10" runat="server" ControlToValidate="txtSupplyPrice" ValidationGroup="2"
                                Display="Dynamic" EnableClientScript="true" ErrorMessage='*' />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="server" Width="160px" Text="1" CssClass="niceTextBox shortTextBoxClass3"/>
                            <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtAmount" ValidationGroup="2"
                                Display="Dynamic" EnableClientScript="true" ErrorMessage='*' MaximumValue="100000" MinimumValue="-100000"
                                Type="Double" />
                            <asp:RequiredFieldValidator ID="RangeValidator12" runat="server" ControlToValidate="txtAmount" ValidationGroup="2"
                                Display="Dynamic" EnableClientScript="true" ErrorMessage='*' />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View runat="server" ID="viewMultiOffer">
                <asp:Label runat="server" ID="lErrorOffer" Text="*" ForeColor="Red" Visible="false" />
                <table class="table-offer">
                    <tr>
                        <th>
                            <%= Resources.Resource.Admin_Product_Main %>
                        </th>
                        <th>
                            <%= Resources.Resource.Admin_Product_StockNumber%>
                        </th>
                        <th>
                            <% =SettingsCatalog.SizesHeader%>
                        </th>
                        <th>
                            <% =SettingsCatalog.ColorsHeader%>
                        </th>
                        <th>
                            <%= Resources.Resource.Admin_Product_Price %>
                        </th>
                        <th>
                            <%= Resources.Resource.Admin_Product_SupplyPrice %>
                        </th>
                        <th>
                            <%= Resources.Resource.Admin_Product_Amount %>
                        </th>
                    </tr>
                    <asp:ListView ID="lvOffers" runat="server" OnItemCommand="lvOffers_ItemCommand" OnItemDataBound="lvOffers_ItemDataBound">
                        <EmptyDataTemplate>
                            <tr>
                                <td colspan="6">
                                    <%= Resources.Resource.Admin_Product_NoOffres %>
                                </td>
                            </tr>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="cbMultiMain" runat="server" CssClass="cbMain" Checked='<%# Eval("Main") %>' />
                                </td>
                                <td>
                                    <asp:HiddenField runat="server" ID="hfOfferID" Value='<%# Eval("OfferID") %>' />
                                    <asp:TextBox ID="txtMultySKU" runat="server" Width="100px" Text='<%# Eval("ArtNo") %>' CssClass="niceTextBox shortTextBoxClass3" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMultySKU"
                                        Display="Dynamic" EnableClientScript="true" ErrorMessage='*' />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMultiSize" runat="server" Width="100px"
                                        onmousedown="javascript:loadsizes(this)" onchange="javascript:changeItem(this)">
                                        <Items>
                                            <asp:ListItem Text="––––" Value="none"></asp:ListItem>
                                        </Items>
                                    </asp:DropDownList>
                                    <asp:HiddenField runat="server" ID="hfSelectSizeId" Value='<%# Eval("SizeID") %>' />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMultiColor" runat="server" Width="100px"
                                        onmousedown="loadcolors(this)" onchange="changeItem(this)">
                                        <Items>
                                            <asp:ListItem Text="––––" Value="none"></asp:ListItem>
                                        </Items>
                                    </asp:DropDownList>
                                    <asp:HiddenField runat="server" ID="hfSelectColorId" Value='<%# Eval("ColorID") %>' />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMultiPrice" runat="server" Width="100px" Text='<%# Eval("BasePrice") %>' CssClass="niceTextBox shortTextBoxClass3" />
                                    <asp:RangeValidator ID="RangeValidator16" runat="server" ControlToValidate="txtMultiPrice" ValidationGroup="2"
                                        Display="Dynamic" EnableClientScript="true" ErrorMessage='*' MaximumValue="1000000000" MinimumValue="0"
                                        Type="Double" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMultiPrice"
                                        ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='*' />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMultiSupplyPrice" runat="server" Width="100px" Text='<%# Eval("SupplyPrice") %>' CssClass="niceTextBox shortTextBoxClass3" />
                                    <asp:RangeValidator ID="RangeValidator17" runat="server" ControlToValidate="txtMultiSupplyPrice" ValidationGroup="2"
                                        Display="Dynamic" EnableClientScript="true" ErrorMessage='*' MaximumValue="1000000000" MinimumValue="0"
                                        Type="Double" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMultiSupplyPrice"
                                        ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='*' />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMultiAmount" runat="server" Width="100px" Text='<%# Eval("Amount") %>' CssClass="niceTextBox shortTextBoxClass3" />
                                    <asp:RangeValidator ID="RangeValidator18" runat="server" ControlToValidate="txtMultiAmount" ValidationGroup="2"
                                        Display="Dynamic" EnableClientScript="true" ErrorMessage='*' MaximumValue="100000" MinimumValue="-100000"
                                        Type="Double" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtMultiAmount"
                                        ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='*' />
                                </td>
                                <td>
                                    <asp:ImageButton runat="server" ImageUrl="~/Admin/images/deletebtn.png" CommandName="DeleteOffer" CommandArgument='<%# Eval("OfferID") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </table>
                <div style="margin-top:10px;">
                    <asp:Button runat="server" Text="<%$ Resources:Resource, Admin_Product_NewOffer %>" CssClass="btn btn-middle btn-add"
                        ID="lbNewOffer" OnClientClick="clearCache()" OnClick="lbNewOffer_Click" />
                </div>
            </asp:View>
        </asp:MultiView>
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    
    var colorsCache;
    var sizesCache;
    var arrLoaded = {};
    var isCacheLoaded = false;

    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() {
        clearCache();
    });

    document.addEventListener('DOMContentLoaded', function ready() {
        
        if (!isCacheLoaded) {

            $(document.body).on("click", ".cbMain>input", function() {
                if ($(this).is(":checked")) {
                    $(".cbMain>input").removeAttr("checked");
                    $(this).attr("checked", "checked");
                }
            });

            $.getJSON('HttpHandlers/Product/GetColors.ashx?q=' + Math.random(), function(data) {
                colorsCache = data;
            });

            $.getJSON('HttpHandlers/Product/GetSizes.ashx?q=' + Math.random(), function(data) {
                sizesCache = data;
            });

            isCacheLoaded = true;
        }
    });

    function clearCache()
    {
        arrLoaded = {};
    }

    function loadcolors(obj) {
        if (arrLoaded[obj.getAttribute('name')]) return;

        renderItem(colorsCache, $(obj));
    }

    function loadsizes(obj) {
        if (arrLoaded[obj.getAttribute('name')]) return;

        renderItem(sizesCache, $(obj));
    }

    function renderItem(items, target) {
        var selectval = target.find('option:selected').val(),
            itemSelect;

        if (target.find('option').length > 1) {
            target.find('option').last().remove();
        }

        for (i = 0; i < items.length; i++) {
            var temp = $('<option></option>').val(items[i].Id).html(items[i].Name);
            if (temp.val() == selectval) {
                temp.attr("selected", "selected");
                itemSelect = temp;
            }
            target.append(temp);
        }

        arrLoaded[target.attr('name')] = true;
    }

    function changeItem(obj) {
        var target = $(obj);
        target.siblings('input[type=hidden]').val(target.find(":selected").val());
    }
</script>
