using System;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Cricetidae.Helper;
using Newtonsoft.Json.Linq;

namespace Cricetidae.DTO
{
    public class BonusProductEvent
    { 
        public BonusProductEvent(JObject bonusItem)
        {
            if (bonusItem.ContainsKey("id"))
            {
                var shield = bonusItem["shield"] as JObject;
                var card = bonusItem["card"] as JObject;
                
                var control = bonusItem["control"] as JObject;

                this.Year = DateTime.Now.Year;
                this.Week = ISOWeek.GetWeekOfYear(DateTime.Now);
                this.Title = TryAndGetField<string>(bonusItem, "title");
                if (card == null)
                {
                    this.Id = TryAndGetField<long>(bonusItem, "id");
                    this.Link = TryAndGetField<string>(bonusItem, "link");
                }
                else
                {
                    var productOfCard = card["products"] as JArray;
                    this.Id = TryAndGetField<long>(card, "id");
                    this.Link = TryAndGetField<string>(productOfCard[0] as JObject, "link");
                }
                this.Brand  = TryAndGetField<string>(bonusItem, "brand");
                if (bonusItem.ContainsKey("discount"))
                {
                    var discount = bonusItem["discount"] as JObject;
                    var price = bonusItem["price"] as JObject;
                    

                    if (discount.ContainsKey("fromPrice"))
                    {
                        this.FromPriceInCents = discount["fromPrice"].Value<int>();
                    }
                    else if (price != null && price.ContainsKey("was"))
                    {
                        this.FromPriceInCents = ParseDecimalPrice(price["was"].Value<string>());
                    }

                    if (discount.ContainsKey("forPrice"))
                    {
                        this.ForPriceInCents = discount["forPrice"].Value<int>();
                    }
                    else if (price != null && price.ContainsKey("now"))
                    {
                        this.ForPriceInCents = ParseDecimalPrice(price["now"].Value<string>());
                    }

                    this.StartDate = TryAndGetField<DateTime>(discount, "startDate");
                    this.EndDate = TryAndGetField<DateTime>(discount, "endDate");

                    if (discount.ContainsKey("text"))
                    {
                        this.DiscountText = discount["text"].Value<string>();
                    }
                    else if (shield != null && shield.ContainsKey("text"))
                    {
                        this.DiscountText = shield["text"].Value<string>();
                    }

                    this.Store = TryAndGetField<string>(control, "theme");
                    if (this.Store == null)
                    {
                        this.Store = TryAndGetField<string>(control, "bonusType");
                    }
                    if (this.Store == null || this.Store.Equals("bonus")) 
                    {
                        this.Store = "ah";
                    }
                }
                 
                this.UnitSize = TryAndGetField<string>(bonusItem, "unitSize");
                this.Category = TryAndGetField<string>(bonusItem, "category");
       
                this.NumberOfProducts = TryAndGetField<int>(bonusItem, "numberOfProducts");
                var fromPrice = this.FromPriceInCents;
                var forPrice = this.ForPriceInCents;
                var amountOfProducts = 1;
                BonusTextAnalyser.AnalyseBonusText(this.DiscountText, ref fromPrice, ref forPrice, ref amountOfProducts);
                this.FromPriceInCents = fromPrice;
                this.ForPriceInCents = forPrice;
                this.ActiveAtNumberOfProducts = amountOfProducts;


            }
            else
            {
                this.Id = long.MinValue;
            }
        }

        public BonusProductEvent(int productcode)
        {
            this.Id = productcode;
        }

        private int ParseDecimalPrice(string value)
        {
            var decimalPrice = decimal.Parse(value, NumberStyles.AllowDecimalPoint);
            return (int)(decimalPrice * 100.0m);
        }

        public int FromPriceInCents { get; set; }
        public int ForPriceInCents { get; set; }

        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public string Title { get; }

        public int Year { get; }

        public int Week { get; }

        public long Id { get; }

        public string DiscountText { get; set; }
        public string Brand { get; }
        public string UnitSize { get; }
        public string Category { get; }
        public string Link { get; }

        [JsonIgnore]
        public int NumberOfProducts { get; }
        public int ActiveAtNumberOfProducts { get; }
        public string Store { get; }

        private T TryAndGetField<T>(JObject item, string element)
        {
            return (item != null && item.ContainsKey(element)) ? item.Value<T>(element) : default;
        }

    }
}