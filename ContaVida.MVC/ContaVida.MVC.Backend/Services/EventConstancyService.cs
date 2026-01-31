using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.DataAccess.DataAccess;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static iTextSharp.text.pdf.AcroFields;

namespace ContaVida.MVC.Backend.Services
{
    public class EventConstancyService : IEventConstancyService
    {
        private IHttpContextAccessor _accessor;
        private ContaVidaDbContext _dbContext;
        public EventConstancyService(IHttpContextAccessor accesor, ContaVidaDbContext dbContext)
        {
            _accessor = accesor;
            _dbContext = dbContext;
        }

        public async Task<byte[]> GenerateConstancyDocument(Guid eventID, string imagePath)
        {

            var eventCounter = await _dbContext.EventCounters.Include(i=> i.PersonalProfile).FirstOrDefaultAsync(f=> f.Id == eventID);

            if (eventCounter == null)
            {
                throw new Exception("Event not found.");
            }

            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4.Rotate(), 70f, 70f, 10f, 0f);
            MemoryStream workStream = new MemoryStream();
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            // Add a simple and wellknown phrase to the document in a flow layout manner  
            doc.Add(Chunk.NEWLINE);

            iTextSharp.text.Font blankParagraphFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 20, iTextSharp.text.Font.NORMAL, BaseColor.BLUE);
            Paragraph blankParagraph = new Paragraph("  ", blankParagraphFont);
            blankParagraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(blankParagraph);

            var request = _accessor.HttpContext.Request;
            var localImageServerPath = $"{request.Scheme}://{request.Host}/constancy.jpg";
            var localcontaVidaImageserverPath = $"{request.Scheme}://{request.Host}/contavidaLogo.png";

            if (localImageServerPath.Contains("localhost"))
            {
                var dir = Directory.GetCurrentDirectory();
                string fullPathBackground = Path.Combine(dir, "Assets\\constancy.jpg");
                localImageServerPath = fullPathBackground;

                string fullPathContaVida = Path.Combine(dir, "Assets\\contavidaLogo.png");
                localcontaVidaImageserverPath = fullPathContaVida;
            }
            var image = iTextSharp.text.Image.GetInstance(localImageServerPath);
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(image);

            jpg.Alignment = iTextSharp.text.Image.UNDERLYING;

            jpg.SetAbsolutePosition(0, 0); // set the position to bottom left corner of pdf
            jpg.ScaleAbsolute(iTextSharp.text.PageSize.A4.Rotate().Width, iTextSharp.text.PageSize.A4.Rotate().Height); // set the height and width of image to PDF page size

            doc.Add(jpg);

            iTextSharp.text.Font normalTextFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 42, iTextSharp.text.Font.NORMAL, BaseColor.BLUE);
            Paragraph paragraph = new Paragraph("Constancia.", normalTextFont);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);

            // ContaVida logo UNDER the title
            var contaVidaLogo = iTextSharp.text.Image.GetInstance(localcontaVidaImageserverPath);
            contaVidaLogo.ScaleToFit(200f, 200f); // medium size
            contaVidaLogo.Alignment = Element.ALIGN_CENTER;
            contaVidaLogo.SpacingBefore = 5f;
            contaVidaLogo.SpacingAfter = 10f;

            doc.Add(contaVidaLogo);



            iTextSharp.text.Font nameParagraphFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 25, iTextSharp.text.Font.UNDERLINE, BaseColor.BLACK);
            Paragraph nameParagraph = new Paragraph(eventCounter.PersonalProfile.Name + " " + eventCounter.PersonalProfile.LastName1, nameParagraphFont);
            nameParagraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(nameParagraph);


            iTextSharp.text.Font congratsParagraphFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 20, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Paragraph paragraphCongrats = new Paragraph("¡Felicidades!. De parte de ContaVida te hicimos un reconocimiento por: ", congratsParagraphFont);
            paragraphCongrats.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraphCongrats);


            var date = new DateTime((int)eventCounter.StartYear, eventCounter.StartMonth, eventCounter.StartDay);
            var now = DateTime.UtcNow;

            var nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time (Mexico)");
            DateTime nzDateTime = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.Utc, nzTimeZone);

            var diffDays = (nzDateTime - date).Days;
            var timeUnit = "";
            var quantityTime = 0;

            if (diffDays < 365)
            {
                timeUnit = "días";
                quantityTime= diffDays;
            }
            else
            {
                var timeSpan = nzDateTime - date;
                int age = new DateTime(timeSpan.Ticks).Year - 1;
                quantityTime = age;
                timeUnit = age == 1 ? "año" : "años";

            }

            iTextSharp.text.Font eventParagraphFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 20, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Paragraph eventParagraph = new Paragraph(eventCounter.EventName + ": " + quantityTime + " " + timeUnit, eventParagraphFont);
            eventParagraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(eventParagraph);


            iTextSharp.text.Font stampParagraphFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            var stampText = eventCounter.Id.ToString().ToUpper() + "|" +  eventCounter.UserId.ToString().ToUpper()  +"|" + eventCounter.PersonalProfileId.ToString();
            doc.Add(blankParagraph);
            doc.Add(blankParagraph);


            Paragraph stampParagraph = new Paragraph("Sello de autenticidad: "+ stampText, stampParagraphFont);
            stampParagraph.Alignment = Element.ALIGN_RIGHT;
            doc.Add(stampParagraph);

            // Close the document  
            doc.Close();
            // Close the writer instance  
            var docBytes = workStream.ToArray();
            return docBytes;
        }
    }
}
