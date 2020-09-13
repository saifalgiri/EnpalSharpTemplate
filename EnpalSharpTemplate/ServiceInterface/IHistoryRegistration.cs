using EnpalSharpTemplate.Model;
using EnpalSharpTemplate.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnpalSharpTemplate.ServiceInterface
{
    public interface IHistoryRegistration 
    {
        Task<bool> Create(PayloadModel payload);
    }
}
