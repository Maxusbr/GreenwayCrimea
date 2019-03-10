<%@ Page Language="C#" AutoEventWireup="true" Inherits="ClientPages.err500" Codebehind="err500.aspx.cs" %>
<%@ Import Namespace="Resources" %>

<!DOCTYPE html>
<html>
<head>
    <title><%= Resource.Internal_Server_Error%></title>
    <style type="text/css">
        body {
            font-family: Arial, Helvetica, sans-serif;
            color: #4b4f58;
            font-size: 16px;
        }

        .wrapper {
            width: 80%;
            margin: 0 auto;
        }

        .code-error {
            font-size: 60px;
            font-weight: bold;
            margin-top: 75px;
        }

        .code-descripition {
            font-size: 30px;
            margin-top: 25px;
        }

        .to-do {
        }

            .to-do .head {
                font-weight: bold;
                font-size: 18px;
                margin-top: 60px;
            }

        ul {
            list-style: none;
            padding: 0;
            margin: 0;
        }

            ul li:before {
                content: "- ";
            }

        .to-do .to-do-items li {
            margin: 15px 0;
        }
    </style>
</head>
<body>
    <div class="wrapper">
        <div class="code-error">
            <%=Resource.Client_Error %> 500
        </div>
        <div class="code-descripition">
            <%= Resource.Client_Error_Descripition%>
        </div>
        <div class="to-do">
            <div class="head">
                <%= Resource.Client_Try_The_Following%>:
            </div>
            <ul class="to-do-items">
                <li>
                    <%= Resource.Client_Refresh_Page%>
                </li>
                <li>
                    <%= Resource.Client_Return_Later%>
                </li>
                <li>
                    <%= Resource.Return_To%> <a href="<%= Request.ApplicationPath %>"><%= Resource.GotToMain%></a>
                </li>
            </ul>

        </div>
    </div>
</body>
</html>