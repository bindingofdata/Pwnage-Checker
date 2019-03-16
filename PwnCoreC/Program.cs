using System;
using System.Text;

using PwndAPILib.Models;
using PwndAPILib.ViewModel;

namespace PwnCoreC
{
    internal class Program
    {
        private static PwndVM PwndVM;

        private static void Main(string[] args)
        {
            PwndVM = new PwndVM();

            bool exitApplication = false;
            while (!exitApplication)
            {
                exitApplication = MainMenu();
            }
        }

        private static bool MainMenu()
        {
            Console.Clear();
            bool exitApplication = false;
            StringBuilder message = new StringBuilder();
            message.Append("---=== Have You Been Pwnd? ===---\n\n");
            message.Append("Main Menu\n");
            message.Append("1. Check [U]sername/[E]mail\n");
            message.Append("2. Check [P]assword\n");
            message.Append("3. [H]elp\n");
            message.Append("4. [E]xit\n");
            message.Append("--> ");
            Console.Write(message.ToString());

            switch (Console.ReadLine().ToLower())
            {
                case "1":
                case "u":
                case "e":
                    LookupUsernameOrEmail();
                    PrintBreaches();
                    break;
                default:
                    break;
            }

            return exitApplication;
        }

        private static void LookupUsernameOrEmail()
        {
            Console.Clear();
            Console.WriteLine("Enter the username or email address to look up.");
            Console.Write("---> ");
            string accountName = Console.ReadLine();

            PwndVM.GetBreaches(accountName, false);
        }

        private static void PrintBreaches()
        {
            if (PwndVM.Breaches.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("No breaches found!!!");
                Console.WriteLine();
                Console.WriteLine("press any key to continue...");
                Console.ReadKey();
                return;
            }

            StringBuilder results = new StringBuilder();
            int breachCount = 1;

            foreach (Breach breach in PwndVM.Breaches)
            {
                results.Clear();
                results.AppendFormat("~~~ Breach #{0}/{1} - {2} ~~~\n", breachCount, PwndVM.Breaches.Count, breach.Title);
                results.AppendFormat("Site Name: {0}\n", breach.Name);
                results.AppendFormat("URL: {0}\n", breach.Domain);
                results.AppendFormat("Date of Breach: {0}\n", breach.BreachDate);
                results.AppendFormat("Date Added to DB: {0}\n", breach.AddedDate);
                results.AppendFormat("Breached Data: \n");
                foreach (string dataItem in breach.DataClasses)
                {
                    results.AppendFormat("---> {0}\n", dataItem);
                }
                results.AppendFormat("Breach Verified: {0}\n", breach.IsVerified ? "True" : "False" );
                results.AppendFormat("~~~~~~~~~~~~~~~~~~~~~~\n\n");
                Console.WriteLine(results.ToString());
                Console.WriteLine("press any key to continue...");
                Console.ReadKey();
                breachCount++;
            }
        }
    }
}
