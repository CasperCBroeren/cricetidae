using Cricetidae.DTO;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Cricetidae.Tests
{
    public class ProductDataReaderTests
    {
        [Fact]
        public async Task Get_wi103193_price()
        {
            var logger = new Mock<ILogger<ProductPriceDataReader>>();
            using (var client = new System.Net.Http.HttpClient())
            {
                var productcode = 103193;
                var reader = new ProductPriceDataReader(client, logger.Object);
                var context = new BonusContext()
                {
                    Items = new BonusProductEvent[]
                {
                    new BonusProductEvent(productcode)
                }
                };
                await reader.DoWork(context);

                context.Items[0].FromPriceInCents.ShouldBeGreaterThan(1000);
            }
        }
    }
}
