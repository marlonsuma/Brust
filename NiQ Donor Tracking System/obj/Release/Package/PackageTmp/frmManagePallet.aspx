<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmManagePallet.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmManagePallet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmManagePallet" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Manage Pallet" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
            <asp:Label ID="milkKitCollectionLabel" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 321px; width: 235px;" Text="Milk Collection Kit ID:" Visible="False"></asp:Label>
        <asp:TextBox ID="palletNumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="10" Style="z-index: 2; left: 46px; position: absolute; top: 216px; width: 159px; bottom: 168px; right: 664px; height: 16px;" TabIndex="1" Height="16px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnLoadPallet_Click" Style="z-index: 106; left: 220px; position: absolute; top: 214px; width: 89px; height: 27px;" Text="Load Pallet" TabIndex="4" Height="16px" />
        <asp:Label ID="palletGradeLabel" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 255px; width: 235px; height: 15px;" Text="Pallet Grade" Visible="False"></asp:Label>
        <asp:RadioButton ID="grade1" GroupName="grade" Text="Grade 1" runat="server" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 275px; width: 75px; height: 15px;" Visible="False"/>
        <asp:RadioButton ID="grade2" GroupName="grade" Text="Any Grade" runat="server" Checked="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 133px; position: absolute; top: 275px; width: 90px; height: 15px;" Visible="False" />
        <asp:Label ID="lblMilkKitID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 86px;" Text="Pallet #"></asp:Label>
        <asp:TextBox ID="txtMilkKitID" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="9" Style="z-index: 2; left: 45px; position: absolute; top: 343px; width: 254px; bottom: 94px; right: 562px;" TabIndex="1" Height="16px" Visible="False"></asp:TextBox>
        <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCancel_Click" Style="z-index: 106; left: 47px; position: absolute; top: 629px" Text="Cancel" Width="88px" UseSubmitBehavior="False" TabIndex="5" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 676px; width: 673px;"></asp:Label>
        <asp:Button ID="btnSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSubmit_Click" Style="z-index: 106; left: 195px; position: absolute; top: 629px; width: 112px;" Text="Submit" UseSubmitBehavior="False" TabIndex="4" Visible="False" />
        <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 313px; position: absolute; top: 341px; width: 65px; height: 27px;" Text="Search" TabIndex="4" Height="16px" Visible="False" />
        <asp:ListBox ID="lstMilkKits" runat="server" style="z-index: 1; left: 45px; top: 379px; position: absolute; height: 204px; width: 260px" Height="204px" Visible="False"></asp:ListBox>
        <asp:Label ID="lblTotalVolumeTitle" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 316px; position: absolute; top: 405px; width: 90px; margin-top: 0px; right: 463px;" Text="Total Ounces: " Visible="False"></asp:Label>
        <asp:Label ID="lblTotalVolume" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 421px; position: absolute; top: 404px; width: 181px; margin-top: 0px;" Text="0" Visible="False"></asp:Label>
        <asp:Label ID="totalMilkKitsLabel" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 316px; position: absolute; top: 435px; width: 131px; margin-top: 0px; right: 422px;" Text="Total Milk Kits: " Visible="False"></asp:Label>
        <asp:Label ID="totalMilkKitsValue" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 419px; position: absolute; top: 436px; width: 189px; margin-top: 0px;" Text="0" Visible="False"></asp:Label>
        <asp:Label ID="gradeLabel" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 316px; position: absolute; top: 468px; width: 131px; margin-top: 0px; right: 422px;" Text="Grade: " Visible="False"></asp:Label>
        <asp:Label ID="gradeValue" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 419px; position: absolute; top: 468px; width: 189px; margin-top: 0px;" Text="0" Visible="False"></asp:Label>
       
    </form>
</body>
</html>
