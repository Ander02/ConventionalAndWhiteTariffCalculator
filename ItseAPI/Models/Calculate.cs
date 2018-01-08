using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItseAPI.Models
{
    public class Calculate
    {
        public class DateInitAndFinish
        {
            public DateTime DateTimeInit { get; set; }
            public DateTime DateTimeFinish { get; set; }
        }

        public class RequestModel
        {
            public double Power { get; set; }
            public int Quantity { get; set; }
            public List<DateInitAndFinish> UseOfMounth { get; set; }
        }

        public class ResponseModel
        {
            public double WhiteTariffEnergySpending { get; set; }
            public double CurrentTariffEnergySpending { get; set; }
        }

    }
}
