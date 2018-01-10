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
using static ItseAPI.Features.Calculate.Calculate;

namespace ItseAPI.Features.Calculate
{
    public class TariffUtil
    {
        //Método que calcula o valor da tarifa por equipamento no novo sistema
        public static async Task<double> WhiteTariffCalc(Db dataBaseContext, Command req)
        {
            double equipConsume = 0;
            double total = 0;

            var offPeackITariffValue = (await dataBaseContext.Tariff.Where(t => t.Name.Contains("WhiteTariffOffPeackI")).FirstAsync()).BaseValue;

            foreach (var dateInterval in req.UseOfMonth)
            {
                var periodList = await DateConsistenceList(dataBaseContext, dateInterval.DateTimeInit, dateInterval.DateTimeFinish);

                foreach (var period in periodList)
                {
                    equipConsume += req.Power * req.Quantity * (period.TotalMinutes / 60) / 1000;
                    total += equipConsume * (period.TariffValue / 1000) * 22 + equipConsume * (offPeackITariffValue / 1000) * 8;
                }
            }

            return Math.Abs(total);
        }

        //Método que calcula o valor da tarifa por equipamento no atual sistema
        public static async Task<double> ConventionalTariffCalc(Db dataBaseContext, Calculate.Command req)
        {
            var conventionalTariffValue = (await dataBaseContext.Tariff.Where(t => t.Name.Contains("Conventional")).FirstAsync()).BaseValue;

            double equipConsume = 0;
            foreach (var dateInterval in req.UseOfMonth)
            {
                var diff = dateInterval.DateTimeFinish - dateInterval.DateTimeInit;

                equipConsume += req.Power * req.Quantity * (diff.TotalMinutes / 60) / 1000;
            }
            equipConsume = equipConsume * (conventionalTariffValue / 1000) * 30;

            return Math.Abs(equipConsume);
        }

        //Date Consistence Object Aux
        public class DateConsistence
        {
            public double TariffValue { get; set; }
            public double TotalMinutes { get; set; }
        }

        //Método que verifica se duas datas estão no mesmo dia
        public static bool SameDay(DateTime dateInit, DateTime dateFinish)
        {
            return dateInit.DayOfYear == dateFinish.DayOfYear;
        }

        //Método que verifica se uma data está no período indicado
        public static bool VerifyPeriod(Tariff whiteTariffType, DateTime date)
        {
            var initTime = new Time(whiteTariffType.InitTime.Hours, whiteTariffType.InitTime.Minutes, whiteTariffType.InitTime.Seconds);
            var finishTime = new Time(whiteTariffType.FinishTime.Hours, whiteTariffType.FinishTime.Minutes, whiteTariffType.FinishTime.Seconds);

            return date.IsBetween(initTime, finishTime);
        }

        //Método que retorna uma lista com os períodos da tarifa branca entre duas datas
        public static async Task<List<DateConsistence>> DateConsistenceList(Db dataBaseContext, DateTime dateInit, DateTime dateFinish)
        {
            var dateConsistenceList = new List<DateConsistence>();

            //Criar Lista de Datas
            var periodDateList = new List<DateInitAndFinish>();

            while (dateInit < dateFinish)
            {
                var tariffs = await dataBaseContext.Tariff.Where(t => t.Name.Contains("WhiteTariff")).ToListAsync();

                foreach (var tariff in tariffs)
                {
                    DateInitAndFinish date = new DateInitAndFinish();
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
                            if (tariff.FinishTime > dateFinish.TimeOfDay)
                            {
                                date.DateTimeFinish = dateFinish;
                            }
                            else
                            {
                                date.DateTimeFinish = dateInit.SetTime(tariff.FinishTime.Hours, tariff.FinishTime.Minutes, tariff.FinishTime.Seconds);
                            }
                        }

                        periodDateList.Add(date);

                        //Adiciona na lista consistente
                        dateConsistenceList.Add(new DateConsistence()
                        {
                            TariffValue = tariff.BaseValue,
                            TotalMinutes = TotalMinutes(date.DateTimeInit, date.DateTimeFinish)
                        });

                        dateInit = date.DateTimeFinish.AddSeconds(1);
                    }
                }
            }
            return dateConsistenceList;
        }

        public static double TotalMinutes(DateTime data1, DateTime data2)
        {
            return Math.Abs((data1 - data2).TotalMinutes);
        }

        public static double TotalMinutes(List<DateConsistence> dateConsistenceList)
        {
            double total = 0;

            foreach (var consistence in dateConsistenceList)
            {
                total += consistence.TotalMinutes;
            }
            return total;
        }

        public static async Task<double> TotalMinutes(Db dataBaseContext, List<DateInitAndFinish> dateList)
        {
            List<DateConsistence> consistenceList = new List<DateConsistence>();
            foreach (var dateInterval in dateList)
            {
                consistenceList.AddRange(await DateConsistenceList(dataBaseContext, dateInterval.DateTimeInit, dateInterval.DateTimeFinish));
            }
            double total = 0;
            foreach (var consistence in consistenceList)
            {
                total += consistence.TotalMinutes;
            }

            return total;
        }

    }
}
