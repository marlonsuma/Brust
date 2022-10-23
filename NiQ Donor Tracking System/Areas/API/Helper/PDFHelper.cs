using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;

namespace NiQ_Donor_Tracking_System.Areas.API.Helper
{
    public class PDFHelper
    {
        public void generateSignatureImage(string signatureText, string dateString, string outputPath, int width, int height, Font font, Color color)
        {
            Bitmap signature = new Bitmap(width, height);
            Graphics drawing = Graphics.FromImage(signature);
            drawing.TextRenderingHint = TextRenderingHint.AntiAlias;
            drawing.Clear(Color.Transparent);
            Brush textBrush = new SolidBrush(color);
            drawing.DrawString(signatureText, font, textBrush, 5, 3);
            drawing.DrawString(dateString, font, textBrush, 410, 3);

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            signature.Save(outputPath, ImageFormat.Png);
        }

        public void generatePadImage(string outputPath, int width, int height)
        {
            Bitmap pad = new Bitmap(width, height);

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            pad.Save(outputPath, ImageFormat.Png);
        }

        public Font addFontFile(string fontPath, int fontSize = 12, FontStyle fontStyle = FontStyle.Regular, GraphicsUnit unit = GraphicsUnit.Pixel)
        {
            try
            {
                PrivateFontCollection fontFam = new PrivateFontCollection();
                fontFam.AddFontFile(fontPath);
                Font font = new Font(fontFam.Families[0], fontSize, fontStyle, unit);
                return font;
            }

            catch(Exception e)
            {
                throw e;
            }
           
        }

        public string cmdConvertSignatureImage2PDF(string imagePath, string outputPath)
        {
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                System.Security.SecureString ssPwd = new System.Security.SecureString();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                //startInfo.FileName = @"C:\Program Files\ImageMagick-7.0.9-Q16\convert.exe";
                startInfo.FileName = "C:\\inetpub\\DonorTrackingSite\\Storage\\apps\\ImageMagick-7.0.9-Q16\\convert.exe";
                startInfo.Arguments = "/C \"" + imagePath + "\" -resize 76% -transparent white -page a4+80+180 -quality 75 \"" + outputPath + "\"";
                
                process.StartInfo = startInfo;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
               
                process.WaitForExit();

                process.Close();
               

                return output;
            }

            catch(Exception ex)
            {
                return ex.Message;
            }
           
        }

        public List<string> cmdConvertPadImage2PDF(string imagePath, string outputDirectory, int pages)
        {

            List<string> pdfs = new List<string>();

            for(int i = 0; i < pages; i++)
            {
                if (File.Exists(outputDirectory + "page-pad-" + i + ".pdf"))
                {
                    File.Delete(outputDirectory + "page-pad-" + i + ".pdf");
                }

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                System.Security.SecureString ssPwd = new System.Security.SecureString();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                //startInfo.FileName = @"C:\Program Files\ImageMagick-7.0.9-Q16\convert.exe";
                startInfo.FileName = @"C:\inetpub\DonorTrackingSite\Storage\\apps\\ImageMagick-7.0.9-Q16\convert.exe";
                startInfo.Arguments = "/C \"" + imagePath + "\" -resize 76% -transparent white -page a4+1+1 -quality 75 \"" + outputDirectory + "page-pad-" + i + ".pdf\"";
               
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                process.Close();
                pdfs.Add(outputDirectory + "page-pad-" + i + ".pdf");
               
            }

            return pdfs;
        }

        public string mergePDFFiles(List<string> files, string outputPath)
        {
            try
            {
                if (File.Exists(outputPath))
                {
                    File.Delete(outputPath);
                }

                string cmd = "";

                foreach (string file in files)
                {
                    cmd += "\"" + file + "\" ";
                }

                cmd += "output \"" + outputPath + "\"";

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                System.Security.SecureString ssPwd = new System.Security.SecureString();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                startInfo.RedirectStandardError = true;
                //startInfo.FileName = "\"C:\\Users\\Josh\\Desktop\\DT\\NiQ Donor Tracking System\\Storage\\apps\\PDFtk Server\\bin\\pdftk.exe\"";
                startInfo.FileName = "\"C:\\inetpub\\DonorTrackingSite\\Storage\\apps\\PDFtk Server\\bin\\pdftk.exe\"";
                //return cmd;
                startInfo.Arguments = cmd;

                /*startInfo.UserName = "CodeMe";
                string password = "JGet_p@ss!";

                for (int x = 0; x < password.Length; x++)
                {
                    ssPwd.AppendChar(password[x]);
                }

                startInfo.Password = ssPwd; */
               
                process.StartInfo = startInfo;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                process.Close();

              
                return output;

            }

            catch(Exception ex)
            {
                return ex.Message;
            }



        }

        public void generateW9FDFFile(System.Net.Http.Formatting.FormDataCollection inputs, string outputFile)
        {
            string FDF = "%FDF-1.2 \n";
            FDF += "1 0 obj << /FDF << /Fields [ \n";
            FDF += "<< /T(topmostSubform[0].Page1[0].f1_1[0]) /V(" + inputs["name"] + ") >> \n";

            FDF += "<< /T(topmostSubform[0].Page1[0].Address[0].f1_7[0]) /V("+ inputs["address"] +") >> \n";
            FDF += "<< /T(topmostSubform[0].Page1[0].Address[0].f1_8[0]) /V(" + inputs["citystzip"] + ") >> \n";

            if (inputs["business"] != null)
            {
                  FDF += "<< /T(topmostSubform[0].Page1[0].f1_2[0]) /V(" + inputs["business"] + ") >> \n";
            }

            if (inputs["indSol"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].FederalClassification[0].c1_1[0]) /V(1) >> \n";
            }

            if (inputs["ccorp"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].FederalClassification[0].c1_1[1]) /V(2) >> \n";
            }

            if (inputs["scorp"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].FederalClassification[0].c1_1[2]) /V(3) >> \n";
            }

            if (inputs["partnership"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].FederalClassification[0].c1_1[3]) /V(4) >> \n";
            }

            if (inputs["trust"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].FederalClassification[0].c1_1[4]) /V(5) >> \n";
            }

            if (inputs["llc"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].FederalClassification[0].c1_1[5]) /V(6) >> \n";
            }

            if (inputs["llc_letter"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].FederalClassification[0].f1_3[0]) /V("+ inputs["llc_letter"]+") >> \n";
            }

            if (inputs["other"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].FederalClassification[0].c1_1[6]) /V(7) >> \n";
            }

            if (inputs["other_exp"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].FederalClassification[0].f1_4[0]) /V(" + inputs["other_exp"] + ") >> \n";
            }

            if (inputs["except"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].Exemptions[0].f1_5[0]) /V(" + inputs["except"] + ") >> \n";
            }

            if (inputs["facta"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].Exemptions[0].f1_6[0]) /V(" + inputs["facta"] + ") >> \n";
            }

            if (inputs["requester"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].f1_9[0]) /V(" + inputs["requester"] + ") >> \n";
            }

            if (inputs["accounts"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].f1_10[0]) /V(" + inputs["accounts"] + ") >> \n";
            }

            if (inputs["social_1"] != null && inputs["social_2"] != null && inputs["social_3"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].SSN[0].f1_11[0]) /V("+ inputs["social_1"] + inputs["social_2"] + inputs["social_3"] +") >> \n";
            }

            if (inputs["social_4"] != null && inputs["social_5"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].SSN[0].f1_12[0]) /V(" + inputs["social_4"] + inputs["social_5"] +") >> \n";
            }

            if (inputs["social_6"] != null && inputs["social_7"] != null && inputs["social_8"] != null && inputs["social_9"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].SSN[0].f1_13[0]) /V(" + inputs["social_6"] + inputs["social_7"] + inputs["social_8"] + inputs["social_9"] + ") >> \n";
            }

            if (inputs["ein_1"] != null && inputs["ein_2"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].EmployerID[0].f1_14[0]) /V(" + inputs["ein_1"] + inputs["ein_2"] + ") >> \n";
            }

            if (inputs["ein_3"] != null && 
                inputs["ein_4"] != null && 
                inputs["ein_5"] != null && 
                inputs["ein_6"] != null &&
                inputs["ein_7"] != null &&
                inputs["ein_8"] != null &&
                inputs["ein_9"] != null)
            {
                FDF += "<< /T(topmostSubform[0].Page1[0].EmployerID[0].f1_15[0]) /V(" + inputs["ein_3"] + inputs["ein_4"] + inputs["ein_5"] + inputs["ein_6"] + inputs["ein_7"] + inputs["ein_8"] + inputs["ein_9"] + ") >> \n";
            }

            FDF += "] >> >> \n";
            FDF += "endobj \n";
            FDF += "trailer \n";
            FDF += "<< /Root 1 0 R >> \n";
            FDF += "%%EOF";

            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            File.WriteAllText(outputFile, FDF);  
        }

        public void testgenerateW9FDFFile(Dictionary<string, string> inputs, string outputFile)
        {
            string FDF = "%FDF-1.2 \n";
            FDF += "1 0 obj << /FDF << /Fields [ \n";
            FDF += "<< /T(topmostSubform[0].Page1[0].f1_1[0]) /V(" + inputs["name"] + ") >> \n";

            FDF += "<< /T(topmostSubform[0].Page1[0].Address[0].f1_7[0]) /V(" + inputs["address"] + ") >> \n";
            FDF += "<< /T(topmostSubform[0].Page1[0].Address[0].f1_8[0]) /V(" + inputs["citystzip"] + ") >> \n";

            

            FDF += "] >> >> \n";
            FDF += "endobj \n";
            FDF += "trailer \n";
            FDF += "<< /Root 1 0 R >> \n";
            FDF += "%%EOF";

            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            File.WriteAllText(outputFile, FDF);
        }

        public void mergeFDFWithPDF(string FDF, string pdfPath, string outputPath)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            System.Security.SecureString ssPwd = new System.Security.SecureString();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "\"C:\\inetpub\\DonorTrackingSite\\Storage\\apps\\PDFtk Server\\bin\\pdftk.exe\"";
            //startInfo.FileName = "\"C:\\Users\\Josh\\Desktop\\DT\\NiQ Donor Tracking System\\Storage\\apps\\PDFtk Server\\bin\\pdftk.exe\"";
            startInfo.Arguments = "\"" + pdfPath + "\" fill_form \"" + FDF + "\" output \"" + outputPath + "\"";
          
            process.StartInfo = startInfo;
            process.Start();
           
            process.WaitForExit();

            process.Close();
           
            
        }

        public void stampSignature(string signaturePDF, string filledPDF, string outputPath)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            System.Security.SecureString ssPwd = new System.Security.SecureString();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "\"C:\\inetpub\\DonorTrackingSite\\Storage\\apps\\PDFtk Server\\bin\\pdftk.exe\"";
            //startInfo.FileName = "\"C:\\Users\\Josh\\Desktop\\DT\\NiQ Donor Tracking System\\Storage\\apps\\PDFtk Server\\bin\\pdftk.exe\"";
            startInfo.Arguments = "\"" + filledPDF + "\" multistamp \"" + signaturePDF + "\" output \"" + outputPath + "\"";
           

            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit();

            process.Close();
        }

        public void clean(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}