using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.EventCounter
{
    public class CounterDataModel
    {
        public string Name { get; set; }
        public Guid CounterID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minutes { get; set; }
        public bool IsPublicCounter { get; set; }
        public int MinutesToRefresh { get; set; }
        public string CounterRandomPhrase { get; set; }
        public string CounterRandomAuthor { get; set; }
    }
}
