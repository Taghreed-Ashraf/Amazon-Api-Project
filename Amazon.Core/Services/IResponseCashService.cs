using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Services
{
    public interface IResponseCashService
    {
        Task CashResponseAsync(string cashKey, object response, TimeSpan timeToLive);
    
         Task<string> GetCashedResponseAsync(string cashKey);
    }
}
