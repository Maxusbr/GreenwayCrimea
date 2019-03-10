<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.BillControl" Codebehind="Bill.ascx.cs" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px;
    margin-top: 5px;">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <h4 style="display: inline; font-size: 12pt;">
                <asp:Localize ID="Localize27" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_CompanyName %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCompanyName" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Bill_CompanyName_Description %>" /><asp:Label
                    runat="server" ID="msgCompanyName" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_TransactAccount %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtTransAccount" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Bill_TransactAccount_Description %>" /><asp:Label
                    runat="server" ID="msgTransAccount" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_CorAccount %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCorAccount" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Bill_CorAccount_Description %>" /><asp:Label
                    runat="server" ID="msgCorAccount" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_Address %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtAddress" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Bill_Address_Description %>" /><asp:Label
                    runat="server" ID="msgAddress" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_Telephone %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtTelephone" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Bill_Telephone_Description %>" /><asp:Label
                    runat="server" ID="msgTelephone" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_INN %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtINN" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Bill_INN_Description %>" /><asp:Label
                    runat="server" ID="msgINN" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize15" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_KPP %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtKPP" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Bill_KPP_Description %>" /><asp:Label
                    runat="server" ID="msgKPP" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize17" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_BIK %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtBIK" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Bill_BIK_Description %>" /><asp:Label
                    runat="server" ID="msgBIK" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize19" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_BankName %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtBankName" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Bill_BankName_Description %>" /><asp:Label
                    runat="server" ID="msgBankName" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize21" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_Director %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtDirector" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Bill_Director_Description %>" /><asp:Label
                    runat="server" ID="msgDirector" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_PositionDirector %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPosDirector" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="" /><asp:Label
                    runat="server" ID="Label1" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize23" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_Accountant %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtAccountant" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Bill_Accountant_Description %>" /><asp:Label
                    runat="server" ID="msgAccountant" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_PositionAccountant %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPosAccountant" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="" /><asp:Label
                    runat="server" ID="Label2" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize25" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_Manager %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtManager" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_Bill_Manager_Description %>" /><asp:Label
                    runat="server" ID="msgManager" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Bill_PositionManager %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPosManager" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="" /><asp:Label
                    runat="server" ID="Label3" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            Печать
        </td>
        <td class="columnVal">
            <div class="stamp-b">
            <% if (!string.IsNullOrEmpty(StampImg))
               { %>
                <img src="<%= FoldersHelper.GetPath(FolderType.Pictures, StampImg, false) %>"/>
            <% } %>
            </div>
            <input type="file" id="stampImgUpload" name="stampImgUpload" />
            <input id="hfStampImg" type="hidden" class="hfStampImg" runat="server" />
            <div style="padding: 10px 0">
                <a href="javascript:void(0);" class="btn btn-small btn-action" onclick="uploadPicture()">Загрузить</a>
                <a href="javascript:void(0);" class="btn btn-small btn-action" onclick="deletePicture()">Удалить</a>
            </div>
        </td>
        <td>
            
        </td>
    </tr>
</table>
<script type="text/javascript" src="js/ajaxfileupload.js"></script>
<script type="text/javascript">

    function uploadPicture() {

	    var arrId = [];
	    arrId.push("stampImgUpload");

	    $.ajaxFileUpload({
	        url: '../admin/HttpHandlers/PaymentMethods/BillStamp.ashx?methodId=' + $("#methodId").val(),
		    secureuri: false,
		    fileElementId: arrId,
		    dataType: 'json',
		    success: function (data, status) {
			    if (data != null && data.error == false) {
			        $(".stamp-b").html('<img src="' + data.src + '" />');
			        $(".hfStampImg").val(data.imgName);
			    }
		    },
		    error: function (data, status, e) {
			    notify(e, notifyType.error, false, { timer: 10000 });
		    }
	    });
    }

    function deletePicture() {

        $.ajax({
            url: '../admin/HttpHandlers/PaymentMethods/BillStampDelete.ashx?methodId=' + $("#methodId").val(),
            dataType: 'json',
            cache: false,
            success: function (data, status) {
                $(".stamp-b").html('');
                $(".hfStampImg").val('');
            },
            error: function (data, status, e) {
                notify(e, notifyType.error, false, { timer: 10000 });
            }
        });
    }

</script>
