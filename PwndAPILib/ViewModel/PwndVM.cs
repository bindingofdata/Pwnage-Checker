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
        public ObservableCollection<PwndPassword> Passwords { get; set; }

        public bool PasswordMatch
        {
            get
            {
                if (Passwords[0] == null)
                    return false;
                return Passwords[0].OccuranceCount > 0;
            }
        }

        public PwndVM()
        {
            Breaches = new ObservableCollection<Breach>();
            Passwords = new ObservableCollection<PwndPassword>();
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
            List<PwndPassword> passwords;

            if (async)
                passwords = await PwndAPI.GetPwndPasswordsAsync(password).ConfigureAwait(false);
            else
                passwords = PwndAPI.GetPwndPasswords(password);

            Passwords.Clear();

            foreach (PwndPassword pwndPassword in passwords)
            {
                Passwords.Add(pwndPassword);
            }
        }
    }
}
