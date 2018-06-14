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
    }
}