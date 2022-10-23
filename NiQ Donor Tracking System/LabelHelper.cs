using System.Text;

namespace NiQ_Donor_Tracking_System
{
    public static class LabelHelper
    {
        public static string GetDonorLabel(string donorId, string quantity)
        {
            StringBuilder labelBuilder = new StringBuilder();
            labelBuilder.AppendLine("^XA");
            labelBuilder.AppendLine("^MD15");
            labelBuilder.AppendLine("^LH0,0");
            labelBuilder.AppendLine("^PR2");
            labelBuilder.AppendLine("^BY2,2");
            labelBuilder.AppendLine("^PW525");
            labelBuilder.AppendLine($"^FO50,50^BCN,110,N,N,N,A^FD{donorId}^FS");
            labelBuilder.AppendLine($"^FO50,175^A0N,30,30^FD{donorId}^FS");
            labelBuilder.AppendLine($"^PQ{quantity}");
            labelBuilder.AppendLine("^XZ");

            return labelBuilder.ToString();
        }

        public static string GetMilkKitLabel(string barcode, string quantity)
        {
            StringBuilder labelBuilder = new StringBuilder();
            labelBuilder.AppendLine("^XA");
            labelBuilder.AppendLine("^MD15");
            labelBuilder.AppendLine("^LH0,0");
            labelBuilder.AppendLine("^PR2");
            labelBuilder.AppendLine("^BY3,2");
            labelBuilder.AppendLine("^PW525");
            labelBuilder.AppendLine($"^FO100,50^BCN,110,N,N,N,A^FD{barcode}^FS");
            labelBuilder.AppendLine($"^FO200,175^A0N,30,30^FD{barcode}^FS");
            labelBuilder.AppendLine($"^PQ{quantity}");
            labelBuilder.AppendLine("^XZ");

            return labelBuilder.ToString();
        }
    }
}