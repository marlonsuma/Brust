<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DonorForms.aspx.cs" Inherits="NiQ_Donor_Tracking_System.DonorForms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Donor Forms</title>
        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        
        function jssearchtextchange() {

            $('#TextBox1').val("");
            $('#ddrstatus').val("Select Status")  ;
        }
        function jsnametextchnage() {
            $('#searchTextBox').val("");
            $('#ddrstatus').val("Select Status") ;
            
        }
        function statuschanged() {
            $('#searchTextBox').val("");
            $('#TextBox1').val("");
          
        } 

    </script>
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

        #SearchBtn {
            float: right;
        }

        #searchTextBox {
            width: 270px;
            margin-top: 1px;
        }

        #ResultMessage {
            color: red;
            font-size: 12pt;
            font-weight: bold;
        }

        #search {
            float: left;
        }

        #donorDisplay {
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
            margin-left: 50px;
        }

        .value {
            float: left;
            margin-left: 4px;
        }

        .mailing {
            display: block;
        }
    </style>
</head>
<body>
    <div id="pageHeader">
        <img id="logo" src="/images/Ni-Q Logo.png" />
        <h1>Donor Tracking System</h1>
        <h3>Donor Forms</h3>
    </div>
    <form id="form1" runat="server">
        <asp:HiddenField ID="SelectedDonorId" runat="server" />
        <div style="width: 1050px; margin-left: 45px">
            <div id="search">
                <asp:Label runat="server" Text="Donor Id" /><br />
                
                    <asp:TextBox  ID="searchTextBox" onchange="return jssearchtextchange()" runat="server"/>
                    <asp:Label runat="server" Text="Name" />
                      <asp:TextBox runat="server" ID="TextBox1" onchange="return jsnametextchnage()"/>
                    <asp:Label runat="server" Text="Status" CssClass="formLabel" />
                    <asp:DropDownList runat="server" ID="ddrstatus">
                        <asp:ListItem>Select Status</asp:ListItem>
                        <asp:ListItem> Active</asp:ListItem>
                        <asp:ListItem>In Active</asp:ListItem>
                    </asp:DropDownList>

                    <asp:Button ID="SearchBtn" Text="Search" runat="server" OnClick="SearchBtn_Click" />
                
            <br />
            <asp:Label ID="ResultMessage" Text="[Results]" runat="server" />
            <br />
            <div id="formFields" runat="server">
                <div class="input">
                    <p>
                        <asp:CheckBox ID="receivedConsent" Text="Received consent form?" runat="server" />
                    </p>
                    <p>
                        <asp:CheckBox ID="receivedFinancial" Text="Received financial form?" runat="server" />
                    </p>
                </div>
                <br />
            </div>
            <div style="width: 350px">
                <asp:Button ID="SaveButton" Text="Save" Style="float: right" runat="server" OnClick="SaveButton_Click" />
                <asp:Button ID="CancelBtn" Text="Cancel" Style="float: left" runat="server" OnClick="CancelBtn_Click" />
            </div>

        </div>
        <div id="donorDisplay" runat="server">
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
                <asp:Label ID="donorResultLabel" Text="Donor" runat="server" /></h5>
            <div class="gridContainer">
                <asp:GridView ID="donorGrid" AutoGenerateSelectButton="True" OnSelectedIndexChanged="donorGrid_OnSelectedIndexChanged" CssClass="resultGrid" runat="server">
                    <HeaderStyle Height="10px" />
                    <RowStyle Height="10px" Wrap="True" />
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
