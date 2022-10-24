<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLotTransfer.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmLotTransfer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Lot Transfer / Pouch Labels</title>
    <style>
        #frmLotTransfer{
            width: 1050px;
            margin-left: 45px;
        }
    </style>
</head>
<body>
<asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute; top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
<asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; top: 143px" Text="Lot Transfer / Pouch Labels" Width="334px"></asp:Label>
<asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108; left: 22px; position: absolute; top: 4px" />
    <form id="LotTransfer" runat="server">
        <div id="TransferControls">
            <asp:Label ID="lblLotNumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="Lot Number:"></asp:Label>
            <asp:TextBox ID="txtLotNumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="9" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 299px; bottom: 419px;" TabIndex="1"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 361px; position: absolute; top: 213px; width: 65px; height: 26px;" Text="Search" TabIndex="4" />

            <asp:Label ID="lblGtin" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 253px; width: 103px; right: 2849px; height: 16px;" Text="Product:"></asp:Label>
            <asp:DropDownList ID="ddlGtin" runat="server" style="z-index: 4; left: 45px; top: 273px; position: absolute; width: 359px" TabIndex="7" />

            <asp:Label ID="lblTotalCases" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; position: absolute; top: 310px; left: 45px; width: 103px;" Text="Total Cases:"></asp:Label>
            <asp:TextBox ID="txtTotalCases" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="3" Style="z-index: 2; position: absolute; top: 329px; left: 45px; width: 78px;" TabIndex="1" Enabled="False"></asp:TextBox>
            <asp:Label ID="lblSamplePouches" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; position: absolute; top: 310px; left: 165px; width: 120px;" Text="Sample Pouches:"></asp:Label>
            <asp:TextBox ID="txtSamplePouches" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="2" Style="z-index: 2; position: absolute; top: 329px; left: 165px;  width: 78px;" TabIndex="2" Enabled="False"></asp:TextBox>
            <asp:Label ID="lblStartingPouchNumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; position: absolute; top: 310px; left: 285px; width: 110px;" Text="Starting Pouch:" Visible="False"></asp:Label>
            <asp:TextBox ID="txtStartingPouchNumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="5" Style="z-index: 2;  position: absolute; top: 329px; left: 285px; width: 78px;" TabIndex="2" Visible="False">1</asp:TextBox>
            <asp:Label ID="lblQuantity" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; position: absolute; top: 310px; left: 405px; width: 140px;" Text="Pouch Quantity:" Visible="False"></asp:Label>
            <asp:TextBox ID="txtQuantity" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="5" Style="z-index: 2; position: absolute; top: 329px; left: 405px; width: 78px;" TabIndex="2" Visible="False">1</asp:TextBox>
         

            <asp:Label ID="lblExpDateTitle" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 370px; width: 108px;" Text="Expiration Date:" Visible="False"></asp:Label>
            <asp:Label ID="lblExpDate" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 392px; width: 108px;" Text="YYMMDD" ForeColor="#3399FF" Visible="False"></asp:Label>
        
            <asp:Button ID="btnBack" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnBack_Click" Style="z-index: 106; left: 45px; position: absolute; top: 427px" Text="Back" Width="88px" UseSubmitBehavior="False" />
            <asp:Button ID="btnSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSubmit_Click" Style="z-index: 106; left: 164px; position: absolute; top: 427px; width: 112px;" Text="Transfer" UseSubmitBehavior="False" TabIndex="4" Visible="False" />
            <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px; position: absolute; top: 467px; width: 673px;"></asp:Label>

        </div>
        <div id="labelControls">
            <asp:Label ID="lblSelectPrinter" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; POSITION: absolute; TOP: 370px; LEFT: 600px;" Text="Select Printer" Visible="False" Width="121px"></asp:Label>
            <asp:DropDownList ID="ddlPrinters" runat="server" style="z-index: 4; left: 600px; top: 392px; position: absolute; width: 392px" TabIndex="7" Visible="False"></asp:DropDownList>
            <asp:Button ID="btnNext" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnNext_Click" Style="z-index: 106;  position: absolute; top: 427px; left: 600px; width: 112px; height: 26px;" Text="Next Lot" UseSubmitBehavior="False" TabIndex="4" Visible="False" />
            <asp:Button ID="btnPrint" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnPrint_Click" style="Z-INDEX: 103; POSITION: absolute; TOP: 427px; LEFT: 847px; width: 112px;" TabIndex="6" Text="Print Label(s)" Visible="False" UseSubmitBehavior="False" />
        </div>
    </form>
</body>
</html>
