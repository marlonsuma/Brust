<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMilkTestResults.aspx.cs" Inherits="NiQ_Donor_Tracking_System.frmMilkTestResults" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ni-Q Donor Tracking System - Record Milk Test Results</title>
</head>
<body>
    <form id="frmMilkTestResults" runat="server">    
        <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 100; left: 45px; position: absolute;
            top: 115px" Text="Donor Tracking System" Width="334px"></asp:Label>
        <asp:Label ID="lblSubTitle" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="16pt" Style="z-index: 101; left: 45px; position: absolute; 
            top: 143px" Text="Record Milk Test Results" Width="334px"></asp:Label>
        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Ni-Q Logo.png" Style="z-index: 108;
            left: 22px; position: absolute; top: 4px" />
        <asp:Label ID="lblMilkKitID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 196px; width: 235px;" Text="Milk Collection Kit ID:"></asp:Label>
        <asp:TextBox ID="txtMilkKitID" runat="server" Font-Names="Arial" Font-Size="10pt" MaxLength="9" Style="z-index: 2; left: 45px; position: absolute; top: 216px; width: 254px; bottom: 419px;" TabIndex="1"></asp:TextBox>
        <asp:Button ID="btnCancel" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnCancel_Click" Style="z-index: 106; left: 45px; position: absolute; top: 450px" Text="Cancel" Width="88px" UseSubmitBehavior="False" TabIndex="5" />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Height="110px" Style="z-index: 101; left: 45px;
            position: absolute; top: 490px; width: 673px;"></asp:Label>
        <asp:Label ID="lblMicrobialTest" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 46px; position: absolute; top: 253px; width: 136px;" Text="Microbial Test:"></asp:Label>
        <asp:RadioButton ID="radMicrobialPass" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpMicrobial" style="z-index: 1; left: 46px; top: 273px; position: absolute" Text="Pass" Enabled="False" AutoPostBack="True" OnCheckedChanged="radMicrobialPass_CheckedChanged" />
        <asp:RadioButton ID="radMicrobialFail" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpMicrobial" style="z-index: 1; left: 111px; top: 273px; position: absolute" Text="Fail" Enabled="False" AutoPostBack="True" OnCheckedChanged="radMicrobialFail_CheckedChanged" />
        <asp:Label ID="lblToxicologyTest" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 301px; width: 183px;" Text="Toxicology"></asp:Label>
        <asp:RadioButton ID="radToxiPass" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpToxi" style="z-index: 1; left: 46px; top: 320px; position: absolute" Text="Pass" Enabled="False" AutoPostBack="True" OnCheckedChanged="radToxiPass_CheckedChanged" />
        <asp:RadioButton ID="radToxiFail" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpToxi" style="z-index: 1; left: 111px; top: 320px; position: absolute; height: 20px;" Text="Fail" Enabled="False" AutoPostBack="True" OnCheckedChanged="radToxiFail_CheckedChanged" />
        <asp:Label ID="lblGeneticsTest" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="z-index: 1; left: 45px; position: absolute; top: 350px; width: 136px;" Text="Genetics"></asp:Label>
        <asp:RadioButton ID="radGeneticPass" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpGenetics" style="z-index: 1; left: 46px; top: 369px; position: absolute" Text="Pass" Enabled="False" AutoPostBack="True" OnCheckedChanged="radGeneticPass_CheckedChanged" />
        <asp:RadioButton ID="radGeneticFail" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpGenetics" style="z-index: 1; left: 111px; top: 369px; position: absolute" Text="Fail" Enabled="False" AutoPostBack="True" OnCheckedChanged="radGeneticFail_CheckedChanged" />
        <asp:RadioButton ID="radGeneticNA" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="grpGenetics" style="z-index: 1; left:170px; top: 369px; position: absolute" Text="N/A" Enabled="False" AutoPostBack="True" OnCheckedChanged="radGeneticsNA_CheckedChanged" />
        <asp:CheckBox ID="checkBoxFinalize" runat="server" style="z-index: 1; left: 45px; top: 410px; position:absolute; right: 1039px; width:auto" Enabled="false" Text="Finalize Results" />
        <asp:Button ID="btnSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSubmit_Click" Style="z-index: 106; left: 170px; position: absolute; top: 450px; width: 88px;" Text="Save" UseSubmitBehavior="False" TabIndex="4" Visible="False" />

        
        <asp:Button ID="btnSearch" runat="server" Font-Names="Arial" Font-Size="10pt" OnClick="btnSearch_Click" Style="z-index: 106; left: 313px; position: absolute; top: 213px; width: 65px;" Text="Search" TabIndex="4" />
        <div style="position:relative; left: 300px; top: 100px; border: #ccc 1px solid; height: 300px;">
            
        </div>
    </form>
</body>
</html>
