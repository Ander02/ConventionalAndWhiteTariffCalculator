using ItseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateTimeExtensions;
using DateTimeExtensions.WorkingDays;

namespace ItseAPI.Features
{
    public class CalculateMethods
    {
        public static double WhiteTariffCalc(List<Calculate.RequestModel> data)
        {
            double value = 0;
            foreach (var equip in data)
            {
                double consumoEquip = 0;
                foreach (var dateInterval in equip.UseOfMounth)
                {
                    var diff = dateInterval.DateTimeFinish - dateInterval.DateTimeInit;



                    consumoEquip += equip.Power * equip.Quantity * (diff.TotalMinutes / 60) / 1000;
                }
                value += consumoEquip * (TariffValues.Conventional / 1000) * 30;
            }

            return value;
        }

        public static double CurrentTariffCalc(List<Calculate.RequestModel> data)
        {
            double value = 0;
            foreach (var equip in data)
            {
                double consumoEquip = 0;
                foreach (var dateInterval in equip.UseOfMounth)
                {
                    var diff = dateInterval.DateTimeFinish - dateInterval.DateTimeInit;

                    consumoEquip += equip.Power * equip.Quantity * (diff.TotalMinutes / 60) / 1000;
                }
                value += consumoEquip * (TariffValues.Conventional / 1000) * 30;
            }

            return value;
        }

        public struct TariffValues
        {
            public const double Conventional = 419.61;
            public const double OffPeack = 353.25;
            public const double Intermediate = 496.66;
            public const double OnPeack = 760.21;
        }

        public static double VerifyTariffType(DateTime date)
        {
            if (date.IsBetween(new DateTimeExtensions.TimeOfDay.Time(00, 00, 00), new DateTimeExtensions.TimeOfDay.Time(16, 29, 59)) || date.IsBetween(new DateTimeExtensions.TimeOfDay.Time(21, 30, 01), new DateTimeExtensions.TimeOfDay.Time(23, 59, 59)) || date.IsHoliday(new WorkingDayCultureInfo("pt-BR")))
            {
                return TariffValues.OffPeack;
            }
            else if (date.IsBetween(new DateTimeExtensions.TimeOfDay.Time(16, 30, 00), new DateTimeExtensions.TimeOfDay.Time(17, 29, 59)) || date.IsBetween(new DateTimeExtensions.TimeOfDay.Time(20, 30, 01), new DateTimeExtensions.TimeOfDay.Time(21, 30, 00)))
            {
                return TariffValues.Intermediate;
            }
            else if (date.IsBetween(new DateTimeExtensions.TimeOfDay.Time(17, 30, 00), new DateTimeExtensions.TimeOfDay.Time(20, 30, 00)))
            {
                return TariffValues.OnPeack;
            }
            else
            {
                return TariffValues.OffPeack;
            }
        }
    }
}
