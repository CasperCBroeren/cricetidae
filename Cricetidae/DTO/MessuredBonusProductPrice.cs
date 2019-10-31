using System;

namespace Cricetidae.DTO
{
    public class MessuredBonusProductPrice
    {
        public DateTime EndDate { get; internal set; }
        public DateTime StartDate { get; internal set; }
        public int FromPriceInCents { get; internal set; }
        public int ForPriceInCents { get; internal set; }
        public int Delta { get; internal set; }
        public string DiscountText { get; internal set; }
        public string UnitSize { get; internal set; }
        public int ActiveAtNumberOfProducts { get; internal set; }
        public int Year { get; internal set; }
        public int Week { get; internal set; }
    }
}