using System;

namespace PearUp.Infrastructure.Azure
{
    public class AzureSasKey
    {
        public string SasKey { get; set; }
        public DateTimeOffset ExpiryTimeInUtc { get; set; }
    }
}
