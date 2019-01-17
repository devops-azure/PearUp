using System;
using System.Threading.Tasks;

namespace PearUp.Infrastructure
{
    public interface IEmailHelper : IDisposable
    {
        Task SendAsync(string message);
    }
}