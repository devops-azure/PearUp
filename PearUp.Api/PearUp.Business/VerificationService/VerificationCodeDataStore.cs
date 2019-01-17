using PearUp.IBusiness;
using System.Collections.Generic;

namespace PearUp.Business
{
    public class VerificationCodeDataStore : IVerificationCodeDataStore
    {
        private static readonly Dictionary<string, int> verificationCodes = new Dictionary<string, int>();

        public void Add(string key, int value)
        {
            verificationCodes.Add(key, value);
        }
        public void Remove(string key)
        {
            verificationCodes.Remove(key);
        }
        public int GetValueOrDefault(string key)
        {
            int result = 0;
            verificationCodes.TryGetValue(key, out result);
            return result;
        }
        public bool ContainsKey(string key)
        {
            return verificationCodes.ContainsKey(key);
        }

    }
}
