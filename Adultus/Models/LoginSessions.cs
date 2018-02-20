using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adultus.Models
{
    public class LoginSessions
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public DateTime LoginDate { get; set; }
    }
}