using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculator.Domain
{
    public class Concessionary
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        #region NavigationProps
        public virtual List<Tariff> Tariffs { get; set; }
        #endregion
    }
}
