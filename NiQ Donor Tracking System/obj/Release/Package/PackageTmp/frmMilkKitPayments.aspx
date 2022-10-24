<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMilkKitPayments.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmMilkKitPayments" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Milk Kit Payments</title>
    <style>
        body {
            font-family: Arial, Helvetica, sans-serif;
        }

        #pageHeader {
            margin-left: 20px;
        }

        #SearchControls {
            width: 350px;
            margin: 5px 0;
        }

        #calendarField {
            width: 325px;
            margin: 5px 0;
            overflow: hidden;
        }

        #SearchBtn {
            float: right;
        }
        #ClearSearchBtn {
            float: right;
            margin: 0 10px;
        }
        #searchTextBox, #paymentDate {
            width: 270px;
        }

        #ResultMessage {
            color: red;
            font-size: 12pt;
            font-weight: bold;
        }

        #calendarGroup {
            margin: 20px 0;
        }

        #calendar {
            margin: 20px 0;
        }

        #paymentCalendar {
            margin-top: 10px;
        }

        #imgCalendar {
            height: 28px;
            width: 28px;
            float: right;
        }

        .formButtons {
            width: 350px;
            margin: 20px 0;
        }

        .controlContainer {
            width: 1050px;
            margin-left: 45px
        }
    </style>
</head>
<body>
    <div id="pageHeader">
        <img id="logo" src="/images/Ni-Q Logo.png" />
        <h1>Donor Tracking System</h1>
        <h3>Milk Kit Payment</h3>
    </div>
    <form id="MilkKitPaymentsForm" runat="server">
        <asp:HiddenField ID="SelectedMilkKit" runat="server" />
        <div class="controlContainer">
            <asp:Label runat="server" Text="Milk Kit Barcode" /><br />
            <div id="SearchControls">
                <asp:TextBox runat="server" ID="searchTextBox" />
                <asp:Button ID="ClearSearchBtn" Text="Clear" runat="server" OnClick="ClearSearchBtn_Click" />
                <asp:Button ID="SearchBtn" Text="Search" runat="server" OnClick="SearchBtn_Click" />
            </div>
            <br />
            <asp:Label ID="ResultMessage" Text="[Results]" runat="server" />
            <br />
            <div id="calendarGroup" runat="server">
                <asp:Label ID="lblCalendar" runat="server" Text="Date Paid" />
                <div id="calendarControls">
                    <div id="calendarField">
                        <asp:TextBox ID="paymentDate" runat="server" />
                        <asp:ImageButton ID="imgCalendar" runat="server" ImageUrl="~/images/calendar.png" OnClick="imgCalendar_Click" />
                    </div>
                    <asp:Calendar ID="paymentCalendar" runat="server" OnSelectionChanged="paymentCalendar_SelectionChanged" />
                </div>
            </div>
            <div class="formButtons">
                <asp:Button ID="SaveButton" Text="Save" Style="float: right" runat="server" OnClick="SaveButton_Click" />
                <asp:Button ID="CancelBtn" Text="Cancel" Style="float: left" runat="server" OnClick="CancelBtn_Click" />
            </div>
        </div>
    </form>
</body>
</html>