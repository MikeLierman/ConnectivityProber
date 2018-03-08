/*
 
  This Source Code is subject to the terms of the APACHE
  LICENSE 2.0. You can obtain a copy of the terms at
  https://www.apache.org/licenses/LICENSE-2.0
  Copyright (C) 2018 Invise Labs

  Learn more about Invise Labs and our projects by visiting: http://invi.se/labs

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectivityProber
{
    class Program
    {
        static void Main(string[] args)
        {
            DoStuff();
        }

        /* VARIABLES */
        private static bool checkFailed = false;

        private static void DoStuff()
        {
            /* KEEP THINGS OUT OF Main() */
            PingTesting();

            /* What did we find, boys? */
            /* Return the exit code. */
            if (checkFailed) { Console.WriteLine("CHECK FAILED;"); Environment.Exit(-3); }
            else { Console.WriteLine("CHECK PASSED;"); Environment.Exit(0); }
        }

        private static void PingTesting()
        {
            // Variables
            long trip = 1; /* RESPONSE TIME */
            long avg = 0; /* TOTAL AVERAGE TIME */
            int packets = 0; /* NUMBER OF TOTAL PING PACKETS SENT OUT */
            int err = 0; /* ERRORS ENCOUNTERED */
            var p = new Ping();
            PingReply pr;

            while (packets < 200)
            {
                packets++;
                pr = p.Send("google.com");
                if (pr.Status == IPStatus.Success)
                {
                    trip = pr.RoundtripTime;
                    avg += trip;
                }
                else
                { err++; }

                Thread.Sleep(500);
            }

            /* GET AVERAGE */
            avg = avg / packets;

            /* NO WAY OUR AVERAGE ROUNDTRIP TIME IS LESS THAN 2MS OR GREATER THAN 100. 
             * ALSO FAIL CHECK IF DROPPED PACKETS OVER 5, OR AV */
            if (avg < 2)
            { Console.WriteLine("No connection detected; "); err = 200; checkFailed = true; }
            else if (avg > 100)
            { Console.WriteLine("Heavy jitter detected; "); checkFailed = true; }
            else if (err > 5)
            {  checkFailed = true; }
            Console.WriteLine("Sent="+packets+", Received="+(packets-err).ToString()+", Lost="+err+", Avg=" + avg+"; ");
        }
    }

}
