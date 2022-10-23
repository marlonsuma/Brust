<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmManageLocations.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmManageLocations" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Manage Locations</title>
</head>
<body>
    <form id="frmManageLocations" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Manage Locations" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
                    <asp:TreeView ID="treeLocations" SelectedNodeStyle-ForeColor="Red" runat="server" Style="position: absolute; overflow:auto; top: 217px; left: 45px; height: 449px; width: 249px; " BorderStyle="Solid" BorderWidth="1px" ExpandDepth="0" Font-Bold="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Black" ShowLines="True" OnTreeNodePopulate="treeLocations_TreeNodePopulate" OnSelectedNodeChanged="treeLocations_SelectedNodeChanged">
                <SelectedNodeStyle ForeColor="Red" />
            </asp:TreeView>
        <asp:Label ID="lblLocations" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
            Style="z-index: 102; left: 45px; position: absolute; top: 192px" Text="Locations:"
            Width="159px"></asp:Label>
        <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCancel_Click"
            Style="z-index: 106; left: 45px; position: absolute; top: 673px" Text="Back"
            Width="88px" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial"
            Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 707px" Width="594px"></asp:Label>
        <asp:Label ID="lblParentLocationName" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 337px; POSITION: absolute; TOP: 192px" Text="Parent Location" Width="121px" height="16px"></asp:Label>
        <asp:TextBox ID="txtParentLocation" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="10pt" MaxLength="50" style="Z-INDEX: 103; LEFT: 337px; POSITION: absolute; TOP: 217px" Width="165px"></asp:TextBox>
        <asp:Button ID="btnAdd" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnAdd_Click" style="Z-INDEX: 103; LEFT: 528px; POSITION: absolute; TOP: 341px" Text="Add New Location" Width="171px" />
        <asp:Button ID="btnRemove" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnRemove_Click" style="Z-INDEX: 103; LEFT: 528px; POSITION: absolute; TOP: 248px; width: 171px;" Text="Remove Parent Location" />
        <asp:Label ID="lblNewLocation" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" style="Z-INDEX: 101; LEFT: 337px; POSITION: absolute; TOP: 320px; width: 239px;" Text="New Location" height="16px"></asp:Label>
        <asp:TextBox ID="txtNewLocation" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="10pt" MaxLength="50" style="Z-INDEX: 103; LEFT: 337px; POSITION: absolute; TOP: 343px" Width="165px"></asp:TextBox>
        <asp:Button ID="btnSave" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnSave_Click" style="Z-INDEX: 103; LEFT: 528px; POSITION: absolute; TOP: 217px" Text="Update Parent Location" Width="171px" />    
    </form>
</body>
</html>
