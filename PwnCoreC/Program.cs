using System;
using System.IO;
using System.Text;

using PwndAPILib.Models;
using PwndAPILib.ViewModel;

namespace PwnCoreC
{
    internal class Program
    {
        private static PwndVM PwndVM;
        private const string pressToContinue = "press any key to continue...";
        private static string accountName;

        private static void Main(string[] args)
        {
            PwndVM = new PwndVM();
            accountName = string.Empty;

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
            message.Append("1. Check [U]sername/Email\n");
            message.Append("2. Check [P]assword\n");
            message.Append("3. [H]elp\n");
            message.Append("4. [E]xit\n");
            message.Append("--> ");
            Console.Write(message.ToString());

            string userSelection = Console.ReadLine().ToLower();

            switch (userSelection ?? string.Empty)
            {
                case "1":
                case "u":
                    LookupUsernameOrEmail();
                    PrintBreaches();
                    break;
                case "2":
                case "p":
                    LookupPassword();
                    PrintResult();
                    break;
                case "3":
                case "h":
                case "?":
                    DisplayHelp();
                    break;
                case "4":
                case "e":
                case "q":
                    exitApplication = true;
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Unrecognized command. Please select an option from the menu.");
                    Console.WriteLine(pressToContinue);
                    break;
            }

            return exitApplication;
        }

        private static void DisplayHelp()
        {
            throw new NotImplementedException();
        }

        private static void LookupPassword()
        {
            Console.Clear();
            Console.WriteLine("Enter the password you wish to look up.");
            Console.Write("---> ");
            string password = string.Empty;
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            PwndVM.CheckPassword(password, false);
        }

        private static void PrintResult()
        {
            Console.Clear();

            if (PwndVM.PasswordMatch)
                Console.WriteLine($"Your password was found " + PwndVM.Passwords[0].OccuranceCount + " times.");
            else
                Console.WriteLine("Your password was not found! Congrats!");

            Console.WriteLine(pressToContinue);
            Console.ReadKey();
        }

        private static void LookupUsernameOrEmail()
        {
            Console.Clear();
            Console.WriteLine("Enter the username or email address to look up.");
            Console.Write("---> ");
            accountName = Console.ReadLine();

            PwndVM.GetBreaches(accountName, false);
        }

        private static void PrintBreaches()
        {
            if (PwndVM.Breaches.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("No breaches found!!!");
                Console.WriteLine();
                Console.WriteLine(pressToContinue);
                Console.ReadKey();
                return;
            }

            StringBuilder currentResult = new StringBuilder();
            StringBuilder combinedResults = new StringBuilder();
            int breachCount = 1;

            foreach (Breach breach in PwndVM.Breaches)
            {
                Console.Clear();
                currentResult.Clear();
                currentResult.AppendFormat("~~~ Breach #{0}/{1} - {2} ~~~\n", breachCount, PwndVM.Breaches.Count, breach.Title);
                currentResult.AppendFormat("Site Name: {0}\n", breach.Name);
                currentResult.AppendFormat("URL: {0}\n", breach.Domain);
                currentResult.AppendFormat("Date of Breach: {0}\n", breach.BreachDate);
                currentResult.AppendFormat("Date Added to DB: {0}\n", breach.AddedDate);
                currentResult.AppendFormat("Breached Data: \n");
                foreach (string dataItem in breach.DataClasses)
                {
                    currentResult.AppendFormat("---> {0}\n", dataItem);
                }
                currentResult.AppendFormat("Breach Verified: {0}\n", breach.IsVerified ? "True" : "False" );
                currentResult.AppendFormat("~~~~~~~~~~~~~~~~~~~~~~\n\n");
                Console.WriteLine(currentResult.ToString());
                combinedResults.Append(currentResult.ToString());
                Console.WriteLine(pressToContinue);
                Console.ReadKey();
                breachCount++;
            }

            PromptSave(combinedResults);
        }

        private static void PromptSave(StringBuilder dataToSave)
        {
            bool choiceMade = false;

            while (!choiceMade)
            {
                Console.Clear();
                Console.WriteLine("Would you like to save your breach data to the desktop? [y/n]");
                Console.Write("---> ");
                string userChoice = Console.ReadLine().ToLower();

                switch (userChoice)
                {
                    case "y":
                        SaveData(dataToSave);
                        choiceMade = true;
                        break;
                    case "n":
                        Console.Clear();
                        Console.WriteLine("Are you sure to want to exit without saving? [y/n]");
                        Console.Write("---> ");
                        choiceMade = (Console.ReadLine().ToLower() == "y");
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Unrecognized command. Please enter either \'y\' or \'n\'");
                        Console.WriteLine(pressToContinue);
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void SaveData(StringBuilder dataToSave)
        {
            string date = DateTime.Now.ToString();
            char[] invalidPathCharacters = Path.GetInvalidPathChars();
            char[] invalidFileNameCharacters = Path.GetInvalidFileNameChars();
            string fileName = $"Account Search - {accountName} - {date}.txt";
            foreach (char character in invalidFileNameCharacters)
                fileName = fileName.Replace(character, '-');
            string filePath = Environment.ExpandEnvironmentVariables(@"%HOMEPATH%\Desktop\") + fileName;
            foreach (char character in invalidPathCharacters)
                filePath = filePath.Replace(character, '-');

            using (StreamWriter saver = new StreamWriter(new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
                StringReader reader = new StringReader(dataToSave.ToString());
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    saver.WriteLine(line);
                }
            }
        }
    }
}
