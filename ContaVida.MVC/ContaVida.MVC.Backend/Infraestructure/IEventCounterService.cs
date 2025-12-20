using ContaVida.MVC.Models.EventCounter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Backend.Infraestructure
{
    public interface IEventCounterService
    {
        Task AddEventCounter(NewEventCounterModel eventCounter);
        Task<List<EventCounterItemModel>> GetCounterList();
        Task<CounterDataModel> GetCounterData(Guid id);
        Task SetPrivacyCounter(Guid counterID, CounterPrivacySetModel setting);
        Task SetRefresherCounterUI(Guid counterID, CounterRefreshTimerSetModel setting);
        Task UpdateEventCounter(Guid counterID, CounterDataModel counter, bool isRelapse = false, string relapseMessage = null, int? relapseReason = null);


        Task<CounterResultsModel> GetCounterResults();
        Task DeleteEventCounterByID(Guid eventCounterID);
    }
}
