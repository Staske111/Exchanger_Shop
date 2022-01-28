using System;
using System.Collections.Generic;
using System.Text;

namespace Exchanger_Shop.Models
{
    public class PayClass
    {
        public string result { get; set; }
        public List<PayLoad> payload { get; set; }
    }
    public class PayLoad
    {
        public string name { get; set; }
        public string code { get; set; }
        public string address { get; set; }
        public int confirm { get; set; }
    }
}
