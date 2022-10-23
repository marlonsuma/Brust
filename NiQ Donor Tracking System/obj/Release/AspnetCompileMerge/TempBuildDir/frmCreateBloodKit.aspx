<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateBloodKit.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmCreateBloodKit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Create Blood Kit</title>
</head>
<body>
    <form id="frmCreateBloodKit" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Create Blood Kit" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:Label ID="lblDonorNumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="Donor Number:"></asp:Label>
        <asp:TextBox ID="txtDonorNumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 186px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCancel_Click" Style="z-index: 106; left: 45px; position: absolute; top: 430px" Text="Cancel" Width="88px" UseSubmitBehavior="False" TabIndex="5" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 480px; width: 673px;"></asp:Label>
        <asp:Label ID="lblTrackingNumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 5; left: 45px; position: absolute; top: 316px; width: 235px;" Text="Tracking/Order Number:"></asp:Label>
        <asp:TextBox ID="txtTrackingNumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 6; left: 45px; position: absolute; top: 335px; width: 269px; bottom: 300px;" TabIndex="3"></asp:TextBox>
        
        <asp:Label ID="lblShippingService" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 3; left: 45px; position: absolute; top: 253px; width: 235px;" Text="Shipping Service:"></asp:Label>
        <asp:DropDownList ID="ddlShipping" runat="server" style="z-index: 4; left: 45px; top: 274px; position: absolute; width: 186px" TabIndex="2">
            <asp:ListItem>QUEST</asp:ListItem>
            <asp:ListItem>USPS</asp:ListItem>
            <asp:ListItem>UPS</asp:ListItem>
            <asp:ListItem>FedEx</asp:ListItem>
        </asp:DropDownList>
                <asp:DropDownList ID="ddlPrinters" runat="server" style="z-index: 4; left: 354px; top: 335px; position: absolute; width: 359px" TabIndex="7" Visible="False" AutoPostBack="True">
        </asp:DropDownList>
        <asp:Button ID="btnCreate" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCreate_Click" Style="z-index: 106; left: 210px; position: absolute; top: 430px; width: 112px;" Text="Create" UseSubmitBehavior="False" TabIndex="4" />
            <asp:Button ID="btnPrint" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnPrint_Click" style="Z-INDEX: 103; LEFT: 540px; POSITION: absolute; TOP: 370px" TabIndex="6" Text="Print Label" Visible="False" Width="173px" UseSubmitBehavior="False" />
            <asp:Label ID="lblSelectPrinter" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 353px; POSITION: absolute; TOP: 316px; height: 18px;" Text="Select Printer" Visible="False" Width="121px"></asp:Label>
        <asp:Button ID="btnNext" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnNext_Click" Style="z-index: 106; left: 354px; position: absolute; top: 370px; width: 112px;" Text="Next Kit" UseSubmitBehavior="False" TabIndex="4" Visible="False" />
        <asp:Label ID="lblLabelInfo" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 353px; position: absolute; top: 196px; width: 235px;" Text="Label Information:" Font-Underline="True" Visible="False"></asp:Label>
        <asp:Label ID="lblISBTDINTitle" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 353px; position: absolute; top: 231px; width: 103px;" Text="ISBT 128 DIN:" Visible="False"></asp:Label>
        <asp:Label ID="lblISBTDIN" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 469px; position: absolute; top: 231px; width: 223px;" Text="=W4237YYSSSSSS00" ForeColor="#3399FF" Visible="False"></asp:Label>
      
       
    </form>
</body>
</html>
