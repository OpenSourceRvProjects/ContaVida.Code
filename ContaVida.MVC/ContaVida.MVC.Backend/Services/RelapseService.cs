using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.Constants;
using ContaVida.MVC.DataAccess.DataAccess;
using ContaVida.MVC.Models;
using ContaVida.MVC.Models.Relapses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Backend.Services
{
    public class RelapseService : IRelapseService
    {
        private IHttpContextAccessor _accessor;
        private ContaVidaDbContext _dbContext;
        public RelapseService(IHttpContextAccessor accessor, ContaVidaDbContext dbContext)
        {
            _accessor = accessor;
            _dbContext = dbContext;

        }
        public async Task<RelapsesDataModel> GetEventRelapses(Guid counterEventID)
        {
            var currentUserID = Guid.Parse(_accessor.HttpContext.Session.GetString("userID"));

            var relapseData = new RelapsesDataModel();

            var relapses = _dbContext.Relapses.Include(i => i.EventCounter).Where(w => w.UserId == currentUserID && w.EventCounterId == counterEventID);
            //.Select(s => new RelapseDetailModel()
            //{
            //	Id = s.Id,
            //	EventCounterID = s.EventCounterId,
            //	EventName = s.EventCounter.EventName,
            //	DateString = s.RelapseYear + "/" + s.RelapseMonth + "/" + s.RelapseDay,
            //	RelapseDate = new DateTime((int)s.RelapseYear, s.RelapseMonth, s.RelapseDay),
            //	DaysSinceLastRelapse = (new DateTime((int)s.RelapseYear, (int)s.RelapseMonth, (int)s.RelapseDay, (int)s.RelapseHour, (int)s.RelapseHour, 0)
            //	- new DateTime((int)s.PreviousYear, (int)s.PreviousMonth, (int)s.PreviousDay, (int)s.PreviousHour, (int)s.PreviousMinutes, 0)).Days,


            //});

            var relapsesDetailList = new List<RelapseDetailModel>();

            foreach (var r in relapses)
            {
                var item = new RelapseDetailModel();
                item.Id = r.Id;
                item.EventCounterID = r.EventCounterId;
                item.EventName = r.EventCounter.EventName;
                item.DateString = r.RelapseYear + "/" + r.RelapseMonth + "/" + r.RelapseDay;
                item.RelapseDate = new DateTime((int)r.RelapseYear, r.RelapseMonth, r.RelapseDay);
                item.CreationDate = r.CreationDate;

                var timeSinceLastRelapse = (new DateTime((int)r.RelapseYear, (int)r.RelapseMonth, (int)r.RelapseDay, (int)r.RelapseHour, (int)r.RelapseMinute, 0)
                - new DateTime((int)r.PreviousYear, (int)r.PreviousMonth, (int)r.PreviousDay, (int)r.PreviousHour, (int)r.PreviousMinutes, 0));

                if (timeSinceLastRelapse.TotalDays >= 365)
                    item.TimeSinceLastRelapseString = (new DateTime(timeSinceLastRelapse.Ticks).Year - 1) + " año(s)";

                if (timeSinceLastRelapse.TotalDays < 365)
                    item.TimeSinceLastRelapseString = ((int)timeSinceLastRelapse.TotalDays) + " días";

                if (timeSinceLastRelapse.TotalHours < 24)
                    item.TimeSinceLastRelapseString = ((int)timeSinceLastRelapse.TotalHours) + " horas";

                if (timeSinceLastRelapse.TotalHours < 1)
                    item.TimeSinceLastRelapseString = timeSinceLastRelapse.TotalMinutes + " minutos";

                item.DaysSinceLastRelapse = (int)(timeSinceLastRelapse.TotalDays);
                relapsesDetailList.Add(item);

            }



            var relapsesList = relapsesDetailList.OrderByDescending(od => od.RelapseDate).ThenByDescending(obd => obd.CreationDate).ToList();


            relapseData.Items = relapsesList;

            var eventCounter = await _dbContext.EventCounters.FirstAsync(f => f.Id == counterEventID);

            var date = new DateTime((int)eventCounter.StartYear, eventCounter.StartMonth, eventCounter.StartDay);
            var now = DateTime.UtcNow;

            var nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time (Mexico)");
            DateTime nzDateTime = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.Utc, nzTimeZone);

            var diffDays = (nzDateTime - date).Days;

            if (diffDays < 365)
            {
                relapseData.TimeUnit = "días";
                relapseData.TimeQuantitySinceLastIssue = diffDays;
            }
            else
            {
                var timeSpan = nzDateTime - date;
                int age = new DateTime(timeSpan.Ticks).Year - 1;
                relapseData.TimeQuantitySinceLastIssue = age;
                relapseData.TimeUnit = age == 1 ? "año" : "años";

            }

            return relapseData;
        }

        public List<TextValueModel> GetRelapseReasons()
        {
            return RelapseTypesConstant.GetRelapseReasons();
        }
    }
}
