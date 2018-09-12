using System;
using HmxLabs.Core.DateTIme;

namespace HmxLabs.Core.Tests.Base
{
    public class FixedTimeProvider : ITimeProvider
    {
        public FixedTimeProvider(DateTime dateTime_)
        {
            Now = dateTime_;
        }

        public DateTime Now { get; }
    }
}
