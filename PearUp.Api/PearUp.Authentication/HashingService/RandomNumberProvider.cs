using System;

namespace PearUp.Utilities
{
    public class RandomNumberProvider : IRandomNumberProvider
    {
        private readonly Random _random = new Random();

        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
