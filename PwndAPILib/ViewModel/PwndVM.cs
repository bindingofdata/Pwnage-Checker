using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using PwndAPILib.Models;

namespace PwndAPILib.ViewModel
{
    public class PwndVM
    {
        public ObservableCollection<Breach> Breaches { get; set; }

        public bool PasswordMatch { get; private set; }
        public string PasswordOccurrences { get; private set; }

        public PwndVM()
        {
            Breaches = new ObservableCollection<Breach>();
        }

        public async void GetBreaches(string accountName, bool async)
        {
            List<Breach> breaches;

            if (async)
                breaches = await PwndAPI.GetBreachesAsync(accountName).ConfigureAwait(false);
            else
                breaches = PwndAPI.GetBreaches(accountName);

            Breaches.Clear();

            foreach (Breach breach in breaches)
            {
                Breaches.Add(breach);
            }
        }

        public async void CheckPassword(string password, bool async)
        {
            Dictionary<string,string> passwords;
            string passwordHash = PwndAPI.GetPasswordHash(password);
            string passwordOccurrences = string.Empty;

            if (async)
                passwords = await PwndAPI.GetPwndPasswordsAsync(passwordHash).ConfigureAwait(false);
            else
                passwords = PwndAPI.GetPwndPasswords(passwordHash);

            PasswordMatch = passwords.TryGetValue(passwordHash.Substring(5), out passwordOccurrences);
            PasswordOccurrences = string.IsNullOrWhiteSpace(passwordOccurrences) ? "0" : passwordOccurrences;
        }
    }
}
