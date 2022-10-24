<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReceiveBloodKit.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmReceiveBloodKit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Receive Blood Kit</title>
</head>
<body>
    <form id="frmReceiveBloodKit" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Receive Blood Kit" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:Label ID="lblDIN" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="ISBT 128 DIN:"></asp:Label>
        <asp:TextBox ID="txtDIN" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="16" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 254px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCancel_Click" Style="z-index: 106; left: 45px; position: absolute; top: 370px" Text="Cancel" Width="88px" UseSubmitBehavior="False" TabIndex="5" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 418px; width: 673px;"></asp:Label>
        <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 265px; width: 136px;" Text="Status:"></asp:Label>
        <asp:RadioButton ID="radPass" runat="server" Checked="True" Font-Names="Arial" Font-Size="10pt" GroupName="grpStatus" style="z-index: 1; left: 46px; top: 285px; position: absolute" Text="Pass" Enabled="False" />
        <p>
        <asp:RadioButton ID="radFail" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpStatus" style="z-index: 1; left: 111px; top: 285px; position: absolute" Text="Fail" Enabled="False" />
        <asp:Button ID="btnSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSubmit_Click" Style="z-index: 106; left: 195px; position: absolute; top: 370px; width: 112px;" Text="Submit" UseSubmitBehavior="False" TabIndex="4" Visible="False" />
        <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 313px; position: absolute; top: 213px; width: 65px;" Text="Search" TabIndex="4" />
            </p>
    </form>
</body>
</html>
