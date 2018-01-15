using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculator.Domain
{
    public class Tariff
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TariffType { get; set; }
        public TimeSpan InitTime { get; set; }
        public TimeSpan FinishTime { get; set; }
        public double BaseValue { get; set; }
        public Guid ConcessionaryId { get; set; }

        #region NavigationProps
        public virtual Concessionary Concessionary { get; set; }
        #endregion
    }
}
