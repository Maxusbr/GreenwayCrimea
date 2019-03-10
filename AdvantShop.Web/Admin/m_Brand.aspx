<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="m_MasterPage.master" Inherits="Admin.m_Brand" ValidateRequest="false" CodeBehind="m_Brand.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Core.Modules.Interfaces" %>


<asp:Content ID="contentCenter" runat="server" ContentPlaceHolderID="cphCenter">
    <div>
        <center>
            <asp:Label ID="lblCustomer" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_m_Brand_Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblCustomerName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_m_Brand_SubHeader %>"></asp:Label>
        </center>
        <asp:Panel ID="pnlAdd" runat="server" Height="84px" Width="100%">
            <center>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>&nbsp;</center>
            <table class="info-center-tb">
                <tr>
                    <td class="lpart">
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_m_Brand_Name%>"></asp:Label>: 
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Width="300px" CssClass="brandName"></asp:TextBox>
                        <asp:Label ID="Label12" runat="server" Font-Italic="True" ForeColor="DarkGray" Text="<%$ Resources:Resource, Admin_m_Brand_Required %>"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName"
                            ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="lpart">
                        <asp:Label ID="lblStringID" runat="server" Text="<%$ Resources:Resource, Admin_m_Brand_Url%>"></asp:Label>&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtURL" runat="server" Width="224px"></asp:TextBox>
                        <asp:Label ID="Label6" runat="server" Font-Italic="True" ForeColor="DarkGray" Text="<%$ Resources:Resource, Admin_m_Brand_Required %>"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtURL"
                            ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>"></asp:RequiredFieldValidator>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="lpart">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_m_Brand_Country %>"></asp:Label>&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCountry" Width="300px"></asp:DropDownList>
                    </td>
                </tr>
                <tr class="picture-td">
                    <td class="lpart">
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, Admin_m_Brand_Logo %>"></asp:Label>&nbsp;
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upnlPhoto" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnDeleteImage" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <div>
                                    &nbsp;<asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, Admin_m_Brand_CurentLogo %>"></asp:Label>
                                </div>
                                <div>
                                    <asp:Image ID="imgLogo" runat="server" Width="100px"/>
                                </div>
                                <div>
                                    <asp:Button ID="btnDeleteImage" runat="server" Text="<%$  Resources:Resource, Admin_Delete%>"
                                        OnClick="btnDeleteLogo_Click" CssClass="btn btn-action btn-small" EnableViewState="false" />
                                </div>
                                <asp:FileUpload ID="fuBrandLogo" runat="server" Width="308px" Height="20px" />
                                <asp:Label ID="lblImageInfo" runat="server" Font-Bold="False" Font-Size="Smaller"
                                    ForeColor="Gray"></asp:Label>
                                <% if (AdvantShop.Core.Modules.AttachedModules.GetModules<IPhotoSearcher>().Any())
                                   { %>
                                <div>
                                    <a href="javascript:void(0)" class="btn btn-action btn-small" id="aSearchPhotos">
                                        <%= Resources.Resource.Admin_m_Product_SearchPhotos %>
                                    </a>
                                    <span data-picture-chosen></span><span data-picture-remove>X</span>
                                    <asp:HiddenField ID="hfGoogleLinks" runat="server" />
                                </div>
                                <% } %>
                                <%--<asp:LinkButton ID="lnkUpdatePhotoPanel" runat="server" OnClick="lnkUpdatePhotoPanel_Click"></asp:LinkButton>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="lpart">
                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Resource, Admin_m_Brand_Enabled%>"></asp:Label>&nbsp;
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEnabled" runat="server" Checked="True" />
                    </td>
                </tr>
                <tr>
                    <td class="lpart">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_m_Brand_SortOrder%>"></asp:Label>&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtSortOrder" runat="server" Text="0" />
                    </td>
                </tr>
                <tr>
                    <td class="lpart">
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_m_Brand_BrandSiteUrl%>"></asp:Label>&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtBrandSiteUrl" runat="server" Text="0" />
                    </td>
                </tr>
                <tr>
                    <td class="lpart">
                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UseDefaultMeta%>"></asp:Label>&nbsp;
                    </td>
                    <td>
                        <asp:CheckBox ID="chbDefaultMeta" runat="server" Checked="True" />
                    </td>
                </tr>
                <tr>
                    <td class="lpart">
                        <asp:Localize ID="Localize_Admin_m_Product_HeadTitle" runat="server" Text="<%$ Resources: Resource, Admin_m_Brand_PageTitle %>"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server" Width="354" CssClass="niceTextBox textBoxClass" />
                        <br />
                        <asp:Label runat="server" CssClass="info-hint-text" Text="<%$ Resources: Resource, Admin_m_Brand_UseGlobalVariables %>" />
                    </td>
                </tr>
                <tr>
                    <td class="lpart">H1
                    </td>
                    <td>
                        <asp:TextBox ID="txtH1" runat="server" Width="354" CssClass="niceTextBox textBoxClass"/>
                        <br />
                        <asp:Label runat="server" CssClass="info-hint-text" Text="<%$ Resources: Resource, Admin_m_Brand_UseGlobalVariables %>" />
                    </td>
                </tr>
                <tr>
                    <td class="lpart">
                        <asp:Localize ID="Localize_Admin_m_Product_MetaKeywords" runat="server" Text="<%$ Resources: Resource, Admin_m_Brand_MetaKeywords %>"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMetaKeywords" runat="server" TextMode="MultiLine" Width="354" Height="50px" CssClass="niceTextBox textBoxClass"/>
                        <br />
                        <asp:Label runat="server" CssClass="info-hint-text" Text="<%$ Resources: Resource, Admin_m_Brand_UseGlobalVariables %>" />
                    </td>
                </tr>
                <tr>
                    <td class="lpart">
                        <asp:Localize ID="Localize_Admin_m_Product_MetaDescription" runat="server" Text="<%$ Resources: Resource, Admin_m_Brand_MetaDescription %>"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMetaDescription" runat="server" TextMode="MultiLine" Width="354" Height="50px" CssClass="niceTextBox textBoxClass"/>
                        <br />
                        <asp:Label runat="server" CssClass="info-hint-text" Text="<%$ Resources: Resource, Admin_m_Brand_UseGlobalVariables %>" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfMetaId" runat="server" />
            <!-- editor -->
            <table border="0" id="table1" style="width: 100%; height: 206px;" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center">
                        <br />
                        <asp:Panel ID="Panel4" runat="server" HorizontalAlign="Left" Width="700px" CssClass="all-mar">
                            <asp:Label ID="lblText" runat="server" Text="<%$ Resources:Resource, Admin_m_Brand_Description %>"></asp:Label>
                            <asp:TextBox ID="FCKDescription" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="500px" Width="700px" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <br />
                        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left" Width="700px" CssClass="all-mar">
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_m_Brand_BriefDescription %>"></asp:Label>
                            <asp:TextBox ID="FCKBriefDescription" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="500px" Width="700px" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>

            <!-- editor -->
            <br />
            <center>
                <asp:Button ID="btnOK" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Ok %>"
                    Width="110px" OnClick="btnOK_Click" />&nbsp;</center>
            <br />
        </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="contentScript" runat="server" ContentPlaceHolderID="cphScript">
    <script type="text/javascript">

        function fillUrl() {
            var text = $('#<%=txtName.ClientID %>').val();
            var url = $('#<%=txtURL.ClientID %>').val();
            if ((text != "") && (url == "")) {
                $('#<%=txtURL.ClientID %>').val(translite(text));
            }
        }
        $('#<%=txtURL.ClientID %>').focus(fillUrl);
        
        var _imgLogo;
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { initImagesSearch(); });
        function initImagesSearch() {
            _imgLogo = $("#<%= imgLogo.ClientID %>");
            _imgLogo.data("picture-prev", _imgLogo.attr("src"));
        }

        $(document).ready(function () {

            $(".picture-td").on("click", "[data-picture-remove]", function () {
                _imgLogo.attr("src", _imgLogo.data("picture-prev"));
                $("[data-picture-chosen").text("");
                $("[data-picture-remove").hide();
            });

            var modalPhotos = $.advModal({
                title: "Поиск фото в интернете",
                clickOut: false,
                htmlContent: "<div class='photosSearch'><img src='images/ajax-loader.gif'/></div><div class='photo-message'></div><input type='hidden' id='photosPage' value='0'/>",
                control: "#aSearchPhotos",
                beforeOpen: function () {
                    searchPhotos(modalPhotos, 0);
                },
                buttons: [
                    {
                        textBtn: 'Найти еще',
                        classBtn: 'btn-action',
                        func: function () {
                            var page = parseInt($("#photosPage").val()) + 1;
                            searchPhotos(modalPhotos, page);
                            $("#photosPage").val(page);
                            $(".photo-message").hide();
                        }
                    }, {
                        textBtn: 'Добавить выбранное',
                        classBtn: 'btn-submit',
                        func: function () {
                            var imgSelected = $(".photosSearch input[type=radio]:checked:first"),
                                link = imgSelected.val();
                            $("#<%= hfGoogleLinks.ClientID %>").val(link);
                            if (link.length) {
                                _imgLogo.attr("src", imgSelected.data("thumb"));
                                $("[data-picture-chosen").text("Выбрано: 1");
                                $("[data-picture-remove").show();
                            }
                            modalPhotos.modalClose();
                            //var progress = Advantshop.ScriptsManager.Progress.prototype.Init($('.photosSearch'));
                            //progress.Show();
                        }
                    }, {
                       textBtn: 'Закрыть',
                       classBtn: 'btn-confirm',
                       func: function () {
                           modalPhotos.modalClose();
                           $(".photo-message").text('');
                           //__doPostBack('<%--=lnkUpdatePhotoPanel.UniqueID--%>', '');
                       }
                   }
                ]
            });


        });

        function searchPhotos(modalPhotos, page) {
            $(".photosSearch").empty();

            var term = $('.brandName').val();
            if (term.length) {
                term += ' logo';
            } else {
                $(".photosSearch").html("Фотографии не найдены. Укажите название бренда.");
                return;
            }

            $.ajax({
                dataType: "json",
                data: { term: term, page: page },
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

            var label = $('<label class="search-photos progress"></label>');
            $(".photosSearch").append(label);
            checkImage(item.image.thumbnailLink, function () {
                var str = '<input type="radio" value="' + item.link + '" name="searchBrandPhoto" data-thumb="' + item.image.thumbnailLink + '" />' +
                    '<img src="' + item.image.thumbnailLink + '" title="' + item.title + '" /> <div class="photo-size">' + item.image.width + ' x ' + item.image.height + '</div>';
                label.removeClass('progress').html(str);
            }, function () {
                label.remove();
            });
        }

        function checkImage(src, good, bad) {
            var img = new Image();
            img.onload = good;
            img.onerror = bad;
            img.src = src;
        }

        $(function () {
            if ($('#<%= chbDefaultMeta.ClientID %>').is(":checked")) {
                $('#<%=txtTitle.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtH1.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaDescription.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaKeywords.ClientID %>').attr("disabled", "disabled");
            }
        });

        $('#<%= chbDefaultMeta.ClientID %>').click(function () {
            if ($('#<%= chbDefaultMeta.ClientID %>').is(":checked")) {
                $('#<%=txtTitle.ClientID %>').val("");
                $('#<%=txtH1.ClientID %>').val("");
                $('#<%=txtMetaDescription.ClientID %>').val("");
                $('#<%=txtMetaKeywords.ClientID %>').val("");

                $('#<%=txtTitle.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtH1.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaDescription.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaKeywords.ClientID %>').attr("disabled", "disabled");

            } else {
                $('#<%=txtTitle.ClientID %>').removeAttr("disabled");
                $('#<%=txtH1.ClientID %>').removeAttr("disabled");
                $('#<%=txtMetaDescription.ClientID %>').removeAttr("disabled");
                $('#<%=txtMetaKeywords.ClientID %>').removeAttr("disabled");
            }
        });

    </script>
</asp:Content>
