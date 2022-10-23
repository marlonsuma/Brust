<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAdminMenu.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmAdminMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Administrator Menu</title>
</head>
<body>
    <form id="frmAdminMenu" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Administrator Menu" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
                <asp:Button ID="btnTransactions" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnTransactions_Click"
            Style="z-index: 106; left: 47px; position: absolute; top: 379px; width: 225px;" Text="Transactions" />
        <asp:Button ID="btnManageBloodKits" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnManageBloodKits_Click"
            Style="z-index: 106; left: 47px; position: absolute; top: 191px; width: 227px;" Text="Manage Blood Kits" />
        <asp:Button ID="btnManageMilkKits" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnManageMilkKits_Click"
            Style="z-index: 106; left: 47px; position: absolute; top: 229px; width: 227px;" Text="Manage Milk Collection Kits" />
        <%--<asp:Button ID="btnManageDNAKits" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnManageDNAKits_Click"
            Style="z-index: 106; left: 47px; position: absolute; top: 267px; width: 227px;" Text="Manage DNA Kits" />--%>
        <asp:Button ID="btnManagePallets" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnManagePallets_Click"
            Style="z-index: 106; left: 47px; position: absolute; top: 267px; width: 227px; right: 1047px;" Text="Manage Pallets" />
        <asp:Button ID="btnManageLots" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnManageLots_Click"
            Style="z-index: 106; left: 47px; position: absolute; top: 304px; width: 227px; right: 1047px;" Text="Manage Lots" />
        <asp:Button ID="btnManageCases" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnManageCases_Click"
            Style="z-index: 106; left: 47px; position: absolute; top: 341px; width: 227px;" Text="Manage Cases" />
        
        <asp:Button ID="btnManageUsers" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnManageUsers_Click"
                    Style="z-index: 106; left: 288px; position: absolute; top: 191px; width: 227px;" Text="Manage Users" />
        <asp:Button ID="btnManageApiUser" runat="server" Font-Names="Arial" Font-Size="10pt"
                    Style="z-index: 106; left: 288px; position: absolute; top: 229px; width: 227px;" Text="Create Api User" OnClick="btnManageApiUser_Click" />
        <asp:Button ID="btnManagePrinters" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnManagePrinters_Click"
                    Style="z-index: 106; left: 288px; position: absolute; top: 267px; width: 227px;" Text="Manage Printers" />
        <asp:Button ID="btnManageLocations" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnManageLocations_Click"
                    Style="z-index: 106; left: 288px; position: absolute; top:304px; width: 227px;" Text="Manage Locations" />
        <asp:Button ID="btnManageDonors" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnManageDonors_Click" 
                    Style="z-index: 106; left: 288px; position:absolute; top: 341px; width: 227px;" Text="Manage Donors" />
         <asp:Button ID="Button1" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnManageDonors_Click" 
                    Style="z-index: 106; left: 288px; position:absolute; top: 341px; width: 227px;" Text="Manage Donors" />
    </form>
</body>
</html>
