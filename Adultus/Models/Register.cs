using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adultus.Models
{
    public class Register
    {
        public string UserName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string EmailAddress { get; set; }
    }
}