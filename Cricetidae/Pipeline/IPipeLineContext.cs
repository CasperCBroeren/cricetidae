using System;
using System.Collections.Generic;
using System.Text;

namespace Cricetidae.Pipeline
{
    public abstract class APipeLineContext
    {
        public DateTime Started { get; set; }
        public DateTime? Ended { get; set; }
        public int StepsCompleted { get; set; }
    }
}
