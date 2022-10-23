<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateLot.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmCreateLot" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Create Lot</title>
</head>
<body>
    <form id="frmCreateLot" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Create Lot / VAT" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:Label ID="lblMilkKitID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="Milk Collection Kit ID:"></asp:Label>
        <asp:TextBox ID="txtMilkKitID" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="9" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 254px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCancel_Click" Style="z-index: 106; left: 45px; position: absolute; top: 459px" Text="Cancel" Width="88px" UseSubmitBehavior="False" TabIndex="5" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 501px; width: 673px;"></asp:Label>
        <asp:Button ID="btnSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSubmit_Click" Style="z-index: 106; left: 195px; position: absolute; top: 461px; width: 112px;" Text="Submit" UseSubmitBehavior="False" TabIndex="4" Visible="False" />
        <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 313px; position: absolute; top: 213px; width: 65px;" Text="Search" TabIndex="4" />
        <asp:ListBox ID="lstMilkKits" runat="server" style="z-index: 1; left: 45px; top: 244px; position: absolute; height: 208px; width: 260px"></asp:ListBox>
        <asp:Label ID="lblTotalVolumeTitle" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 316px; position: absolute; top: 248px; width: 90px; margin-top: 0px;" Text="Total Ounces: "></asp:Label>
        <asp:Label ID="lblTotalVolume" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 413px; position: absolute; top: 248px; width: 189px; margin-top: 0px;" Text="0"></asp:Label>
    </form>
</body>
</html>
