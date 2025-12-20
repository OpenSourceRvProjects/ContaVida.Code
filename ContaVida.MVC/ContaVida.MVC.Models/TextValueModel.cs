using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models
{
    public class TextValueModel
    {
        public TextValueModel(object value, string text)
        {
            this.Text = text;
            this.Value = value;
        }
        public string Text { get; set; }
        public object Value { get; set; }
    }
}
