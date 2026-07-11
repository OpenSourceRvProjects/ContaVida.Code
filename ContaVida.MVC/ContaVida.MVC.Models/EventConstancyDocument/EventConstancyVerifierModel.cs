using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.EventConstancyDocument
{
    public class EventConstancyVerifierModel
    {
        public bool IsVerified { get; set; }
        public string IssuedTo { get; set; }
        public DateTime OriginalSetUpDate { get; set; }
        public int RelapseEpisodes { get; set; }
        public string UserName { get; set; }
    }
}
