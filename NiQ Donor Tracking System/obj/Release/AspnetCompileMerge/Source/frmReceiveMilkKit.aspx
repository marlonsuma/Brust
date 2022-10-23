<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReceiveMilkKit.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmReceiveMilkKit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Receive Milk Collection Kit</title>
</head>
<body>
    <form id="frmReceiveMilkKit" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Receive Milk Collection Kit" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:Label ID="lblMilkKitID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="Milk Collection Kit ID:"></asp:Label>
        <asp:TextBox ID="txtMilkKitID" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="9" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 254px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCancel_Click" Style="z-index: 106; left: 45px; position: absolute; top: 520px" Text="Cancel" Width="88px" UseSubmitBehavior="False" TabIndex="5" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 568px; width: 673px;"></asp:Label>
        <asp:Button ID="btnSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSubmit_Click" Style="z-index: 106; left: 195px; position: absolute; top: 520px; width: 112px;" Text="Submit" UseSubmitBehavior="False" TabIndex="4" Visible="False" />
        <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 313px; position: absolute; top: 213px; width: 65px;" Text="Search" TabIndex="4" />
        <asp:Label ID ="lblReceiveCalendar" runat="server" Font-Bold="true" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 256px; width: 235px; right: 1130px;" Text="Date Milk Kit Was Received:"></asp:Label>
        <asp:TextBox ID="txtCalendarReceiveDate" runat="server" Font-Names="Arial" Font-Size="10pt" Style="z-index: 2; left: 45px; position: absolute; top: 276px; width: 254px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:ImageButton ID="imgReceiveCalendar" runat="server" OnClick="imgReceiveCalendar_Click" Style="z-index: 106; left: 313px; position: absolute; top: 275px; height: 28px; width: 28px;" ImageUrl="~/images/calendar.png" />
        <asp:Calendar ID="ReceiveCalendar" runat="server" Style="z-index: 106; left: 45px; position: absolute; top: 306px; right: 1067px; margin-right: 0px;" OnSelectionChanged="receiveCalendar_SelectionChanged"></asp:Calendar>
    </form>
</body>
</html>