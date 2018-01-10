using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItseAPI.Domain
{
    public class Tariff
    {
        public Guid Id { get; set; }
        public TimeSpan InitTime { get; set; }
        public TimeSpan FinishTime { get; set; }
        public double BaseValue { get; set; }
        public string Name { get; set; }
    }
}
