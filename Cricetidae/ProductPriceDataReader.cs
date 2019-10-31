using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cricetidae.DTO;
using Cricetidae.Interfaces;
using Cricetidae.Pipeline;
using Microsoft.Extensions.Logging;

namespace Cricetidae
{
    public class ProductPriceDataReader : IProductPriceDataReader, IPipeLineItem
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<ProductPriceDataReader> logger;
        private const string AH_PRODUCT_URL = "https://www.ah.nl/producten/product/wi{0}/aPRoduct";

        public ProductPriceDataReader(HttpClient httpClient, ILogger<ProductPriceDataReader> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task DoWork(APipeLineContext context)
        {
            var bContext = ((BonusContext)context); 
            var noPriceItems = bContext.Items.Where(x => x.ForPriceInCents == 0 || x.FromPriceInCents == 0).ToList();
            logger.LogInformation($"Looking up {noPriceItems.Count} items");
            await GetProductPriceData(noPriceItems);

        }

        public async Task GetProductPriceData(IEnumerable<BonusProductEvent> noPriceItems)
        {
            var setPricesTasks = noPriceItems.Select(x => SetPriceForProduct(x));
            await Task.WhenAll(setPricesTasks);
        }

        private async Task SetPriceForProduct(BonusProductEvent product)
        {
            var url = string.Format(AH_PRODUCT_URL, product.Id);
            try
            {
                var response = await httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(content);
                var priceNode = doc.DocumentNode.SelectSingleNode(@"//*[@class=""product-card-hero-price_root__PgaiS""]");

                var fromPriceNode = priceNode.ChildNodes[0];
                var fromPriceWhole = int.Parse(fromPriceNode.ChildNodes[0].InnerText) * 100;
                var fromPriceCents = int.Parse(fromPriceNode.ChildNodes[2].InnerText);

                product.FromPriceInCents = fromPriceWhole + fromPriceCents;

                var forPriceNode = priceNode.ChildNodes[1];
                var forPriceWhole = int.Parse(forPriceNode.ChildNodes[0].InnerText) * 100;
                var forPriceCents = int.Parse(forPriceNode.ChildNodes[2].InnerText);

                product.ForPriceInCents = forPriceWhole + forPriceCents;
            }
            catch(Exception exc)
            {
                this.logger.LogError("Unable to get price for {0}; {1}", product.Id, exc.Message);
            }
        }
    }
}
