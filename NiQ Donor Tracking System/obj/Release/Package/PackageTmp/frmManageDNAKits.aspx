<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmManageDNAKits.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmManageDNAKits" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Manage DNA Kits</title>
</head>
<body>
    <form id="frmManageDNAKits" runat="server">
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Manage DNA Kits" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:Button ID="btnBack" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnBack_Click" Style="z-index: 106; left: 45px; position: absolute; top: 500px" Text="Back" Width="88px" UseSubmitBehavior="False" TabIndex="8" />
        <asp:Label ID="lblDNAKit" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="DNA Kit ID:"></asp:Label>
        <asp:TextBox ID="txtDNAKit" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="9" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 299px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 540px; width: 673px;"></asp:Label>
        <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 361px; position: absolute; top: 213px; width: 65px;" Text="Search" TabIndex="2" />
        <asp:Label ID="lblMilkCollectionKitID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 254px; width: 235px;" Text="Milk Collection Kit ID:"></asp:Label>
        <asp:TextBox ID="txtMilkKitID" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="9" Style="z-index: 2; left: 45px; position: absolute; top: 274px; width: 186px; bottom: 361px;" TabIndex="3" Enabled="False"></asp:TextBox>
        <asp:Label ID="lblTrackingNumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 5; left: 45px; position: absolute; top: 372px; width: 235px;" Text="Tracking/Order Number:"></asp:Label>
        <asp:TextBox ID="txtTrackingNumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 6; left: 45px; position: absolute; top: 392px; width: 269px; bottom: 243px;" TabIndex="5" Enabled="False"></asp:TextBox>
        <asp:Label ID="lblShippingService" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 3; left: 45px; position: absolute; top: 310px; width: 235px;" Text="Shipping Service:"></asp:Label>
        <asp:DropDownList ID="ddlShipping" runat="server" style="z-index: 4; left: 45px; top: 330px; position: absolute; width: 186px" TabIndex="4" Enabled="False">
            <asp:ListItem>USPS</asp:ListItem>
            <asp:ListItem>UPS</asp:ListItem>
            <asp:ListItem>FedEx</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSubmit_Click" Style="z-index: 106; left: 195px; position: absolute; top: 500px; width: 112px;" Text="Submit" UseSubmitBehavior="False" TabIndex="7" Visible="False" />
        <asp:CheckBox ID="chkActive" runat="server" Enabled="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" style="z-index: 1; left: 287px; top: 192px; position: absolute" Text="Active" TabIndex="6" />
    
        <asp:CheckBox ID="chkMicrobial" runat="server" Enabled="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" style="z-index: 1; left: 45px; top: 422px; position: absolute" Text="Microbial Test Finished" TabIndex="6" />
        <asp:CheckBox ID="chkGenetic" runat="server" Enabled="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" style="z-index: 1; left: 45px; top: 442px; position: absolute" Text="Genetic Test Finished" TabIndex="6" />
        <asp:CheckBox ID="chkToxicology" runat="server" Enabled="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" style="z-index: 1; left: 45px; top: 462px; position: absolute" Text="Toxicology Test Finished" TabIndex="6" />

    </form>
</body>
</html>
