using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cricetidae.DTO;

namespace Cricetidae.Interfaces
{
    public interface IMeasuredBonusProductRepository
    {
        Task<IReadOnlyList<MeasuredBonusProduct>> GetRecentAndHistory();
        Task<IEnumerable<MeasuredBonusProduct>> GetTopDeltaForStore(string store);
    }
}
