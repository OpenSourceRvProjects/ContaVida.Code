using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.DataAccess.DataAccess;
using ContaVida.MVC.Models.EventConstancyDocument;
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

            var eventCounter = await _dbContext.EventCounters.Include(i => i.PersonalProfile).Include(thi=> thi.User).FirstOrDefaultAsync(f => f.Id == eventID);

            if (eventCounter == null)
            {
                throw new Exception("Event not found.");
            }

            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4.Rotate(), 70f, 70f, 10f, 0f);
            MemoryStream workStream = new MemoryStream();
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            // Add a simple and wellknown phrase to the document in a flow layout manner  
            //AddBlankSpace(doc);

            var request = _accessor.HttpContext.Request;
            var localImageServerPath = $"{request.Scheme}://{request.Host}/constancyCompressed.jpg";
            var localcontaVidaImageserverPath = $"{request.Scheme}://{request.Host}/contavidaLogoCompressed.png";

            if (localImageServerPath.Contains("localhost"))
            {
                var dir = Directory.GetCurrentDirectory();
                string fullPathBackground = Path.Combine(dir, "Assets\\constancyCompressed.jpg");
                localImageServerPath = fullPathBackground;

                string fullPathContaVida = Path.Combine(dir, "Assets\\contavidaLogoCompressed.png");
                localcontaVidaImageserverPath = fullPathContaVida;
            }
            var image = iTextSharp.text.Image.GetInstance(localImageServerPath);
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(image);

            jpg.Alignment = iTextSharp.text.Image.UNDERLYING;

            jpg.SetAbsolutePosition(0, 0); // set the position to bottom left corner of pdf
            jpg.ScaleAbsolute(iTextSharp.text.PageSize.A4.Rotate().Width, iTextSharp.text.PageSize.A4.Rotate().Height); // set the height and width of image to PDF page size

            doc.Add(jpg);

            iTextSharp.text.Font normalTextFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 36, iTextSharp.text.Font.NORMAL, new BaseColor(38, 95, 43));
            Paragraph paragraph = new Paragraph("CONSTANCIA", normalTextFont);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);

            InsertContaVidaLogo(doc, localcontaVidaImageserverPath);
            InsertProfileNameForUser(eventCounter, doc);

            iTextSharp.text.Font congratsParagraphFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Paragraph paragraphCongrats = new Paragraph("¡Felicidades!. De parte de ContaVida te hicimos un reconocimiento ", congratsParagraphFont);
            paragraphCongrats.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraphCongrats);

            InsertEventNameAndTime(eventCounter, doc);
            InsertQR(eventCounter, doc);

            var legalStatement = "*Aviso: El presente documento tiene únicamente fines informativos y de referencia. No constituye un documento oficial, certificado," +
                " ni tiene validez legal ante autoridades públicas o privadas. Contavida no asume responsabilidad alguna por el uso indebido, alteración, reproducción, falsificación" +
                " o interpretación del contenido de este documento, ni por cualquier daño o perjuicio derivado directa o indirectamente de su utilización. La verificación proporcionada" +
                " por el sistema tiene carácter meramente informativo y no implica garantía de autenticidad, integridad o vigencia del documento.";
            // Close the document  

            iTextSharp.text.Font legalParagraphFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Paragraph legalParagraph = new Paragraph(legalStatement, legalParagraphFont);
            legalParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            doc.Add(legalParagraph);
            doc.Close();
            // Close the writer instance  
            var docBytes = workStream.ToArray();
            return docBytes;
        }

        public async Task<EventConstancyVerifierModel> VerifyConstancyDocument(string stamp)
        {
            var verificationResult = new EventConstancyVerifierModel() { IsVerified = false };

            try
            {
                var stampParts = stamp.Split('|');
                if (stampParts.Length != 3)
                {
                    return verificationResult;
                }
                var eventId = Guid.Parse(stampParts[0]);
                var userId = Guid.Parse(stampParts[1]);
                var personalProfileId = Guid.Parse(stampParts[2]);
                var eventCounter = await _dbContext.EventCounters
                    .Include(i => i.PersonalProfile)
                    .Include(thi => thi.User)
                    .FirstOrDefaultAsync(f => f.Id == eventId && f.UserId == userId && f.PersonalProfileId == personalProfileId);

                if (eventCounter == null)
                    return verificationResult;

                return new EventConstancyVerifierModel()
                {
                    IsVerified = true,
                    IssuedTo = eventCounter.PersonalProfile.Name + " " + eventCounter.PersonalProfile.LastName1,
                    OriginalSetUpDate = new DateTime((int)eventCounter.StartYear, eventCounter.StartMonth, eventCounter.StartDay),
                    RelapseEpisodes = await _dbContext.Relapses.CountAsync(c => c.EventCounterId == eventCounter.Id),
                    UserName = eventCounter.User.UserName
                };
            }
            catch
            {
                return new EventConstancyVerifierModel() { IsVerified = false };
            }
        }

        private void InsertQR(EventCounter eventCounter, Document doc)
        {
            var request = _accessor.HttpContext.Request;
            iTextSharp.text.Font stampParagraphFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            var stampText = eventCounter.Id.ToString().ToUpper() + "|" + eventCounter.UserId.ToString().ToUpper() + "|" + eventCounter.PersonalProfileId.ToString().ToUpper();
            BarcodeQRCode barcodeQRCode = new BarcodeQRCode($"{request.Scheme}://{request.Host}/counter/verification?stamp=" + stampText, 120, 120, null);
            var qrImage = barcodeQRCode.GetImage();
            qrImage.Alignment = Element.ALIGN_RIGHT;
            doc.Add(qrImage);

            var linkFont = new Font(
                stampParagraphFont.BaseFont,
                stampParagraphFont.Size,
                Font.UNDERLINE,
                BaseColor.BLUE
            );

            Paragraph stampParagraph = new Paragraph();
            stampParagraph.Alignment = Element.ALIGN_RIGHT;

            // Create clickable link
            var link = new Anchor("Verificación: " + stampText, linkFont)
            {
                Reference = $"{request.Scheme}://{request.Host}/counter/verification?stamp=" + stampText // must be a valid URL
            };

            stampParagraph.Add(link);
            doc.Add(stampParagraph);
        }

        private static void InsertEventNameAndTime(EventCounter eventCounter, Document doc)
        {
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
                quantityTime = diffDays;
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
        }

        private static void InsertProfileNameForUser(EventCounter eventCounter, Document doc)
        {
            iTextSharp.text.Font nameParagraphFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 20, iTextSharp.text.Font.UNDERLINE, BaseColor.BLACK);
            Paragraph nameParagraph = new Paragraph(eventCounter.PersonalProfile.Name + " " + eventCounter.PersonalProfile.LastName1, nameParagraphFont);
            nameParagraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(nameParagraph);

            iTextSharp.text.Font userParagraphFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Paragraph userParagraph = new Paragraph("Usuario: " + eventCounter.User.UserName, userParagraphFont);
            userParagraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(userParagraph);
        }

        private static void InsertContaVidaLogo(Document doc, string localcontaVidaImageserverPath)
        {
            // ContaVida logo UNDER the title
            var contaVidaLogo = iTextSharp.text.Image.GetInstance(localcontaVidaImageserverPath);
            contaVidaLogo.ScaleToFit(125f, 125f); // medium size
            contaVidaLogo.Alignment = Element.ALIGN_CENTER;
            contaVidaLogo.SpacingBefore = 1f;
            contaVidaLogo.SpacingAfter = 2;

            doc.Add(contaVidaLogo);
        }

        private static void AddBlankSpace(Document doc)
        {
            iTextSharp.text.Font blankParagraphFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLUE);
            Paragraph blankParagraph = new Paragraph("  ", blankParagraphFont);
            blankParagraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(blankParagraph);
        }
    }
}
