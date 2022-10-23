<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReturnCase.aspx.cs" Inherits="NiQ_Donor_Tracking_System.ReturnCase" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmReturnCase" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Create &amp; Ship Case" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:TextBox ID="txtCaseBarCode" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 2; left: 45px; position: absolute; top: 235px; width: 430px; bottom: 183px;" TabIndex="3" Enabled="False"></asp:TextBox>
        <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCancel_Click" Style="z-index: 106; left: 47px; position: absolute; top: 509px" Text="Cancel" Width="88px" UseSubmitBehavior="False" TabIndex="5" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 592px; width: 673px;"></asp:Label>
        <asp:Button ID="btnShip" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnShip_Click" Style="z-index: 106; left: 272px; position: absolute; top: 508px; width: 149px;" Text="Create &amp; Ship" UseSubmitBehavior="False" TabIndex="7" Enabled="False" EnableViewState="False" />
        <p>
        <asp:Label ID="lblCaseBarCOde" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 208px; width: 100px;" Text="Case Barcode:"></asp:Label>
        </p>
        <p>
            &nbsp;</p>
        <p>
        <asp:Button ID="btnBack" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnBack_Click" Style="z-index: 106; left: 492px; position: absolute; top: 232px" Text="Search" Width="88px" UseSubmitBehavior="False" TabIndex="9" />
        </p>
    </form>
</body>
</html>
