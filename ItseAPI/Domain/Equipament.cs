using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItseAPI.Domain
{
    public class Equipament
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double DefaultPower { get; set; }
    }
}
