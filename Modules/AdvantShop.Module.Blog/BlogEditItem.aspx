<%@ Page Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.Blog.BlogEditItem" CodeBehind="BlogEditItem.aspx.cs" %>

<%@ Register TagPrefix="adv" TagName="BlogProducts" Src="BlogProducts.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style>
        .reviewEditTable {
            width: 100%;
            border-collapse: collapse;
        }

            .reviewEditTable td {
                height: 30px;
                width: 50%;
            }

            .reviewEditTable tr.altRow {
                background-color: #eff0f1;
            }

            .reviewEditTable .tdRight {
                text-align: right;
                padding-right: 10px;
            }

            .reviewEditTable .tdLeft {
                text-align: left;
                padding-left: 10px;
            }

            .reviewEditTable input[type='text'] {
                width: 250px;
            }

        .head {
            font-family: Verdana;
            font-size: 18pt;
            text-transform: uppercase;
        }

        .subHead {
            color: #666666;
            font-family: Verdana;
            font-size: 10pt;
        }
    </style>
    <link href="../../admin/css/AdminStyle.css" rel="stylesheet" />
    <script type="text/javascript" src="../../admin/js/jq/jquery-1.7.1.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1"
            ScriptMode="Release">
        </asp:ScriptManager>
        <asp:Literal runat="server" ID="lBase" />
        <div style="text-align: center;">
            <asp:Label ID="lblHeader" CssClass="head" runat="server" Text="<%$ Resources: Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblSubHeader" CssClass="subHead" runat="server" Text="<%$ Resources: SubHeader %>"></asp:Label>
            <br />
            <asp:Label ID="lError" runat="server" ForeColor="red" Text="<%$ Resources: WrongDateFormat %>" Visible="false" />
        </div>
        <br />
        <br />
        <table class="reviewEditTable">
            <tr>
                <td class="tdRight">
                    <asp:Label ID="lblTitle" runat="server" Text='<%$ Resources: Title %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="translit" />
                </td>
            </tr>
            <tr class="altRow">
                <td class="tdRight">
                    <asp:Label ID="lblBlogCategory" runat="server" Text='<%$ Resources: Category %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:DropDownList ID="ddlBlogCategory" runat="server" DataTextField="Name" DataValueField="ItemCategoryId" />
                </td>
            </tr>
            <tr>
                <td class="tdRight">
                    <asp:Label ID="lblUrlPath" runat="server" Text='<%$ Resources: UrlPath %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtUrlPath" runat="server" CssClass="urlpath" />
                </td>
            </tr>
            <tr class="altRow">
                <td class="tdRight">
                    <asp:Label ID="LAddingDate" runat="server" Text='<%$ Resources: AddingDate %>'></asp:Label>
                </td>
                <td class="tdLeft">

                    <asp:TextBox ID="txtDate" runat="server" Width="91px" />
                    <%--<ajaxtoolkit:maskededitextender id="MaskedEditExtender1" runat="server" targetcontrolid="txtDate"
                        mask="9999-99-99" autocomplete="False" autocompletevalue="false" clearmaskonlostfocus="False">
                    </ajaxtoolkit:maskededitextender>--%>

                    <asp:TextBox ID="txtTime" runat="server" Width="50px"></asp:TextBox><span id="validTime" style="color: red; display: none;">*</span>
                    <%--   <ajaxtoolkit:maskededitextender id="meeTime" runat="server" targetcontrolid="txtTime"
                        mask="99:99" autocomplete="False" masktype="Time">
                    </ajaxtoolkit:maskededitextender>--%>
                </td>
            </tr>
            <tr>
                <td class="tdRight">
                    <asp:Label ID="Label1" runat="server" Text='<%$ Resources: Enable %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:CheckBox ID="ckbEnable" runat="server" Checked="True"></asp:CheckBox>
                </td>
            </tr>
            <tr class="altRow">
                <td class="tdRight">
                    <asp:Label ID="lblPhoto" runat="server" Text="<%$ Resources: Picture %>"></asp:Label>:
                </td>
                <td class="tdLeft">
                    <asp:Panel ID="pnlImage" runat="server" Width="100%">
                        &nbsp;<asp:Label ID="Label11" runat="server" Text="<%$ Resources:CurrentPicture %>"></asp:Label>
                        <br />
                        &nbsp;<asp:Image ID="Image1" runat="server" Width="200px" />
                        <br />
                        <asp:Button ID="btnDeleteImage" runat="server" Text="<%$ Resources:Delete %>"
                            OnClick="btnDeleteImage_Click" />
                        <br />
                    </asp:Panel>
                    <asp:FileUpload ID="PictureFileUpload" runat="server" Height="20px" Width="308px" />
                    <asp:Label ID="Label10" runat="server" Text="Label" Visible="False"></asp:Label>
                    <br />
                    <asp:Label ID="lblImageInfo" runat="server" Font-Bold="False" Font-Size="Smaller"
                        ForeColor="Gray" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tdRight">
                    <asp:Label ID="lblMetaTitle" runat="server" Text='<%$ Resources: MetaTitle %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtMetaTitle" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="altRow">
                <td class="tdRight">
                    <asp:Label ID="lblMetaKeywords" runat="server" Text='<%$ Resources: MetaKeywords %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtMetaKeywords" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdRight">
                    <asp:Label ID="lblMetaDescription" runat="server" Text='<%$ Resources: MetaDescription %>'></asp:Label>
                </td>
                <td class="tdLeft">
                    <asp:TextBox ID="txtMetaDescription" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="altRow">
                <td colspan="2">
                    <div style="width: 700px; margin: auto; padding: 10px 0px;">
                        <asp:Label ID="lblTextAnnotation" runat="server" Text='<%$ Resources: TextAnnotation %>'></asp:Label><br />
                        <asp:TextBox ID="txtTextAnnotation" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="500px" Width="100%" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="width: 700px; margin: auto; padding: 10px 0px;">
                        <asp:Label ID="lblTextToPublication" runat="server" Text='<%$ Resources: TextToPublication %>'></asp:Label><br />
                        <asp:TextBox ID="txtTextToPublication" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="500px" Width="100%" />
                    </div>
                </td>
            </tr>
        </table>
        <adv:BlogProducts ID="BlogProduct" runat="server" />
        <div style="text-align: center; margin: 20px;">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSaveClick" Text='<%$ Resources: Save %>' />
        </div>
    </form>
    <script type="text/javascript">
        window.CKEDITOR_BASEPATH = '<%=(Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + (!Request.ApplicationPath.EndsWith("/") ? "/" : string.Empty)) %>vendors/ckeditor/';
    </script>
    <script src="../../vendors/ckeditor/ckeditor.js?update=5.0"></script>
    <script src="../../admin/js/ckeditorInit.js"></script>
    <script type="text/javascript">
        
		$('#<%=txtUrlPath.ClientID %>').focus(fillUrl);

        function fillUrl() {
            var text = $('#<%=txtTitle.ClientID %>').val();
            var url = $('#<%=txtUrlPath.ClientID %>').val();
            if ((text != "") & (url == "")) {
                $('#<%=txtUrlPath.ClientID %>').val(translite(text));
            }
        }
		
		function translite(src) {
			var res = "";
			$.ajax({
				dataType: "json",
				cache: false,
				type: "POST",
				async: false,
				data: {
				source: src
				},
				url: "../../admin/httphandlers/translit.ashx",
				success: function (data) {
					res = data;
				},
				error: function (data) {
				//alert("can't translite '" + src + "'");
        }
    });
    return res;
}
    </script>
</body>
</html>
