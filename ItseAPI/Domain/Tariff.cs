using System;

namespace ConventionalAndWhiteTariffCalculatorAPI.Domain
{
    public class Tariff
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TariffType { get; set; }
        public TimeSpan InitTime { get; set; }
        public TimeSpan FinishTime { get; set; }
        public double BaseValue { get; set; }
        public Guid PowerDistribuitorId { get; set; }

        #region NavigationProps
        public virtual PowerDistribuitor PowerDistribuitor { get; set; }
        #endregion
    }
}
