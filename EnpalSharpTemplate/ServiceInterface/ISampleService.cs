using EnpalSharpTemplate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnpalSharpTemplate.ServiceInterface
{
    public interface ISampleService
    {
         Task<PayloadModel> ExecuteResultAsync(string key);
    }
}
