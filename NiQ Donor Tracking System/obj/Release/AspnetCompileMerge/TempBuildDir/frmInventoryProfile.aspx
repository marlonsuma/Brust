<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInventoryProfile.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmInventoryProfile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Inventory Profile</title>
</head>
<body>
    <form id="frmInventoryProfile" runat="server">
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Inventory Profile" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
                <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCancel_Click" Style="z-index: 106; left: 45px; position: absolute; top: 757px" Text="Cancel" Width="88px" UseSubmitBehavior="False" TabIndex="5" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 797px; width: 673px;"></asp:Label>
        <asp:Label ID="lblBarcode" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="Barcode:"></asp:Label>
        <asp:TextBox ID="txtBarcode" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 254px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 313px; position: absolute; top: 213px; width: 65px;" Text="Search" TabIndex="4" />

        <asp:Label ID="lblFirstName" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 420px; position: absolute; top: 165px; width: 235px;" Text="First Name:"></asp:Label>
        <asp:TextBox ID="txtFirstNameSearch" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 2; left: 420px; position: absolute; top: 185px; width: 254px; bottom: 419px;" TabIndex="1"></asp:TextBox>
         <asp:Label ID="lblLastName" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 420px; position: absolute; top: 215px; width: 235px;" Text="Last Name:"></asp:Label>
        <asp:TextBox ID="txtLastNameSearch" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="50" Style="z-index: 2; left: 420px; position: absolute; top: 235px; width: 254px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:Button ID="btnSearchNames" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearchNames_Click" Style="z-index: 106; left: 700px; position: absolute; top: 205px; width: 65px;" Text="Search" TabIndex="4" />

        <asp:Label ID="lblFirstTitle" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 252px; width: 1036px;" Text="[First Title]"></asp:Label>
        <div runat="server" id="divFirst" style="border-style: solid; border-color: inherit; border-width: 1px; z-index: 1; left: 45px; top: 273px; position: absolute; height: 130px; width: 1048px; overflow: auto; background-color:#FFFFFF">
            <asp:GridView ID="dgvFirst" runat="server" Font-Names="Arial" Font-Size="10pt" HorizontalAlign="Left" ShowFooter="True" ShowHeaderWhenEmpty="True" style="z-index: 1; left: 0px; top: 0px; position: absolute; height: 130px; width: 1024px">
                <AlternatingRowStyle Height="10px" />
                <HeaderStyle Height="10px" HorizontalAlign="Center" />
                <RowStyle Height="10px" Wrap="True" HorizontalAlign="Center" />
            </asp:GridView>
        </div>
        
        <asp:Label ID="lblSecondTitle" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top:417px; width: 1039px;" Text="[Second Title]"></asp:Label>
        <div runat="server" id="divSecond" style="border-style: solid; border-color: inherit; border-width: 1px; z-index: 1; left: 45px; top: 438px; position: absolute; height: 130px; width: 1048px; overflow: auto; background-color:#FFFFFF">
            <asp:GridView ID="dgvSecond" runat="server" Font-Names="Arial" Font-Size="10pt" HorizontalAlign="Center" ShowFooter="True" ShowHeaderWhenEmpty="True" style="z-index: 1; left: 0px; top: 0px; position: absolute; height: 130px; width: 1024px; bottom: 33px;" Height="130px">
                <AlternatingRowStyle Height="10px" />
                <HeaderStyle Height="10px" HorizontalAlign="Center" />
                <RowStyle Height="10px" Wrap="True" HorizontalAlign="Center" />
            </asp:GridView>
        </div>
        
        <asp:Label ID="lblThirdTitle" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 583px; width: 1038px;" Text="[Third Title]"></asp:Label>
        <div runat="server" id="divThird" style="border-style: solid; border-color: inherit; border-width: 1px; z-index: 1; left: 45px; top: 606px; position: absolute; height: 130px; width: 1048px; overflow: auto; background-color:#FFFFFF">
            <asp:GridView ID="dgvThird" runat="server" Font-Names="Arial" Font-Size="10pt" HorizontalAlign="Center" ShowFooter="True" ShowHeaderWhenEmpty="True" style="z-index: 1; left: 0px; top: 0px; position: absolute; height: 130px; width: 1024px; bottom: -4px;" Height="130px">
                <AlternatingRowStyle Height="10px" />
                <HeaderStyle Height="10px" HorizontalAlign="Center" />
                <RowStyle Height="10px" Wrap="True" HorizontalAlign="Center" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
