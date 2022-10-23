<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageDonors.aspx.cs" Inherits="NiQ_Donor_Tracking_System.ManageDonors" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Manage Donor</title>
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

        #SearchBtn, #ClearSearchBtn {
            float: right;
        }

        #SearchTextBox, #InactiveReasonText {
            width: 270px;
            margin-top: 1px;
        }

        #ResultMessage {
            color: red;
            font-size: 12pt;
            font-weight: bold;
        }

        #Search {
            float: left;
        }

        #DonorDisplay {
            float: right;
            min-width: 500px;
            border-style: solid;
            border-width: 1px;
        }

        .resultContainer {
            clear: both;
            margin-left: 45px;
            border-style: solid;
            border-width: 1px;
            margin-top: 20px;
            width: 1048px;
        }

        .resultContainer h5 {
            margin-top: 5px;
            margin-left: 15px;
            margin-bottom: 5px;
        }

        .resultGrid {
            min-width: 1020px;
            font-size: 10pt;
        }

        .gridContainer {
            width: 1020px;
            max-height: 500px;
            margin-left: 15px;
            margin-bottom: 10px;
            overflow: auto;
        }

        .field {
            margin: 4px 0;
            overflow: hidden;
        }

        .description {
            width: 150px;
            float: left;
            text-align: right;
            margin-right: 4px;
            font-weight: bold;
        }

        .input {
            margin-left: 0;
        }

        .value {
            float: left;
            margin-left: 4px;
        }

        .mailing {
            display: block;
        }

        .responseContainer {
            margin: 20px 0;
        }
    </style>
</head>
<body>
<div id="pageHeader">
    <img id="logo" src="/images/Ni-Q Logo.png" />
    <h1>Donor Tracking System</h1>
    <h3>Donor Forms</h3>
</div>
<form id="ManageDonorForm" runat="server">
    <asp:HiddenField ID="SelectedDonorId" runat="server" />
        <div style="width: 1050px; margin-left: 45px">
            <div id="Search">
                <asp:Label runat="server" Text="Donor Search" /><br />
                <div id="SearchControls">
                    <asp:TextBox runat="server" ID="SearchTextBox" />
                    <asp:Button ID="ClearSearchBtn" Text="Clear" runat="server" OnClick="ClearSearchBtn_Click" />
                    <asp:Button ID="SearchBtn" Text="Search" runat="server" OnClick="SearchBtn_Click" />
                </div>
                <div class="responseContainer">
                    <asp:Label ID="ResultMessage" Text="[Results]" runat="server" />
                </div>
                <div id="formFields" runat="server">
                    <div class="input">
                        <p>
                            <asp:CheckBox ID="Inactive" Text="Inactive" runat="server" />
                        </p>
                        <div>
                            <asp:Label Text="Inactive Reason" runat="server" /><br />
                            <div>
                                <asp:TextBox ID="InactiveReasonText" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <div style="width: 350px">
                    <asp:Button ID="SaveButton" Text="Save" Style="float: right" runat="server" OnClick="SaveButton_Click" />
                    <asp:Button ID="CancelBtn" Text="Cancel" Style="float: left" runat="server" OnClick="CancelBtn_Click" />
                </div>
            </div>
            <div id="DonorDisplay" runat="server">
                <div class="field">
                    <div class="description">Donor Id</div>
                    <div class="value">
                        <asp:Label ID="DonorIdValue" Text="999999999" runat="server" />
                    </div>
                </div>
                <div class="field">
                    <div class="description">Name</div>
                    <div class="value">
                        <asp:Label ID="DonorNameValue" Text="Lastname, firstname" runat="server" />
                    </div>
                </div>
                <div class="field">
                    <div class="description">Date Of Birth</div>
                    <div class="value">
                        <asp:Label ID="DonorDobValue" Text="1/1/01" runat="server" />
                    </div>
                </div>
                <div class="field">
                    <div class="description">EMail</div>
                    <div class="value">
                        <asp:Label ID="DonorEmailValue" Text="mail@box.com" runat="server" />
                    </div>
                </div>
                <div class="field">
                    <div class="description">Mailing Address</div>
                    <div class="value">
                        <asp:Label ID="DonorMailing1Value" Text="address goes here" CssClass="mailing" runat="server" />
                        <asp:Label ID="DonorMailing2Value" Text="address goes here" CssClass="mailing" runat="server" />
                        <asp:Label ID="DonorMailing3Value" Text="City state zip" CssClass="mailing" runat="server" />
                    </div>
                </div>
                <div class="field">
                    <div class="description">Shipping Address</div>
                    <div class="value">
                        <asp:Label ID="DonorShipping1Value" Text="address goes here" CssClass="mailing" runat="server" />
                        <asp:Label ID="DonorShipping2Value" Text="" CssClass="mailing" runat="server" />
                        <asp:Label ID="DonorShipping3Value" Text="City state zip" CssClass="mailing" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div style="clear: both;"></div>
        <div id="donorResults" class="resultContainer" runat="server">
            <h5>
                <asp:Label Text="Donor" runat="server" /></h5>
            <div class="gridContainer">
                <asp:GridView ID="DonorGrid" AutoGenerateSelectButton="True" OnSelectedIndexChanged="DonorGrid_OnSelectedIndexChanged" CssClass="resultGrid" runat="server">
                    <HeaderStyle Height="10px" />
                    <RowStyle Height="10px" Wrap="True" />
                </asp:GridView>
            </div>
        </div>
</form>
</body>
</html>
