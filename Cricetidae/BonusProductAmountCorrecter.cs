using Cricetidae.Interfaces;
using Cricetidae.Pipeline;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cricetidae
{
    public class BonusProductAmountCorrecter : IBonusProductAmountCorrecter
    {
        private readonly string ConnectionString;
        private readonly ILogger<BonusProductAmountCorrecter> logger;

        public BonusProductAmountCorrecter(IConfiguration configuration, ILogger<BonusProductAmountCorrecter> logger)
        {
            this.ConnectionString = configuration.GetConnectionString("DefaultConnection");
            this.logger = logger;
        }

        public async Task DoWork(APipeLineContext context)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                await connection.OpenAsync();
                var command1 = new SqlCommand(@"  update  [dbo].[BonusProductsEvents] set
  [FromPriceInCents] = [FromPriceInCents],
  [ForPriceInCents] = [ForPriceInCents]/2,
  [ActiveAtNumberOfProducts] = 2
  where (DiscountText like '2 voor%' or DiscountText like '2e gratis' ) and ActiveAtNumberOfProducts=1 and [ForPriceInCents]>0", connection);

                await command1.ExecuteNonQueryAsync();

            }
        }
    }
}
