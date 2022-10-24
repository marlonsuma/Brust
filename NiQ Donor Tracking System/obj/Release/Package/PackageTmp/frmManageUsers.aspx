<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmManageUsers.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmManageUsers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Manage Users</title>
</head>
<body>
    <form id="frmManageUsers" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Manage Users" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />

        <asp:ListBox ID="lstExistingUsers" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstExistingUsers_SelectedIndexChanged" style="Z-INDEX: 105; LEFT: 45px; POSITION: absolute; TOP: 210px; height: 233px; width: 168px;"></asp:ListBox>
        <asp:Label ID="lblExistingUsers" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 45px; POSITION: absolute; TOP: 189px" Text="Existing Users" Width="172px"></asp:Label>
        <asp:Label ID="lblUsername" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 234px; POSITION: absolute; TOP: 189px" Text="Username" Width="121px" height="16"></asp:Label>
        <asp:Label ID="lblPassword" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 234px; POSITION: absolute; TOP: 241px; width: 239px;" Text="Password" height="16"></asp:Label>
        <asp:Label ID="lblSubPassword" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="8pt" style="Z-INDEX: 101; LEFT: 417px; POSITION: absolute; TOP: 264px; width: 239px;" Text="(Leave Blank to Keep Existing Password)" height="16px"></asp:Label>
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" style="Z-INDEX: 101; LEFT: 45px; POSITION: absolute; top: 500px; width: 624px;"></asp:Label>
        <asp:Label ID="lblPermissionLevel" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 234px; POSITION: absolute; TOP: 301px" Text="Permission Level" Width="138px"></asp:Label>
        <asp:TextBox ID="txtUsername" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="10pt" MaxLength="20" style="Z-INDEX: 103; LEFT: 234px; POSITION: absolute; TOP: 210px" Width="165px"></asp:TextBox>
        <asp:TextBox ID="txtPassword" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="10pt" MaxLength="20" style="Z-INDEX: 103; LEFT: 234px; POSITION: absolute; TOP: 262px" Width="165px" TextMode="Password"></asp:TextBox>
        <asp:RadioButton ID="radAdministrator" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpPermission" style="Z-INDEX: 105; LEFT: 327px; POSITION: absolute; TOP: 327px; height: 20px; width: 111px;" Text="Administrator" />
        <asp:RadioButton ID="radUser" runat="server" Checked="True" Font-Names="Arial" Font-Size="10pt" GroupName="grpPermission" style="Z-INDEX: 105; LEFT: 234px; POSITION: absolute; TOP: 327px; width: 79px;" Text="Operator" />
        <asp:Button ID="btnSave" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnSave_Click" style="Z-INDEX: 103; LEFT: 234px; POSITION: absolute; TOP: 458px" Text="Save" Width="165px" />
        <asp:Label ID="lblAccountStatus" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 234px; POSITION: absolute; TOP: 373px" Text="Account Status" Width="138px"></asp:Label>
        <asp:RadioButton ID="radInactive" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpStatus" style="Z-INDEX: 105; LEFT: 327px; POSITION: absolute; TOP: 404px" Text="Inactive" />
        <asp:RadioButton ID="radActive" runat="server" Checked="True" Font-Names="Arial" Font-Size="10pt" GroupName="grpStatus" style="Z-INDEX: 105; LEFT: 234px; POSITION: absolute; TOP: 404px; width: 75px;" Text="Active" />
        <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnCancel_Click" style="Z-INDEX: 103; LEFT: 45px; POSITION: absolute; TOP: 458px" Text="Back" Width="165px" />

    </form>
</body>
</html>
