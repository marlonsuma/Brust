<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="ApiUsers.aspx.cs" Inherits="NiQ_Donor_Tracking_System.ApiUsers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Create API User</title>
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
        #CreateApiUser {
            width: 1050px;
            margin: 0 45px;
        }
        #Submit {
            width: 90px;
            float: right;
        }
        #Cancel {
            width: 90px;
            float: left;
        }
        .control {
            margin: 20px 0;
        }
        
        .control label {
            display: block;
            font-weight: bold;
        }

        .control input {
            width: 250px;
            margin: 5px 0;
        }

        .buttons {
            width: 250px;
        }
    </style>
</head>
<body>
<div id="pageHeader">
    <img id="logo" src="/images/Ni-Q Logo.png" />
    <h1>Donor Tracking System</h1>
    <h3>Create API User</h3>
</div>
<form id="CreateApiUser" runat="server">
    <div>
        <div class="control">
            <label for="UserName">User Name</label>
            <asp:TextBox ID="UserName" runat="server" />
        </div>
        <div class="control">
            <label for="Password">Password</label>
            <asp:TextBox ID="Password" runat="server" TextMode="Password" />
        </div>
        <asp:Label ID="ResultMessage" runat="server" />
        <div class="control buttons">
            <asp:Button ID="Cancel" Text="Cancel" OnClick="Cancel_OnClick" runat="server" />
            <asp:Button ID="Submit" Text="Submit" runat="server" OnClick="Submit_Click" />

        </div>
    </div>

    
</form>
</body>
</html>
