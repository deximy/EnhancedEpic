using EnhancedEpic.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EnhancedEpic.Services
{
    public class EpicdbService
    {
        private readonly HttpClient http_client_ = new();

        public EpicdbService()
        {
        }

        public async Task<GameOffer> GetGameOffer(string game_id)
        {
            var result = new GameOffer() {
            };

            var api_result = await FetchOfferData(game_id);
            Console.WriteLine(api_result);

            return result;
        }

        async Task<EpicDbApiResult> FetchOfferData(string offer_id)
        {
            var response = await http_client_.GetAsync($"https://epicgamesdb.info/_next/data/1dpDNSnQyX3wrB0NgBx5f/p/{offer_id}/placeholder.json");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EpicDbApiResult>(content);
            if (result == null)
            {
                throw new InvalidCastException($"Unable to deserialize data from EpicInfoDb. Offer ID: {offer_id}");
            }
            if (result.props == null)
            {
                throw new InvalidCastException($"Unable to deserialize props in data. Offer ID: {offer_id}");
            }
            if (result.props.prices == null)
            {
                throw new InvalidCastException($"Unable to deserialize prices in props. Offer ID: {offer_id}");
            }
            if (result.props.price_logs == null)
            {
                throw new InvalidCastException($"Unable to deserialize price logs in data. Offer ID: {offer_id}");
            }
            return result;
        }
    }

    class EpicDbApiResult
    {
        [JsonPropertyName("pageProps")]
        public EpicDbProps? props {  get; set; }
    }

    class EpicDbProps
    {
        public EpicDbPrice[]? prices { get; set; }

        [JsonPropertyName("priceLogs")]
        public EpicDbPrice[]? price_logs { get; set; }
    }

    class EpicDbPrice
    {
        [JsonPropertyName("countryCode")]
        public string? country_code { get; set; }

        [JsonPropertyName("currencyCode")]
        public string? currency_code { get; set; }

        [JsonPropertyName("currentPrice")]
        public int basis_price { get; set; }

        [JsonPropertyName("appliedRule")]
        public EpicDbDiscountRule? discount_rule { get; set; }
    }

    class EpicDbDiscountRule
    {
        [JsonPropertyName("startDate")]
        public string? start_date { get; set; }

        [JsonPropertyName("endDate")]
        public string? end_date { get; set; }

        [JsonPropertyName("discountSetting")]
        public EpicDbDiscountSetting? discount_setting { get; set; }
    }

    class EpicDbDiscountSetting
    {
        [JsonPropertyName("discountPercentage")]
        public int discount_percentage { get; set; }
    }
}
