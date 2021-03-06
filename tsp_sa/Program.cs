﻿using System;
using System.Diagnostics;
using static System.Console;


namespace tsp_sa
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0].Equals("-help") || args[0].Equals("-h"))
            {
                ShowHelp();
            }
            else
            {
                string fileName = args[0];
                double? t = null;
                double? f = null;
                double? r = null;

                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i].Equals("-t")) t = Double.Parse(args[i + 1]);
                    if (args[i].Equals("-f")) f = Double.Parse(args[i + 1]);
                    if (args[i].Equals("-r")) r = Double.Parse(args[i + 1]);
                }

                TSP tsp = new TSP();
                tsp.GetCitiesInfo(fileName);
                //tsp.GetBestTourInfo(@"..\..\Lib\eil51.opt.tour");

                SA sa = new SA(tsp);
                Run(sa, t, f, r);
                //sa.DisPlayBestTour();
            }
#if DEBUG
            WriteLine("Press any key for closing...");
            ReadKey();
#endif
        }

        private static void ShowHelp()
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine("Traveling Salesman Problem solver with Simulated Annealing algorithm");
            ResetColor();
            WriteLine();
            WriteLine("Syntax:    [FileName] [Options]");
            WriteLine();
            WriteLine("[File Name] \t: File name path, file format as TSPLIB95, egde weigh is EUC_2D");
            WriteLine();
            WriteLine("Options:");
            WriteLine("    -p \t: Percentage of intial solution cost (intial temperature)");
            WriteLine("    -f \t: Final temperature that will stop program");
            WriteLine("    -r \t: Temperature reduction factor");
            WriteLine();
            WriteLine("For Example: eli51.tsp -p 10 -f 0.001 -r 0.99");
        }

        private static void Run(SA sa, double? t, double? f, double? r)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            if (t == null && f == null && r == null) sa.Execute();
            else if (t == null && f == null && r != null) sa.Execute(coolingRate: r.Value);
            else if (t == null && f != null && r == null) sa.Execute(finalTemp: f.Value);
            else if (t != null && f == null && r == null) sa.Execute(tempPercent: t.Value);
            else if (t != null && f != null && r == null) sa.Execute(tempPercent: t.Value, finalTemp: f.Value);
            else if (t == null && f != null && r != null) sa.Execute(finalTemp: f.Value, coolingRate: r.Value);
            else if (t != null && f == null && r != null) sa.Execute(tempPercent: t.Value, coolingRate: r.Value);
            else sa.Execute(t.Value, f.Value, r.Value);

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;
            WriteLine($"Run Time {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}");

        }
    }
}

