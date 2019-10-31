using Cricetidae.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cricetidae.Interfaces
{
    public interface IProductPriceDataReader
    {
        Task GetProductPriceData(IEnumerable<BonusProductEvent> products);
    }
}
