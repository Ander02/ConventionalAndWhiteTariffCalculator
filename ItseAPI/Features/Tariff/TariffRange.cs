using System;

namespace ConventionalAndWhiteTariffCalculatorAPI.Features.Tariff
{
    public class TariffRange
    {
        public TimeSpan TimeInit { get; set; }
        public TimeSpan TimeFinish { get; set; }
        public double BaseValue { get; set; }
    }

    public class TariffDetails
    {
        public string Name { get; set; }
        public string TariffType { get; set; }
        public TimeSpan TimeInit { get; set; }
        public TimeSpan TimeFinish { get; set; }
        public double BaseValue { get; set; }
    }
}
