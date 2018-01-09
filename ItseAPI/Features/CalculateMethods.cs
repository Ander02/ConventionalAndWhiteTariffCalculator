using ItseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateTimeExtensions;
using DateTimeExtensions.TimeOfDay;
using DateTimeExtensions.WorkingDays;

namespace ItseAPI.Features
{
    public class CalculateMethods
    {
        public static double WhiteTariffCalc(Calculate.RequestModel data)
        {
            double TotalMinutesOffPeack = 0;
            double TotalMinutesIntermediate = 0;
            double TotalMinutesOnPeack = 0;

            double consumoEquip = 0;
            foreach (var item in data.UseOfMounth)
            {

            }

            return 0;
        }

        public static double CurrentTariffCalc(Calculate.RequestModel req)
        {
            double consumoEquip = 0;
            foreach (var dateInterval in req.UseOfMounth)
            {
                var diff = dateInterval.DateTimeFinish - dateInterval.DateTimeInit;

                consumoEquip += req.Power * req.Quantity * (diff.TotalMinutes / 60) / 1000;
            }
            consumoEquip = consumoEquip * (TariffValues.Conventional / 1000) * 30;

            return Math.Abs(consumoEquip);
        }

        public struct TariffValues
        {
            public const double Conventional = 419.61;
            public const double OffPeack = 353.25;
            public const double Intermediate = 496.66;
            public const double OnPeack = 760.21;
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

        public static bool SamePeriod(DateTime dateInit, DateTime dateFinish)
        {
            //OffPeack I
            if (dateInit.IsBetween(new Time(00, 00, 00), new Time(16, 30, 00)))
            {
                if (dateFinish.IsBetween(new Time(00, 00, 00), new Time(16, 30, 00)))
                {
                    return true;
                }
                return false;
            }
            //Medium I
            else if (dateInit.IsBetween(new Time(16, 30, 01), new Time(17, 29, 59)))
            {
                if (dateFinish.IsBetween(new Time(16, 30, 01), new Time(17, 29, 59)))
                {
                    return true;
                }
                return false;
            }
            //Peack
            else if (dateInit.IsBetween(new Time(17, 30, 00), new Time(20, 30, 00)) && dateFinish.IsBetween(new Time(17, 30, 00), new Time(20, 30, 00)))
            {
                return true;
            }
            //Medium II
            else if (dateInit.IsBetween(new Time(20, 30, 01), new Time(21, 29, 59)))
            {
                if (dateFinish.IsBetween(new Time(20, 30, 01), new Time(21, 29, 59)))
                {
                    return true;
                }
                return false;
            }
            //OffPeack II
            else if (dateInit.IsBetween(new Time(21, 30, 00), new Time(23, 59, 59)))
            {
                if (dateFinish.IsBetween(new Time(21, 30, 00), new Time(23, 59, 59)))
                {
                    return true;
                }
                return false;
            }
            //Dois Períodos
            else
            {
                return false;
            }
        }

        public static List<DateConsistence> VerifyDateConsistence(DateTime dateInit, DateTime dateFinish)
        {
            var dateConsistenceList = new List<DateConsistence>();

            //Criar Lista de Datas
            var initDayDateList = new List<DateTime>();
            var finishDayDateList = new List<DateTime>();
            while (dateInit < dateFinish)
            {
                initDayDateList.Add(dateInit);

                if (dateInit.AddDays(1) < dateFinish)
                {
                    finishDayDateList.Add(dateInit.AddDays(1));
                }
                else
                {
                    finishDayDateList.Add(dateFinish);
                }
                dateInit = dateInit.AddDays(1);
            }
            


            //Se as Datas estão em períodos iguais
            if (SamePeriod(dateInit, dateFinish))
            {
                //Verificar em qual período ela está

                //OffPeack I
                if (dateInit.IsBetween(new Time(00, 00, 00), new Time(16, 30, 00)))
                {
                    if (dateFinish.IsBetween(new Time(00, 00, 00), new Time(16, 30, 00)))
                    {
                        dateConsistenceList.Add(new DateConsistence
                        {
                            TariffValue = TariffValues.OffPeack,
                            TotalMinutes = Math.Abs((dateFinish - dateInit).TotalMinutes)
                        });
                    }
                }
                //Intermediate I
                else if (dateInit.IsBetween(new Time(16, 30, 01), new Time(17, 29, 59)))
                {
                    if (dateFinish.IsBetween(new Time(16, 30, 01), new Time(17, 29, 59)))
                    {
                        dateConsistenceList.Add(new DateConsistence
                        {
                            TariffValue = TariffValues.Intermediate,
                            TotalMinutes = Math.Abs((dateFinish - dateInit).TotalMinutes)
                        });
                    }
                }
                //Peack
                else if (dateInit.IsBetween(new Time(17, 30, 00), new Time(20, 30, 00)) && dateFinish.IsBetween(new Time(17, 30, 00), new Time(20, 30, 00)))
                {
                    if (dateFinish.IsBetween(new Time(16, 30, 01), new Time(17, 29, 59)))
                    {
                        dateConsistenceList.Add(new DateConsistence
                        {
                            TariffValue = TariffValues.OnPeack,
                            TotalMinutes = Math.Abs((dateFinish - dateInit).TotalMinutes)
                        });
                    }
                }
                //Medium II
                else if (dateInit.IsBetween(new Time(20, 30, 01), new Time(21, 29, 59)))
                {
                    if (dateFinish.IsBetween(new Time(20, 30, 01), new Time(21, 29, 59)))
                    {
                        if (dateFinish.IsBetween(new Time(16, 30, 01), new Time(17, 29, 59)))
                        {
                            dateConsistenceList.Add(new DateConsistence
                            {
                                TariffValue = TariffValues.Intermediate,
                                TotalMinutes = Math.Abs((dateFinish - dateInit).TotalMinutes)
                            });
                        }
                    }
                }
                //OffPeack II
                else if (dateInit.IsBetween(new Time(21, 30, 00), new Time(23, 59, 59)))
                {
                    if (dateFinish.IsBetween(new Time(21, 30, 00), new Time(23, 59, 59)))
                    {
                        if (dateFinish.IsBetween(new Time(16, 30, 01), new Time(17, 29, 59)))
                        {
                            dateConsistenceList.Add(new DateConsistence
                            {
                                TariffValue = TariffValues.OffPeack,
                                TotalMinutes = Math.Abs((dateFinish - dateInit).TotalMinutes)
                            });
                        }
                    }
                }
            }
            //As datas estão em períodos diferentes
            else
            {




            }
            return dateConsistenceList;
        }
    }
}
