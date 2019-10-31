using Cricetidae.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using Cricetidae.Interfaces;
using Microsoft.Extensions.Logging;
using Cricetidae.Pipeline;

namespace Cricetidae
{
    public class BonusDataReader : IBonusDataReader
    {
        private const string AH_BASE_URL = "https://www.ah.nl";
        private const string AH_BONUS_PATH = "/bonus/api/bonus-lane";
        private readonly int currentWeekOfYear;
        private readonly HttpClient client;
        private readonly ILogger logger;

        public BonusDataReader(HttpClient client, ILogger<BonusDataReader> logger)
        {
            this.client = client;
            this.logger = logger;
            this.currentWeekOfYear = ISOWeek.GetWeekOfYear(DateTime.Now);
            logger.LogInformation("Starting to get bonus from week {CurrentWeek}", this.currentWeekOfYear);
        }

        public async Task<IReadOnlyList<BonusProductEvent>> Get()
        {
            var source = AH_BASE_URL + AH_BONUS_PATH;
            try
            {
                var response = await client.GetAsync(source).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var laneContent = JObject.Parse(content);

                var baseList = laneContent["lanes"]
                    .SelectMany(x => x["items"])
                    .Select(x => new BonusProductEvent(x as JObject))
                    .Where(x => x.Id != long.MinValue)
                    .ToList();
                var mo = baseList.FirstOrDefault(x => x.Id == 103193);
                var expandList = baseList.Where(x => x.NumberOfProducts > 1).Select(x => GetBonusProductsFromSubPage(x)).ToList();
                await Task.WhenAll(expandList);
                baseList.RemoveAll(x => x.NumberOfProducts > 1);

                baseList.AddRange(expandList.SelectMany(x => x.Result));

                return baseList
                    .GroupBy(x => x.Id)
                    .Select(grp => grp.FirstOrDefault())
                    .ToList();
            }
            catch (Exception exc)
            {
                this.logger.LogError(exc, "Something went wrong retrieving the data at {source}", source);
                return Array.Empty<BonusProductEvent>();
            }
        }

        private async Task<IReadOnlyList<BonusProductEvent>> GetBonusProductsFromSubPage(BonusProductEvent product)
        {
            var source = AH_BASE_URL + ConstructApiPath(product.Id);
            try
            {    
                var response = await client.GetAsync(source).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var detailContent = JObject.Parse(content);

                return detailContent["cards"]
                    .SelectMany(x => x["products"])
                    .Select(x => new BonusProductEvent(x as JObject))
                    .ToList();
            }
            catch (Exception exc)
            {
                this.logger.LogError(exc, "Something went wrong retrieving the subpage at {source}", source);
                return Array.Empty<BonusProductEvent>();
            }
        }

        private string ConstructApiPath(long id)
        { 
            // To; /bonus/api/segment/36/268499/0/30
            return $"/bonus/api/segment/{this.currentWeekOfYear}/{id}/0/30";
        }

        public async Task DoWork(APipeLineContext context)
        {
            ((BonusContext)context).Items = await Get();
        }
         
    }
}
