<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MilkKitReport.aspx.cs" Inherits="NiQ_Donor_Tracking_System.MilkKitReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<body>
      <style>

          .section{
              padding: 20px;
              border: #ccc 1px solid;
          }

          .donorFieldslbl{
             margin-top: 30px;
          }

           .container{
             display:inline-block;
             vertical-align:top;
             width: 250px;
          }
    </style>

     <div id="pageHeader">
        <img id="logo" src="/images/Ni-Q Logo.png" />
        <h1>Donor Tracking System</h1>
        <h3>Milk Kit Reports</h3>
    </div>
    <form id="MilkKitReportForm" runat="server">
        <div class="section">
            <h3>Pick what fields you want to show on your report</h3>
            <div class="container">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Milk Kit Fields"></asp:Label>
                <asp:CheckBoxList ID="MilkKitFields" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="0" CellSpacing="10" Width="206px">
                    <asp:ListItem Selected="True">Donor ID</asp:ListItem>
                    <asp:ListItem Selected="True" Value="Barcode">Milk Kit Barcode</asp:ListItem>
                    <asp:ListItem Selected="True">Recieved Date</asp:ListItem>
                    <asp:ListItem Selected="True">Quarintined Date</asp:ListItem>
                    <asp:ListItem Selected="True">Finalized Date</asp:ListItem>
                    <asp:ListItem Selected="True">Paid Date</asp:ListItem>
                    <asp:ListItem Selected="True">Volume</asp:ListItem>
                    <asp:ListItem Selected="True">Grade</asp:ListItem>
                    <asp:ListItem Value="LotID">LotID</asp:ListItem>
                    <asp:ListItem Value="PalletID">PalletID</asp:ListItem>
                    <asp:ListItem Selected="True">Shipping Service</asp:ListItem>
                    <asp:ListItem Selected="True">Tracking Number</asp:ListItem>
                    <asp:ListItem Selected="True">DNATest</asp:ListItem>
                    <asp:ListItem Selected="True">Toxicoligy</asp:ListItem>
                    <asp:ListItem Selected="True">Microbial</asp:ListItem>
                    <asp:ListItem Selected="True">APC</asp:ListItem>
                    <asp:ListItem Selected="True">EB</asp:ListItem>
                    <asp:ListItem Selected="True">CC</asp:ListItem>
                    <asp:ListItem Selected="True">RYM</asp:ListItem>
                    <asp:ListItem Selected="True">STX</asp:ListItem>
                    <asp:ListItem Selected="True">E.Coli</asp:ListItem>
                    <asp:ListItem Selected="True">SAL</asp:ListItem>
                    <asp:ListItem Selected="True">Active</asp:ListItem>
           
                </asp:CheckBoxList>
            </div>
            <div class="container">
                <asp:Label ID="donorFieldslbl" runat="server" Font-Bold="True" Text="Donor Fields"></asp:Label>
                <asp:CheckBoxList ID="DonorFields" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="0" CellSpacing="10" Width="206px">
                    <asp:ListItem Selected="True">Donor ID</asp:ListItem>
                    <asp:ListItem Selected="True">Donor First Name</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Last Name</asp:ListItem>
                    <asp:ListItem Selected="True">Donor First Name</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Email</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Active</asp:ListItem>
                    <asp:ListItem Selected="False">Donor Received Consent Form</asp:ListItem>
                    <asp:ListItem Selected="False">Donor Received Financial Form</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Inactive Date</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Inactive Reason</asp:ListItem>
                </asp:CheckBoxList>
            </div>
             <div class="container">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Lot Fields"></asp:Label>
                <asp:CheckBoxList ID="LotFields" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="0" CellSpacing="10" Width="206px">
                    <asp:ListItem Selected="True">Donor ID</asp:ListItem>
                    <asp:ListItem Selected="True">Donor First Name</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Last Name</asp:ListItem>
                    <asp:ListItem Selected="True">Donor First Name</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Email</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Active</asp:ListItem>
                    <asp:ListItem Selected="False">Donor Received Consent Form</asp:ListItem>
                    <asp:ListItem Selected="False">Donor Received Financial Form</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Inactive Date</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Inactive Reason</asp:ListItem>
                </asp:CheckBoxList>
               
            </div>
            <div class="container">
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Pallet Fields"></asp:Label>
                <asp:CheckBoxList ID="PalletFields" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="0" CellSpacing="10" Width="206px">
                    <asp:ListItem Selected="True">Donor ID</asp:ListItem>
                    <asp:ListItem Selected="True">Donor First Name</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Last Name</asp:ListItem>
                    <asp:ListItem Selected="True">Donor First Name</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Email</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Active</asp:ListItem>
                    <asp:ListItem Selected="False">Donor Received Consent Form</asp:ListItem>
                    <asp:ListItem Selected="False">Donor Received Financial Form</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Inactive Date</asp:ListItem>
                    <asp:ListItem Selected="True">Donor Inactive Reason</asp:ListItem>
                </asp:CheckBoxList>
            </div>
        </div>
        <div class="section">
            <h3>Milk Kit Received Condition</h3>
            <p>By Setting this condition you will only show milk kits that have been received or not been received base on your selection below.</p>
            <div class="container">
                <asp:Label ID="Label4" runat="server" Font-Bold="True" Text="Milk Kit Fields"></asp:Label>
                <asp:RadioButtonList ID="RecievedCondition" runat="server">
                    <asp:ListItem Selected="False">Has Been Received</asp:ListItem>
                    <asp:ListItem Selected="False">Has Not Been Received</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
         <div class="section">
            <h3>Milk Kit Qaurintined Condition</h3>
            <p>By Setting this condition you will only show milk kits that have been quarintined or have not been quarintined base on your selection below.</p>
            <div class="container">
                <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="Milk Kit Fields"></asp:Label>
                <asp:RadioButtonList ID="QuarintineCondition" runat="server">
                    <asp:ListItem Selected="False">Has Been Quarintined</asp:ListItem>
                    <asp:ListItem Selected="False">Has Not Been Quarintined</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="section">
            <h3>Milk Kit DNA Test Condition</h3>
            <p>By Setting this condition you will only show milk kits that have passed or failed the DNA Test based on your selection below.</p>
            <div class="container">
                <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="Milk Kit Fields"></asp:Label>
                <asp:RadioButtonList ID="DNATestCondition" runat="server">
                    <asp:ListItem Selected="False">Has Passed DNA Test</asp:ListItem>
                    <asp:ListItem Selected="False">Has Failed DNA Test</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
         <div class="section">
            <h3>Milk Kit Toxicology Condition</h3>
            <p>By Setting this condition you will only show milk kits that have passed or failed the Toxicology Test based on your selection below.</p>
            <div class="container">
                <asp:Label ID="Label7" runat="server" Font-Bold="True" Text="Milk Kit Fields"></asp:Label>
                <asp:RadioButtonList ID="ToxicologyCondition" runat="server">
                    <asp:ListItem Selected="False">Has Passed Toxicology Test</asp:ListItem>
                    <asp:ListItem Selected="False">Has Failed Toxicology Test</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
         <div class="section">
            <h3>Milk Kit Microbial Condition</h3>
            <p>By Setting this condition you will only show milk kits that have passed or failed the Microbial Test based on your selection below.</p>
            <div class="container">
                <asp:Label ID="Label8" runat="server" Font-Bold="True" Text="Milk Kit Fields"></asp:Label>
                <asp:RadioButtonList ID="MicrobialCondition" runat="server">
                    <asp:ListItem Selected="False">Has Passed Microbial Test</asp:ListItem>
                    <asp:ListItem Selected="False">Has Failed Microbial Test</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
         <div class="section">
            <h3>Milk Kit Finalized Condition</h3>
            <p>By Setting this condition you will only show milk kits that have been finalized or have not been finalized base on your selection below.</p>
            <div class="container">
                <asp:Label ID="Label9" runat="server" Font-Bold="True" Text="Milk Kit Fields"></asp:Label>
                <asp:RadioButtonList ID="FinalizedCondition" runat="server">
                    <asp:ListItem Selected="False">Has Been Finalized</asp:ListItem>
                    <asp:ListItem Selected="False">Has Not Been Finalized</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>

         <div class="section">
            <h3>Milk Kit Paid Condition</h3>
            <p>By Setting this condition you will only show milk kits that have been Paid or have not been Paid base on your selection below.</p>
            <div class="container">
                <asp:Label ID="Label10" runat="server" Font-Bold="True" Text="Milk Kit Fields"></asp:Label>
                <asp:RadioButtonList ID="PaidCondition" runat="server">
                    <asp:ListItem Selected="False">Has Been Paid</asp:ListItem>
                    <asp:ListItem Selected="False">Has Not Been Paid</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>

        <div class="section">
            <h3>Milk Kit Lot Condition</h3>
            <p>By Setting this condition you will only show milk kits that have been Paid or have not been Paid base on your selection below.</p>
            <div class="container">
                <asp:Label ID="Label11" runat="server" Font-Bold="True" Text="Milk Kit Fields"></asp:Label>
                <asp:RadioButtonList ID="LotCondition" runat="server">
                    <asp:ListItem Selected="False">In A Lot</asp:ListItem>
                    <asp:ListItem Selected="False">Not In A Lot</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>

         <div class="section">
            <h3>Milk Kit Pallet Condition</h3>
            <p>By Setting this condition you will only show milk kits that have been Paid or have not been Paid base on your selection below.</p>
            <div class="container">
                <asp:Label ID="Label12" runat="server" Font-Bold="True" Text="Milk Kit Fields"></asp:Label>
                <asp:RadioButtonList ID="PalletCondition" runat="server">
                    <asp:ListItem Selected="False">In A Pallet</asp:ListItem>
                    <asp:ListItem Selected="False">Not In A Pallet</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>

        <div class="section">
            <h3>Milk Kit Grade Condition</h3>
            <p>By Setting this condition you will only show milk kits that have been Paid or have not been Paid base on your selection below.</p>
            <div class="container">
                <asp:Label ID="Label13" runat="server" Font-Bold="True" Text="Milk Kit Fields"></asp:Label>
                <asp:RadioButtonList ID="GradeCondition" runat="server">
                    <asp:ListItem Selected="False">Must Be Grade 1</asp:ListItem>
                    <asp:ListItem Selected="False">Must Be Grade 2</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>

        <div class="section">
            <h3>Milk Kit Active Condition</h3>
            <p>By Setting this condition you will only show milk kits that have been Paid or have not been Paid base on your selection below.</p>
            <div class="container">
                <asp:Label ID="Label14" runat="server" Font-Bold="True" Text="Milk Kit Fields"></asp:Label>
                <asp:RadioButtonList ID="ActiveCondition" runat="server">
                    <asp:ListItem Selected="False">Must Be Active</asp:ListItem>
                   
                </asp:RadioButtonList>
            </div>
        </div>
         <div class="section">
            <h3>Milk Kit Report Title</h3>
            <p>By Setting this condition you will only show milk kits that have been Paid or have not been Paid base on your selection below.</p>
            <div class="container">
                <asp:Label ID="Label15" runat="server" Font-Bold="True" Text="Report Title"></asp:Label>
                <div style="margin-bottom:15px;">
                    <asp:TextBox ID="ReportName" runat="server" Width="240px"/>
                </div>
                 <asp:Button ID="SaveReportBtn" runat="server" Text="Save Report" OnClick="SaveReportBtn_Click" />
            </div>
        </div>
        <asp:Label ID="ErrorLabel" runat="server" Font-Bold="True" Text="" Visible="false" ForeColor="Red"></asp:Label>
      
    </form>
</body>
</html>
