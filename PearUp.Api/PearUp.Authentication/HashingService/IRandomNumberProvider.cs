using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.Utilities
{
    public interface IRandomNumberProvider
    {
        int Next(int minValue, int maxValue);
    }
}
