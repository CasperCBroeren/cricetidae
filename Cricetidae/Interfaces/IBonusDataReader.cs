using Cricetidae.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cricetidae.Interfaces
{
    public interface IBonusDataReader: IPipeLineItem
    {
        Task<IReadOnlyList<BonusProductEvent>> Get();
    }
}
