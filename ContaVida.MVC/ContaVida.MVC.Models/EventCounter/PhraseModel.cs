using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.EventCounter
{
    public class FrasesMotivacionale
    {
        public string frase { get; set; }
        public string autor { get; set; }
    }

    public class PhraseModel
    {
        public List<FrasesMotivacionale> frases_motivacionales { get; set; }
    }
}
