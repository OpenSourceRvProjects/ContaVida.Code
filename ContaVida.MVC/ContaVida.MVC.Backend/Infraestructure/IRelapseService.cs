using ContaVida.MVC.Models;
using ContaVida.MVC.Models.Relapses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Backend.Infraestructure
{
    public interface IRelapseService
    {
        Task<RelapsesDataModel> GetEventRelapses(Guid counterEventID);
        List<TextValueModel> GetRelapseReasons();
    }
}
