<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportParameters.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmReportParameters" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Report Parameters</title>
</head>
<body>
    <form id="ReportParametersForm" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Report Parameters" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:CheckBox ID="chkAllDates" runat="server" AutoPostBack="true" Checked="True" Font-Names="Tahoma" Font-Size="Small" OnCheckedChanged="chkAllDates_CheckedChanged" style="Z-INDEX: 104; LEFT: 150px; POSITION: absolute; TOP: 189px" Text="All Dates" />
        <asp:Label ID="lblDateFrom" runat="server" Enabled="False" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" height="16px" style="Z-INDEX: 101; LEFT: 56px; POSITION: absolute; TOP: 215px; width: 73px;" Text="From"></asp:Label>
        <asp:Label ID="lblDateTo" runat="server" Enabled="False" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" height="16px" style="Z-INDEX: 101; LEFT: 285px; POSITION: absolute; TOP: 215px; width: 52px;" Text="To"></asp:Label>
        <asp:Calendar ID="calDateFrom" runat="server" Enabled="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" style="z-index: 1; left: 56px; position: absolute; height: 188px; width: 186px; top: 239px" TabIndex="13"></asp:Calendar>
        <asp:Calendar ID="calDateTo" runat="server" Enabled="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" style="z-index: 1; left: 285px; position: absolute; height: 188px; width: 186px; top: 239px" TabIndex="14"></asp:Calendar>
        <asp:Label ID="lblDateRange" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" height="16px" style="Z-INDEX: 101; LEFT: 56px; POSITION: absolute; TOP: 192px; width: 87px;" Text="Date Range:"></asp:Label>
        <asp:Label ID="lblTransactionType" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" style="Z-INDEX: 104; LEFT: 56px; POSITION: absolute; TOP: 501px" Text="Transaction Type:" Width="131px"></asp:Label>
        <asp:DropDownList ID="ddlTransactionType" runat="server" style="Z-INDEX: 104; LEFT: 56px; POSITION: absolute; TOP: 523px; height: 20px; width: 446px;" AutoPostBack="True" OnSelectedIndexChanged="ddlTransactionType_SelectedIndexChanged">
        </asp:DropDownList>
        <asp:Label ID="lblUserID" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" style="Z-INDEX: 104; LEFT: 56px; POSITION: absolute; TOP: 560px; height: 16px;" Text="User ID:" Width="94px"></asp:Label>
        <asp:TextBox ID="txtUserID" runat="server" Font-Names="Tahoma" MaxLength="20" style="Z-INDEX: 105; LEFT: 56px; POSITION: absolute; TOP: 580px; width: 209px;"></asp:TextBox>
        <asp:Label ID="lblItemID" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" style="Z-INDEX: 104; LEFT: 285px; POSITION: absolute; TOP: 560px; width: 162px;" Text="Item ID:"></asp:Label>
        <asp:TextBox ID="txtItemID" runat="server" Font-Names="Tahoma" MaxLength="50" style="Z-INDEX: 105; LEFT: 285px; POSITION: absolute; TOP: 580px" Width="209px"></asp:TextBox>
        <asp:Button ID="searchbtn" runat="server"  OnClick="searchbtn_Click" Text="Search" ></asp:Button>

        <asp:Button ID="btnBack" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnBack_Click" style="Z-INDEX: 103; LEFT: 56px; POSITION: absolute; TOP: 622px; width: 123px;" TabIndex="21" Text="Back" UseSubmitBehavior="False" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" style="Z-INDEX: 101; LEFT: 56px; POSITION: absolute; top: 666px; width: 639px;"></asp:Label>
        <asp:Button ID="btnGenerate" runat="server" Font-Names="Arial" Font-Size="10pt" Height="24px" onclick="btnGenerate_Click" style="Z-INDEX: 103; LEFT: 285px; POSITION: absolute; TOP: 622px; width: 209px;" TabIndex="21" Text="Generate" UseSubmitBehavior="False" />
        <asp:Label ID="lblReportType" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" style="Z-INDEX: 104; LEFT: 56px; POSITION: absolute; TOP: 442px" Text="Report Type:" Width="131px"></asp:Label>
        <asp:DropDownList ID="ddlReportType" runat="server" style="Z-INDEX: 104; LEFT: 56px; POSITION: absolute; TOP: 464px; height: 20px; width: 446px;" AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
            <asp:ListItem>Audit Report</asp:ListItem>
            <asp:ListItem>Inventory Report</asp:ListItem>
        </asp:DropDownList>
    </form>
</body>
</html>
