using System;
using System.Collections.Generic;
using System.Numerics;
using System.Timers;
using System.Windows.Input;

namespace TaxiCostSim
{
    class Program
    {
        static public DateTime time = new DateTime();
        static public DateTime timestart = new DateTime();
        static public Timer tim = new Timer();

        static public bool Running = false;
        static public bool WeekendCost = false;
        static public bool NightCost = false;

        static public float TimePassed = 0;
        static public float Dist = 0;
        static public float Traveled = 0;
        static public float Speed = 60;

        static public float CostMinDay = 0.25f;
        static public float CostMinNight = 0.45f;
        static public float CostMinDayWeek = 0.29f;
        static public float CostMinNightWeek = 0.52f;
        static public float CostKM = 1;
        static public float CostKMWeekend = 1.15f;

        static public float RideCost = 0;
        static public List<DayOfWeek> Weekend = new List<DayOfWeek>();

        static void Main(string[] args)
        {
            Weekend.Add(DayOfWeek.Friday);
            Weekend.Add(DayOfWeek.Saturday);
            Weekend.Add(DayOfWeek.Sunday);
            Weekend.Add(DayOfWeek.Monday);
            Console.WriteLine("Welcome to Taxi Cost Simulator!");
            CheckTime();
            Console.WriteLine("Please Enter a Distance Value");
            Dist = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please Enter a Travel Speed Value");
            Speed = Convert.ToInt32(Console.ReadLine());
            Speed = Speed / 60 / 60;
            RunSet();
            Console.ReadKey();
        }

        static void CheckTime()
        {
            tim.Interval = 100;
            tim.Elapsed += new ElapsedEventHandler(timer_tick);
            tim.Enabled = true;

        }
        static void RunSet()
        {
            timestart = DateTime.UtcNow;
            Running = true;
        }
        static void timer_tick(object sender, ElapsedEventArgs e)
        {
            time = DateTime.UtcNow;
            CheckDay();
            CheckHour();
            //Console.WriteLine("HH: " + time.ToString()[9] + time.ToString()[10] + " MM: " + time.ToString()[12] + time.ToString()[13] + " SS: " + time.ToString()[15] + time.ToString()[16]);
            
            if (Running)
            {
                Console.Clear();
                if(Convert.ToString(RideCost).Length > 3)
                {
                    Console.WriteLine("Cost Total:      " + Convert.ToString(RideCost).Substring(0, Convert.ToString(RideCost).IndexOf(".")) + Convert.ToString(RideCost)[Convert.ToString(RideCost).IndexOf(".")] + Convert.ToString(RideCost)[Convert.ToString(RideCost).IndexOf(".") +1 ] + Convert.ToString(RideCost)[Convert.ToString(RideCost).IndexOf(".")+2]);
                }
                else
                {
                    Console.WriteLine("Cost Total:      " + RideCost);
                }
                Console.WriteLine("DistTraveled:    " + Traveled);
                Console.WriteLine("StartTime:       " + timestart);
                Console.WriteLine("TimeNow:         " + time);
                Console.WriteLine("Seconds Passed:  " + TimePassed);
                Console.WriteLine("Day:             " + time.DayOfWeek);
                Console.WriteLine("Weekend:         " + WeekendCost);
                Console.WriteLine("Night:           " + NightCost);
                TimePassed += 0.1f;
                if (Traveled < Dist)
                {
                    Traveled = Speed * TimePassed;
                    if (WeekendCost)
                    {
                        if (NightCost)
                        {
                            RideCost = Traveled * CostKMWeekend + TimePassed / 60 * CostMinNightWeek;
                        }
                        if (!NightCost)
                        {
                            RideCost = Traveled * CostKMWeekend + TimePassed / 60 * CostMinDayWeek;
                        }
                    }
                    if (!WeekendCost)
                    {
                        if (NightCost)
                        {
                            RideCost = Traveled * CostKM + TimePassed / 60 * CostMinNight;
                        }
                        if (!NightCost)
                        {
                            RideCost = Traveled * CostKM + TimePassed / 60 * CostMinDay;
                        }
                    }
                }
            }
        }

        static void CheckDay()
        {
            if (Weekend.Contains(time.DayOfWeek))
            {
                if (time.DayOfWeek == DayOfWeek.Friday)
                {
                    if (time.Hour >= 22)
                    {
                        WeekendCost = true;
                    }
                    if (time.Hour < 22)
                    {
                        WeekendCost = false;
                    }
                }
                if (time.DayOfWeek == DayOfWeek.Saturday)
                {
                    WeekendCost = true;
                }
                if (time.DayOfWeek == DayOfWeek.Sunday)
                {
                    WeekendCost = true;
                }
                if (time.DayOfWeek == DayOfWeek.Monday)
                {
                    if (time.Hour < 7)
                    {
                        WeekendCost = true;
                    }
                    if (time.Hour >= 7)
                    {
                        WeekendCost = false;
                    }
                }
            }
            if (!Weekend.Contains(time.DayOfWeek))
            {
                WeekendCost = false;
            }
        }
        
        static void CheckHour()
        {
            if(time.Hour >= 18 || time.Hour < 8)
            {
                NightCost = true;
            }
            if(time.Hour < 18 && time.Hour >= 8)
            {
                NightCost = false;
            }
        }
    }
}
