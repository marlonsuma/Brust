<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReprintLabels.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmReprintLabels" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Print/Reprint Labels</title>
</head>
<body>
    <form id="frmReprintLabels" runat="server">
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Print/Reprint Labels" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
                <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCancel_Click" Style="z-index: 106; left: 45px; position: absolute; top: 370px" Text="Cancel" Width="88px" UseSubmitBehavior="False" TabIndex="5" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 428px; width: 732px;"></asp:Label>
                <asp:DropDownList ID="ddlPrinters" runat="server" style="z-index: 4; left: 418px; top: 335px; position: absolute; width: 359px" TabIndex="7" Visible="False">
        </asp:DropDownList>
            <asp:Button ID="btnPrint" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnPrint_Click" style="Z-INDEX: 103; LEFT: 604px; POSITION: absolute; TOP: 370px" TabIndex="6" Text="Print Label" Visible="False" Width="173px" UseSubmitBehavior="False" />
            <asp:Label ID="lblSelectPrinter" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 418px; POSITION: absolute; TOP: 316px; height: 18px;" Text="Select Printer" Visible="False" Width="121px"></asp:Label>
        <asp:Button ID="btnNext" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnNext_Click" Style="z-index: 106; left: 418px; position: absolute; top: 370px; width: 112px; height: 26px;" Text="Next Barcode" UseSubmitBehavior="False" TabIndex="4" Visible="False" />
        <asp:Label ID="lblLabelInfo" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 418px; position: absolute; top: 196px; width: 235px; bottom: 445px;" Text="Label Information:" Font-Underline="True" Visible="False"></asp:Label>
        <asp:Label ID="lblBarcodeTitle" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 418px; position: absolute; top: 231px; width: 84px; " Text="Barcode:" Visible="False"></asp:Label>
        <asp:Label ID="lblPrintedBarcode" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 495px; position: absolute; top: 231px; width: 388px;" Text="[BARCODE VALUE]" ForeColor="#3399FF" Visible="False"></asp:Label>
        <asp:Label ID="lblEnterBarcode" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="Enter Barcode Value:"></asp:Label>
        <asp:TextBox ID="txtBarcode" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 254px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 313px; position: absolute; top: 213px; width: 65px;" Text="Submit" TabIndex="4" />
    </form>
</body>
</html>
