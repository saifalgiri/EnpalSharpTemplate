using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnpalSharpTemplate.Model;
using EnpalSharpTemplate.ServiceInterface;
using EnpalSharpTemplate.ViewModel;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnpalSharpTemplate.Services
{
    public class SampleService : ISampleService
    {
        private IHistorySearch _historySearch;

        public SampleService(IHistorySearch historySearch)
        {
            _historySearch = historySearch;
        }
        public async Task<PayloadModel> ExecuteResultAsync(string key)
        {
            return await _historySearch.GetSingleVersion(key);
        }
    }
}
