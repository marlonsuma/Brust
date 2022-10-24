<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmQuarantineMilk.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmQuarantineMilk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Quarantine Milk Bag</title>
</head>
<body>
    <form id="frmQuarantineMilk" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Quarantine Milk Bag" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:Label ID="lblMilkKitID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="Milk Collection Kit ID:"></asp:Label>
        <asp:TextBox ID="txtMilkKitID" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="9" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 254px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCancel_Click" Style="z-index: 106; left: 45px; position: absolute; top: 374px" Text="Cancel" Width="88px" UseSubmitBehavior="False" TabIndex="9" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 409px; width: 673px;"></asp:Label>
        <asp:Button ID="btnSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSubmit_Click" Style="z-index: 106; left: 195px; position: absolute; top: 374px; width: 112px;" Text="Submit" UseSubmitBehavior="False" TabIndex="5" Visible="False" />
        <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 313px; position: absolute; top: 213px; width: 65px;" Text="Search" TabIndex="2" />
            <asp:Button ID="btnPrint" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnPrint_Click" style="Z-INDEX: 103; LEFT: 540px; POSITION: absolute; TOP: 374px" TabIndex="7" Text="Print Label" Visible="False" Width="173px" UseSubmitBehavior="False" />
            <asp:Label ID="lblSelectPrinter" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 47px; POSITION: absolute; TOP: 305px; height: 18px;" Text="Select Printer" Visible="False" Width="121px"></asp:Label>
        <asp:Button ID="btnNext" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnNext_Click" Style="z-index: 106; left: 354px; position: absolute; top: 374px; width: 112px;" Text="Next Kit" UseSubmitBehavior="False" TabIndex="8" Visible="False" />
        <asp:Label ID="lblVolume" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 5; left: 45px; position: absolute; top: 251px; width: 148px;" Text="Volume (Ounces):" Visible="False"></asp:Label>
        <asp:TextBox ID="txtVolume" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="10" Style="z-index: 6; left: 45px; position: absolute; top: 271px; width: 254px; bottom: 364px;" TabIndex="3" Visible="False" Enabled="False"></asp:TextBox>
        <asp:DropDownList ID="ddlPrinters" runat="server" AutoPostBack="True" Font-Names="Arial" Font-Size="10pt" style="z-index: 1; left: 46px; top: 324px; position: absolute; height: 27px; width: 359px" Visible="False">
        </asp:DropDownList>
    </form>
</body>
</html>
