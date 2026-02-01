using ContaVida.MVC.Models.EventConstancyDocument;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Backend.Infraestructure
{
    public interface IEventConstancyService
    {
        public Task<byte[]> GenerateConstancyDocument(Guid eventID, string imagePath);
        public Task<EventConstancyVerifierModel> VerifyConstancyDocument(string stamp);
    }
}
