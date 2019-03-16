using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using PwndAPILib.Models;

namespace PwndAPILib.ViewModel
{
    internal static class PwndAPI
    {
        public const string BASE_ACCOUNT_URL = "https://haveibeenpwned.com/api/v2/breachedaccount/";
        public const string BASE_PASSWORD_URL = "https://api.pwnedpasswords.com/range/";
        public const string USER_AGENT = "PwnCore-for-Dot-Net-Core";

        public static async Task<List<Breach>> GetBreachesAsync(string accountName)
        {
            List<Breach> breaches = new List<Breach>();

            string url = BASE_ACCOUNT_URL + accountName;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                breaches = JsonConvert.DeserializeObject<List<Breach>>(json);
            }

            return breaches;
        }

        public static List<Breach> GetBreaches(string accountName)
        {
            List<Breach> breaches = new List<Breach>();

            string url = BASE_ACCOUNT_URL + accountName;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string json = response.Content.ReadAsStringAsync().Result;

                breaches = JsonConvert.DeserializeObject<List<Breach>>(json);
            }

            return breaches;
        }

        public static async Task<List<PwndPassword>> GetPwndPasswordsAsync(string password)
        {
            throw new NotImplementedException();
        }

        public static List<PwndPassword> GetPwndPasswords(string password)
        {
            throw new NotImplementedException();
        }
    }
}
