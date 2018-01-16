using System;

namespace ConventionalAndWhiteTariffCalculator.Features.Calculate
{
    public class DateInitAndFinish
    {
        public DateTime DateInit { get; set; }
        public DateTime DateFinish { get; set; }
        public TimeSpan TimeInit { get; set; }
        public TimeSpan TimeFinish { get; set; }
    }

    public class DateTimeInitAndFinish
    {
        public DateTime DateTimeInit { get; set; }
        public DateTime DateTimeFinish { get; set; }
    }
}
