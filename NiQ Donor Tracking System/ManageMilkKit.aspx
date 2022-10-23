<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageMilkKit.aspx.cs" Inherits="NiQ_Donor_Tracking_System.ManageMilkKit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body {
            font-family: Arial, Helvetica, sans-serif;
        }

        #pageHeader {
            margin-left: 20px;
        }

        #ManageMilkKitForm {
            width: 1050px;
            margin-left: 45px;
        }

        #SearchControls {
            width: 350px;
        }

        #SearchControls label,
        .input label,
        .calendarGroup span {
            display: block;
            font-weight: bold;
            margin-bottom: 5px;
        }

        #activeControl label, #LabTable caption {
            font-weight: bold;
        }

        #SearchBtn, #ClearSearchBtn, #Update {
            float: right;
        }

        #SearchTextBox, 
        #NewReceiveDate,
        #NewQuarantineDate,
        .input input,
        .input select{
            width: 250px;
            margin-top: 1px;
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

        #ManageMilkKitControls {
            width: 525px;
            clear: both;
            overflow: hidden;
        }

        #ManageMilkKitControls p {
            margin: 8px 0;
        }

        #InputControls {
            float: left;
        }

        #NavControls {
            margin-top: 20px;
            margin-bottom: 80px;

        }

        .calendarField {
            width: 325px;
            margin: 5px 0;
            overflow: hidden;
        }

        .calendarGroup, .input {
            margin: 20px 0;
        }

        #calendar {
            margin: 20px 0;
        }

        #ReceiveCalendar {
            margin-top: 10px;
        }

        #QuarantineCalendarImage, #ReceiveCalendarImage, #PaidDateCalenderImage,  #FinalizedDateCalenderImage {
            height: 28px;
            width: 28px;
            float: right;
        }

        #LabTable, #LabTable th, #LabTable td {
            border: 1px solid black;
            border-collapse: collapse;
            padding: 2px 8px; 
        }

        #LabTable {
            margin: 20px 0;
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
    <h3>Manage Milk Kit</h3>
</div>
    <form id="ManageMilkKitForm" runat="server">
        <asp:HiddenField ID="SelectedLabKit" runat="server" />
        <asp:HiddenField ID="SelectedMilkKit" runat="server" />
        <asp:HiddenField ID="SelectedDonorId" runat="server" />
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
            <div id="ManageMilkKitControls" runat="server">
                <p id="activeControl"><asp:CheckBox ID="Active" Text="Active" runat="server" /></p>
                <div class="input">
                    <label for="Volume">Volume</label>
                    <asp:TextBox ID="NewVolume" runat="server" />
                </div>
                <div class="input">
                    <label for="NewDonorId">Donor Id</label>
                    <asp:TextBox ID="NewDonorId" runat="server"></asp:TextBox>
                </div>
                <div class="calendarGroup">
                    <asp:Label ID="lblCalendar" runat="server" Text="Received Date" />
                    <div>
                        <div class="calendarField">
                            <asp:TextBox ID="NewReceiveDate" runat="server" />
                            <asp:ImageButton ID="ReceiveCalendarImage" runat="server" ImageUrl="~/images/calendar.png" OnClick="ReceiveCalendarImage_Click" />
                        </div>
                        <asp:Calendar ID="ReceiveCalendar" runat="server" OnSelectionChanged="ReceiveCalendar_SelectionChanged" />
                    </div>
                </div>
                <div class="input">
                    <label for="ShippingService">Shipping Service</label>
                    <asp:DropDownList ID="ShippingService" runat="server">
                        <asp:ListItem>FedEx</asp:ListItem>
                        <asp:ListItem>UPS</asp:ListItem>
                        <asp:ListItem>USPS</asp:ListItem>
                        <asp:ListItem>QUEST</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="input">
                    <label for="TrackingNumber">Tracking Number</label>
                    <asp:TextBox ID="TrackingNumber" runat="server" />
                </div>
                <div class="calendarGroup">
                    <asp:Label ID="lblQuarantineCal" runat="server" Text="Quarantine Date" />
                    <div>
                        <div class="calendarField">
                            <asp:TextBox ID="NewQuarantineDate" runat="server" />
                            <asp:ImageButton ID="QuarantineCalendarImage" runat="server" ImageUrl="~/images/calendar.png" OnClick="QuarantineCalendarImage_Click" />
                        </div>
                        <asp:Calendar ID="QuarantineCalendar" runat="server" OnSelectionChanged="QuarantineCalendar_SelectionChanged" />
                    </div>
                </div>
                 <div class="calendarGroup">
                    <asp:Label ID="lblPaidDate" runat="server" Text="Paid Date" />
                    <div>
                        <div class="calendarField">
                            <asp:TextBox ID="NewPaidDate" runat="server" />
                            <asp:ImageButton ID="PaidDateCalenderImage" runat="server" ImageUrl="~/images/calendar.png" OnClick="PaidDateCalendarImage_Click" />
                        </div>
                        <asp:Calendar ID="PaidDateCalender" runat="server" OnSelectionChanged="PaidDateCalendar_SelectionChanged" />
                    </div>
                </div>
                 <div class="calendarGroup">
                    <asp:Label ID="lblFinalized" runat="server" Text="Finalized Date" />
                    <div>
                        <div class="calendarField">
                            <asp:TextBox ID="newFinalizedDate" runat="server" />
                            <asp:ImageButton ID="FinalizedDateCalenderImage" runat="server" ImageUrl="~/images/calendar.png" OnClick="FinalizedDateCalendarImage_Click" />
                        </div>
                        <asp:Calendar ID="FinalizedDateCalender" runat="server" OnSelectionChanged="FinalizedDateCalendar_SelectionChanged" />
                    </div>
                </div>
                <div>
                    <table id="LabTable">
                        <caption>Lab Details</caption>
                        <tr>
                            <th>Test</th>
                            <th>Ordered</th>
                            <th>Order Date</th>
                            <th>Result</th>
                        </tr>
                        <tr>
                            <td>MicrobialTex</td>
                            <td><asp:CheckBox Text="Ordered" ID="MicrobialOrdered" runat="server"/></td>
                            <td><asp:TextBox ID="MicrobialOrderDate" runat="server" /></td>
                            <td><span><asp:RadioButton ID="MicrobialPass" Text="Pass" GroupName="MicrobialTest" runat="server"/>
                                <asp:RadioButton ID="MicrobialFail" GroupName="MicrobialTest" Text="Fail" runat="server"/></span></td>
                        </tr>
                        <tr>
                            <td>Toxicology</td>
                            <td><asp:CheckBox Text="Ordered" ID="ToxicologyOrdered" runat="server"/></td>
                            <td><asp:TextBox ID="ToxicologyOrderDate" runat="server" /></td>
                            <td><span><asp:RadioButton ID="ToxicologyPass" GroupName="ToxicologyTest" Text="Pass" runat="server"/>
                                <asp:RadioButton ID="ToxicologyFail" Text="Fail" GroupName="ToxicologyTest" runat="server"/></span></td>
                        </tr>
                        <tr>
                            <td>Genetics</td>
                            <td><asp:CheckBox Text="Ordered" ID="GeneticsOrdered" runat="server"/></td>
                            <td><asp:TextBox ID="GeneticsOrderDate" runat="server" /></td>
                            <td><span><asp:RadioButton ID="GeneticsPass" Text="Pass" GroupName="GeneticsTest" runat="server"/>
                                <asp:RadioButton ID="GeneticsFail" Text="Fail" GroupName="GeneticsTest" runat="server"/></span></td>
                        </tr>
                    </table>
                </div>
                <div class="input">
                    <label for="Lot">Lot</label>
                    <asp:TextBox ID="Lot" runat="server" />
                </div>
                
            </div>
            <div id="NavControls">
                <asp:Button ID="Update" Text="Update" OnClick="Update_OnClick" runat="server"/>
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
