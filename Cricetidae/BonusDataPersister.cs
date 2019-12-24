using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cricetidae.DTO;
using Cricetidae.Pipeline;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cricetidae.UpdateData
{
    public class BonusDataPersister : IPipeLineItem
    {
        private readonly string ConnectionString;
        private readonly ILogger<BonusDataPersister> logger;

        public BonusDataPersister(IConfiguration configuration, ILogger<BonusDataPersister> logger)
        {
            this.ConnectionString = configuration.GetConnectionString("DefaultConnection");
            this.logger = logger;
        } 

        public async Task DoWork(APipeLineContext context)
        {
            var bContext = ((BonusContext)context);
           
            logger.LogInformation($"Saving {bContext.Items.Count} items");
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                await connection.OpenAsync();
                        ///                                        1     2     3   4        5 
                var insertPart = "insert into BonusProductsEvents(Year, Week, ID, Title, StartDate, EndDate, FromPriceInCents, ForPriceInCents, DiscountText, UnitSize, Category, Link, Brand, ActiveAtNumberOfProducts, Store) VALUES ";
                var valuePart = "({0}, {1}, {2}, '{3}', '{4}', '{5}', {6}, {7}, '{8}', '{9}', '{10}', '{11}', '{12}', {13}, '{14}')"; 
                var index = 0; 
                var buffer = new List<string>();
                while(index < bContext.Items.Count)
                {
                    var item = bContext.Items[index];
                    buffer.Add(string.Format(valuePart, 
                        item.Year, 
                        item.Week, 
                        item.Id,
                        CleanString(item.Title),
                        FormatDate(item.StartDate),
                        FormatDate(item.EndDate),
                        item.FromPriceInCents,
                        item.ForPriceInCents,
                        CleanString(item.DiscountText),
                        CleanString(item.UnitSize),
                        CleanString(item.Category),
                        CleanString(item.Link),
                        CleanString(item.Brand),
                        item.ActiveAtNumberOfProducts,
                        CleanString(item.Store)));
                    if (index % 1000 == 999)
                    {
                        await FlushData(connection, insertPart, buffer);
                    }
                    index++;
                }
                await FlushData(connection, insertPart, buffer);
                await connection.CloseAsync();
            } 
        }

        private object FormatDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        private string CleanString(string title)
        {
            return title?.Replace("'", "''");
        }

        private static async Task FlushData(SqlConnection connection, string insertPart, List<string> buffer)
        {
            var command = new SqlCommand(insertPart + string.Join(",", buffer), connection);

            await command.ExecuteNonQueryAsync();
            buffer.Clear();
        }
    }
}
