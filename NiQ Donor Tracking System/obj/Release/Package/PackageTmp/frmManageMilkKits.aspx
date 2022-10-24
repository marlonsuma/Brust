<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmManageMilkKits.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmManageMilkKits" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Manage Milk Collection Kits</title>
</head>
<body>
    <form id="frmManageMilkKits" runat="server">
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Manage Milk Collection Kits" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:Button ID="btnBack" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnBack_Click" Style="z-index: 106; left: 45px; position: absolute; top: 579px" Text="Back" Width="88px" UseSubmitBehavior="False" TabIndex="17" />
        <asp:Label ID="lblMilkKitID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="Milk Collection Kit ID:"></asp:Label>
        <asp:TextBox ID="txtMilkKitID" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="9" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 299px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 621px; width: 673px;"></asp:Label>
        <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 361px; position: absolute; top: 213px; width: 65px;" Text="Search" TabIndex="2" />
        <asp:Label ID="lblDonorID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 252px; width: 84px;" Text="Donor ID:"></asp:Label>
        <asp:TextBox ID="txtDonorID" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 2; left: 45px; position: absolute; top: 270px; width: 186px; bottom: 399px;" TabIndex="3" Enabled="False"></asp:TextBox>
        <asp:Label ID="lblTrackingNumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 5; left: 45px; position: absolute; top: 357px; width: 235px;" Text="Tracking Number:"></asp:Label>
        <asp:TextBox ID="txtTrackingNumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 6; left: 45px; position: absolute; top: 376px; width: 269px; bottom: 316px;" TabIndex="5" Enabled="False"></asp:TextBox>
        <asp:Label ID="lblShippingService" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 3; left: 45px; position: absolute; top: 303px; width: 235px;" Text="Shipping Service:"></asp:Label>
        <asp:DropDownList ID="ddlShipping" runat="server" style="z-index: 4; left: 45px; top: 322px; position: absolute; width: 186px" TabIndex="4" Enabled="False">
            <asp:ListItem>USPS</asp:ListItem>
            <asp:ListItem>UPS</asp:ListItem>
            <asp:ListItem>FedEx</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSubmit_Click" Style="z-index: 106; left: 361px; position: absolute; top: 579px; width: 112px;" Text="Submit" UseSubmitBehavior="False" TabIndex="16" Visible="False" />
        <asp:CheckBox ID="chkActive" runat="server" Enabled="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" style="z-index: 1; left: 287px; top: 192px; position: absolute" Text="Active" TabIndex="15" />
        <asp:Label ID="lblLotNumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 5; left: 45px; position: absolute; top: 517px; width: 235px;" Text="Lot Number:"></asp:Label>
        <asp:TextBox ID="txtLotNumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="9" Style="z-index: 6; left: 45px; position: absolute; top: 536px; width: 186px; bottom: 152px; " TabIndex="8" Enabled="False"></asp:TextBox>
        <asp:Label ID="lblVolume" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 5; left: 45px; position: absolute; top: 409px; width: 235px;" Text="Volume (Ounces):"></asp:Label>
        <asp:TextBox ID="txtVolume" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="10" Style="z-index: 6; left: 45px; position: absolute; top: 429px; width: 186px; bottom: 259px; right: 1273px;" TabIndex="6" Enabled="False"></asp:TextBox>
        <asp:Label ID="lblPallet" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 5; left: 45px; position: absolute; top: 463px; width: 235px;" Text="Pallet Number:"></asp:Label>
        <asp:TextBox ID="txtPallet" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="10" Style="z-index: 6; left: 45px; position: absolute; top: 482px; width: 186px; bottom: 206px; right: 1273px;" TabIndex="7" Enabled="False"></asp:TextBox>
        <asp:Label ID="lblDNATest" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 361px; position: absolute; top: 253px; width: 136px;" Text="DNA Test:"></asp:Label>
        <asp:RadioButton ID="radDNAPass" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpDNA" style="z-index: 1; left: 361px; top: 273px; position: absolute" Text="Pass" Enabled="False" AutoPostBack="True" TabIndex="9"/>
        <asp:RadioButton ID="radDNAFail" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpDNA" style="z-index: 1; left: 438px; top: 273px; position: absolute; height: 20px;" Text="Fail" Enabled="False" AutoPostBack="True" TabIndex="10"/>
        <asp:Label ID="lblDrugAlcoholTest" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 361px; position: absolute; top: 301px; width: 183px;" Text="Drug and Alcohol Test:"></asp:Label>
        <asp:RadioButton ID="radDrugPass" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpDrug" style="z-index: 1; left: 361px; top: 320px; position: absolute; right: 1101px;" Text="Pass" Enabled="False" AutoPostBack="True" TabIndex="11"/>
        <asp:RadioButton ID="radDrugFail" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpDrug" style="z-index: 1; left: 438px; top: 320px; position: absolute; height: 20px;" Text="Fail" Enabled="False" AutoPostBack="True" TabIndex="12"/>
        <asp:Label ID="lblMicrobialTest" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 361px; position: absolute; top: 350px; width: 136px;" Text="Microbial Test:"></asp:Label>
        <asp:RadioButton ID="radMicrobialPass" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpMicrobial" style="z-index: 1; left: 361px; top: 369px; position: absolute" Text="Pass" Enabled="False" AutoPostBack="True" TabIndex="13"/>
        <asp:RadioButton ID="radMicrobialFail" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpMicrobial" style="z-index: 1; left: 438px; top: 369px; position: absolute" Text="Fail" Enabled="False" AutoPostBack="True" TabIndex="14"/>
    </form>
</body>
</html>
