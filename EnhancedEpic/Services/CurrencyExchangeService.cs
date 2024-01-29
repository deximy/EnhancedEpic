using System.Collections.Concurrent;
using System.Text.Json;

namespace EnhancedEpic.Services
{
    public class CurrencyExchangeService
    {
        private Dictionary<(string from, string to), (decimal rate, DateTime update_time)> exchange_rates = new();
        private HttpClient http_client = new();
        private ConcurrentDictionary<string, Task> ongoing_updates = new();

        private async Task UpdateRatesFromApiAsync(string base_currency)
        {
            if (ongoing_updates.TryGetValue(base_currency, out var existing_update))
            {
                Console.WriteLine($"The update from {base_currency} exists. Waiting for it in case of duplicating task.");
                await existing_update;
                return;
            }

            var UpdateRatesFromApiInternalAsync = async (string base_currency) => {
                Console.WriteLine("Start to update rate...");
                string url = $"https://open.er-api.com/v6/latest/{base_currency}";
                var response = await http_client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                Console.WriteLine("Fetching exchange rates from API successfully.");

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ExchangeRateApiResult>(content);

                if (result == null)
                {
                    throw new FormatException("Unable to deserialize response.");
                }
                if (result.result != "success" || result.rates == null)
                {
                    throw new InvalidDataException("Not succeed according to response content.");
                }

                DateTime next_update_time = DateTimeOffset.FromUnixTimeSeconds(result.time_next_update_unix).UtcDateTime;

                foreach (var rate in result.rates)
                {
                    string target_currency = rate.Key;
                    decimal exchange_rate = rate.Value;

                    UpdateExchangeRage(base_currency, target_currency, exchange_rate, next_update_time);
                }
            };
            var update_task = UpdateRatesFromApiInternalAsync(base_currency);
            ongoing_updates[base_currency] = update_task;

            await update_task;
            ongoing_updates.Remove(base_currency, out _);
        }

        private void UpdateExchangeRage(string from, string to, decimal rate, DateTime next_update_time)
        {
            exchange_rates[(from, to)] = (rate, next_update_time);
            Console.WriteLine($"Exchange rate from {from} to {to} has already been updated. Next update time: {next_update_time.ToLocalTime()}");
        }

        public async Task<decimal> ConvertCurrency(decimal amount, string from, string to)
        {
            if (!exchange_rates.TryGetValue((from, to), out var rate_info) || DateTime.UtcNow > rate_info.update_time)
            {
                Console.WriteLine("Unable to find the specific exchange rate or the exchange rate has expired. Try to update.");
                await UpdateRatesFromApiAsync(from);
            }

            if (exchange_rates.TryGetValue((from, to), out rate_info))
            {
                return amount * rate_info.rate;
            }
            
            throw new InvalidOperationException($"Unable to find the specific exchange rate from {from} to {to}.");
        }
    }

    class ExchangeRateApiResult
    {
        public string? result { get; set; }

        public long time_next_update_unix { get; set; }

        public Dictionary<string, decimal>? rates { get; set; }
    }
}
