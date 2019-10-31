using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cricetidae.Tests
{
    public class BonusDataReaderTests
    {
        [Fact]
        public async Task Get_ShouldReturnAtLeast100BonusProducts()
        {
            var logger = new Mock<ILogger<BonusDataReader>>();
            using (var client = new System.Net.Http.HttpClient())
            {
                var reader = new BonusDataReader(client, logger.Object);

                var items = await reader.Get();

                items.Count.ShouldBeGreaterThan(100);
            } 
        }


        [Fact]
        public async Task Get_Week38OnlyShouldReturnMossels()
        {
            var logger = new Mock<ILogger<BonusDataReader>>();
            using (var client = new System.Net.Http.HttpClient())
            {
                var reader = new BonusDataReader(client, logger.Object);

                var items = await reader.Get();
                var mo = items.FirstOrDefault(x => x.Id == 103193);
                mo.ShouldNotBe(null);
            }
        }
    }
}
