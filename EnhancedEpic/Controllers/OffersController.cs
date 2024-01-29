using EnhancedEpic.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnhancedEpic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OffersController : ControllerBase
    {
        private CurrencyExchangeService currency_exchange_service_;
        private EpicdbService epicdb_service_;

        public OffersController(CurrencyExchangeService currency_exchange_service, EpicdbService epicdb_service)
        {
            currency_exchange_service_ = currency_exchange_service;
            epicdb_service_ = epicdb_service;
        }

        [HttpGet("{offer_id}")]
        public async Task<IActionResult> GetOffers([FromRoute] string offer_id)
        {
            return Ok(await epicdb_service_.GetGameOffer(offer_id));
        }
    }
}
