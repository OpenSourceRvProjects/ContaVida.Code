using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.Images
{
    public class ImageListModel
    {
        public ImageListModel()
        {
            this.Images = new List<string>();
        }
        public List<string> Images { get; set; }
    }
}
