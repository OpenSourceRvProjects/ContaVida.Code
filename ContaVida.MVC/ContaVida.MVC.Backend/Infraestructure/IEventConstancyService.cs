using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Backend.Infraestructure
{
    public interface IEventConstancyService
    {
        public byte[] GenerateConstancyDocument(Guid eventID, string imagePath);
    }
}
