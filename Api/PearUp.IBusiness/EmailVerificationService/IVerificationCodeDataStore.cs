using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.IBusiness
{
    public interface IVerificationCodeDataStore
    {
        void Add(string key, int value);
        void Remove(string key);
        int GetValueOrDefault(string key);
        bool ContainsKey(string key);
    }
}
