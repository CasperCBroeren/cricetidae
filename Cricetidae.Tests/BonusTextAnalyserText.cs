using Cricetidae.Helper;
using Shouldly;
using Xunit;

namespace Cricetidae.Tests
{
    public class BonusTextAnalyserText
    {
        [Fact]
        public void AnalyseBonusText_bonusTextFromFor()
        {
            var fromPrice = 0;
            var forPrice = 800;
            var amountOfProducts = 1;
            BonusTextAnalyser.AnalyseBonusText("2 VOOR 4.99", ref fromPrice, ref forPrice, ref amountOfProducts);

            fromPrice.ShouldBe(800);
            forPrice.ShouldBe(249);
            amountOfProducts.ShouldBe(2);
        }

        [Fact]
        public void AnalyseBonusText_bonusTextFromFor3()
        {
            var fromPrice = 0;
            var forPrice = 800;
            var amountOfProducts = 1;
            BonusTextAnalyser.AnalyseBonusText("3 VOOR 9", ref fromPrice, ref forPrice, ref amountOfProducts);

            fromPrice.ShouldBe(800);
            forPrice.ShouldBe(300);
            amountOfProducts.ShouldBe(3);
        }

        [Fact]
        public void AnalyseBonusText_bonusTextFromForParts()
        {
            var fromPrice = 0;
            var forPrice = 800;
            var amountOfProducts = 1;
            BonusTextAnalyser.AnalyseBonusText("3 STUKS 9.00", ref fromPrice, ref forPrice, ref amountOfProducts);

            fromPrice.ShouldBe(800);
            forPrice.ShouldBe(300);
            amountOfProducts.ShouldBe(3);
        }

        [Fact]
        public void AnalyseBonusText_bonusTextMoneyDeducted()
        {
            var fromPrice = 0;
            var forPrice = 800;
            var amountOfProducts = 1;
            BonusTextAnalyser.AnalyseBonusText("3 EURO KORTING", ref fromPrice, ref forPrice, ref amountOfProducts);

            fromPrice.ShouldBe(800);
            forPrice.ShouldBe(500);
            amountOfProducts.ShouldBe(1);
        }

        [Fact]
        public void AnalyseBonusText_bonusTextSecondHalvePrice()
        {
            var fromPrice = 0;
            var forPrice = 800;
            var amountOfProducts = 1;
            BonusTextAnalyser.AnalyseBonusText("2e halve prijs", ref fromPrice, ref forPrice, ref amountOfProducts);

            fromPrice.ShouldBe(800);
            forPrice.ShouldBe(600);
            amountOfProducts.ShouldBe(2);
        }

        [Fact]
        public void AnalyseBonusText_bonusTextPercentDeducted()
        {
            var fromPrice = 0;
            var forPrice = 1000;
            var amountOfProducts = 1;
            BonusTextAnalyser.AnalyseBonusText("10% korting", ref fromPrice, ref forPrice, ref amountOfProducts);

            fromPrice.ShouldBe(1000);
            forPrice.ShouldBe(900);
            amountOfProducts.ShouldBe(1);
        }

        [Fact]
        public void AnalyseBonusText_bonusTextSecondGratis()
        {
            var fromPrice = 0;
            var forPrice = 300;
            var amountOfProducts = 1;
            BonusTextAnalyser.AnalyseBonusText("1 + 1 GRATIS", ref fromPrice, ref forPrice, ref amountOfProducts);

            fromPrice.ShouldBe(300);
            forPrice.ShouldBe(150);
            amountOfProducts.ShouldBe(2);
        }

        [Fact]
        public void AnalyseBonusText_bonusTextSecondGratisAlt()
        {
            var fromPrice = 0;
            var forPrice = 300;
            var amountOfProducts = 1;
            BonusTextAnalyser.AnalyseBonusText("2=1", ref fromPrice, ref forPrice, ref amountOfProducts);

            fromPrice.ShouldBe(300);
            forPrice.ShouldBe(150);
            amountOfProducts.ShouldBe(2);
        }

        [Fact]
        public void AnalyseBonusText_bonusText2Plus2Free()
        {
            var fromPrice = 0;
            var forPrice = 400;
            var amountOfProducts = 1;
            BonusTextAnalyser.AnalyseBonusText("2 + 2 gratis", ref fromPrice, ref forPrice, ref amountOfProducts);

            fromPrice.ShouldBe(400);
            forPrice.ShouldBe(100);
            amountOfProducts.ShouldBe(4);
        }
    }
}
