using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography;
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
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
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

        public static async Task<Dictionary<string, string>> GetPwndPasswordsAsync(string password)
        {
            throw new NotImplementedException();
        }

        public static string GetPasswordHash(string password)
        {
            SHA1 hasher = SHA1.Create();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashedBytes = hasher.ComputeHash(passwordBytes);

            StringBuilder hashConverter = new StringBuilder();
            foreach (byte b in hashedBytes)
                hashConverter.Append(b.ToString("x2"));

            return hashConverter.Length > 0 ? hashConverter.ToString().ToUpper() : string.Empty;
        }

        public static Dictionary<string,string> GetPwndPasswords(string passwordHash)
        {
            Dictionary<string,string> passwords = new Dictionary<string,string>();
            string passwordHashPrefix = passwordHash.Substring(0, 5).ToUpper();

            string url = BASE_PASSWORD_URL + passwordHashPrefix;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                StringReader reader = new StringReader(response.Content.ReadAsStringAsync().Result);
                string line = string.Empty;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] result = line.Split(':');
                    passwords.Add(result[0], result[1]);
                }
            }

            return passwords;
        }
    }
}
