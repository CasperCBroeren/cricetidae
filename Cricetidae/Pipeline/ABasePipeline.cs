using Cricetidae.DTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cricetidae.Pipeline
{
    public abstract class ABasePipeline<TContext>: IPipeLine where TContext : APipeLineContext, new()
    {
        protected readonly IPipeLineItem[] Steps;
        protected readonly ILogger Logger;

        public ABasePipeline(IPipeLineItem[] steps, ILogger logger)
        {
            Steps = steps;
            Logger = logger;
        }

        public async Task Start()
        {
            var context = new TContext();
            context.Started = DateTime.Now;
            foreach (var step in Steps)
            {
                await step.DoWork(context);
                context.StepsCompleted++;
            }
            context.Ended = DateTime.Now;

            Logger.LogInformation("The pipeline started at {startDate} is done in {steps} steps at {endDate}",
                context.Started.ToShortTimeString(),
                context.StepsCompleted,
                context.Ended?.ToShortTimeString());
        }
    }
}
