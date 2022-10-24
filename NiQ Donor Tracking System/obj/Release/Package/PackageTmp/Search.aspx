<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="NiQ_Donor_Tracking_System.Search" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ni-Q Donor Tracking System - Search</title>
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

        //$('#ddrstatus').change(function() {
        //    $('#searchTextBox').val("");
        //    $('#TextBox1').val("");

        //});
        $('.DropdownList1').change(function () {
            alert("Handler for .change() called.");
        });
    </script>
    <style>
        body {
            font-family: Arial, Helvetica, sans-serif;
        }

        #pageHeader {
            margin-left: 20px;
        }

        #form1 {
            margin-left: 45px;
        }

        .formLabel {
            font-weight: bold;
            font-size: 10pt;
            width: 200px;
        }

        .formField {
            font-size: 10pt;
            width: 195px;
            height: auto;
        }

        .resultContainer {
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

        #ResultMessage {
            font-weight: bold;
            font-size: 12pt;
            color: red;
        }
    </style>
</head>
<body>
    <div id="pageHeader">
        <img id="logo" src="/images/Ni-Q Logo.png" />
        <h1>Donor Tracking System</h1>
        <h3>Search Inventory / Donors</h3>
    </div>
    <form id="form1" runat="server">
        <div>

            <asp:Label runat="server" Text="Inventory/Donor Id" CssClass="formLabel" /><br />
            <%--<input  type="text" Id="searchTextBox"  onfocusout="jssearchtextchange()"></input>--%>
            <asp:TextBox runat="server" ID="searchTextBox" CssClass="formField" onchange="return jssearchtextchange()"/>
             <asp:Label runat="server" Text="Name" CssClass="formLabel" />
            <asp:TextBox runat="server" ID="TextBox1"  onchange=" return jsnametextchnage()"/>
               
                    
            <asp:Label runat="server" Text="Status" CssClass="formLabel" />
            <asp:DropDownList runat="server" CssClass="DropdownList1" ID="ddrstatus" > <asp:ListItem>Select Status</asp:ListItem>
            <asp:ListItem>Active</asp:ListItem> <asp:ListItem>In Active</asp:ListItem> 
            </asp:DropDownList>
                        
            <br />
            <br />
            <div style="width: 200px">
                <asp:Button ID="SearchBtn" Text="Search" Style="float: right" runat="server" OnClick="SearchBtn_Click" />
                <asp:Button ID="CancelBtn" Text="Cancel" Style="float: left" runat="server" OnClick="CancelBtn_Click" />
            </div>
            <br />
            <br />
            <asp:Label ID="ResultMessage" Text="[Results]" runat="server"></asp:Label>
                
        </div>
        <div id="firstResults" class="resultContainer" runat="server">
            <h5>
                <asp:Label ID="firstResultLabel" runat="server" /></h5>
            <div class="gridContainer">
                <asp:GridView ID="firstResultGrid" CssClass="resultGrid" runat="server">
                    <AlternatingRowStyle Height="10px" />
                    <HeaderStyle Height="10px" />
                    <RowStyle Height="10px" Wrap="True" />
                </asp:GridView>
            </div>
        </div>
        <div id="secondResults" class="resultContainer" runat="server">
            <h5>
                <asp:Label ID="secondResultLabel" runat="server" /></h5>
            <div class="gridContainer">
                <asp:GridView ID="secondResultGrid" CssClass="resultGrid" runat="server">
                    <AlternatingRowStyle Height="10px" />
                    <HeaderStyle Height="10px" />
                    <RowStyle Height="10px" Wrap="True" />
                </asp:GridView>
            </div>

        </div>
        <div id="thirdResults" class="resultContainer" runat="server">
            <h5>
                <asp:Label ID="thirdResultLabel" runat="server" /></h5>
            <div class="gridContainer">
                <asp:GridView ID="thirdResultGrid" CssClass="resultGrid" runat="server">
                    <AlternatingRowStyle Height="10px" />
                    <HeaderStyle Height="10px" />
                    <RowStyle Height="10px" Wrap="True" />
                </asp:GridView>
            </div>

        </div>
        <div id="fourthResults" class="resultContainer" runat="server">
            <h5>
                <asp:Label ID="fourthResultLabel" runat="server" /></h5>
            <div class="gridContainer">
                <asp:GridView ID="fourthResultGrid" CssClass="resultGrid" runat="server">
                    <AlternatingRowStyle Height="10px" />
                    <HeaderStyle Height="10px" />
                    <RowStyle Height="10px" Wrap="True" />
                </asp:GridView>
            </div>

        </div>
    </form>
</body>
</html>
