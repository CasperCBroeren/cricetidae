using Cricetidae.DTO;
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
        private TimeSpan Period = new TimeSpan(7, 0, 0, 0);
        public BasicPipeline(IServiceProvider serviceProvider, ILogger<BasicPipeline> logger): base (
            new IPipeLineItem[]
            {
                (IPipeLineItem)serviceProvider.GetService(typeof(BonusDataReader)),
                (IPipeLineItem)serviceProvider.GetService(typeof(ProductPriceDataReader)),
                (IPipeLineItem)serviceProvider.GetService(typeof(BonusDataPersister)),
                (IPipeLineItem)serviceProvider.GetService(typeof(BonusProductAmountCorrecter)), 
            },
            logger)
        { 
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await this.Start();
                await Task.Delay(Period, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
