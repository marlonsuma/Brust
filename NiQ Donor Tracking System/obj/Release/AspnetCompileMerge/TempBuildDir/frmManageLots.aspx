<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmManageLots.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmManageLots" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Manage Lots</title>
</head>
<body>
    <form id="frmManageLots" runat="server">
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Manage Lots" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:Button ID="btnBack" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnBack_Click" Style="z-index: 106; left: 48px; position: absolute; top: 476px" Text="Back" Width="88px" UseSubmitBehavior="False" TabIndex="8" />
        <asp:Label ID="lblLotNumber" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="Lot Number:"></asp:Label>
        <asp:TextBox ID="txtLotNumber" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="9" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 299px; " TabIndex="1"></asp:TextBox>
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Style="z-index: 101; left: 45px;
            position: absolute; top: 528px; width: 673px; height: 88px;"></asp:Label>
        <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 361px; position: absolute; top: 213px; width: 65px;" Text="Search" TabIndex="2" />
        <asp:CheckBox ID="chkClosed" runat="server" Enabled="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" style="z-index: 1; left: 287px; top: 192px; position: absolute" Text="Closed" TabIndex="6" />
        <asp:Label ID="lblTotalCases" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 257px; width: 88px;" Text="Total Cases:"></asp:Label>
        <asp:TextBox ID="txtTotalCases" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="3" Style="z-index: 2; left: 45px; position: absolute; top: 277px; width: 141px; " TabIndex="3" Enabled="False" onchange="myFunction()"></asp:TextBox>
        <asp:Label ID="lblSamplePouches" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 5; left: 45px; position: absolute; top: 365px; width: 139px;" Text="Sample Pouches:"></asp:Label>
        <asp:TextBox ID="txtSamplePouches" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="2" Style="z-index: 6; left: 45px; position: absolute; top: 385px; width: 141px; " TabIndex="5" Enabled="False"></asp:TextBox>
        <asp:Label ID="lblCasesRemaining" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 310px; width: 121px; height: 16px;" Text="Cases Remaining:"></asp:Label>
        <asp:TextBox ID="txtCasesRemaining" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="3" Style="z-index: 2; left: 45px; position: absolute; top: 329px; width: 141px; " TabIndex="4" Enabled="False" onchange="myFunction()"></asp:TextBox>
         <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 416px; width: 121px; height: 16px; right: 703px;" Text="Lot Date:"></asp:Label>
        <asp:TextBox ID="lotDate" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="255" Style="z-index: 2; left: 45px; position: absolute; top: 438px; width: 141px; " TabIndex="4" Enabled="False"></asp:TextBox>
        <asp:Button ID="btnSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSubmit_Click" Style="z-index: 106; left: 209px; position: absolute; top: 479px; width: 112px;" Text="Submit" UseSubmitBehavior="False" TabIndex="7" Visible="False" />
    </form>
</body>
</html>
  


<script type="text/javascript">

    let txtTotalCases = parseInt(document.getElementById("txtTotalCases").value);
    let txtCasesRemaining = parseInt(document.getElementById("txtCasesRemaining").value);
    let txtTotalCasesValue;

    function myFunction() {
        txtTotalCasesValue = parseInt(document.getElementById("txtTotalCases").value);
        let test = (txtTotalCasesValue - txtTotalCases);
        let totaltxt = (txtCasesRemaining + test);
        document.getElementById("txtCasesRemaining").value = totaltxt;
    }
</script>
