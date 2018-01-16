using System;
using System.Collections.Generic;

namespace ConventionalAndWhiteTariffCalculator.Domain
{
    public class PowerDistribuitor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        #region NavigationProps
        public virtual List<Tariff> Tariffs { get; set; }
        #endregion
    }
}
