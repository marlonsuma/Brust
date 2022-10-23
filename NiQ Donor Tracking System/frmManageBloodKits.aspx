<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmManageBloodKits.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmManageBloodKits" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Manage Blood Kits</title>
</head>
<body>
    <form id="frmManageBloodKits" runat="server">
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Manage Blood Kits" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:Button ID="btnBack" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnBack_Click" Style="z-index: 106; left: 45px; position: absolute; top: 490px" Text="Back" Width="88px" UseSubmitBehavior="False" TabIndex="10" />
        <asp:Label ID="lblLotNumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="ISBT 128 DIN:"></asp:Label>
        <asp:TextBox ID="txtDIN" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="16" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 299px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 528px; width: 673px;"></asp:Label>
        <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 361px; position: absolute; top: 213px; width: 65px;" Text="Search" TabIndex="2" />
        <asp:Label ID="lblDonorNumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 257px; width: 235px;" Text="Donor Number:"></asp:Label>
        <asp:TextBox ID="txtDonorNumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 2; left: 45px; position: absolute; top: 277px; width: 186px; bottom: 358px;" TabIndex="3" Enabled="False"></asp:TextBox>
        <asp:Label ID="lblTrackingNumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 5; left: 45px; position: absolute; top: 372px; width: 235px;" Text="Tracking/Order Number:"></asp:Label>
        <asp:TextBox ID="txtTrackingNumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 6; left: 45px; position: absolute; top: 392px; width: 269px; bottom: 243px;" TabIndex="5" Enabled="False"></asp:TextBox>
        <asp:Label ID="lblShippingService" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 3; left: 45px; position: absolute; top: 313px; width: 235px;" Text="Shipping Service:"></asp:Label>
        <asp:DropDownList ID="ddlShipping" runat="server" style="z-index: 4; left: 45px; top: 334px; position: absolute; width: 186px" TabIndex="4" Enabled="False">
            <asp:ListItem>QUEST</asp:ListItem>
            <asp:ListItem>USPS</asp:ListItem>
            <asp:ListItem>UPS</asp:ListItem>
            <asp:ListItem>FedEx</asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 431px; width: 136px;" Text="Status:"></asp:Label>
        <asp:RadioButton ID="radPass" runat="server" Checked="True" Font-Names="Arial" Font-Size="10pt" GroupName="grpStatus" style="z-index: 1; left: 46px; top: 451px; position: absolute" Text="Pass" Enabled="False" TabIndex="6" />
        <asp:RadioButton ID="radFail" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpStatus" style="z-index: 1; left: 111px; top: 451px; position: absolute" Text="Fail" Enabled="False" TabIndex="7" />
        <asp:Button ID="btnSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSubmit_Click" Style="z-index: 106; left: 195px; position: absolute; top: 490px; width: 112px;" Text="Submit" UseSubmitBehavior="False" TabIndex="9" Visible="False" />
        <asp:CheckBox ID="chkActive" runat="server" Enabled="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" style="z-index: 1; left: 287px; top: 192px; position: absolute" Text="Active" TabIndex="8" />
    </form>
</body>
</html>
