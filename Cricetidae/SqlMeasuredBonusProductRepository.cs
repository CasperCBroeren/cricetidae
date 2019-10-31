using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Cricetidae.DTO;
using Cricetidae.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Cricetidae
{
    public class SqlMeasuredBonusProductRepository : IMeasuredBonusProductRepository
    {
        private readonly string ConnectionString; 

        public SqlMeasuredBonusProductRepository(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<IReadOnlyList<MeasuredBonusProduct>> GetRecentAndHistory()
        {
            var currentWeek = ISOWeek.GetWeekOfYear(DateTime.Now);
            var currentYear = DateTime.Now.Year;
            var result = new List<MeasuredBonusProduct>();
            var query = @"select [Id]
                                ,[Title]
                                ,[BonusOccurance]
                                ,[Brand]
                                ,[Store] 
                                ,[Category]
                                ,[Link]
                                    ,[StartDate]
                                    ,[EndDate]
                                    ,[FromPriceInCents]
                                    ,[ForPriceInCents]
                                    ,[Delta] 
                                    ,[DiscountText]
                                    ,[UnitSize]  
                                    ,[ActiveAtNumberOfProducts]
                                    ,[Week]
                                    ,[Year]
                            from [dbo].[vwBonusView] order by [Id], [Year] desc, [Week] desc";
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                var command = new SqlCommand(query, connection); 
                await connection.OpenAsync().ConfigureAwait(false);
                using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while(await reader.ReadAsync().ConfigureAwait(false))
                    {
                        var item = CreateMeassuredBonusProduct(reader);
                        if (item != null)
                        {
                            result.Add(item);
                        }
                    }

                    await reader.CloseAsync().ConfigureAwait(false);
                }
            }
            result.RemoveAll(x => x.Prices.Count(y => y.Week == currentWeek && y.Year == currentYear) == 0);
            return result.OrderBy(x => x.RawCategory).ToList();
        }

        private MeasuredBonusProduct workingItem;

        private MeasuredBonusProduct CreateMeassuredBonusProduct(SqlDataReader reader)
        {
            var id = reader.GetFieldValue<long>(0);
            var messuredBonusProductPrice = new MessuredBonusProductPrice()
            {
                StartDate = reader.GetFieldValue<DateTime>(7),
                EndDate = reader.GetFieldValue<DateTime>(8),
                FromPriceInCents = reader.GetFieldValue<int>(9),
                ForPriceInCents = reader.GetFieldValue<int>(10),
                Delta = reader.GetFieldValue<int>(11),
                DiscountText = reader.GetFieldValue<string>(12),
                UnitSize = reader.GetFieldValue<string>(13),
                ActiveAtNumberOfProducts = reader.GetFieldValue<int>(14),
                Week = reader.GetFieldValue<int>(15),
                Year = reader.GetFieldValue<int>(16),
            };
            if (workingItem == null
                || workingItem.Id != id)
            {
                var prices = new List<MessuredBonusProductPrice>
                {
                    messuredBonusProductPrice
                };


                workingItem = new MeasuredBonusProduct()
                {
                    Id = id,
                    Title = reader.GetFieldValue<string>(1),
                    BonusOccurance = reader.GetFieldValue<int>(2),
                    Brand = reader.GetFieldValue<string>(3),
                    Store = reader.GetFieldValue<string>(4),
                    RawCategory = reader.GetFieldValue<string>(5),
                    Link = reader.GetFieldValue<string>(6),
                    Prices = prices
                };
                return workingItem;
            }
            else
            {
                workingItem.Prices.Add(messuredBonusProductPrice);
                return null;
            }
        }

        public async Task<IEnumerable<MeasuredBonusProduct>> GetTopDeltaForStore(string store)
        {
            var items = await this.GetRecentAndHistory().ConfigureAwait(false);
            
            return items.Where(x => x.Store.Equals(store, StringComparison.InvariantCultureIgnoreCase) && x.Prices[0].Delta > 0)
                        .OrderByDescending(x => x.Prices[0]?.Delta).ToList();
        }
    }
}
