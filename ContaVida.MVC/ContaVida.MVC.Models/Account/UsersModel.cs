using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.Account
{
    public class UsersModel
    {
        public string NickName { get; set; }
        public int LoginCount { get; set; }
        public Guid UserID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int CounterEventsCount { get; set; }
        public int RelapsesCount { get; set; }
        public string Email { get; set; }
    }
}
