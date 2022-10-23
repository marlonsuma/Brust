<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmManagePrinters.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmManagePrinters" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Manage Printers</title>
</head>
<body>
    <form id="frmManagePrinters" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Manage Printers" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />

                <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnCancel_Click" style="Z-INDEX: 103; LEFT: 45px; POSITION: absolute; TOP: 448px; width: 119px;" Text="Back" />
        <asp:ListBox ID="lstExistingPrinters" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstExistingPrinters_SelectedIndexChanged" style="Z-INDEX: 105; LEFT: 45px; POSITION: absolute; TOP: 208px; height: 233px; width: 237px;"></asp:ListBox>
        <asp:Label ID="lblExistingPrinters" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 45px; POSITION: absolute; TOP: 186px" Text="Existing Printers" Width="172px"></asp:Label>
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" style="Z-INDEX: 101; LEFT: 45px; POSITION: absolute; top: 484px; width: 512px;"></asp:Label>
        <asp:Label ID="lblPrinterName" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 300px; POSITION: absolute; TOP: 186px" Text="Printer Name" Width="121px" height="16px"></asp:Label>
        <asp:Label ID="lblIP" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 300px; POSITION: absolute; TOP: 244px; width: 239px;" Text="IP Address" height="16px"></asp:Label>
        <asp:TextBox ID="txtPrinterName" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="10pt" MaxLength="50" style="Z-INDEX: 103; LEFT: 300px; POSITION: absolute; TOP: 208px; width: 236px;"></asp:TextBox>
        <asp:TextBox ID="txtIP" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="10pt" MaxLength="15" style="Z-INDEX: 103; LEFT: 300px; POSITION: absolute; TOP: 264px" Width="236px"></asp:TextBox>
        <asp:Button ID="btnSave" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnSave_Click" style="Z-INDEX: 103; LEFT: 409px; POSITION: absolute; TOP: 448px; width: 135px;" Text="Save" />
        <asp:Button ID="btnRemove" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnRemove_Click" style="Z-INDEX: 103; LEFT: 409px; POSITION: absolute; TOP: 292px; width: 135px;" Text="Remove Printer" />

    </form>
</body>
</html>
