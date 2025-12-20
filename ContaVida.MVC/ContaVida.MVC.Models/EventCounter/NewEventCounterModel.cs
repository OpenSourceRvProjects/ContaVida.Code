using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.EventCounter
{
    public class NewEventCounterModel
    {
        public string ImageCollection { get; set; }
        public bool IsPublic { get; set; }
        public int Month { get; set; }
        public string EventName { get; set; }
        public int Day { get; set; }
        public string CustomMessage { get; set; }
        public int Year { get; set; }
        public int Hour { get; set; }
        public int Minutes { get; set; }
    }
}
