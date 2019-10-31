using System.Collections.Generic;
using System.Threading.Tasks;
using Cricetidae.DTO;
using Cricetidae.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Cricetinea.Web.Controllers
{
    [ApiController]
    public class MeasuredBonusProductController : ControllerBase
    {
        private readonly IMeasuredBonusProductRepository measuredBonusProductProductRepository;

        public MeasuredBonusProductController(IMeasuredBonusProductRepository measuredBonusProductProductRepository)
        {
            this.measuredBonusProductProductRepository = measuredBonusProductProductRepository;
        }

        [EnableCors]
        [HttpGet]
        [Route("api/MeasuredBonusProduct")]
        [ResponseCache(Duration = 3660)]
        public async Task<IEnumerable<MeasuredBonusProduct>> Get()
        {
            return await this.measuredBonusProductProductRepository.GetRecentAndHistory();
        }

        [EnableCors]
        [HttpGet]
        [Route("api/TopDelta/{store}")]
        [ResponseCache(Duration = 3660)]
        public async Task<IEnumerable<MeasuredBonusProduct>> TopDelta(string store)
        {
            return await this.measuredBonusProductProductRepository.GetTopDeltaForStore(store);
        }
    }
}
