<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transactions.aspx.cs" Inherits="NiQ_Donor_Tracking_System.Transactions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <style>
        #item{
            margin-bottom: 10px;
        }

        .searchArea{
            padding-bottom: 20px;
        }

        #back{
            margin-top: 20px;
        }
    </style>
<body>
     <div id="pageHeader">
        <img id="logo" src="/images/Ni-Q Logo.png" />
        <h1>Donor Tracking System</h1>
        <h3>Transactions
        </h3>
    </div>
    <form id="form1" runat="server">
        <div class="searchArea">
            <asp:TextBox ID="Item" runat="server" style="margin-top: 0px" Height="20px" Width="198px"></asp:TextBox>
            <asp:Button ID="search" runat="server" Text="Search" Height="27px" OnClick="search_Click" />
        </div>
        
        <asp:GridView ID="GridView1" runat="server" Width="100%">
        </asp:GridView>
        <asp:Button ID="back" runat="server" Text="Back" OnClick="back_Click" />
    </form>
   
</body>
</html>
