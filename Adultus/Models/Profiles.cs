using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adultus.Models
{
    public class Profiles
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        public string name { get; set; }
    }
}