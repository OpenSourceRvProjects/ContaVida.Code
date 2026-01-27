using ContaVida.MVC.Backend.Infraestructure;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Backend.Services
{
    public class EventConstancyService : IEventConstancyService
    {
        public byte[] GenerateConstancyDocument(Guid eventID)
        {
            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4.Rotate(), 70f, 70f, 10f, 0f);
            MemoryStream workStream = new MemoryStream();
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            // Add a simple and wellknown phrase to the document in a flow layout manner  
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);

            var dir = Directory.GetCurrentDirectory();

            string fullPath = Path.Combine(dir, "Assets\\constancy.jpg");
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(fullPath);

            jpg.Alignment = iTextSharp.text.Image.UNDERLYING;

            jpg.SetAbsolutePosition(0, 0); // set the position to bottom left corner of pdf
            jpg.ScaleAbsolute(iTextSharp.text.PageSize.A4.Rotate().Width, iTextSharp.text.PageSize.A4.Rotate().Height); // set the height and width of image to PDF page size

            doc.Add(jpg);

            iTextSharp.text.Font normalTextFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 30, iTextSharp.text.Font.NORMAL, BaseColor.BLUE);
            Paragraph paragraph = new Paragraph("Constancia.", normalTextFont);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);

            // Close the document  
            doc.Close();
            // Close the writer instance  
            var docBytes = workStream.ToArray();
            return docBytes;
        }
    }
}
