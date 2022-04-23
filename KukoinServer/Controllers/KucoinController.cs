using KuCoinApiClient.Model;
using KuCoinApiClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace KuCoinApiClient.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KucoinController : ControllerBase
    {
        private readonly KucoinProviderService kucoinProvider;

        public KucoinController(KucoinProviderService kucoinProvider)
        {
            this.kucoinProvider = kucoinProvider;
        }

        [HttpGet("default")]
        public Task<ActionResult<StatusDTO>> GetKucoin()
        {
            return GetKucoin("BTC-USDT");
        }

        [HttpGet("{pairId}")]
        public async Task<ActionResult<StatusDTO>> GetKucoin(string pairId)
        {
            var result = await kucoinProvider.GetInfo(pairId);

            if (result == null)
            {
                return NotFound($"Pair={pairId} can not be received!");
            }

            return result;
        }
    }
}
