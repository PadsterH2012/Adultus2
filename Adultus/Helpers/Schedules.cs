using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Adultus.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Adultus.Controllers;

namespace Adultus.Helpers
{
    public class Schedules
    {
        public void Schedulers()
        {
            //CheckAllActiveUserTickets();
        }

        //public List<String> GetContext(List<Users> users)
        //{
        //    //HttpContext ctx = HttpContext.Current;
        //    //Thread t = new Thread(new ThreadStart(() =>
        //    //{
        //    //    HttpContext.Current = ctx;
        //    //    //worker.DoWork();
        //    //}));
        //    //t.Start();
        //    return CheckAllActiveUserTickets(users);
        //    //t.Join();
        //}

        //public List<String> CheckAllActiveUserTickets(List<Users> users)
        //{
        //    //SqlHelper.DbContext();
        //    AccountController ac = new AccountController();
        //    List<String> activeUsers = new List<String>();
        //    List<FormsAuthenticationTicket> formsAuthenticationTickets = ac.GetAllActiveUserTickets(users);
        //    foreach (FormsAuthenticationTicket ticket in formsAuthenticationTickets)
        //    {
        //        var timeout = ticket.Expiration;
        //        var issued = ticket.IssueDate;
        //        double duration = DateTime.Now.Minute - issued.Minute;
        //        TimeSpan minutes1 = new TimeSpan(0, DateTime.Now.Minute, DateTime.Now.Second);
        //        TimeSpan minutes2 = new TimeSpan(0, issued.Minute, issued.Second);

        //        double timeDifference = minutes1.TotalMinutes - minutes2.TotalMinutes;
        //        if (timeDifference > 60)
        //        {
        //            activeUsers.Add(ticket.Name);
        //            //SqlHelper.SetOnlineStatusToOffline(ticket.Name);
        //            var ctx = HttpContext.Current;
        //            ctx.Session[ticket.Name] = "logout";
        //        }
        //    }

        //    return activeUsers;
        //}
    }
}