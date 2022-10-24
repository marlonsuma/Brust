<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Login</title>
    <style>
        body {
            font-family: Arial, Helvetica, sans-serif;
        }

        #pageHeader {
            margin-left: 20px;
        }

        #ResultMessage {
            color: red;
            font-size: 12pt;
            font-weight: bold;
            display: block;
            margin: 20px 0;
        }
        #LoginForm {
            width: 1050px;
            margin: 0 45px;
        }
        .control {
            margin: 20px 0;
        }
        .control label {
            display: block;
            font-weight: bold;
        }

        /*.control input, #Submit {
            width: 250px;
            margin: 5px 0;
        }*/

    </style>
</head>
<body>
    <div id="pageHeader">
        <img id="logo" src="/images/Ni-Q Logo.png" />
        <h1>Donor Tracking System</h1>
        <h3>Login</h3>
    </div>
    <form id="LoginForm" runat="server">
        <div class="controlContainer">
            <div class="control">
                <label for="txtUsername">User Name</label>
                <asp:TextBox ID="txtUsername" runat="server" />
            </div>
            <div class="control">
                <label for="txtPassword">Password</label>
                <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" />
            </div>

            <asp:Button ID="Submit" Text="Login" OnClick="Submit_Click" runat="server" />
        </div>
        <asp:Label ID="ResultMessage" runat="server" />
    </form>
</body>
</html>
