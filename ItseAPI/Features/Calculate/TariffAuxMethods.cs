using ItseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateTimeExtensions;
using DateTimeExtensions.TimeOfDay;
using DateTimeExtensions.WorkingDays;
using ItseAPI.Infraestructure;
using ItseAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ItseAPI.Features.Calculate
{

    public class TariffAuxMethods
    {
        //Método que calcula o valor da tarifa por equipamento no novo sistema
        public static double WhiteTariffCalc(Db dataBaseContext, Calculate.Command req)
        {


            return 0;
        }

        //Método que calcula o valor da tarifa por equipamento no atual sistema
        public static async Task<double> ConventionalTariffCalc(Db dataBaseContext, Calculate.Command req)
        {
            var conventionalTariffValue = (await dataBaseContext.Tariff.Where(t => t.Name.Contains("Conventional")).FirstAsync()).BaseValue;

            double equipConsume = 0;
            foreach (var dateInterval in req.UseOfMounth)
            {
                var diff = dateInterval.DateTimeFinish - dateInterval.DateTimeInit;

                equipConsume += req.Power * req.Quantity * (diff.TotalMinutes / 60) / 1000;
            }
            equipConsume = equipConsume * (conventionalTariffValue / 1000) * 30;

            return Math.Abs(equipConsume);
        }

        public class DateConsistence
        {
            public double TariffValue { get; set; }
            public double TotalMinutes { get; set; }
        }

        public static bool SameDay(DateTime dateInit, DateTime dateFinish)
        {
            return dateInit.DayOfYear == dateFinish.DayOfYear;
        }

        public static bool VerifyPeriod(Tariff whiteTariffType, DateTime date)
        {
            var initTime = new Time(whiteTariffType.InitTime.Hours, whiteTariffType.InitTime.Minutes, whiteTariffType.InitTime.Seconds);
            var finishTime = new Time(whiteTariffType.FinishTime.Hours, whiteTariffType.FinishTime.Minutes, whiteTariffType.FinishTime.Seconds);

            return date.IsBetween(initTime, finishTime);
        }

        public static async Task<List<DateConsistence>> VerifyDateConsistenceAsync(Db dataBaseContext, DateTime dateInit, DateTime dateFinish)
        {
            var dateConsistenceList = new List<DateConsistence>();

            //Criar Lista de Datas
            var periodDateList = new List<Calculate.DateInitAndFinish>();


            while (dateInit < dateFinish)
            {
                var tariffs = await dataBaseContext.Tariff.Where(t => t.Name.Contains("WhiteTariff")).ToListAsync();


                foreach (var tariff in tariffs)
                {
                    Calculate.DateInitAndFinish date = new Calculate.DateInitAndFinish();
                    date.DateTimeInit = dateInit;

                    if (VerifyPeriod(tariff, dateInit))
                    {
                        //Se for o último período e as datas não estão no mesmo dia
                        if (tariff.Name.Equals("WhiteTariffOffPeackII"))
                        {
                            if (SameDay(dateInit, dateFinish))
                            {
                                date.DateTimeFinish = dateFinish;
                            }
                            else
                            {
                                var offPeack1 = tariffs.First();
                                date.DateTimeFinish = dateInit.AddDays(1).SetTime(offPeack1.FinishTime.Hours, offPeack1.FinishTime.Minutes, offPeack1.FinishTime.Seconds);
                            }
                        }
                        else
                        {
                            date.DateTimeFinish = dateInit.SetTime(tariff.FinishTime.Hours, tariff.FinishTime.Minutes, tariff.FinishTime.Seconds);
                        }

                        periodDateList.Add(date);

                        //Adiciona na lista consistente
                        dateConsistenceList.Add(new DateConsistence()
                        {
                            TariffValue = tariff.BaseValue,
                            TotalMinutes = Math.Abs((date.DateTimeFinish - date.DateTimeInit).TotalMinutes)
                        });

                        dateInit = date.DateTimeFinish.AddSeconds(1);
                    }
                }
            }

            return dateConsistenceList;
        }

    }
}
