﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateTimeExtensions;
using DateTimeExtensions.TimeOfDay;
using ConventionalAndWhiteTariffCalculatorAPI.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace ConventionalAndWhiteTariffCalculatorAPI.Features.Calculate
{
    public class TariffUtil
    {
        public class TariffDetail
        {
            public int Id { get; set; }
            public double ConventionalTariffValue { get; set; }
            public double WhiteTariffValue { get; set; }
            public string TimeOfUse { get; set; }
        }
        public class DateConsistence
        {
            public double WhiteTariffValue { get; set; }
            public double TotalMinutes { get; set; }
        }

        //Método que calcula o valor da tarifa convencional e branca
        public static async Task<TariffDetail> AllTariffCalc(Db dataBaseContext, Guid powerDistribuitorId, double power, int quantity, List<DateInitAndFinish> useOfMonth)
        {
            double equipConsume = 0;
            double whiteTariffTotal = 0;
            double conventionalTariffTotal = 0;
            double totalMinutes = 0;

            //Lista com todas as tarifas
            var tariffsList = await dataBaseContext.Tariff.Where(t => t.PowerDistribuitorId.Equals(powerDistribuitorId)).ToListAsync();

            //Se a lista estiver vazia, lança uma NotFoundException
            if (!tariffsList.Any()) throw new NotFoundException("Não foi encontrada nenhuma tarifa relacionada a esse distribuidor de energia");

            //Lista com as tarifas brancas
            var whiteTariffsList = tariffsList.Where(t => t.TariffType.Contains("White")).ToList();

            //Valor da tarifa convencional nessa distribuidora
            var conventionalTariffValue = tariffsList.Where(t => t.TariffType.Contains("Conventional")).First().BaseValue;

            //Para cada item do uso no mês
            foreach (var item in useOfMonth)
            {
                //enquanto a data inicial for menor que a data final
                while (item.DateInit <= item.DateFinish)
                {
                    //Define dia com data e hora
                    var initDateTime = item.DateInit.Value.SetTime(item.TimeInit.Value.Hours, item.TimeInit.Value.Minutes, item.TimeInit.Value.Seconds);
                    var finishDateTime = item.DateInit.Value.SetTime(item.TimeFinish.Value.Hours, item.TimeFinish.Value.Minutes, item.TimeFinish.Value.Seconds);

                    //Cria uma lista de períodos com faixas de horário
                    var periodList = DateConsistenceList(whiteTariffsList, initDateTime, finishDateTime);

                    //Para cada período
                    foreach (var period in periodList)
                    {
                        //Realiza os cálculos
                        equipConsume = power * quantity * (period.TotalMinutes / 60) / 1000;
                        whiteTariffTotal += (equipConsume * (period.WhiteTariffValue));
                        conventionalTariffTotal += (equipConsume * (conventionalTariffValue));
                        totalMinutes += period.TotalMinutes;
                    }
                    //Incrementa um dia
                    item.DateInit = item.DateInit.Value.AddDays(1);
                }
            }

            //Detalhes da tarifa
            var tariffDetail = new TariffDetail()
            {
                ConventionalTariffValue = conventionalTariffTotal,
                WhiteTariffValue = whiteTariffTotal,
            };

            //Tempo de uso total
            var useTime = TotalTime(quantity * totalMinutes);

            //Formata tempo de uso
            if (useTime.TotalHours < 1) tariffDetail.TimeOfUse = useTime.Minutes + "min";

            else if (useTime.Minutes == 0) tariffDetail.TimeOfUse = useTime.Days * 24 + useTime.Hours + "h";

            else tariffDetail.TimeOfUse = useTime.Days * 24 + useTime.Hours + "h" + useTime.Minutes + "min";

            return tariffDetail;
        }

        //Método que verifica se uma data está no período indicado
        public static bool VerifyPeriod(Domain.Tariff whiteTariffType, DateTime date)
        {
            var tariffInitTime = new Time(whiteTariffType.InitTime.Hours, whiteTariffType.InitTime.Minutes, whiteTariffType.InitTime.Seconds);
            var tariffFinishTime = new Time(whiteTariffType.FinishTime.Hours, whiteTariffType.FinishTime.Minutes, whiteTariffType.FinishTime.Seconds);

            //Verifica se essa data está entre os extremos do período da tarifa
            return date.IsBetween(tariffInitTime, tariffFinishTime);
        }

        //Método que retorna uma lista com os períodos da tarifa branca entre duas datas
        public static List<DateConsistence> DateConsistenceList(List<Domain.Tariff> whiteTariffsList, DateTime dateInit, DateTime dateFinish)
        {
            var dateConsistenceList = new List<DateConsistence>();

            while (dateInit < dateFinish)
            {
                //Se não for um dia útil
                if (dateInit.IsHoliday() || dateInit.DayOfWeek.Equals(DayOfWeek.Saturday) || dateInit.DayOfWeek.Equals(DayOfWeek.Sunday))
                {
                    //Adiciona na lista consistente
                    dateConsistenceList.Add(new DateConsistence()
                    {
                        //Nesse dia todas respeitam os valores da tarifa no horário fora de ponta
                        WhiteTariffValue = whiteTariffsList.Where(t => t.Name.Contains("OffPeack")).First().BaseValue,
                        TotalMinutes = TotalMinutes(dateInit, dateFinish)
                    });
                    dateInit = dateInit.AddDays(1).SetTime(0, 0, 0);
                }
                else
                {
                    //Para cada tarifa branca
                    foreach (var tariff in whiteTariffsList)
                    {
                        DateTimeInitAndFinish date = new DateTimeInitAndFinish
                        {
                            DateTimeInit = dateInit
                        };

                        //Se a tarifa estiver no mesmo período
                        if (VerifyPeriod(tariff, dateInit))
                        {
                            //Se a hora final da tarifa for maior que a hora da data final
                            if (tariff.FinishTime > dateFinish.TimeOfDay)
                            {
                                date.DateTimeFinish = dateFinish;
                            }
                            else
                            {
                                date.DateTimeFinish = dateInit.SetTime(tariff.FinishTime.Hours, tariff.FinishTime.Minutes, tariff.FinishTime.Seconds).AddSeconds(1);
                            }
                            //Adiciona na lista consistente
                            dateConsistenceList.Add(new DateConsistence()
                            {
                                WhiteTariffValue = tariff.BaseValue,
                                TotalMinutes = TotalMinutes(date.DateTimeInit, date.DateTimeFinish)
                            });
                            dateInit = date.DateTimeFinish;
                        }
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

        //Método que calcula do total de minutos entre duas datas
        public static double TotalMinutes(DateTime data1, DateTime data2)
        {
            return Math.Abs((data2 - data1).TotalMinutes);
        }

        //Método que retorna o tempo total com base no número de minutos
        public static TimeSpan TotalTime(double minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }
    }
}
