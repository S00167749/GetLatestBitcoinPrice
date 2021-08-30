using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GetLatestBitcoinPrice
{
    class Program
    {
        static void Main(string[] args)
        {

            var data = GetJsonDataAsync().GetAwaiter().GetResult();

            if (data == null)
            {
                Console.WriteLine("Error");
            }

            Console.WriteLine($"BTC Price EUR {data}.");

            Console.ReadKey();
        }

        private static async Task<string> GetJsonDataAsync()
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                
                return ParseJson(jsonData);
            }

            return null;
        }

        private static string ParseJson(string data)
        {
            JObject jsonObject = JObject.Parse(data);

            var chartName = (string)jsonObject["chartName"];
            var code = (string)jsonObject["bpi"]["EUR"]["code"];
            var rate = (string)jsonObject["bpi"]["EUR"]["rate"];

            if (chartName == "Bitcoin" && code == "EUR")
            {
                return rate;
            }

            return null; 
        }
    }
}
