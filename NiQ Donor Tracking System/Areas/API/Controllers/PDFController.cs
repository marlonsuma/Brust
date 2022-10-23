using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using PdfSharp.Pdf.AcroForms;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.IO;
using NiQ_Donor_Tracking_System.Areas.API.Helper;
using System.Net.Http.Headers;
using System.Text;

namespace NiQ_Donor_Tracking_System.Areas.API.Controllers
{
    [RoutePrefix("api/pdf")]
    public class PDFController:NiQ_Donor_Tracking_System.API.Controllers.NiqController
    {
        private string rootPath;
        private string storagePath;
        private string workingDirectory;
        private string completedW9PDFPath;
        private string originalW9ODFPath;

        private string signatureImagePath;
        private string signaturePaddingImagePath;

        private string signatureImagePDFPath;
        private string signaturePaddingImagePDFPath;

        private string signedPDFPath;
        private string w9FDFPath;
        private string w9FormFilledPDFPath;


        [HttpPost]
        public HttpResponseMessage Generate(System.Net.Http.Formatting.FormDataCollection formData)
        {
            try
            {
                this.rootPath = HttpContext.Current.Server.MapPath("~");
                this.storagePath = this.rootPath + "Storage/";
                this.workingDirectory = this.storagePath + Guid.NewGuid() + "/";
                Directory.CreateDirectory(this.workingDirectory);

                this.completedW9PDFPath = this.workingDirectory + "w9.pdf";
                this.originalW9ODFPath = this.storagePath + "fw9.pdf";
                this.signatureImagePath = this.workingDirectory + "signature.png";
                this.signatureImagePDFPath = this.workingDirectory + "signature.pdf";
                this.signaturePaddingImagePath = this.workingDirectory + "padding.png";
                this.signaturePaddingImagePDFPath = this.workingDirectory + "padding.pdf";
                this.signedPDFPath = this.workingDirectory + "signed.pdf";
                this.w9FDFPath = this.workingDirectory + "w9.fdf";
                this.w9FormFilledPDFPath = this.workingDirectory + "filled.pdf";

                if (File.Exists(this.completedW9PDFPath))
                {
                    File.Delete(this.completedW9PDFPath);
                }

                PDFHelper helper = new PDFHelper();
                Font font = helper.addFontFile(storagePath + "HomemadeApple-Regular.ttf");
                helper.generateSignatureImage(formData["signature"], formData["date"], this.signatureImagePath, 500, 30, font, Color.Black);

                if (!File.Exists(this.signatureImagePath))
                {
                    throw new Exception("Signature image was not created");
                }

                helper.generatePadImage(this.signaturePaddingImagePath, 1, 1);

                if (!File.Exists(this.signaturePaddingImagePath))
                {
                    throw new Exception("Pad image was not created");
                }

                helper.cmdConvertSignatureImage2PDF(this.signatureImagePath, this.signatureImagePDFPath);

             

                if (!File.Exists(this.signatureImagePDFPath))
                {
                    throw new Exception("Signature PDF was not created");
                }

               

                List<string> pdfs = helper.cmdConvertPadImage2PDF(this.signaturePaddingImagePath, this.workingDirectory, 5);
                pdfs.Insert(0, this.signatureImagePDFPath);

                helper.mergePDFFiles(pdfs, this.signedPDFPath);

                
                helper.generateW9FDFFile(formData, this.w9FDFPath);
                helper.mergeFDFWithPDF(this.w9FDFPath, this.originalW9ODFPath, this.w9FormFilledPDFPath);
                helper.stampSignature(this.signedPDFPath, this.w9FormFilledPDFPath, this.completedW9PDFPath);
                helper.clean(this.signatureImagePath);
                helper.clean(this.signaturePaddingImagePath);
                
                foreach(string file in pdfs)
                {
                    helper.clean(file);
                }

                helper.clean(this.w9FDFPath);
                helper.clean(this.w9FormFilledPDFPath);
                helper.clean(this.signedPDFPath);

                if(File.Exists(this.completedW9PDFPath))
                {
                   
                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(new FileStream(this.completedW9PDFPath, FileMode.Open, FileAccess.Read))
                    };

                    result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "w9.pdf"
                    };

                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    return result;
                }

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Error: Unable to create w9 file.", Encoding.UTF8, "text/html")
                };


                return response;

                

            }
            catch (Exception ex)
            {
              
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message, Encoding.UTF8, "text/html")
                };

                return response;

            }
        }

       
      
    }
}