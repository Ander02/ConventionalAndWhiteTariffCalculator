using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateTimeExtensions;
using DateTimeExtensions.TimeOfDay;
using ConventionalAndWhiteTariffCalculator.Infraestructure;
using ConventionalAndWhiteTariffCalculator.Domain;
using Microsoft.EntityFrameworkCore;

namespace ConventionalAndWhiteTariffCalculator.Features.Calculate
{
    public class TariffUtil
    {
        public static async Task<TariffDetail> AllTariffCalc(Db dataBaseContext, Guid concessionaryId, double power, int quantity, List<DateInitAndFinish> useOfMonth)
        {
            double equipConsume = 0;
            double whiteTariffTotal = 0;
            double conventionalTariffTotal = 0;
            double totalMinutes = 0;

            var conventionalTariffValue = (await dataBaseContext.Tariff.Where(t => t.ConcessionaryId.Equals(concessionaryId) && t.TariffType.Contains("Conventional")).FirstAsync()).BaseValue;

            foreach (var item in useOfMonth)
            {
                while (item.DateInit <= item.DateFinish)
                {
                    var initDateTime = item.DateInit.SetTime(item.TimeInit.Hours, item.TimeInit.Minutes, item.TimeInit.Seconds);
                    var finishDateTime = item.DateInit.SetTime(item.TimeFinish.Hours, item.TimeFinish.Minutes, item.TimeFinish.Seconds);

                    var periodList = await DateConsistenceList(dataBaseContext, initDateTime, finishDateTime, concessionaryId);

                    foreach (var period in periodList)
                    {
                        equipConsume = power * quantity * (period.TotalMinutes / 60) / 1000;
                        whiteTariffTotal += (equipConsume * (period.WhiteTariffValue / 1000));
                        conventionalTariffTotal += (equipConsume * (conventionalTariffValue / 1000));
                        totalMinutes += period.TotalMinutes;
                    }

                    item.DateInit = item.DateInit.AddDays(1);
                }
            }

            return new TariffDetail()
            {
                ConventionalTariffValue = conventionalTariffTotal,
                WhiteTariffValue = whiteTariffTotal,
                TimeOfUse = TotalTime(totalMinutes)
            };
        }

        public class TariffDetail
        {
            public double ConventionalTariffValue { get; set; }
            public double WhiteTariffValue { get; set; }
            public TimeSpan TimeOfUse { get; set; }
        }

        //Date Consistence Object Aux
        public class DateConsistence
        {
            public double WhiteTariffValue { get; set; }
            public double TotalMinutes { get; set; }
        }

        //Método que verifica se uma data está no período indicado
        public static bool VerifyPeriod(Tariff whiteTariffType, DateTime date)
        {
            var initTime = new Time(whiteTariffType.InitTime.Hours, whiteTariffType.InitTime.Minutes, whiteTariffType.InitTime.Seconds);
            var finishTime = new Time(whiteTariffType.FinishTime.Hours, whiteTariffType.FinishTime.Minutes, whiteTariffType.FinishTime.Seconds);

            return date.IsBetween(initTime, finishTime);
        }

        //Método que retorna uma lista com os períodos da tarifa branca entre duas datas
        public static async Task<List<DateConsistence>> DateConsistenceList(Db dataBaseContext, DateTime dateInit, DateTime dateFinish, Guid concessionaryId)
        {
            var dateConsistenceList = new List<DateConsistence>();

            var whiteTariffs = await dataBaseContext.Tariff.Where(t => t.ConcessionaryId.Equals(concessionaryId) && t.TariffType.Contains("White")).ToListAsync();

            while (dateInit < dateFinish)
            {
                foreach (var tariff in whiteTariffs)
                {
                    DateTimeInitAndFinish date = new DateTimeInitAndFinish();
                    date.DateTimeInit = dateInit;

                    if (VerifyPeriod(tariff, dateInit))
                    {

                        if (tariff.FinishTime > dateFinish.TimeOfDay)
                        {
                            date.DateTimeFinish = dateFinish;
                        }
                        else
                        {
                            date.DateTimeFinish = dateInit.SetTime(tariff.FinishTime.Hours, tariff.FinishTime.Minutes, tariff.FinishTime.Seconds);
                        }

                        //Adiciona na lista consistente
                        dateConsistenceList.Add(new DateConsistence()
                        {
                            WhiteTariffValue = tariff.BaseValue,
                            TotalMinutes = TotalMinutes(date.DateTimeInit, date.DateTimeFinish)
                        });

                        dateInit = date.DateTimeFinish.AddSeconds(1);
                    }
                }
            }
            return dateConsistenceList;
        }

        //Método que verifica se duas datas estão no mesmo dia
        public static bool SameDay(DateTime dateInit, DateTime dateFinish)
        {
            return (dateInit.DayOfYear == dateFinish.DayOfYear) && (dateInit.Year == dateFinish.Year);
        }

        public static double TotalMinutes(DateTime data1, DateTime data2)
        {
            return Math.Abs((data2 - data1).TotalMinutes);
        }

        public static TimeSpan TotalTime(double minutes)
        {
            return new DateTime().AddMinutes(minutes).TimeOfDay;
        }
    }
}
