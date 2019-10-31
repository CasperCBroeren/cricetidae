using Cricetidae.Pipeline;
using System;
using System.Collections.Generic;

namespace Cricetidae.DTO
{
    public class BonusContext : APipeLineContext
    { 
        public IReadOnlyList<BonusProductEvent> Items { get; set; }
    }
}
