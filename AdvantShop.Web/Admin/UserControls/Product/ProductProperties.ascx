<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductProperties.ascx.cs"
    Inherits="Admin.UserControls.Products.ProductProperties" %>
<script type="text/javascript">
    function ClickCboPropertyName() {
        $("#<%= txtNewPropertyName.ClientID %>").attr("class", "disabled");
        $("#<%= cboPropertyName.ClientID %>").removeAttr("class");
        $("#<%= cboPropertyName.ClientID %> option").attr("class", ".option");
        //Enable checkbox
        $("#<%= RadioButtonExistPropertyValue.ClientID %>").removeAttr("disabled");
        //Enable dropdown
        $("#<%= cboPropertyValue.ClientID %>").removeAttr("disabled");
    }

    function ClickTdExistPropertyName() {
        document.getElementById('<%=RadioButtonExistProperty.ClientID%>').click();
    }

    function ClickCboPropertyValue() {
        $("#<%= cboPropertyValue.ClientID %>").removeAttr("class");
        $("#<%= cboPropertyValue.ClientID %> option").attr("class", ".option");
        $("#<%= txtNewPropertyValue.ClientID %>").attr("class", "disabled");
    }

    function ClickTdExistPropertyValue() {
        document.getElementById('<%=RadioButtonExistPropertyValue.ClientID%>').click();
    }

    function ClickTxtNewPropertyName() {
        $("#<%= txtNewPropertyName.ClientID %>").removeAttr("class");
        $("#<%= cboPropertyName.ClientID %>").attr("class", "disabled");
        $("#<%= cboPropertyName.ClientID %> option").removeAttr("class");
        $("#<%= RadioButtonNewPropertyValue.ClientID %>").click();
        //Disable checkbox
        $("#<%= RadioButtonExistPropertyValue.ClientID %>").attr("disabled", "disabled");
        //Disable dropdown
        $("#<%= cboPropertyValue.ClientID %>").attr("disabled", "disabled");
    }

    function ClickTdNewPropertyName() {
        document.getElementById('<%=RadioButtonAddNewProperty.ClientID%>').click();
    }

    function ClickTxtNewPropertyValue() {
        $("#<%= txtNewPropertyValue.ClientID %>").removeAttr("class");
        $("#<%= cboPropertyValue.ClientID %>").attr("class", "disabled");
        $("#<%= cboPropertyValue.ClientID %> option").removeAttr("class");
    }

    function ClickTdNewPropertyValue() {
        document.getElementById('<%=RadioButtonNewPropertyValue.ClientID%>').click();
    }
    function SwitchDvValue(sender) {
        $(sender).closest("table").find("input[type=text]").attr("class", "disabled");
        $(sender).closest("table").find("select").attr("class", "disabled");
        $(sender).closest("table").find("select option").attr("class", ".option");
        $(sender).closest("tr").find("select").removeAttr("class");
        $(sender).closest("tr").find("select option").removeAttr("class");
        $(sender).closest("tr").find("input[type=text]").removeAttr("class");
    }
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Label ID="lblErrorAddProp" runat="server" CssClass="mProductLabelInfo" EnableViewState="False"
            Font-Names="Verdana" Font-Size="14px" ForeColor="Red" Visible="False" />
        <table class="table-p">
            <tr>
                <td class="formheader" colspan="4">
                    <h2><%=Resources.Resource.Admin_Product_ProductProperties%></h2>
                </td>
            </tr>
            <tr class="formheaderfooter">
                    <td colspan="4">
            </tr>
            <tr>
                <td style="width:40%; padding-bottom:4px;">
                    <asp:RadioButton ID="RadioButtonExistProperty" runat="server" GroupName="PropName"
                        Checked="True" OnClick="ClickCboPropertyName()" CssClass="checkly-align" style="height:14px;" />
                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_ExistProperties %>"
                        Font-Bold="False" AssociatedControlID="RadioButtonExistProperty" Style="margin-left: 1px;" EnableViewState="false"></asp:Label>
                    <span style="color: red;">&nbsp;*</span>
                </td>
                <td style="width:40%; padding-bottom:4px;">
                    <asp:RadioButton ID="RadioButtonExistPropertyValue" runat="server" GroupName="PropValue"
                        Checked="True" OnClick="ClickCboPropertyValue()" CssClass="checkly-align" style="height:14px;" />
                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_Value %>"
                        AssociatedControlID="RadioButtonExistPropertyValue" Style="margin-left: 1px;" EnableViewState="false"></asp:Label>
                    <span style="color: red;">&nbsp;*</span>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="height:19px;">
                    <asp:DropDownList ID="cboPropertyName" runat="server" Width="92%" AutoPostBack="True"
                        onfocus="ClickTdExistPropertyName();return false;" OnSelectedIndexChanged="cboPropertyName_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="height:19px;">
                    <asp:DropDownList ID="cboPropertyValue" runat="server" Width="92%" onfocus="ClickTdExistPropertyValue(); return false;">
                    </asp:DropDownList>
                    &nbsp;
                </td>
                <td style="height:19px;">
                    <asp:Button ID="btnAddNewExistProperty" runat="server" CssClass="btn btn-middle btn-add" Text="<%$ Resources:Resource, Admin_Product_Add %>"
                        OnClick="btnAddNewExistProperty_Click" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="height:28px; width:40%; vertical-align:bottom; padding-bottom:4px;">
                    <asp:RadioButton ID="RadioButtonAddNewProperty" runat="server" GroupName="PropName"
                        OnClick="ClickTxtNewPropertyName()" CssClass="checkly-align" style="height:14px;" />
                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_AddNewProperty %>"
                        Font-Bold="False" AssociatedControlID="RadioButtonAddNewProperty" Style="margin-left: 1px;" EnableViewState="false"></asp:Label>
                    <span style="color: red;">&nbsp;*</span>
                </td>
                <td style="height:28px; width:40%; vertical-align:bottom; padding-bottom:4px;">
                    <asp:RadioButton ID="RadioButtonNewPropertyValue" runat="server" GroupName="PropValue"
                        OnClick="ClickTxtNewPropertyValue()" CssClass="checkly-align" style="height:14px;" />
                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_NewValue %>"
                        AssociatedControlID="RadioButtonNewPropertyValue" Style="margin-left: 1px;" EnableViewState="false"></asp:Label>
                    <span style="color: red;">&nbsp;*</span>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr style="background-color: #eff0f1;">
                <td style="height: 19px; width: 40%;" onclick="ClickTdNewPropertyName()">
                    <asp:TextBox ID="txtNewPropertyName" runat="server" style="width:90%; height: 23px; border: 1px solid gray; padding-left: 3px;"/>
                </td>
                <td style="height: 19px; width: 40%;" onclick="ClickTdNewPropertyValue()">
                    <asp:TextBox ID="txtNewPropertyValue" runat="server" style="width:90%; height: 23px; border: 1px solid gray; padding-left: 3px;"/>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr style="background-color: #eff0f1;">
                <td style="height: 13px; width: 40%;">
                    <asp:Label ID="lblValueRequired" runat="server" Visible="false" EnableViewState="false"
                        ForeColor="Red" Text="<%$Resources:Resource, Admin_m_Product_RequiredField %>" />
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4">
                    <div class="dvSubHelp4">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                        <a href="http://www.advantshop.net/help/pages/goods-properties" target="_blank">Инструкция. Настройка фильтров и свойств товара.</a>
                    </div> 
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <%= RenderingProperties%>
                    <input type="hidden" id="hfpropselected" class="hfpropselected" runat="server" />
                    <input type="hidden" id="hfpropresult" class="hfpropresult" runat="server" />
                    <input type="hidden" id="hfnewpropresult" class="hfnewpropresult" runat="server" />
                    <input type="hidden" id="hfpropproductId" class="hfpropproductId" runat="server" />
                    <br />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>