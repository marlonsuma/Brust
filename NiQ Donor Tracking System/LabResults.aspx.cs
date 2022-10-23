using System;
using System.Collections.Generic;
using System.Web.UI;
using DonorTracking.Data;


namespace NiQ_Donor_Tracking_System
{
    public partial class LabResults : Page
    {
        private string _searchValue;
        private Global.UserInfo _userInfo;
        public IDonorRepository DonorRepository { get; set; }
        public ILabKitRepository LabKitRepository { get; set; }
        public IMilkKitRepository MilkKitRepository { get; set; }

        public ITransactionLog Logger { get; set; }

        protected void Cancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void ClearSearchBtn_Click(object sender, EventArgs e)
        {
            ClearResults();
        }

        protected void LabOrderNav_OnClick(object sender, EventArgs e)
        {
            Response.Redirect($"LabOrder.aspx?milkkit={SelectedMilkKit.Value}");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) ClearResults();
            _userInfo = (Global.UserInfo)Session["ui"];
        }

        protected void Save_Clicked(object sender, EventArgs e)
        {

            try
            {
                Dictionary<string, string> changed = new Dictionary<string, string>();
                bool updated = false;

                MilkKit milkKit = MilkKitRepository.Get(SelectedMilkKit.Value);

                if (MicrobialPass.Checked || MicrobialFail.Checked)
                {
                    changed.Add("Microbial Results", MicrobialPass.Checked.ToString());
                    milkKit.MicrobialTest = MicrobialPass.Checked;
                }

                if (ToxicologyPass.Checked || ToxicologyFail.Checked)
                {
                    changed.Add("Toxicology Results", ToxicologyPass.Checked.ToString());
                    milkKit.DrugAlcoholTest = ToxicologyPass.Checked;
                }

                if (GeneticPass.Checked || GeneticFail.Checked)
                {
                    changed.Add("Genetic Results", GeneticPass.Checked.ToString());
                    milkKit.Dnatest = GeneticPass.Checked;
                }

                if (Finalize.Checked)
                {
                    changed.Add("Finalized", DateTime.Now.ToShortDateString());
                    milkKit.Finalized = DateTime.Now;
                }

                if (grade1.Checked)
                {
                    changed.Add("Grade", "1");
                    milkKit.Grade = 1;
                }
                else if (grade2.Checked)
                {
                    changed.Add("Grade", "2");
                    milkKit.Grade = 2;
                }

                int apcValue;
                if (apc.Text != "" &&  int.TryParse(apc.Text, out apcValue))
                {
                    changed.Add("APC", apcValue.ToString());
                    milkKit.APC = apcValue;
                }

                else
                {
                    milkKit.APC = null;
                }

                int ebVal;
                if (eb.Text != "" && int.TryParse(eb.Text, out ebVal))
                {
                    changed.Add("EB", ebVal.ToString());
                    milkKit.EB = ebVal;
                }

                else
                {
                    milkKit.EB = null;
                }

                int ccVal;
                if (cc.Text != "" && int.TryParse(cc.Text, out ccVal))
                {
                    changed.Add("CC", ccVal.ToString());
                    milkKit.CC = ccVal;
                }

                else
                {
                    milkKit.CC = null;
                }

                int rymVal;
                if (rym.Text != "" && int.TryParse(rym.Text, out rymVal))
                {
                    changed.Add("RYM", rymVal.ToString());
                    milkKit.RYM = rymVal;
                }

                else
                {
                    milkKit.RYM = null;
                }

                if (moldPass.Checked || moldFail.Checked)
                {
                    changed.Add("Mold", moldPass.Checked.ToString());
                    milkKit.Mold = moldPass.Checked;
                }

                if (stxPass.Checked || stxFail.Checked)
                {
                    changed.Add("STX", stxPass.Checked.ToString());
                    milkKit.STX = stxPass.Checked;
                }

                if (ecoliPass.Checked || ecoliFail.Checked)
                {
                    changed.Add("E.Coli", ecoliPass.Checked.ToString());
                    milkKit.ECOLI = ecoliPass.Checked;
                }
                if (salPass.Checked || salFail.Checked)
                {
                    changed.Add("SAL", salPass.Checked.ToString());
                    milkKit.SAL = salPass.Checked;
                }

                MilkKitRepository.Update(milkKit);
                //if(Micoralabortoxical orGenetics Test ar passed means  sending as success)
                // IF ANY FAIL then fail its a fail
               // OLD >> if(Convert.ToBoolean(milkKit.MicrobialTest)|| Convert.ToBoolean(milkKit.DrugAlcoholTest)|| Convert.ToBoolean(milkKit.Dnatest))
                if (Convert.ToBoolean(milkKit.MicrobialTest) && Convert.ToBoolean(milkKit.DrugAlcoholTest) && Convert.ToBoolean(milkKit.Dnatest))
                {
                    string toemail, body, Subject;
                    Subject = "Lab Result";
                    toemail = DonorRepository.Get(milkKit.DonorId).Email;
                    body = "Your milk has passed testing and has been approved for payment. We at Ni-Q sincerely value all the time and energy it takes to be a donor with us. Your efforts do not go unnoticed and are deeply appreciated. Ni-Q makes every attempt to schedule payment within 60 days after a kit has been tested and approved. As we have communicated, due to the many moving parts (volume being the main one but recently other international health issues that affect our lab), giving a structured payment date has proven ineffective and payment dates are variable. We also cannot re-iterate enough that Ni-Q is not your employer. We recommend that you consider this as auxiliary income, and do not rely on it as a scheduled paycheck.";
                    if (!string.IsNullOrEmpty(toemail) && toemail != "")
                        EMailHelper.SendEmail(toemail, body, Subject);
                }
                else
                {
                    string toemail, body, Subject;
                    Subject = "Lab Result";
                    toemail = DonorRepository.Get(milkKit.DonorId).Email;
                    body = "Your milk kit was tested for the presence of microbials. The milk kit failed to meet our standards for microbial testing and is not safe to process. This means that there are either too many total microbes present in your donated milk or a potentially harmful form of bacteria was found in your milk. The milk kit was tracked through shipment and double checked to ensure that it was delivered on time with the correct care."
+System.Environment.NewLine+"This is not a diagnosis of any problems you or your child may have but rather a way for us to ensure that all of our processed milk is safe and meets our high standards. Contamination can happen at any step of the collection and storage process."

+System.Environment.NewLine+"To help with any future donations we encourage you to review our General Guidelines for collection and storage of human donor milk.We have provided a list of common issues that may cause contamination, our General Guidelines, and a link to the FDA guidelines for cleaning breast pumps."

+System.Environment.NewLine+"Please let me know if you have any concerns or questions after looking through this.I am happy to work together to make your donations successful."

+ System.Environment.NewLine + "Common Ways Milk Can Become Contaminated"

+ System.Environment.NewLine + "1.Improperly cleaning pump. Refer to provided FDA guidelines."

+ System.Environment.NewLine + "2.Improperly cleaning your hands and breast before pumping or handling human donor milk(Wash hands with soap and water for at least 20 seconds before pumping and before handling stored milk.Hand sanitizer or wipes may not be enough to kill harmful bacteria.)."

+ System.Environment.NewLine + "3.Wash your breast before and after breastfeeding and pumping."

+ System.Environment.NewLine + "4.Improper storage of pumped milk. Refer to General Guidelines for milk storage."


+ System.Environment.NewLine + "https://www.fda.gov/medicaldevices/productsandmedicalprocedures/homehealthandconsumer/consumerproducts/breastpumps/ucm061950.htm"

+ System.Environment.NewLine + "Refer to the GENERAL GUIDELINES FOR COLLECTING HDM AND STORAGE form"


+ System.Environment.NewLine + "Also the testing section of the consent form, for reference to testing details"

+ System.Environment.NewLine + "Ni - Q will pay you One Dollar($1.00) for each full ounce of your extra milk that you ship to Ni - Q, as long as:"
+ System.Environment.NewLine + "You sign and return this Consent to Ni - Q; ____"

+ System.Environment.NewLine + "You comply with all of Ni - Q's Collection, Storage, and Shipping Required Practices, as amended from time to time (with milk collection kits, available on the donor portal, and can also be obtained by contacting Ni-Q's); ____"

+ System.Environment.NewLine + "As a donor you understand that the milk kit is pooled and tested____"

+ System.Environment.NewLine + "Ni - Q tests and confirms that the milk shipped to Ni - Q by you is one hundred percent(100 %) human breast milk by protein analysis, ruling out foreign bodies such as cows or plant-based milk.____"

+ System.Environment.NewLine + "Ni - Q tests and confirms that the milk sent to Ni - Q by you is free of drug and alcohol[residuals] and does not contain any harmful levels of pathogens.__"

+ System.Environment.NewLine + "If Ni - Q determines that it cannot use your shipped milk, because one or more of the requirements in paragraph 2, directly preceding this paragraph, are not met, Ni - Q will communicate this to you as soon as reasonably possible, indicating the portion of milk that is not compliant.You will only be notified which category failed(microbial, toxicology, protein analysis) and not given a detailed report.__"

+ System.Environment.NewLine + "Ni - Q will not return any of your shipped milk, including any of your shipped milk that is found to be non-compliant, for any reason.___";
                    if (!string.IsNullOrEmpty(toemail) && toemail != "")
                        EMailHelper.SendEmail(toemail, body, Subject);
                }
            



                LabKit labKit = LabKitRepository.Get(SelectedLabOrder.Value);
                Logger.Add(TransactionType.LabResults, _userInfo.ID.ToString(), ItemType.MilkKit, milkKit.Id, changed);
                ClearResults();
                SelectedMilkKit.Value = milkKit.Barcode;
                ResultMessage.Visible = true;
                ResultMessage.Text = "Lab results saved";

                if (!milkKit.Finalized.HasValue && 
                    !labKit.GeneticOrdered.HasValue ||
                    !labKit.ToxicologyOrdered.HasValue || 
                    !labKit.MicrobialOrdered.HasValue) LabOrderNav.Visible = true;
            }
            catch //(Exception exception)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "An error occured saving lab results.";
            }
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            _searchValue = SearchTextBox.Text.Trim().Replace("'", "''");

            if (string.IsNullOrEmpty(_searchValue))
                SearchTextBox.Focus();
            else
            {
                ClearResults();
                SearchMilkKit();
            }
        }

        private void ClearResults()
        {
            LabResultControls.Visible = false;
            MilkKitDisplay.Visible = false;

            SearchTextBox.Text = string.Empty;
            SearchTextBox.Enabled = true;
            SearchBtn.Visible = true;
            ClearSearchBtn.Visible = false;

            ResultMessage.Visible = false;

            Save.Visible = false;
            LabOrderNav.Visible = false;

            MicrobialTestResult.Disabled = false;
            MicrobialPass.Checked = false;
            MicrobialFail.Checked = false;

            ToxicologyTestResult.Disabled = false;
            ToxicologyPass.Checked = false;
            ToxicologyFail.Checked = false;

            GeneticsTestResult.Disabled = false;
            GeneticPass.Checked = false;
            GeneticFail.Checked = false;
            GeneticNa.Checked = false;

            SelectedMilkKit.Value = string.Empty;
            SearchTextBox.Focus();
            testData.Visible = false;
        }

        private void SearchMilkKit()
        {
            MilkKit milkKit = MilkKitRepository.GetWithLot(_searchValue);

            if (milkKit == null)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "No Records Found";

                return;
            }

            LabKit labKit = LabKitRepository.GetByMilkKit(milkKit.Id);

            if (labKit == null || !labKit.MicrobialOrdered.HasValue && !labKit.ToxicologyOrdered.HasValue &&
                !labKit.GeneticOrdered.HasValue)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "No Lab Orders placed for this kit.";
                SelectedMilkKit.Value = milkKit.Barcode;
                LabOrderNav.Visible = true;

                return;
            }

            SetSelectedMilkKit(milkKit, labKit);
        }

        private void SetSelectedMilkKit(MilkKit milkKit, LabKit labKit)
        {
            if (milkKit.Finalized.HasValue)
            {
                SearchTextBox.Text = string.Empty;
                ResultMessage.Visible = true;
                ResultMessage.Text = $"Milk kit {milkKit.Barcode} has been finalized.";

                return;
            }

            Donor donor = DonorRepository.Get(milkKit.DonorId);
            Barcode.Text = milkKit.Barcode;
            DonorId.Text = milkKit.DonorId;
            DonorName.Text = donor != null ? $"{donor.LastName}, {donor.FirstName}" : string.Empty;
            Volume.Text = milkKit.Volume;
            ReceiveDate.Text = milkKit.ReceiveDate?.ToString("d") ?? string.Empty;
            QuarantineDate.Text = milkKit.QuarantineDate?.ToString("d") ?? string.Empty;
            MicrobialStatus.Text =
                labKit.MicrobialOrdered.HasValue
                    ? milkKit.MicrobialTest.HasValue ? milkKit.MicrobialTest.Value ? "Passed" : "Failed" :
                    $"Ordered {labKit.MicrobialOrdered.Value:d}"
                    : "Not Ordered";
            ToxicologyStatus.Text = labKit.ToxicologyOrdered.HasValue
                ? milkKit.DrugAlcoholTest.HasValue ? milkKit.DrugAlcoholTest.Value ? "Passed" : "Failed" :
                $"Ordered {labKit.ToxicologyOrdered.Value:d}"
                : "Not Ordered";
            GeneticsStatus.Text = labKit.GeneticOrdered.HasValue
                ? milkKit.Dnatest.HasValue ? milkKit.Dnatest.Value ? "Passed" : "Failed" :
                $"Ordered {labKit.GeneticOrdered.Value:d}"
                : "Not Ordered";

            SelectedMilkKit.Value = milkKit.Barcode;
            SelectedLabOrder.Value = labKit.Barcode;
            SearchTextBox.Text = milkKit.Barcode;
            SearchTextBox.Enabled = false;
            SearchBtn.Visible = false;
            ClearSearchBtn.Visible = true;
            MilkKitDisplay.Visible = true;
            LabResultControls.Visible = true;
           
            apc.Text = milkKit.APC.HasValue ? milkKit.APC.Value.ToString():"";
            eb.Text = milkKit.EB.HasValue ? milkKit.EB.Value.ToString():"";
            cc.Text = milkKit.CC.HasValue ? milkKit.CC.Value.ToString():"";
            rym.Text = milkKit.RYM.HasValue ? milkKit.RYM.Value.ToString():"";

            APCStatus.Text = milkKit.APC.HasValue ? milkKit.APC.Value.ToString() : "N/A";
            ebStatus.Text = milkKit.EB.HasValue ? milkKit.EB.Value.ToString() : "N/A";
            ccStatus.Text = milkKit.CC.HasValue ? milkKit.CC.Value.ToString() : "N/A";
            rymStatus.Text = milkKit.RYM.HasValue ? milkKit.RYM.Value.ToString() : "N/A";


            if (milkKit.Mold.HasValue && milkKit.Mold.Value == true)
            {
                moldFail.Checked = false;
                moldPass.Checked = true;
                moldStatus.Text = "Positive";
            }

            else
            {
                moldFail.Checked = true;
                moldPass.Checked = false;
                moldStatus.Text = "Negative";
            }

            if (milkKit.STX.HasValue && milkKit.STX.Value == true)
            {
                stxFail.Checked = false;
                stxPass.Checked = true;
                stxStatus.Text = "Positive";
            }

            else
            {
                stxFail.Checked = true;
                stxPass.Checked = false;
                stxStatus.Text = "Negative";
            }

            if (milkKit.ECOLI.HasValue && milkKit.ECOLI.Value == true)
            {
                ecoliFail.Checked = false;
                ecoliPass.Checked = true;
                ecoliStatus.Text = "Positive";
            }

            else
            {
                ecoliFail.Checked = true;
                ecoliPass.Checked = false;
                ecoliStatus.Text = "Negative";
            }

            if (milkKit.SAL.HasValue && milkKit.SAL.Value == true)
            {
                salFail.Checked = false;
                salPass.Checked = true;
                salStatus.Text = "Positive";
            }

            else
            {
                salFail.Checked = true;
                salPass.Checked = false;
                salStatus.Text = "Negative";
            }

            if (milkKit.Grade == 1)
            {
                grade1.Checked = true;
                gradeStatus.Text = "1";
            }
            else
            {
                grade2.Checked = true;
                gradeStatus.Text = "2";
            }
            
            Save.Visible = true;

            if (!labKit.MicrobialOrdered.HasValue)
                MicrobialTestResult.Disabled = true;
            else if (milkKit.MicrobialTest.HasValue)
            {
                if (milkKit.MicrobialTest.Value)
                {
                    MicrobialPass.Checked = true;
                    testData.Visible = true;
                }

                else
                {
                    MicrobialFail.Checked = true;
                }
                   
            }

            if (!labKit.ToxicologyOrdered.HasValue)
                ToxicologyTestResult.Disabled = true;
            else if (milkKit.DrugAlcoholTest.HasValue)
            {
                if (milkKit.DrugAlcoholTest.Value)
                    ToxicologyPass.Checked = true;
                else
                    ToxicologyFail.Checked = true;
            }

            if (!labKit.GeneticOrdered.HasValue)
                GeneticsTestResult.Disabled = true;
            else if (milkKit.Dnatest.HasValue)
            {
                if (milkKit.Dnatest.Value)
                    GeneticPass.Checked = true;
                else
                    GeneticFail.Checked = true;
            }
        }

        protected void MicrobialPass_CheckedChanged(object sender, EventArgs e)
        {
            if (MicrobialPass.Checked)
            {
                testData.Visible = true;
            }

            else
            {
                testData.Visible = false;
            }

        }
    }
}