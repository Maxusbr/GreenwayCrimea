<%@ Page Language="C#" AutoEventWireup="true" Inherits="ClientPages.SessionError" Codebehind="SessionError.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Error</title>
        <style type="text/css">
            body {background: #2F3032 url('/content/info/images/err_bg.gif') repeat-x scroll 0 0; margin:0px; padding:0px; font-family:Tahoma;}
            #dvErrContainer {margin-left:20px; margin-top:20px; text-align:center;}
            #dvErrContainer .ErrDescription {margin-top:10px; color:#d1d1d1; font:bold 18px Arial;}
            #dvErrContainer .ErrNotify {height:19px; color:white; font-family:arial; font-size:13px; margin-top:11px;}
            .ErrTextCss {width:90%; height:200px; background-color:Gray; text-align:left; padding:10px; margin-top:20px; color:White;}
            a {color:White; font-family:Arial;} 
        </style>  
    </head>
    <body>
        <form id="form1" runat="server">

            <div id="dvErrContainer">
                <div class="ErrDescription"><%= Resources.Resource.Client_SessionError_ErrorNotify%></div>
                <%--<div class="ErrNotify">Мы записали ошибку в журнал событий и разберемся с причинами её появления.</div>--%>
            </div>
            <%--
            <div id="dvErrContainer">
                <div class="ErrDescription">We're sorry, an internal server error has occurred</div>
                <div class="ErrNotify">We have logged this error and will investigate the cause.</div>
            </div>
            --%>
            <center>
                <div class="ErrTextCss" id="dvErr" runat="server"></div>
            </center>
        </form>
    </body>
</html>
