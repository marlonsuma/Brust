<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="__TestMail.aspx.cs" Inherits="NiQ_Donor_Tracking_System.__TestMail" %>

<head runat="server">
    <title>Test Mailer</title>
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

        .control input, #Submit {
            width: 250px;
            margin: 5px 0;
        }

    </style>
</head>
<body>
<div id="pageHeader">
    <img id="logo" src="/images/Ni-Q Logo.png" />
    <h1>Donor Tracking System</h1>
    <h3>EMAIL Send Test</h3>
</div>
    <form id="LoginForm" runat="server">
        <div class="controlContainer">
            <div class="control">
                <label for="toAddress">To (email):</label>
                <asp:TextBox ID="toAddress" runat="server" />
            </div>
            <div class="control">
                <label for="subject">Subject:</label>
                <asp:TextBox ID="subject" runat="server" />
            </div>
              <div class="control">
                <label for="body">Body:</label>
                <asp:TextBox TextMode="MultiLine" ID="body" runat="server" Width="500" Height="200"/>
            </div>
            
            <asp:Button ID="Submit" Text="Send >>" OnClick="Submit_Click" runat="server" />
        </div>
        <asp:Label ID="ResultMessage" runat="server"  />
    </form>
</body>
