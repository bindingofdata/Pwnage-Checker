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
    }
}
