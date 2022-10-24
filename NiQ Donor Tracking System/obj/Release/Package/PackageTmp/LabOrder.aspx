<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LabOrder.aspx.cs" Inherits="NiQ_Donor_Tracking_System.LabOrder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Lab Order</title>
    <style>
        body {
            font-family: Arial, Helvetica, sans-serif;
        }

        #pageHeader {
            margin-left: 20px;
        }

        #LabOrderForm {
            width: 1050px;
            margin-left: 45px;
        }

        #SearchControls {
            width: 350px;
        }

            #SearchControls label, #PrintControls label {
                display: block;
                font-weight: bold;
            }

        #SearchBtn, #ClearSearchBtn, #SaveOrder, #Print {
            float: right;
        }

        #SearchTextBox {
            width: 250px;
            margin-top: 1px;
        }

        #Printers {
            margin-top: 1px;
            width: 230px;
            float: left;
        }

        #ResultMessage {
            color: red;
            font-size: 12pt;
            font-weight: bold;
        }

        #MilkKitDisplay {
            float: right;
            min-width: 500px;
            border-style: solid;
            border-width: 1px;
        }

        #LabOrderControls, #PrintControls {
            width: 350px;
            clear: both;
            overflow: hidden;
        }

            #LabOrderControls p {
                margin: 8px 0;
            }

        #PrintControls {
            margin: 20px 0;
        }

        #InputControls {
            float: left;
        }

        .responseContainer {
            margin: 20px 0;
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

        .value {
            float: left;
            margin-left: 4px;
        }
    </style>
</head>
<body>
    <div id="pageHeader">
        <img id="logo" src="/images/Ni-Q Logo.png" />
        <h1>Donor Tracking System</h1>
        <h3>Lab Orders</h3>
    </div>
    <form id="LabOrderForm" runat="server">
        <asp:HiddenField ID="SelectedLabKit" runat="server" />
        <asp:HiddenField ID="SelectedMilkKit" runat="server" />
        <asp:HiddenField ID="SelectedDonorId" runat="server" />
        <asp:HiddenField ID="PrintCount" runat="server" />
        <div id="InputControls">
            <div id="SearchControls">
                <label for="SearchTextBox">Milk Kit</label>
                <asp:TextBox ID="SearchTextBox" runat="server" />
                <asp:Button ID="ClearSearchBtn" Text="Clear" runat="server" OnClick="ClearSearchBtn_Click" />
                <asp:Button ID="SearchBtn" Text="Search" runat="server" OnClick="SearchBtn_Click" />
                <div class="responseContainer">
                    <asp:Label ID="ResultMessage" Text="[Results]" runat="server" />
                </div>
            </div>
            <div id="LabOrderControls" runat="server">
                <p><asp:CheckBox ID="MicrobialOrder" Text="Microbial" runat="server" /></p>
                <p><asp:CheckBox ID="ToxicologyOrder" Text="Toxicology" runat="server" /></p>
                <p><asp:CheckBox ID="GeneticsOrder" Text="Genetics" runat="server" /></p>
                <asp:Button ID="SaveOrder" Text="Order" OnClick="SaveOrder_Clicked" runat="server" />
            </div>
            <div id="PrintControls" runat="server">
                <label for="Printers">Select Printer</label>
                <asp:DropDownList ID="Printers" runat="server" />
                <asp:Button ID="Print" Text="Print Label" OnClick="Print_OnClick" runat="server" />
            </div>
            <div id="NavControls">
                <asp:Button ID="Cancel" Text="Cancel" OnClick="Cancel_OnClick" runat="server"/>
            </div>
        </div>
        <div id="MilkKitDisplay" runat="server">
            <div class="field">
                <div class="description">Barcode</div>
                <div class="value">
                    <asp:Label ID="Barcode" Text="MK0000015" runat="server" />
                </div>
            </div>
            <div class="field">
                <div class="description">Donor Id</div>
                <div class="value">
                    <asp:Label ID="DonorId" Text="0123456789" runat="server" />
                </div>
            </div>
            <div class="field">
                <div class="description">Donor</div>
                <div class="value">
                    <asp:Label ID="DonorName" Text="Lafond, Rachel" runat="server" />
                </div>
            </div>
            <div class="field">
                <div class="description">Volume</div>
                <div class="value">
                    <asp:Label ID="Volume" Text="16.3" runat="server" />
                </div>
            </div>
            <div class="field">
                <div class="description">Receive Date</div>
                <div class="value">
                    <asp:Label ID="ReceiveDate" Text="03/04/2019" runat="server" />
                </div>
            </div>
            <div class="field">
                <div class="description">Quarantine Date</div>
                <div class="value">
                    <asp:Label ID="QuarantineDate" Text="03/05/2019" runat="server" />
                </div>
            </div>
            <div class="field">
                <div class="description">Microbial Test</div>
                <div class="value">
                    <asp:Label ID="MicrobialStatus" Text="Pass" runat="server" />
                </div>
            </div>
            <div class="field">
                <div class="description">Toxicology Test</div>
                <div class="value">
                    <asp:Label ID="ToxicologyStatus" Text="Ordered" runat="server" />
                </div>
            </div>
            <div class="field">
                <div class="description">Genetics Test</div>
                <div class="value">
                    <asp:Label ID="GeneticsStatus" Text="None" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
