using System;

namespace ConventionalAndWhiteTariffCalculator.Domain
{
    public class Equipament
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double DefaultPower { get; set; }
    }
}
