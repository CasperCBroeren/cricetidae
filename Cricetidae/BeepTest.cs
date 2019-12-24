using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cricetidae.Pipeline;

namespace Cricetidae
{
    public class BeepTest : IPipeLineItem
    {
        public Task DoWork(APipeLineContext context)
        {
            Console.WriteLine("Beep");
            return Task.CompletedTask;
        }
    }
}
