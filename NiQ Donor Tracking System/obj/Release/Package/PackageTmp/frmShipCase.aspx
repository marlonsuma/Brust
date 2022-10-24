<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmShipCase.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmShipCase" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Create & Ship Case</title>
</head>
<body>
    <form id="frmShipCase" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Create &amp; Ship Case" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:Label ID="lblLots" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 272px; position: absolute; top: 195px; width: 122px;" Text="Lot Number:"></asp:Label>
        <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCancel_Click" Style="z-index: 106; left: 45px; position: absolute; top: 548px" Text="Cancel" Width="88px" UseSubmitBehavior="False" TabIndex="5" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 592px; width: 673px;"></asp:Label>
        <asp:Label ID="lblTrackingNumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 5; left: 272px; position: absolute; top: 310px; width: 235px;" Text="Tracking Number:"></asp:Label>
        <asp:TextBox ID="txtTrackingNumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 6; left: 272px; position: absolute; top: 333px; width: 269px; bottom: 322px;" TabIndex="4"></asp:TextBox>
        <asp:Label ID="lblShippingService" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 3; left: 272px; position: absolute; top: 254px; width: 235px;" Text="Shipping Service:"></asp:Label>
        <asp:DropDownList ID="ddlShipping" runat="server" style="z-index: 4; left: 272px; top: 275px; position: absolute; width: 186px" TabIndex="3">
            <asp:ListItem>USPS</asp:ListItem>
            <asp:ListItem>UPS</asp:ListItem>
            <asp:ListItem>FedEx</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnShip" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnShip_Click" Style="z-index: 106; left: 272px; position: absolute; top: 508px; width: 149px;" Text="Create &amp; Ship" UseSubmitBehavior="False" TabIndex="7" />
        <asp:Label ID="lblPONumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 5; left: 272px; position: absolute; top: 373px; width: 235px;" Text="PO Number:"></asp:Label>
        <asp:TextBox ID="txtPONumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 6; left: 272px; position: absolute; top: 393px; width: 269px; " TabIndex="5"></asp:TextBox>
        <asp:Label ID="lblLocations" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 195px; width: 122px;" Text="Ship to Location:"></asp:Label>
                    <asp:TreeView ID="treeLocations" SelectedNodeStyle-ForeColor="Red" runat="server" Style="position: absolute; overflow:auto; top: 217px; left: 45px; height: 320px; width: 208px; " BorderStyle="Solid" BorderWidth="1px" ExpandDepth="0" Font-Bold="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Black" ShowLines="True" OnTreeNodePopulate="treeLocations_TreeNodePopulate">
                <SelectedNodeStyle ForeColor="#3399FF" Font-Bold="True" />
            </asp:TreeView>
        <asp:DropDownList ID="ddlLotNumber" runat="server" style="z-index: 4; left: 272px; top: 217px; position: absolute; width: 186px" TabIndex="2" AutoPostBack="True">
        </asp:DropDownList>
        <asp:Button ID="btnNext" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnNext_Click" Style="z-index: 106; left: 576px; position: absolute; top: 422px; width: 112px;" Text="Next Case" UseSubmitBehavior="False" TabIndex="9" Visible="False" />
        <asp:Button ID="btnPrint" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnPrint_Click" Style="z-index: 106; left: 777px; position: absolute; top: 422px; width: 158px;" Text="Print Label" UseSubmitBehavior="False" TabIndex="10" Visible="False" />
        <asp:Label ID="lblCaseIDTitle" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 576px; position: absolute; top: 334px; width: 62px;" Text="Case ID:" Visible="False"></asp:Label>
        <asp:Label ID="lblCaseID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 652px; position: absolute; top: 334px; width: 223px;" Text="CAXXXXXXX" ForeColor="#3399FF" Visible="False"></asp:Label>
        <asp:Label ID="lblLabelInfo" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 576px; position: absolute; top: 310px; width: 235px;" Text="Label Information:" Font-Underline="True" Visible="False"></asp:Label>
            <asp:Label ID="lblSelectPrinter" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 576px; POSITION: absolute; TOP: 373px;" Text="Select Printer" Visible="False" Width="121px"></asp:Label>
        <asp:DropDownList ID="ddlPrinters" runat="server" AutoPostBack="True" style="z-index: 1; left: 577px; top: 393px; position: absolute; height: 20px; width: 361px" Visible="False" TabIndex="8">
        </asp:DropDownList>
        <asp:Label ID="lblNumberofCases" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 5; left: 272px; position: absolute; top: 438px; width: 235px;" Text="Number of Cases:"></asp:Label>
        <asp:TextBox ID="txtNumberofCases" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="3" Style="z-index: 6; left: 272px; position: absolute; top: 461px; width: 75px; " TabIndex="6">1</asp:TextBox>
    </form>
</body>
</html>
