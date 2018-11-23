using System;
using System.IO;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace Fly01.Core.Helpers
{
    public static class ConvertHTMLToPDF
    {
        public static Byte[] Convert(string html)
        {
            Byte[] bytes = null;
            using (var ms = new MemoryStream())
            {
                var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A3);
                pdf.Save(ms);
                bytes = ms.ToArray();
            }

            return bytes;
        }

        public static Byte[] GerarArquivoPDF(string html)
        {
            Byte[] bytes = null;

            var pdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            if (html.Length > 80000)
                pdf.Margins.Top = 95;
            var pdfAtributs = pdf.GeneratePdf(html);
           
            using (MemoryStream fs = new MemoryStream())
            {
                fs.Write(pdfAtributs, 0, pdfAtributs.Length);
                bytes = fs.ToArray();
                fs.Close();
            }
            return bytes;
        }
    }
}