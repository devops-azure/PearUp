using System;

namespace PearUp.Authentication
{
    public interface IAuthToken
    {
        DateTime ValidTo { get; }
        string Value { get; }
    }
}