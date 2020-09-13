using EnpalSharpTemplate.Model;
using EnpalSharpTemplate.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnpalSharpTemplate.ServiceInterface
{
    public interface IHistorySearch
    {
        Task<PayloadModel> GetSingleVersion(string Id);
        Task<List<PayloadViewModel>> GetAll();
        bool CheckKeyExist(string key);
    }
}
