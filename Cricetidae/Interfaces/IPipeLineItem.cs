using Cricetidae.Pipeline;
using System.Threading.Tasks;

namespace Cricetidae
{
    public interface IPipeLineItem
    {
        Task DoWork(APipeLineContext context);
    }
}
