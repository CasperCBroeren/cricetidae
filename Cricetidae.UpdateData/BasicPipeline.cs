using Cricetidae.DTO;
using Cricetidae.Interfaces;
using Cricetidae.Pipeline;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cricetidae.UpdateData
{
    public class BasicPipeline: ABasePipeline<BonusContext>, IHostedService
    { 
        public BasicPipeline(IServiceProvider serviceProvider, ILogger<BasicPipeline> logger): base (
            new IPipeLineItem[]
            {
                (IPipeLineItem)serviceProvider.GetService(typeof(IBonusDataReader)),
                (IPipeLineItem)serviceProvider.GetService(typeof(IProductPriceDataReader)),
                (IPipeLineItem)serviceProvider.GetService(typeof(IBonusDataPersister)),
                (IPipeLineItem)serviceProvider.GetService(typeof(IBonusProductAmountCorrecter)),
            },
            logger)
        { 
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
