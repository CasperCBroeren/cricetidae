using System.Collections.Generic;

namespace Cricetidae.DTO
{
    public class MeasuredBonusProduct
    {
        private string rawCategory;
        public string Title { get; internal set; }
        public long Id { get; internal set; }

        
        public int BonusOccurance { get; internal set; }
        public string Brand { get; internal set; }
        public string Link { get; internal set; }
        public string Store { get; internal set; }
        public string RawCategory
        {
            get { return rawCategory; }
            set { rawCategory = value; }
        }

        public string Category
        {
            get { return (rawCategory.IndexOf('/') > 0) ? rawCategory.Substring(0,rawCategory.IndexOf('/')) : rawCategory; }
        }
        public string SubCategory
        {
            get { return (rawCategory.IndexOf('/') > 0) ? rawCategory.Substring(rawCategory.IndexOf('/')) : string.Empty; }
        }

        public IList<MessuredBonusProductPrice> Prices { get; internal set; }
    }
}
