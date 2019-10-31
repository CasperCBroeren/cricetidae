using System.Globalization;
using System.Text.RegularExpressions;

namespace Cricetidae.Helper
{
    public static class BonusTextAnalyser
    {
        private static Regex bonusTextFromFor = new Regex("^(.*?) VOOR (.*?)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex bonusTextFromForParts = new Regex("^(.*?) STUKS (.*?)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex bonusTextMoneyDeducted = new Regex("^(.*?) EURO KORTING$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex bonusTextPercentDeducted = new Regex("^(.*?)% korting$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static string bonusTextSecondHalvePrice = "2e halve prijs";
        private static string bonusTextSecondGratis = "1 + 1 GRATIS";
        private static string bonusTextSecondTwoGratis = "2 + 2 GRATIS";
        private static string bonusTextSecondGratisAlt = "2=1";
        private static string bonusTextSecondTwoGratisAlt = "4=2";
        private static string bonusTextThirdGratis = "2 + 1 GRATIS";


        public static void AnalyseBonusText(string discountText, ref int fromPrice, ref int forPrice, ref int amountOfProducts)
        {
            if (fromPrice == 0 && forPrice != 0)
            {
                fromPrice = forPrice;
                if (bonusTextFromFor.IsMatch(discountText))
                {
                    amountOfProducts = int.Parse(bonusTextFromFor.Match(discountText).Groups[1].Value);
                    forPrice = (int)((decimal.Parse(bonusTextFromFor.Match(discountText).Groups[2].Value, NumberStyles.AllowDecimalPoint) / amountOfProducts) * 100);
                }
                else if (bonusTextMoneyDeducted.IsMatch(discountText))
                {
                    var amountOfEuroDeduction = int.Parse(bonusTextMoneyDeducted.Match(discountText).Groups[1].Value) * 100;
                    amountOfProducts = 1;
                    forPrice = fromPrice - amountOfEuroDeduction;
                }
                else if (bonusTextPercentDeducted.IsMatch(discountText))
                {
                    var percentageDeduction = int.Parse(bonusTextPercentDeducted.Match(discountText).Groups[1].Value) / 100.0m;
                    amountOfProducts = 1;
                    forPrice = fromPrice - (int)(fromPrice * percentageDeduction);
                }
                else if (bonusTextFromForParts.IsMatch(discountText))
                {
                    amountOfProducts = int.Parse(bonusTextFromForParts.Match(discountText).Groups[1].Value);
                    forPrice = (int)((decimal.Parse(bonusTextFromForParts.Match(discountText).Groups[2].Value, NumberStyles.AllowDecimalPoint) / amountOfProducts) * 100);
                }
                else if (bonusTextSecondHalvePrice.Equals(discountText, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    amountOfProducts = 2;
                    forPrice = (int)((forPrice + (forPrice / 2.0m)) / 2.0m);
                }
                else if (bonusTextSecondGratis.Equals(discountText, System.StringComparison.InvariantCultureIgnoreCase)
                    || bonusTextSecondGratisAlt.Equals(discountText, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    amountOfProducts = 2;
                    forPrice = (int)(forPrice / 2.0m);
                }
                else if (bonusTextSecondTwoGratis.Equals(discountText, System.StringComparison.InvariantCultureIgnoreCase)
                  || bonusTextSecondTwoGratisAlt.Equals(discountText, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    amountOfProducts = 4;
                    forPrice = (int)(forPrice / 4.0m);
                }
                else if (bonusTextThirdGratis.Equals(discountText, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    amountOfProducts = 3;
                    forPrice = (int)((forPrice * 2.0m)/3.0m);
                }
            }
        }
    }
}
