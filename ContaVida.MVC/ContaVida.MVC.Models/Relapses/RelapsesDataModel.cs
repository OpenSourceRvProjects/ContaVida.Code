using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.Relapses
{
    public class RelapseDetailModel
    {
        public string EventName { get; set; }
        public Guid Id { get; set; }
        public Guid EventCounterID { get; set; }
        public string DateString { get; set; }
        public int DaysSinceLastRelapse { get; set; }
        public DateTime RelapseDate { get; set; }
        public string TimeSinceLastRelapseString { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class RelapsesDataModel
    {
        public RelapsesDataModel()
        {
            this.Items = new List<RelapseDetailModel>();
        }

        public List<RelapseDetailModel> Items { get; set; }
        public string TimeUnit { get; set; }
        public decimal TimeQuantitySinceLastIssue { get; set; }
        public double RelapsesTimeAverage
        {
            get
            {
                return Items.Count > 0 ? this.Items.Average(a => a.DaysSinceLastRelapse) : 0;
            }
        }
    }
}
