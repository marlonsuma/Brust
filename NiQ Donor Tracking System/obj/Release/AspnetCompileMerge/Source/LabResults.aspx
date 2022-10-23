<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LabResults.aspx.cs" Inherits="NiQ_Donor_Tracking_System.LabResults" %>

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

        #SearchControls label,
        #PrintControls label {
            display: block;
            font-weight: bold;
        }

        #SearchBtn, #ClearSearchBtn,
        #SaveOrder, #Print {
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

        #LabOrderNav, #Save {
            float: right;
        }

        .responseContainer {
            margin: 20px 0;
        }

        fieldset.radioGroup {
            margin: 10px 0;
        }

        fieldset.radioGroup legend {
            font-weight: bold;
        }

        fieldset.radioGroup label {
            margin-right: 10px;
        }

        .finalize {
            margin: 10px 0;
            font-weight: bold;
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
        <h3>Lab Results</h3>
    </div>
    <form id="LabOrderForm" runat="server">
        <asp:HiddenField ID="SelectedMilkKit" runat="server" />
        <asp:HiddenField ID="SelectedLabOrder" runat="server" />
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
            <div id="LabResultControls" runat="server">
                <fieldset id="MicrobialTestResult" class="radioGroup" runat="server">
                    <legend>Microbial Test</legend>
                    <asp:RadioButton ID="MicrobialPass" GroupName="MicrobialTest" Text="Pass" runat="server" OnCheckedChanged="MicrobialPass_CheckedChanged" AutoPostBack="true" />
                    <asp:RadioButton ID="MicrobialFail" GroupName="MicrobialTest" Text="Fail" runat="server" OnCheckedChanged="MicrobialPass_CheckedChanged"  AutoPostBack="true"  />
                    <asp:Panel ID="testData"  runat="server" style="margin-top:5px; margin-left: 5px; border: #ccc 1px solid; width: 200px; padding: 10px;" Visible="False">
                        <label style="font-weight:bold; display:block;">APC</label>
                        <asp:TextBox ID="apc" runat="server" Width="163px"></asp:TextBox>

                        <label style="font-weight:bold; display:block; margin-top:5px;">EB</label>
                        <asp:TextBox ID="eb" runat="server" Width="163px"></asp:TextBox>

                        <label style="font-weight:bold; display:block; margin-top:5px;">CC</label>
                        <asp:TextBox ID="cc" runat="server" Width="163px"></asp:TextBox>

                         <label style="font-weight:bold; display:block; margin-top:5px;">RYM</label>
                         <asp:TextBox ID="rym" runat="server" Width="163px"></asp:TextBox>

                        <label style="font-weight:bold; display:block; margin-top:5px;">MOLD</label>
                        <asp:RadioButton ID="moldPass" GroupName="mold" Text="Positive" runat="server" />
                        <asp:RadioButton ID="moldFail" GroupName="mold" Text="Negative" runat="server" Checked="True" />

                         <label style="font-weight:bold; display:block; margin-top:5px;">STX</label>
                        <asp:RadioButton ID="stxPass" GroupName="stx" Text="Positive" runat="server" />
                        <asp:RadioButton ID="stxFail" GroupName="stx" Text="Negative" runat="server" Checked="True" />

                        <label style="font-weight:bold; display:block; margin-top:5px;">E.COLI</label>
                        <asp:RadioButton ID="ecoliPass" GroupName="ecoli" Text="Positive" runat="server" />
                        <asp:RadioButton ID="ecoliFail" GroupName="ecoli" Text="Negative" runat="server" Checked="True" />

                        <label style="font-weight:bold; display:block; margin-top:5px;">SAL</label>
                        <asp:RadioButton ID="salPass" GroupName="sal" Text="Positive" runat="server" />
                        <asp:RadioButton ID="salFail" GroupName="sal" Text="Negative" runat="server" Checked="True" />
                    </asp:Panel>
                </fieldset>
                <fieldset id="ToxicologyTestResult" class="radioGroup" runat="server">
                    <legend>Toxicology Test</legend>
                    <asp:RadioButton ID="ToxicologyPass" GroupName="ToxicologyTest" Text="Pass" runat="server" />
                    <asp:RadioButton ID="ToxicologyFail" GroupName="ToxicologyTest" Text="Fail" runat="server" />
                </fieldset>
                <fieldset id="GeneticsTestResult" class="radioGroup" runat="server">
                    <legend>Genetics Test</legend>
                    <asp:RadioButton ID="GeneticPass" GroupName="GeneticsTest" Text="Pass" runat="server" />
                    <asp:RadioButton ID="GeneticFail" GroupName="GeneticsTest" Text="Fail" runat="server" />
                    <asp:RadioButton ID="GeneticNa" GroupName="GeneticsTest" Text="N/A" runat="server" />
                </fieldset>
                 <fieldset id="Grade" class="radioGroup" runat="server">
                    <legend>Milk Grade</legend>
                    <asp:RadioButton ID="grade1" GroupName="grade" Text="Grade 1" runat="server" />
                    <asp:RadioButton ID="grade2" GroupName="grade" Text="Grade 2" runat="server" />
                   
                </fieldset>
                <div class="finalize">
                    <asp:CheckBox ID="Finalize" Text="Finalize" runat="server" />
                </div>
            </div>
            <div id="NavControls">
                <asp:Button ID="Save" Text="Save" OnClick="Save_Clicked" runat="server" />
                <asp:LinkButton ID="LabOrderNav" Text="Lab Orders" OnClick="LabOrderNav_OnClick" runat="server" />
                <asp:Button ID="Cancel" Text="Cancel" OnClick="Cancel_OnClick" runat="server" />
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
                 <div class="description">APC</div>
                <div class="value">
                    <asp:Label ID="APCStatus" Text="N/A" runat="server" />
                </div>
            </div>
             <div class="field">
                <div class="description">EB</div>
                <div class="value">
                    <asp:Label ID="ebStatus" Text="N/A" runat="server" />
                </div>
            </div>

             <div class="field">
                <div class="description">CC</div>
                <div class="value">
                    <asp:Label ID="ccStatus" Text="N/A" runat="server" />
                </div>
            </div>

             <div class="field">
                <div class="description">RYM</div>
                <div class="value">
                    <asp:Label ID="rymStatus" Text="N/A" runat="server" />
                </div>
            </div>
            <div class="field">
                <div class="description">Mold</div>
                <div class="value">
                    <asp:Label ID="moldStatus" Text="Negative" runat="server" />
                </div>
            </div>
              <div class="field">
                <div class="description">STX</div>
                <div class="value">
                    <asp:Label ID="stxStatus" Text="Negative" runat="server" />
                </div>
            </div>
             <div class="field">
                <div class="description">E.Coli</div>
                <div class="value">
                    <asp:Label ID="ecoliStatus" Text="Negative" runat="server" />
                </div>
            </div>
             <div class="field">
                <div class="description">SAL</div>
                <div class="value">
                    <asp:Label ID="salStatus" Text="Negative" runat="server" />
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
                    <asp:Label ID="GeneticsStatus" Text="Not Ordered" runat="server" />
                </div>
            </div>
             <div class="field">
                <div class="description">Grade</div>
                <div class="value">
                    <asp:Label ID="gradeStatus" Text="N/A" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
