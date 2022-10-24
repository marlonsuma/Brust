<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="NiQ_Donor_Tracking_System._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Main Menu</title>
    <style>
        body {
            font-family: Arial, Helvetica, sans-serif;
        }

        #pageHeader {
            margin-left: 20px;
        }

        #btnAdmin {
            background: lightcoral;
            color: white;
        }

        #btnBack {
            background: lightskyblue;
            color: white;
        }

        .column {
            width: 240px;
            float: left;
        }

        .column h5 {
            margin: 5px 7px;
        }

        .wrapper {
            border-style: solid;
            border-width: 1px;
            margin: 0 5px;

        }
        .container, #default {
            margin: 0 20px;
            float: left;
        }

        .navButton {
            width: 220px;
            margin: 7px 10px;
        }

        .navLink {
            float: right;
            margin-bottom: 20px;
        }

        .formButton {
            width: 120px;
            margin: 30px 10px;
        }
    </style>
</head>
<body>
<div id="pageHeader">
    <img id="logo" src="/images/Ni-Q Logo.png" />
    <h1>Donor Tracking System</h1>
    <h3>Main Menu</h3>
</div>
    <form id="default" runat="server">
        <div class="container">
            <div class="column">
                <asp:Button ID="btnInventoryProfile" Text ="Inventory / Donor Search" OnClick="btnInventoryProfile_Click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnDonorForms" Text="Donor Forms" runat="server" CssClass="navButton" OnClick="btnDonorForms_Click" />
                <asp:Button ID="btnPrint" Text ="(Re)Print Labels" OnClick="btnPrint_Click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnReporting" Text ="Reporting" OnClick="btnReporting_Click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnAdmin" Text ="Administrator Options" OnClick="btnAdmin_Click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnBack" Text ="Logout" OnClick="btnBack_Click" runat="server" CssClass="formButton" />
            </div>
            <div class="column wrapper">
                <h5>Blood Kits</h5>
                <asp:Button ID="btnCreateBloodKit" Text ="Create Blood Kit" OnClick="btnCreateBloodKit_Click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnReceiveBloodKit" Text ="Receive Blood Kit" OnClick="btnReceiveBloodKit_Click" runat="server" CssClass="navButton" />
            </div>
            <div class="column wrapper">
                <h5>Milk Kits</h5>
                <asp:Button ID="btnCreateMilkKit" Text ="Create Milk Collection Kit" OnClick="btnCreateMilkKit_Click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnReceiveMilkKit" Text ="Receive Milk Kit" OnClick="btnReceiveMilkKit_Click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnQuarantineMilk" Text ="Quarantine Milk Bag" OnClick="btnQuarantineMilk_Click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnLabOrders" Text ="Lab Orders" OnClick="btnLabOrders_click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnMilkTestResults" Text ="Lab Results" OnClick="btnMilkTestResults_Click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnMilkKitPayments" Text ="Milk Kit Payments" OnClick="btnMilkKitPayments_Click" runat="server" CssClass="navButton" />
            </div>
            <div class="column wrapper">
                <h5>Lot</h5>
                <asp:Button ID="btnCreatePallet" Text ="Create Pallet" OnClick="btnCreatePallet_Click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnCreateLot" Text ="Create Lot / VAT" OnClick="btnCreateLot_Click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnLotTransfer" Text ="Lot Transfer / Pouch Labels" OnClick="btnLotTransfer_Click" runat="server" CssClass="navButton" />
                <asp:Button ID="btnShipCase" Text ="Create & Ship Case" OnClick="btnShipCase_Click" runat="server" CssClass="navButton" />
            </div>

        </div>
    </form>
</body>
</html>
